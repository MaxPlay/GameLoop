using GameLoop.Editor.Modules;
using GameLoop.Extensions;
using GameLoop.Modules;
using GameLoop.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    [CustomEditor(typeof(ModuleSystem))]
    public class ModuleSystemEditor : UnityEditor.Editor
    {
        #region Private Fields

        private bool cardStacksFoldout;
        private bool dataFoldout;
        private bool gameStateFoldout;
        private bool settingsFoldout;
        private DataStorageEditor dataStorageEditor;
        private int selectedActionModule;
        private int selectedLogicModule;
        private List<ModuleBaseUI> moduleUI;
        private ModuleBuilder builder;
        private ModuleSystem ms;
        private ModuleType selectedType;
        private SerializedProperty data;
        private SerializedProperty dataManager;
        private SerializedProperty maxDepth;
        private SerializedProperty initialCardStack;
        private SerializedProperty interfaceControl;
        private XmlSerializer serializer;
        private bool noInteraction;
        private int selectedInterActionModule;

        #endregion Private Fields

        #region Private Enums

        private enum ModuleType
        {
            Action,
            Logic,
            Interaction
        }

        #endregion Private Enums

        #region Public Methods

        public override void OnInspectorGUI()
        {
            dataStorageEditor.OnInspectorGUI();

            GUILayout.Label("Module System", EditorStyles.boldLabel);

            if (!Application.isPlaying)
                ms.Stop();

            noInteraction = ms.State != SystemState.Stopped;
            using (var scope = new EditorGUI.DisabledScope(noInteraction))
            {
                DrawInterface();
                DrawLoop();
            }

            if (noInteraction)
                EditorUtility.SetDirty(target);
        }

        #endregion Public Methods

        #region Private Methods

        private void ClearLoop()
        {
            if (!EditorUtility.DisplayDialog("Warning", "You are about to clear the whole GameLoop. Proceed?", "Ok", "Cancel"))
                return;

            ms.Clear();
            CreateUIObjects();
        }

        private void CreateUIObjects()
        {
            moduleUI.ForEach((module) =>
            {
                module.MoveDown -= MoveDown;
                module.MoveUp -= MoveUp;
                module.DecreaseDepth -= DecreaseDepth;
                module.IncreaseDepth -= IncreaseDepth;
                module.Remove -= Remove;
                module.ForceRefresh -= ForceModuleRefresh;
            });
            moduleUI.Clear();
            foreach (var module in ms)
            {
                var m = ModuleBaseUI.Create(module);
                m.MoveDown += MoveDown;
                m.MoveUp += MoveUp;
                m.DecreaseDepth += DecreaseDepth;
                m.IncreaseDepth += IncreaseDepth;
                m.Remove += Remove;
                m.ForceRefresh += ForceModuleRefresh;
                moduleUI.Add(m);
            }
        }

        private void DecreaseDepth(ModuleBase module)
        {
            if (module.Scope == 0)
                return;
            module.Scope--;
        }

        private void DrawInterface()
        {
            settingsFoldout = EditorGUILayout.Foldout(settingsFoldout, "Settings");
            if (settingsFoldout)
            {
                EditorGUILayout.DelayedIntField(maxDepth);
                EditorGUILayout.ObjectField(initialCardStack);
                EditorGUILayout.ObjectField(interfaceControl);
                if (GUI.changed)
                    Save(true);

                bool compression = EditorGUILayout.Toggle("Use Compression", ms.UseCompression);
                if (compression != ms.UseCompression)
                {
                    ms.LoadData();
                    ms.UseCompression = compression;
                    Save(true);
                }
            }

            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                selectedType = (ModuleType)EditorGUILayout.EnumPopup(selectedType);
                switch (selectedType)
                {
                    case ModuleType.Action:
                        GUI.enabled = builder.ActionModules.Length > 0;
                        selectedActionModule = EditorGUILayout.Popup(selectedActionModule, builder.ActionModuleNames);
                        break;

                    case ModuleType.Logic:
                        GUI.enabled = builder.LogicModules.Length > 0;
                        selectedLogicModule = EditorGUILayout.Popup(selectedLogicModule, builder.LogicModuleNames);
                        break;

                    case ModuleType.Interaction:
                        GUI.enabled = builder.InterActionModules.Length > 0;
                        selectedInterActionModule = EditorGUILayout.Popup(selectedInterActionModule, builder.InterActionModuleNames);
                        break;
                }

                if (GUILayout.Button("+"))
                {
                    switch (selectedType)
                    {
                        case ModuleType.Action:
                            ms.Add(builder.CreateModule(builder.ActionModuleNames[selectedActionModule]));
                            break;

                        case ModuleType.Logic:
                            ms.Add(builder.CreateModule(builder.LogicModuleNames[selectedLogicModule]));
                            break;
                        case ModuleType.Interaction:
                            ms.Add(builder.CreateModule(builder.InterActionModuleNames[selectedInterActionModule]));
                            break;
                    }
                    CreateUIObjects();
                    Save(true);
                }
                GUI.enabled = true;
            }

            using (var outerScope = new EditorGUILayout.VerticalScope())
            {
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Clear"))
                        ClearLoop();
                    if (GUILayout.Button("Save"))
                        Save(false);
                    if (GUILayout.Button("Load"))
                        Load();
                }
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Expand All"))
                        moduleUI.ForEach((module) => { module.Foldout = true; });
                    if (GUILayout.Button("Collapse All"))
                        moduleUI.ForEach((module) => { module.Foldout = false; });
                }
            }
        }

        private void DrawLoop()
        {
            if (noInteraction)
            {
                GUI.enabled = true;
                GUILayout.Label("Can't load Modules while game is running.", EditorStyles.boldLabel);
                GUI.enabled = false;
                return;
            }

            for (int i = 0; i < moduleUI.Count; i++)
            {
                using (var indent = new EditorGUILayout.HorizontalScope())
                {
                    GUI.backgroundColor = moduleUI[i].Background;
                    GUILayout.Space(moduleUI[i].Depth * ModuleSystemPreferences.DepthDistance);
                    using (var scope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        moduleUI[i].OnGUI(builder);
                    }
                }
            }
        }

        private void ForceModuleRefresh(ModuleBaseUI module)
        {
            int index = moduleUI.IndexOf(module);
            moduleUI[index].MoveDown -= MoveDown;
            moduleUI[index].MoveUp -= MoveUp;
            moduleUI[index].DecreaseDepth -= DecreaseDepth;
            moduleUI[index].IncreaseDepth -= IncreaseDepth;
            moduleUI[index].Remove -= Remove;
            moduleUI[index].ForceRefresh -= ForceModuleRefresh;

            moduleUI[index] = ModuleBaseUI.Create(ms[index]);
            moduleUI[index].MoveDown += MoveDown;
            moduleUI[index].MoveUp += MoveUp;
            moduleUI[index].DecreaseDepth += DecreaseDepth;
            moduleUI[index].IncreaseDepth += IncreaseDepth;
            moduleUI[index].Remove += Remove;
            moduleUI[index].ForceRefresh += ForceModuleRefresh;
            moduleUI[index].Foldout = true;
        }

        private void IncreaseDepth(ModuleBase module)
        {
            if (ms.MaxDepth > -1)
            {
                if (ms.MaxDepth == module.Scope)
                    return;
                if (ms.MaxDepth < module.Scope)
                {
                    module.Scope = ms.MaxDepth;
                    return;
                }
            }
            module.Scope++;
        }

        private void Load()
        {
            if (noInteraction)
                return;

            ms.Modules.Clear();
            ms.LoadData();

            CreateUIObjects();
        }

        private void MoveDown(ModuleBase module)
        {
            int index = ms.IndexOf(module);
            if (index == ms.Count - 1)
                return;
            ms.Modules.Swap(index, index + 1);
            CreateUIObjects();
        }

        private void MoveUp(ModuleBase module)
        {
            int index = ms.IndexOf(module);
            if (index == 0)
                return;
            ms.Modules.Swap(index, index - 1);
            CreateUIObjects();
        }

        private void OnDisable()
        {
            if (noInteraction)
                return;

            Save(true);
        }

        private void OnEnable()
        {
            ms = target as ModuleSystem;
            noInteraction = ms.State != SystemState.Stopped;
            dataStorageEditor = new DataStorageEditor(ms);
            data = serializedObject.FindProperty("serializationData");
            maxDepth = serializedObject.FindProperty("maxDepth");
            initialCardStack = serializedObject.FindProperty("initialCardStack");
            interfaceControl = serializedObject.FindProperty("interfaceControl");
            ms.Serializer = new XmlModuleSystemSerializer(ms);
            moduleUI = new List<ModuleBaseUI>();
            builder = new ModuleBuilder();
            builder.Refresh();
            Load();
        }

        private void Remove(ModuleBase module)
        {
            ms.Modules.Remove(module);
            CreateUIObjects();
        }

        private void Save(bool storeInAssetOnly)
        {
            if (target == null || noInteraction)
                return;
            ms.SaveData();
            serializedObject.ApplyModifiedProperties();

            if (!storeInAssetOnly)
                AssetDatabase.SaveAssets();
        }

        #endregion Private Methods
    }
}
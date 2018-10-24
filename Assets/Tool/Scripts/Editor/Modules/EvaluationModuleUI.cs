using GameLoop.Modules;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor.Modules
{
    public class EvaluationModuleUI : ModuleBaseUI
    {
        #region Protected Fields

        protected EvaluationModuleUI a;
        protected EvaluationModuleUI b;
        protected EvaluationType evaluationType;
        protected EvaluationModule module;
        protected EvaluationPreset preset;

        #endregion Protected Fields

        #region Public Constructors

        public EvaluationModuleUI(EvaluationModule module)
        {
            this.module = module;
            if (module is BinaryEvaluationModule)
            {
                evaluationType = EvaluationType.Binary;
                a = new EvaluationModuleUI(((BinaryEvaluationModule)module).A);
                a.ModuleChanged += A_ModuleChanged;
                a.ForceRefresh += ChildForceRefresh;
                b = new EvaluationModuleUI(((BinaryEvaluationModule)module).B);
                b.ModuleChanged += B_ModuleChanged;
                b.ForceRefresh += ChildForceRefresh;
            }
            else if (module is UnaryEvaluationModule)
            {
                evaluationType = EvaluationType.Unary;
                a = new EvaluationModuleUI(((UnaryEvaluationModule)module).A);
                a.ModuleChanged += A_ModuleChanged;
                a.ForceRefresh += ChildForceRefresh;
            }
            else
                evaluationType = EvaluationType.Singular;
            Name = module.GetType().Name;
            preset = new EvaluationPreset()
            {
                Type = evaluationType
            };
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void ModuleChangedEventHandler(EvaluationType type, int index, ModuleBuilder builder);

        #endregion Public Delegates

        #region Public Events

        public event ModuleChangedEventHandler ModuleChanged;

        #endregion Public Events

        #region Public Properties

        public override Color Background
        {
            get
            {
                return ModuleSystemPreferences.EvaluationColor;
            }
        }

        public override int Depth
        {
            get
            {
                return module.Scope;
            }

            set
            {
                module.Scope = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void OnGUI(ModuleBuilder builder)
        {
            GUI.backgroundColor = ModuleSystemPreferences.EvaluationColor;
            using (var scope = new EditorGUILayout.VerticalScope(EditorStyles.textArea))
            {
                EvaluationChooser(builder);
                GUILayout.Label(Name, EditorStyles.boldLabel);
                switch (evaluationType)
                {
                    case EvaluationType.Singular:
                        if (module is ICustomEditor)
                            ((ICustomEditor)module).CustomEditor();
                        break;

                    case EvaluationType.Unary:
                        a.OnGUI(builder);
                        break;

                    case EvaluationType.Binary:
                        a.OnGUI(builder);
                        b.OnGUI(builder);
                        break;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void EvaluationChooser(ModuleBuilder builder)
        {
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Change Evaluator");
                preset.Type = (EvaluationType)EditorGUILayout.EnumPopup(preset.Type);
                preset.Selection = EditorGUILayout.Popup(preset.Selection, builder.EvaluationModuleNamesByType[preset.Type]);
                if (GUILayout.Button("Apply"))
                {
                    OnModuleChanged(preset, builder);
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void A_ModuleChanged(EvaluationType type, int index, ModuleBuilder builder)
        {
            a.ModuleChanged -= A_ModuleChanged;
            switch (evaluationType)
            {
                case EvaluationType.Unary:
                    ((UnaryEvaluationModule)module).A = builder.CreateModule(builder.EvaluationModuleNamesByType[type][index]) as EvaluationModule;
                    break;

                case EvaluationType.Binary:
                    ((BinaryEvaluationModule)module).A = builder.CreateModule(builder.EvaluationModuleNamesByType[type][index]) as EvaluationModule;
                    break;
            }
            OnForceRefresh();
        }

        private void B_ModuleChanged(EvaluationType type, int index, ModuleBuilder builder)
        {
            b.ModuleChanged -= B_ModuleChanged;
            ((BinaryEvaluationModule)module).B = builder.CreateModule(builder.EvaluationModuleNamesByType[type][index]) as EvaluationModule;
            OnForceRefresh();
        }

        private void ChildForceRefresh(ModuleBaseUI module) => OnForceRefresh();

        private void OnModuleChanged(EvaluationPreset preset, ModuleBuilder builder)
        {
            ModuleChanged?.Invoke(preset.Type, preset.Selection, builder);
        }

        #endregion Private Methods

        #region Public Classes

        public class EvaluationPreset
        {
            #region Private Fields

            private readonly Dictionary<EvaluationType, int> selection;

            #endregion Private Fields

            #region Public Constructors

            public EvaluationPreset()
            {
                selection = new Dictionary<EvaluationType, int>
            {
                { EvaluationType.Singular, 0 },
                { EvaluationType.Unary, 0 },
                { EvaluationType.Binary, 0 }
            };
            }

            #endregion Public Constructors

            #region Public Properties

            public int Selection
            {
                get { return selection[Type]; }
                set { selection[Type] = value; }
            }

            public EvaluationType Type { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}
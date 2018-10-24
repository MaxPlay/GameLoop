using GameLoop;
using GameLoop.Data.Setup;
using GameLoop.Internal;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    public class DataStorageEditor
    {
        #region Private Fields

        private bool cardStacksFoldout;
        private bool gameStateFoldout;
        private ModuleSystem moduleSystem;

        #endregion Private Fields

        #region Public Constructors

        public DataStorageEditor(ModuleSystem moduleSystem)
        {
            this.moduleSystem = moduleSystem;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnInspectorGUI()
        {
            GUILayout.Label("Data Storage", EditorStyles.boldLabel);
            DrawSetup();
            DrawStacks();
            DrawGameState();
            EditorUtility.SetDirty(moduleSystem);
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawGameState()
        {
            gameStateFoldout = EditorGUILayout.Foldout(gameStateFoldout, "Current Game State");
            if (gameStateFoldout == false)
                return;

            using (var scope = new EditorGUI.DisabledScope())
            {
                if (moduleSystem.Data.GameState == null)
                    EditorGUILayout.HelpBox("Game State will only be visible while the game is running.", MessageType.Info);
                else
                    moduleSystem.Data.GameState.CustomEditor();
            }
        }

        private void DrawSetup()
        {
            GameSetup setup = EditorGUILayout.ObjectField("Game Setup", moduleSystem.Data.Setup, typeof(GameSetup), false) as GameSetup;
            if (setup != moduleSystem.Data.Setup)
            {
                moduleSystem.Data.Setup = setup;
                EditorUtility.SetDirty(moduleSystem);
            }
        }

        private void DrawStacks()
        {
            cardStacksFoldout = CustomEditorUtil.DrawAssetList("Card Stacks", moduleSystem.Data.CardStacks, "Drop Card Stacks here", cardStacksFoldout, CustomEditorUtil.DisabledObjectDisplay);
        }

        #endregion Private Methods
    }
}
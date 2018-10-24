using GameLoop;
using GameLoop.Components;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    [CustomEditor(typeof(GameLoopComponent))]
    public class GameLoopEditor : UnityEditor.Editor
    {
        #region Private Fields

        private GameLoopComponent tool;

        #endregion Private Fields

        #region Public Methods

        public override void OnInspectorGUI()
        {
            if (tool == null)
                tool = target as GameLoopComponent;

            base.OnInspectorGUI();

            bool valid = (tool.ModuleSystem != null);

            if (!valid)
            {
                Color old = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                GUILayout.Box("The GameLoop can't be executed without a Module System attached.");
                GUI.backgroundColor = old;
            }

            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUI.enabled = valid && EditorApplication.isPlaying && (tool.ModuleSystem?.State != SystemState.Running);
                if (GUILayout.Button("Start"))
                    tool.StartGame();
                GUI.enabled = valid && EditorApplication.isPlaying && tool.ModuleSystem?.State == SystemState.Running;
                if (GUILayout.Button("Pause"))
                    tool.PauseGame();
                GUI.enabled = valid && EditorApplication.isPlaying && tool.ModuleSystem?.State != SystemState.Stopped;
                if (GUILayout.Button("Stop"))
                    tool.StopGame();
                GUILayout.FlexibleSpace();
                GUI.enabled = true;
            }
        }

        #endregion Public Methods
    }
}
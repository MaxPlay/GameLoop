using GameLoop.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    public class ModuleSystemPreferences
    {
        #region Private Fields

        private const string actionModuleKey = "CGL_ActionModulesColor";
        private const string evaluationModuleKey = "CGL_EvaluationModuleColor";
        private const string logicModuleKey = "CGL_LogicModulesColor";
        private const string maxScopeDepthKey = "CGL_MaxScopeDepth";
        private static Color actionColor;
        private static Color evaluationColor;
        private static bool loaded;
        private static Color logicColor;
        private static int maxScopeDepth;
        private static Color interActionColor;
        private static string interActionModuleKey = "CGL_InterActionModulesColor";

        #endregion Private Fields

        #region Public Properties

        public static Color ActionColor { get { Load(); return actionColor; } }
        public static int DepthDistance { get { Load(); return maxScopeDepth; } }
        public static Color EvaluationColor { get { Load(); return evaluationColor; } }
        public static Color LogicColor { get { Load(); return logicColor; } }

        public static Color InterActionColor { get { Load(); return interActionColor; } }

        #endregion Public Properties

        #region Public Methods

        [PreferenceItem("Game Loop")]
        public static void PreferenceEditor()
        {
            Load();
            DrawGUI();

            if (GUI.changed)
                Save();
        }

        #endregion Public Methods

        #region Private Methods

        private static void DrawGUI()
        {
            GUILayout.Label("Color Preferences", EditorStyles.boldLabel);
            actionColor = EditorGUILayout.ColorField("Action Module", actionColor);
            interActionColor = EditorGUILayout.ColorField("Inter Action Module", interActionColor);
            evaluationColor = EditorGUILayout.ColorField("Evaluation Module", evaluationColor);
            logicColor = EditorGUILayout.ColorField("Logic Module", logicColor);
            GUILayout.Label("Technical Preferences", EditorStyles.boldLabel);
            maxScopeDepth = EditorGUILayout.IntSlider("Max. Scope Depth", maxScopeDepth, 0, 100);
        }

        private static void Load()
        {
            if (loaded)
                return;

            actionColor = EditorPrefs.GetInt(actionModuleKey, Color.yellow.ToInt32()).ToColor();
            evaluationColor = EditorPrefs.GetInt(evaluationModuleKey, Color.green.ToInt32()).ToColor();
            logicColor = EditorPrefs.GetInt(logicModuleKey, Color.magenta.ToInt32()).ToColor();
            interActionColor = EditorPrefs.GetInt(interActionModuleKey, new Color32(255, 153, 0, 255).ToInt32()).ToColor();
            maxScopeDepth = EditorPrefs.GetInt(maxScopeDepthKey, 10);
            loaded = true;
        }

        private static void Save()
        {
            EditorPrefs.SetInt(actionModuleKey, actionColor.ToInt32());
            EditorPrefs.SetInt(evaluationModuleKey, evaluationColor.ToInt32());
            EditorPrefs.SetInt(logicModuleKey, logicColor.ToInt32());
            EditorPrefs.SetInt(interActionModuleKey, interActionColor.ToInt32());
            EditorPrefs.SetInt(maxScopeDepthKey, maxScopeDepth);
        }

        #endregion Private Methods
    }
}
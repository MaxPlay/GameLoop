using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    public static class GameLoopEditorUtil
    {
        #region Public Methods

        public static void CreateAsset<T>(string objectName) where T : ScriptableObject, new()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string[] files = Directory.GetFiles(path, objectName + "*.asset");

            if (files.Length > 0)
                objectName += " (" + files.Length + ")";

            objectName += ".asset";

            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, Path.Combine(path, objectName));
        }

        #endregion Public Methods
    }
}
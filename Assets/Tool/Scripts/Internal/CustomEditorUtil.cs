#if UNITY_EDITOR

using GameLoop.Extensions;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Internal
{
    public static class CustomEditorUtil
    {
        public static bool DrawReadOnlyList<T>(string title, IList<T> input, bool foldout, Action<T> customUI = null)
        {
            if (!EditorGUILayout.Foldout(foldout, string.Format("{0} | Elements: {1}", title, input.Count), true))
                return false;

            ReadOnlyListing(input, customUI);
            return true;
        }

        public static void DisabledObjectDisplay<T>(T obj) where T : UnityEngine.Object
        {
            using (var scope = new EditorGUI.DisabledScope(true))
                EditorGUILayout.ObjectField(GUIContent.none, obj, typeof(T), false);
        }

        public static bool DrawList<T>(string title, IList<T> input, string buttonText, bool foldout, Action<T> customUI = null) where T : new()
        {
            if (!EditorGUILayout.Foldout(foldout, string.Format("{0} | Elements: {1}", title, input.Count), true))
                return false;

            Listing(input, customUI);
            if (GUILayout.Button(buttonText))
                input.Add(new T());
            return true;
        }

        private static void ReadOnlyListing<T>(IList<T> input, Action<T> customUI = null)
        {
            for (int i = 0; i < input.Count; i++)
            {
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    if (customUI == null)
                        GUILayout.Label(input[i].ToString());
                    else
                        customUI(input[i]);
                }
            }
        }

        public static bool DrawAssetList<T>(string title, IList<T> input, string dropText, bool foldout, Action<T> customUI = null) where T : ScriptableObject, new()
        {
            if (!EditorGUILayout.Foldout(foldout, string.Format("{0} | Elements: {1}", title, input.Count), true))
                return false;

            Listing(input, customUI);
            DragDropArea(input, dropText);

            return true;
        }

        private static void Listing<T>(IList<T> input, Action<T> customUI = null)
        {
            for (int i = 0; i < input.Count; i++)
            {
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    if (customUI == null)
                        GUILayout.Label(input[i].ToString());
                    else
                        customUI(input[i]);

                    GUILayout.FlexibleSpace();

                    using (var diabledScope = new EditorGUI.DisabledScope(i == 0))
                        if (GUILayout.Button("˄", GUILayout.Width(30)))
                            input.Swap(i, i - 1);

                    using (var diabledScope = new EditorGUI.DisabledScope(i == input.Count - 1))
                        if (GUILayout.Button("˅", GUILayout.Width(30)))
                            input.Swap(i, i + 1);

                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        input.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        internal static void DrawReadOnlyList()
        {
            throw new NotImplementedException();
        }

        private static void DragDropArea<T>(IList<T> input, string dropText) where T : ScriptableObject
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, dropText);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                        {
                            T newElement = DragAndDrop.objectReferences[i] as T;
                            if (newElement == null || input.Contains(newElement))
                                continue;
                            input.Add(newElement);
                        }
                    }
                    break;
            }
        }
    }
}

#endif
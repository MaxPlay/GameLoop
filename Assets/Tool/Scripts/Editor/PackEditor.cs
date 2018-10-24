using GameLoop.Data.Cards;
using GameLoop.Internal;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    [CustomEditor(typeof(Pack))]
    public class PackEditor : UnityEditor.Editor
    {
        #region Private Fields

        private bool cardsFoldout;
        private Pack pack;

        #endregion Private Fields

        #region Public Methods

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Pack", EditorStyles.boldLabel);
            pack.BackSprite = EditorGUILayout.ObjectField(pack.BackSprite, typeof(Sprite), false) as Sprite;
            pack.BackTexture = EditorGUILayout.ObjectField(pack.BackTexture, typeof(Texture2D), false) as Texture2D;
            cardsFoldout = CustomEditorUtil.DrawList("Cards", pack, "Add Cards", cardsFoldout, CardUI);

            EditorUtility.SetDirty(target);
        }

        #endregion Public Methods

        #region Private Methods

        private void CardUI(Card obj)
        {
            obj.Suit = (Suit)EditorGUILayout.EnumPopup(obj.Suit);
            obj.Face = (FaceValue)EditorGUILayout.EnumPopup(obj.Face);
            obj.Sprite = EditorGUILayout.ObjectField(obj.Sprite, typeof(Sprite), false) as Sprite;
            obj.Texture = EditorGUILayout.ObjectField(obj.Texture, typeof(Texture2D), false) as Texture2D;
        }

        private void OnDisable()
        {
            pack.MakeValid();
            EditorUtility.SetDirty(target);
        }

        private void OnEnable()
        {
            pack = target as Pack;
        }

        #endregion Private Methods
    }
}
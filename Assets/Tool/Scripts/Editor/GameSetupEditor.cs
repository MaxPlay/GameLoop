using GameLoop.Data.Setup;
using GameLoop.Internal;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor
{
    [CustomEditor(typeof(GameSetup))]
    public class GameSetupEditor : UnityEditor.Editor
    {
        #region Private Fields

        private bool foldPacks;
        private bool foldPlayers;
        private GameSetup gameSetup;

        #endregion Private Fields

        #region Public Methods

        public override void OnInspectorGUI()
        {
            foldPlayers = CustomEditorUtil.DrawAssetList("Players", gameSetup.Players, "Drop Players here", foldPlayers, CustomEditorUtil.DisabledObjectDisplay);
            if (gameSetup.Players != null)
                using (var scope = new EditorGUI.DisabledScope(gameSetup.Players.Count == 0))
                {
                    string[] playerNames = gameSetup.Players.Select(p => p.name).ToArray();
                    gameSetup.Dealer = EditorGUILayout.Popup("Dealer", gameSetup.Dealer, playerNames);
                }
            else
                using (var scope = new EditorGUI.DisabledScope())
                using (var hscope = new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Dealer");
                    GUILayout.Label("---", EditorStyles.popup);
                }
            foldPacks = CustomEditorUtil.DrawAssetList("Packs", gameSetup.Packs, "Drop Packs here", foldPacks, CustomEditorUtil.DisabledObjectDisplay);

            EditorUtility.SetDirty(target);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEnable()
        {
            gameSetup = target as GameSetup;
        }

        #endregion Private Methods
    }
}
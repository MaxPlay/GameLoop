using GameLoop.Data.CardCollections;
using GameLoop.Data.Cards;
using GameLoop.Internal;
using UnityEngine;

namespace GameLoop.Data.States
{
    public partial class GameState : ScriptableObject
    {
        public Player[] Players;
        public CardStack[] CardStacks;
        public Pack[] Packs;
        public int CurrentPlayer;
        public int CurrentDealer;
        public Suit Trump;
        public PlayDirection Direction;
        public BiddingPot Pot;
        public int CurrentModule;
    }

#if UNITY_EDITOR

    public partial class GameState : ScriptableObject, ICustomEditor
    {
        private bool playersFoldout;
        private bool cardStacksFoldout;
        private bool packsFoldout;

        public void CustomEditor()
        {
            playersFoldout = CustomEditorUtil.DrawReadOnlyList("Players", Players, playersFoldout, CustomEditorUtil.DisabledObjectDisplay);
            cardStacksFoldout = CustomEditorUtil.DrawReadOnlyList("Card Stacks", CardStacks, cardStacksFoldout, CustomEditorUtil.DisabledObjectDisplay);
            packsFoldout = CustomEditorUtil.DrawReadOnlyList("Packs", Packs, packsFoldout, CustomEditorUtil.DisabledObjectDisplay);
            UnityEditor.EditorGUILayout.IntField("Current Player", CurrentPlayer);
            UnityEditor.EditorGUILayout.IntField("Current Dealer", CurrentDealer);
            UnityEditor.EditorGUILayout.EnumPopup("Trump", Trump);
            UnityEditor.EditorGUILayout.EnumPopup("PlayDirection", Direction);
            Pot.CustomEditor();
        }
    }

#endif
}
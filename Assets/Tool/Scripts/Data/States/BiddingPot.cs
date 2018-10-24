using GameLoop.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLoop.Data.States
{
    [Serializable]
    public partial class BiddingPot
    {
        #region Private Fields

        [SerializeField]
        private List<Entry> entries;

        [SerializeField]
        private Player[] players;

        [SerializeField]
        private List<BiddingPot> sidePots;
        [SerializeField]
        private int amount;

        #endregion Private Fields

        #region Public Constructors

        public BiddingPot(Player[] players)
        {
            this.players = players;
            entries = new List<Entry>();
            for (int i = 0; i < players.Length; i++)
                entries.Add(new Entry() { ID = players[i].ID, Value = 0 });
            sidePots = new List<BiddingPot>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int Amount
        {
            get
            {
                return amount;
            }
        }

        public BiddingPot[] SidePots
        {
            get
            {
                return sidePots.ToArray();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddBid(Player player, int value)
        {
            int index = Array.IndexOf(players, player);
            if (index != -1)
            {
                entries[index].Value += value;
                UpdateAmount();
            }
        }

        public void AddSidePot(BiddingPot sidePot)
        {
            if (sidePots.Contains(sidePot))
                return;
            sidePots.Add(sidePot);
        }

        public void CloseSidePot(BiddingPot sidePot)
        {
            if (sidePots.Contains(sidePot))
                sidePots.Remove(sidePot);
        }

        public void GiveValue(Player player)
        {
            player.Credits += amount;
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < players.Length; i++)
                entries[i].Value = 0;
            amount = 0;
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateAmount()
        {
            int amount = 0;

            for (int i = 0; i < players.Length; i++)
                amount += entries[i].Value;

            this.amount = amount;
        }

        private class Entry
        {
            public string ID;
            public int Value;
        }

        #endregion Private Methods
    }

#if UNITY_EDITOR

    public partial class BiddingPot : ICustomEditor
    {
        private bool foldout;
        private bool sidePotFoldout;

        public void CustomEditor()
        {
            GUILayout.Label("Pots", UnityEditor.EditorStyles.boldLabel);
            using (var scope = new UnityEditor.EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(string.Format(" Amount: {0}", amount));
                if (GUILayout.Button("Update Amount"))
                    UpdateAmount();
                GUILayout.FlexibleSpace();
            }
            UnityEditor.EditorGUI.indentLevel++;
            foldout = CustomEditorUtil.DrawReadOnlyList("Entries", entries, foldout, DisplayEntry);
            if (sidePots.Count == 0)
                return;
            GUILayout.Label("Side Pots", UnityEditor.EditorStyles.boldLabel);
            sidePotFoldout = UnityEditor.EditorGUILayout.Foldout(sidePotFoldout, "Side Pots");
            if (sidePotFoldout)
            {
                UnityEditor.EditorGUI.indentLevel++;
                for (int i = 0; i < sidePots.Count; i++)
                    sidePots[i].CustomEditor();
                UnityEditor.EditorGUI.indentLevel--;
            }
            UnityEditor.EditorGUI.indentLevel--;
        }

        private void DisplayEntry(Entry entry)
        {
            CustomEditorUtil.DisabledObjectDisplay(Array.Find(players, f => f.ID == entry.ID));
            entry.Value = UnityEditor.EditorGUILayout.IntField(entry.Value);
        }
    }

#endif
}
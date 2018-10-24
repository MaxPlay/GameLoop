using GameLoop.Data.CardCollections;
using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Action
{
    public partial class DrawAction : ActionModule
    {
        public enum CardSelection
        {
            CardStack,
            FixedPlayer,
            CurrentPlayer,
            NextPlayer,
            PreviousPlayer,
            NextPlayerRelative,
            PreviousPlayerRelative
        }

        public CardSelection Reciever { get; set; }

        public CardSelection Origin { get; set; }

        public Player RecievingPlayer { get; set; }

        public CardStack RecievingCardStack { get; set; }

        public CardStack OriginCardStack { get; set; }

        public Player OriginPlayer { get; set; }

        public bool Random { get; set; }

        public int Count { get; set; }

        public override void Run()
        {
            CardCollection reciever = GetCardBySelection(Reciever, RecievingCardStack, RecievingPlayer);
            CardCollection origin = GetCardBySelection(Origin, OriginCardStack, OriginPlayer);

            for (int i = 0; i < Count; i++)
                reciever.LayOnTop(Random ? origin.DrawRandom() : origin.Draw());
        }

        private CardCollection GetCardBySelection(CardSelection selection, CardStack cardStack, Player player)
        {
            CardCollection collection = null;
            switch (selection)
            {
                case CardSelection.CardStack:
                    collection = OriginCardStack.Cards;
                    break;

                case CardSelection.FixedPlayer:
                    collection = OriginPlayer.Hand;
                    break;

                case CardSelection.CurrentPlayer:
                    collection = System.Data.CurrentPlayer.Hand;
                    break;

                case CardSelection.NextPlayer:
                    collection = System.Data.NextPlayer.Hand;
                    break;

                case CardSelection.PreviousPlayer:
                    collection = System.Data.PreviousPlayer.Hand;
                    break;

                case CardSelection.NextPlayerRelative:
                    {
                        int playerID = Array.IndexOf(System.Data.Players, OriginPlayer);
                        collection = System.Data.Players[System.Data.GetNextPlayer(playerID)].Hand;
                    }
                    break;

                case CardSelection.PreviousPlayerRelative:
                    {
                        int playerID = Array.IndexOf(System.Data.Players, OriginPlayer);
                        collection = System.Data.Players[System.Data.GetPreviousPlayer(playerID)].Hand;
                    }
                    break;
            }

            return collection;
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Reciever)))
            {
                CardSelection reciever;
                if (Enum.TryParse(reader.ReadInnerXml(), out reciever))
                    Reciever = reciever;
            }
            if (reader.IsStartElement(nameof(Origin)))
            {
                CardSelection origin;
                if (Enum.TryParse(reader.ReadInnerXml(), out origin))
                    Origin = origin;
            }
            if (reader.IsStartElement(nameof(RecievingPlayer)))
            {
                string id = reader.ReadInnerXml();
                RecievingPlayer = System.Data.ResolveID(id) as Player;
            }
            if (reader.IsStartElement(nameof(OriginCardStack)))
            {
                string id = reader.ReadInnerXml();
                OriginCardStack = System.Data.ResolveID(id) as CardStack;
            }
            if (reader.IsStartElement(nameof(OriginPlayer)))
            {
                string id = reader.ReadInnerXml();
                OriginPlayer = System.Data.ResolveID(id) as Player;
            }
            if (reader.IsStartElement(nameof(Random)))
                Random = reader.ReadInnerXml() == Boolean.TrueString;
            if (reader.IsStartElement(nameof(Count)))
            {
                int count;
                if (int.TryParse(reader.ReadInnerXml(), out count))
                    Count = count;
            }
        }

        protected override void SetDefaults()
        {
            Reciever = CardSelection.CurrentPlayer;
            Origin = CardSelection.CardStack;
            RecievingPlayer = null;
            OriginCardStack = null;
            OriginPlayer = null;
            Random = false;
            Count = 0;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Reciever), Reciever.ToString());
            writer.WriteElementString(nameof(Origin), Origin.ToString());
            if (RecievingPlayer != null) writer.WriteElementString(nameof(RecievingPlayer), RecievingPlayer.ID);
            if (OriginCardStack != null) writer.WriteElementString(nameof(OriginCardStack), OriginCardStack.ID);
            if (OriginPlayer != null) writer.WriteElementString(nameof(OriginPlayer), OriginPlayer.ID);
            writer.WriteElementString(nameof(Random), Random.ToString());
            writer.WriteElementString(nameof(Count), Count.ToString());
        }
    }

#if UNITY_EDITOR

    public partial class DrawAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("Player that recieves the cards", UnityEditor.EditorStyles.boldLabel);
            Reciever = (CardSelection)UnityEditor.EditorGUILayout.EnumPopup("Reciever", Reciever);

            using (var disabledScope = new UnityEditor.EditorGUI.DisabledScope(Reciever != CardSelection.FixedPlayer))
                RecievingPlayer = UnityEditor.EditorGUILayout.ObjectField("Player", RecievingPlayer, typeof(Player), false) as Player;
            using (var disabledScope = new UnityEditor.EditorGUI.DisabledScope(Reciever != CardSelection.CardStack))
                RecievingCardStack = UnityEditor.EditorGUILayout.ObjectField("Card Stack", RecievingCardStack, typeof(Player), false) as CardStack;

            GUILayout.Label("Origin of the cards", UnityEditor.EditorStyles.boldLabel);
            Origin = (CardSelection)UnityEditor.EditorGUILayout.EnumPopup("Origin", Origin);

            using (var disabledScope = new UnityEditor.EditorGUI.DisabledScope(Origin != CardSelection.FixedPlayer))
                OriginPlayer = UnityEditor.EditorGUILayout.ObjectField("Player", OriginPlayer, typeof(Player), false) as Player;
            using (var disabledScope = new UnityEditor.EditorGUI.DisabledScope(Origin != CardSelection.CardStack))
                OriginCardStack = UnityEditor.EditorGUILayout.ObjectField("Card Stack", OriginCardStack, typeof(Player), false) as CardStack;

            GUILayout.Label("Settings", UnityEditor.EditorStyles.boldLabel);
            Random = UnityEditor.EditorGUILayout.Toggle("Random Drawing", Random);
            Count = UnityEditor.EditorGUILayout.IntField("Amount of Cards", Count);
        }
    }

#endif
}
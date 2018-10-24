using GameLoop.Data.CardCollections;
using System;
using System.Xml;

namespace GameLoop.Modules.Action
{
    public enum CardCollectionSelection
    {
        FixedCardStack,
        FixedPlayer,
        CurrentPlayer,
        AllPlayers,
        AllStacks,
        AllCollections
    }

    [Serializable]
    public partial class ShuffleAction : ActionModule
    {
        public CardCollectionSelection Selection { get; set; }

        public CardStack CardStack { get; set; }

        public Player Player { get; set; }

        #region Public Constructors

        public ShuffleAction()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            switch (Selection)
            {
                case CardCollectionSelection.FixedCardStack:
                    CardStack.Cards.Shuffle();
                    break;

                case CardCollectionSelection.FixedPlayer:
                    Player.Hand.Shuffle();
                    break;

                case CardCollectionSelection.CurrentPlayer:
                    System.Data.CurrentPlayer.Hand.Shuffle();
                    break;

                case CardCollectionSelection.AllPlayers:
                    Array.ForEach(System.Data.Players, p => p.Hand.Shuffle());
                    break;

                case CardCollectionSelection.AllStacks:
                    System.Data.CardStacks.ForEach(p => p.Cards.Shuffle());
                    break;

                case CardCollectionSelection.AllCollections:
                    System.Data.CardStacks.ForEach(p => p.Cards.Shuffle());
                    Array.ForEach(System.Data.Players, p => p.Hand.Shuffle());
                    break;
            }
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Selection)))
            {
                CardCollectionSelection selection;
                if (Enum.TryParse(reader.ReadInnerXml(), out selection))
                    Selection = selection;
            }
            if (reader.IsStartElement(nameof(Player)))
            {
                string id = reader.ReadInnerXml();
                Player = System.Data.ResolveID(id) as Player;
            }
            if (reader.IsStartElement(nameof(CardStack)))
            {
                string id = reader.ReadInnerXml();
                CardStack = System.Data.ResolveID(id) as CardStack;
            }
        }

        protected override void SetDefaults()
        {
            Selection = CardCollectionSelection.FixedCardStack;
            Player = null;
            CardStack = null;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Selection), Selection.ToString());
            if (Player != null) writer.WriteElementString(nameof(Player), Player.ID);
            if (CardStack != null) writer.WriteElementString(nameof(CardStack), CardStack.ID);
        }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class ShuffleAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            Selection = (CardCollectionSelection)UnityEditor.EditorGUILayout.EnumPopup("Selection", Selection);

            using (var scope = new UnityEditor.EditorGUI.DisabledScope(Selection != CardCollectionSelection.FixedCardStack))
                CardStack = UnityEditor.EditorGUILayout.ObjectField("Card Stack", CardStack, typeof(CardStack), false) as CardStack;
            using (var scope = new UnityEditor.EditorGUI.DisabledScope(Selection != CardCollectionSelection.FixedPlayer))
                Player = UnityEditor.EditorGUILayout.ObjectField("Player", Player, typeof(Player), false) as Player;
        }
    }

#endif
}
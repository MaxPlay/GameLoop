using System;
using UnityEngine;

namespace GameLoop.Data.Cards
{
    [Serializable]
    public class Card : IEquatable<Card>
    {
        #region Private Fields

        [SerializeField]
        private FaceValue face;

        [SerializeField]
        private Pack pack;

        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        private Suit suit;

        [SerializeField]
        private Texture2D texture;

        #endregion Private Fields

        #region Public Properties

        public FaceValue Face
        {
            get { return face; }
            set { face = value; }
        }

        public Pack Pack
        {
            get
            {
                return pack;
            }

            set
            {
                pack = value;
            }
        }

        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Suit Suit
        {
            get { return suit; }
            set { suit = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public bool Equals(Card other)
        {
            return EqualValues(other) && other.pack == pack;
        }

        public bool EqualValues(Card other)
        {
            return other.suit == suit && other.face == face;
        }

        public bool IsTrump(DataStorage dataStorage)
        {
            return dataStorage.GameState.Trump == suit;
        }

        public override string ToString()
        {
            return string.Format("({0}|{1})", suit, face);
        }

        #endregion Public Methods
    }
}
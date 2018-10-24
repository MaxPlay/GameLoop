using GameLoop.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Data.Cards
{
    [CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.DATA_DIRECTORY + "Pack of Cards")]
    public class Pack : GameLoopScriptableObject, IList<Card>
    {
        #region Private Fields

        [SerializeField]
        private Sprite backSprite;

        [SerializeField]
        private Texture2D backTexture;

        [SerializeField]
        private List<Card> cards = new List<Card>();

        #endregion Private Fields

        #region Public Properties

        public Sprite BackSprite
        {
            get { return backSprite; }
            set { backSprite = value; }
        }

        public Texture2D BackTexture
        {
            get { return backTexture; }
            set { backTexture = value; }
        }

        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        public int Count { get { return cards.Count; } }

        public bool IsReadOnly { get { return false; } }

        #endregion Public Properties

        #region Public Indexers

        public Card this[int index]
        {
            get { return cards[index]; }
            set
            {
                cards[index].Pack = null;
                cards[index] = value;
                cards[index].Pack = this;
            }
        }

        #endregion Public Indexers

        #region Public Methods

        public void Add(Card item)
        {
            item.Pack = this;
            cards.Add(item);
        }

        public void Clear()
        {
            cards.ForEach(c => c.Pack = null);
            cards.Clear();
        }

        public bool Contains(Card item)
        {
            return cards.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            cards.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cards.GetEnumerator();
        }

        public int IndexOf(Card item)
        {
            return cards.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            item.Pack = this;
            cards.Insert(index, item);
        }

        public void MakeValid()
        {
            cards.ForEach(c => c.Pack = this);
        }

        public bool Remove(Card item)
        {
            if (item.Pack == this)
                item.Pack = null;
            return cards.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < cards.Count)
                cards[index].Pack = null;
            cards.RemoveAt(index);
        }

        #endregion Public Methods
    }
}
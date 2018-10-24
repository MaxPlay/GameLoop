using GameLoop.Data.Cards;
using GameLoop.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Data.CardCollections
{
    [Serializable]
    public class CardCollection : IEnumerable<Card>
    {
        #region Private Fields

        [SerializeField]
        private StackList cards = new StackList();

        #endregion Private Fields

        #region Public Delegates

        public delegate void CardCountChangedEventHandler(CardCollection sender, int change);

        #endregion Public Delegates

        #region Public Events

        public event CardCountChangedEventHandler CardCountChanged;

        #endregion Public Events

        #region Public Properties

        public int Count { get { return cards.Count; } }

        public DataStorage DataStorage { get; set; }

        #endregion Public Properties

        #region Public Indexers

        public Card this[int index] { get { return cards[index]; } }

        #endregion Public Indexers

        #region Public Methods

        public bool Contains(Card card)
        {
            return cards.Contains(card);
        }

        public void Cut(float percent)
        {
            Cut((int)(cards.Count * percent));
        }

        public void Cut(int index)
        {
            if (index < 0 || index >= cards.Count)
                return;

            for (int i = 0; i < index; i++)
            {
                Card card = cards[0];
                cards.RemoveAt(0);
                cards.Add(card);
            }
        }

        public Card Draw()
        {
            Card card = cards.Pop();
            OnCardCountChanged(-1);
            return card;
        }

        public bool Draw(Card card)
        {
            if (cards.Remove(card))
            {
                OnCardCountChanged(-1);
                return true;
            }
            return false;
        }

        public Card DrawRandom()
        {
            int targetIndex = UnityEngine.Random.Range(0, Count);
            Card card = cards[targetIndex];
            cards.RemoveAt(targetIndex);
            return card;
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Card>)cards).GetEnumerator();
        }

        public void Insert(int index, Card item)
        {
            cards.Insert(index, item);
            OnCardCountChanged(1);
        }

        public bool IsFace(FaceValue faceValue)
        {
            return IsPattern(card => card.Face == faceValue);
        }

        public bool IsPattern(Func<Card, bool> predicate)
        {
            if (cards.Count == 0)
                return false;

            for (int i = 0; i < cards.Count; i++)
            {
                if (!predicate(cards[i]))
                    return false;
            }
            return true;
        }

        public bool IsStraight()
        {
            if (cards.Count <= 1)
                return false;
            FaceValue[] values = (FaceValue[])Enum.GetValues(typeof(FaceValue));
            int currentIndex = Array.IndexOf(values, cards[0].Face);
            for (int i = 1; i < cards.Count; i++)
            {
                if (++currentIndex == values.Length)
                    currentIndex = 0;
                if (cards[i].Face != values[currentIndex])
                    return false;
            }
            return true;
        }

        public bool IsSuit(Suit suit)
        {
            return IsPattern(card => card.Suit == suit);
        }

        public void LayOnTop(Card item)
        {
            cards.Push(item);
            OnCardCountChanged(1);
        }

        public Card Peek()
        {
            return cards.Peek();
        }

        public void Shuffle()
        {
            cards.FisherYatesShuffle();
        }

        public void Sort()
        {
            cards.Sort();
        }

        public void Sort(Comparison<Card> comparison)
        {
            cards.Sort(comparison);
        }

        public void Sort(IComparer<Card> comparer)
        {
            cards.Sort(comparer);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Clear()
        {
            int amount = Count;
            cards.Clear();
            OnCardCountChanged(amount);
        }

        #endregion Internal Methods

        #region Private Methods

        private void OnCardCountChanged(int amount)
        {
            CardCountChanged?.Invoke(this, amount);
        }

        #endregion Private Methods
    }
}
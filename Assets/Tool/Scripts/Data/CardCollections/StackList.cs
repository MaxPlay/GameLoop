using GameLoop.Data.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Data.CardCollections
{
    [Serializable]
    public class StackList : IList<Card>, IStack<Card>
    {
        #region Private Fields

        [SerializeField]
        private List<Card> list;

        #endregion Private Fields

        #region Public Constructors

        public StackList()
        {
            list = new List<Card>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int Count { get { return list.Count; } }
        public bool IsReadOnly { get { return false; } }

        #endregion Public Properties

        #region Public Indexers

        public Card this[int index] { get { return list[index]; } set { list[index] = value; } }

        #endregion Public Indexers

        #region Public Methods

        public void Add(Card item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Card item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(Card item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            list.Insert(index, item);
        }

        public Card Peek()
        {
            if (Count == 0)
                return default(Card);
            return list[list.Count - 1];
        }

        public Card Pop()
        {
            if (Count == 0)
                return default(Card);
            var item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        public void Push(Card item)
        {
            list.Add(item);
        }

        public bool Remove(Card item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Sort()
        {
            list.Sort();
        }

        public void Sort(IComparer<Card> comparer)
        {
            list.Sort(comparer);
        }

        public void Sort(Comparison<Card> comparison)
        {
            list.Sort(comparison);
        }

        #endregion Public Methods
    }
}
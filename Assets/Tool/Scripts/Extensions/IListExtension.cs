using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Extensions
{
    public static class IListExtension
    {
        #region Public Methods

        public static int[] CreateIndexArray<T>(this IList<T> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = i;
            return array;
        }

        public static string[] CreateIndexStringArray<T>(this IList<T> list)
        {
            string[] array = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = i.ToString();
            return array;
        }

        public static void FisherYatesShuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int index = Random.Range(0, list.Count - i);
                int lastIndex = list.Count - i - 1;
                T value = list[index];
                list.Swap(index, lastIndex);
                list.RemoveAt(lastIndex);
                list.Add(value);
            }
        }

        public static T RemoveFirst<T>(this IList<T> list)
        {
            if (list.Count > 0)
            {
                T i = list[0];
                list.RemoveAt(0);
                return i;
            }

            return default(T);
        }

        public static T RemoveLast<T>(this IList<T> list)
        {
            if (list.Count > 0)
            {
                T i = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return i;
            }

            return default(T);
        }

        public static void Swap<T>(this IList<T> list, int num1, int num2)
        {
            T temp = list[num1];
            list[num1] = list[num2];
            list[num2] = temp;
        }

        #endregion Public Methods
    }
}
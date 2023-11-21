using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GrandDreams.Core.Utilities
{
    public static class ArrayExtensions
    {

        private static System.Random random = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static void AddNonExists<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void RemoveIfExists<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
            }
        }

        public static void AddRangeNonExists<T>(this List<T> list, T[] items)
        {
            for (int index = 0; index < items.Length; index++)
            {
                list.AddNonExists(items[index]);
            }
        }

        public static void AddRangeNonExists<T>(this List<T> list, List<T> items)
        {
            for (int index = 0; index < items.Count; index++)
            {
                list.AddNonExists(items[index]);
            }
        }

        public static void RemoveRange<T>(this List<T> list, T[] items)
        {
            for (int index = 0; index < items.Length; index++)
            {
                list.RemoveIfExists(items[index]);
            }
        }

        public static void RemoveRange<T>(this List<T> list, List<T> items)
        {
            for (int index = 0; index < items.Count; index++)
            {
                list.RemoveIfExists(items[index]);
            }
        }

        public static T GetRandomItem<T>(this T[] array)
        {
            return array[random.Next(0, array.Length)];
        }

        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[random.Next(0, list.Count)];
        }

        public static bool CompareTo<T>(this List<T> firstList, List<T> secondList)
        {
            if(firstList == null && secondList == null)
            {
                return true;
            }

            if (firstList == null && secondList != null)
            {
                return false;
            }

            if (firstList != null && secondList == null)
            {
                return false;
            }

            var firstNotSecond = firstList.Except(secondList);
            var secondNotFirst = secondList.Except(firstList);

            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }

        public static T GetOrDefault<T>(this T[] array, int idItem, T defaultValue = default(T))
        {
            return (array == null || array.Length == 0) ? defaultValue : (idItem >= array.Length ? defaultValue : array[idItem]);
        }

        public static T GetOrDefault<T>(this List<T> array, int idItem, T defaultValue = default(T))
        {
            return (array == null || array.Count == 0) ? defaultValue : (idItem >= array.Count ? defaultValue : array[idItem]);
        }

        public static T GetOrFirst<T>(this T[] array, int idItem, T defaultValue = default(T))
        {
            return (array == null || array.Length == 0) ? defaultValue : (idItem >= array.Length ? array[0] : array[idItem]);
        }

        public static T GetOrFirst<T>(this List<T> array, int idItem, T defaultValue = default(T))
        {
            return (array == null || array.Count == 0) ? defaultValue : (idItem >= array.Count ? array[0] : array[idItem]);
        }
    }
}
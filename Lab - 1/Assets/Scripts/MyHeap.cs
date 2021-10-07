using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MyHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private readonly Dictionary<int, T> index = new Dictionary<int, T>();
        private readonly Dictionary<T, int> items = new Dictionary<T, int>();

        public int Count => index.Count;

        public void Add(T item)
        {
            index[index.Count] = item;
            items[item] = items.Count;
            SortUp(item);
        }

        public bool Contains(T item)
        {
            return items.ContainsKey(item);
        }

        public T RemoveFirst()
        {
            if (!items.Any())
                throw new InvalidOperationException("Cannot remove first item. Heap is empty.");

            var firstItem = index[0];
            var lastItem = index[index.Count - 1];

            Swap(firstItem, lastItem);

            items.Remove(firstItem);
            index.Remove(index.Count - 1);

            if (items.Count > 1)
                SortDown(lastItem);

            return firstItem;
        }

        public void Update(T item)
        {
            SortUp(item);
        }

        private void SortUp(T item)
        {
            while (true)
            {
                int itemIndex = items[item];
                int parentIndex = (itemIndex - 1) / 2;

                T parentItem = index[parentIndex];

                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
            }
        }

        private void SortDown(T item)
        {
            while (true)
            {
                int count = items.Count;

                int itemIndex = items[item];

                int childIndexLeft = itemIndex * 2 + 1;
                int childIndexRight = itemIndex * 2 + 2;

                if (childIndexLeft >= count && childIndexRight >= count)
                    return;

                int swapIndex = childIndexRight < count
                    && index[childIndexLeft].CompareTo(index[childIndexRight]) < 0
                        ? childIndexRight
                        : childIndexLeft;

                if (item.CompareTo(index[swapIndex]) < 0)
                    Swap(item, index[swapIndex]);
                else
                    return;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            var indexA = items[itemA];
            var indexB = items[itemB];

            index[indexA] = itemB;
            index[indexB] = itemA;

            items[itemA] = indexB;
            items[itemB] = indexA;
        }
    }
}
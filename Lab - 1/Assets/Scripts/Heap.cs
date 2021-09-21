using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Heap<T> where T : IComparable<T>
    {
        HeapItem<T>[] items;
        int currentItemCount;
        public int Count => currentItemCount;
        public HeapItem<T>[] Items => items;

        public Heap(int maxHeapSize)
        {
            items = new HeapItem<T>[maxHeapSize];
        }

        public void Add(T item)
        {
            var heapItem = new HeapItem<T>(item, currentItemCount);
            items[currentItemCount] = heapItem;
            SortUp(heapItem);
            currentItemCount++;
        }

        public T RemoveFirst()
        {
            HeapItem<T> firstItem = items[0];
            currentItemCount--;

            items[0] = items[currentItemCount];
            items[0].Index = 0;
            
            SortDown(items[0]);
            return firstItem.Item;
        }

        public void UpdateItem(T item)
        {
            var heapItem = items.Single(hItem => ReferenceEquals(hItem.Item, item));
            SortUp(heapItem);
        }


        public bool Contains(T item)
        {
            return items.Any(heapItem => ReferenceEquals(heapItem, item));
        }

        void SortDown(HeapItem<T> heapItem)
        {
            while (true)
            {
                int childIndexLeft = heapItem.Index * 2 + 1;
                int childIndexRight = heapItem.Index * 2 + 2;

                if (childIndexLeft >= currentItemCount && childIndexRight >= currentItemCount)
                    return;

                int swapIndex = childIndexRight < currentItemCount
                    && items[childIndexLeft].CompareTo(items[childIndexRight]) < 0
                        ? childIndexRight
                        : childIndexLeft;

                if (heapItem.CompareTo(items[swapIndex]) < 0)
                    Swap(heapItem, items[swapIndex]);
                else
                    return;
            }
        }

        private void SortUp(HeapItem<T> item)
        {
            while (true)
            {
                int parentIndex = (item.Index - 1) / 2;
                HeapItem<T> parentItem = items[parentIndex];

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

        void Swap(HeapItem<T> itemA, HeapItem<T> itemB)
        {
            items[itemA.Index] = itemB;
            items[itemB.Index] = itemA;
            int itemAIndex = itemA.Index;
            itemA.Index = itemB.Index;
            itemB.Index = itemAIndex;
        }
    }
}

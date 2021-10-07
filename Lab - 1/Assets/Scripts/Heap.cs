using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Heap<T> : IHeap<T> where T : IComparable<T>
    {
        HeapItem<T>[] items;
        int count;
        public int Count => count;
        public HeapItem<T>[] Items => items;

        public Heap(int maxHeapSize)
        {
            items = new HeapItem<T>[maxHeapSize];
            count = 0;
        }

        public void Add(T item)
        {
            var heapItem = new HeapItem<T>(item, count);

            items[count] = heapItem;
            SortUp(heapItem);
            count++;
        }

        public T RemoveFirst()
        {
            HeapItem<T> firstItem = items[0];
            HeapItem<T> lastItem = items[count - 1];

            lastItem.Index = 0;
            items[0] = lastItem;

            items[count - 1] = null;

            count--;
            
            SortDown(lastItem);
            return firstItem.Item;
        }

        public void Update(T item)
        {
            var heapItem = items.Where(hItem => hItem != null).Last(hItem => hItem.Item.Equals(item));
            SortUp(heapItem);
        }

        public bool Contains(T item)
        {
            return items.Where(hItem => hItem != null).Any(hItem => hItem.Item.Equals(item));
        }

        private void SortDown(HeapItem<T> heapItem)
        {
            while (true)
            {
                int childIndexLeft = heapItem.Index * 2 + 1;
                int childIndexRight = heapItem.Index * 2 + 2;

                if (childIndexLeft >= count && childIndexRight >= count)
                    return;

                int swapIndex = childIndexRight < count
                    && items[childIndexLeft].CompareTo(items[childIndexRight]) < 0
                        ? childIndexRight
                        : childIndexLeft;

                if (heapItem.CompareTo(items[swapIndex]) < 0)
                    Swap(heapItem, items[swapIndex]);
                else
                {
                    return;
                }
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

        private void Swap(HeapItem<T> itemA, HeapItem<T> itemB)
        {
            items[itemA.Index] = itemB;
            items[itemB.Index] = itemA;
            int itemAIndex = itemA.Index;
            itemA.Index = itemB.Index;
            itemB.Index = itemAIndex;
        }
    }
}

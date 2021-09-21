using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class HeapItem<T>: IComparable<HeapItem<T>> where T : IComparable<T>
    {
        public HeapItem(T item, int index)
        {
            Item = item;
            Index = index;
        }

        public int Index { get; set; }
        public T Item { get; }

        public int CompareTo(HeapItem<T> other)
        {
            return this.Item.CompareTo(other.Item);
        }
    }
}
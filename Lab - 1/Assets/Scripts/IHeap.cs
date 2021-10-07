using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IHeap<T> where T : IComparable<T>
    {
        int Count { get; }
        void Add(T item);
        T RemoveFirst();
        void Update(T item);
        bool Contains(T item);
    }
}
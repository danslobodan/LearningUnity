using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HeapTests
    {
        [Test]
        public void Contains_Returns_False_When_Heap_Is_Empty()
        {
            var heap = GetHeap<int>();
            Assert.False(heap.Contains(20));
        }

        [Test]
        public void Contains_Returns_True_When_Element_Is_Present()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            Assert.True(heap.Contains(10));
        }

        [Test]
        public void Contains_Returns_False_When_Element_Is_Not_Present()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            heap.Add(30);
            Assert.False(heap.Contains(20));
        }

        [Test]
        public void Add_Adds_Element_To_Heap()
        {
            var heap = GetHeap<int>();
            heap.Add(20);
            Assert.AreEqual(1, heap.Count);
            Assert.True(heap.Contains(20));
        }

        [Test]
        public void Add_Adds_Three_Elements_To_Heap()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            heap.Add(20);
            heap.Add(30);
            Assert.AreEqual(3, heap.Count);
            Assert.True(heap.Contains(10));
            Assert.True(heap.Contains(20));
            Assert.True(heap.Contains(30));
        }

        [Test]
        public void RemoveFirst_Throws_When_Heap_Is_Empty()
        {
            var heap = GetHeap<int>();
            Assert.Throws<InvalidOperationException>(() => heap.RemoveFirst());
        }

        [Test]
        public void RemoveFirst_Returns_First_Element()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            var element = heap.RemoveFirst();
            Assert.AreEqual(10, element);
        }

        [Test]
        public void RemoveFirst_Removes_First_Element()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            heap.RemoveFirst();
            Assert.False(heap.Contains(10));
        }

        [Test]
        public void RemoveFirst_Returns_20_When_Added_10_And_Then_Added_20()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            heap.Add(20);
            var item = heap.RemoveFirst();
            Assert.AreEqual(20, item);
        }

        [Test]
        public void RemoveFirst_Returns_20_When_Added_20_And_Then_Added_10()
        {
            var heap = GetHeap<int>();
            heap.Add(20);
            heap.Add(10);
            var item = heap.RemoveFirst();
            Assert.AreEqual(20, item);
        }

        [Test]
        public void RemoveFirst_Returns_Correct_Element_When_Called_The_Second_Time_After_Add()
        {
            var heap = GetHeap<int>();
            heap.Add(10);
            heap.Add(20);
            heap.Add(30);
            heap.Add(40);
            heap.RemoveFirst();
            var item = heap.RemoveFirst();
            Assert.AreEqual(30, item);
        }


        [Test]
        public void RemoveFirst_Returns_Correct_Element_When_Heap_Contains_Objects()
        {
            var heap = GetHeap<ReverseComparisonInt>();
            var expected = new ReverseComparisonInt(5);
            heap.Add(expected);
            heap.Add(new ReverseComparisonInt(15));
            heap.Add(new ReverseComparisonInt(25));

            var actual = heap.RemoveFirst();
            Assert.AreEqual(expected, actual);
        }

        private class ReverseComparisonInt : IComparable<ReverseComparisonInt>
        {
            private int value;

            public ReverseComparisonInt(int value)
            {
                this.value = value;
            }

            public int HeapIndex { get; set; }

            public int CompareTo(ReverseComparisonInt other)
            {
                if (value < other.value)
                    return 1;
                else if (value > other.value)
                    return -1;
                else
                    return 0;
            }
        }

        private IHeap<T> GetHeap<T>() where T : IComparable<T>
        {
            return new Heap<T>();
        }
    }
}

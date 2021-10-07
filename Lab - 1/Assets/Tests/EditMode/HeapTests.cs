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
            var Heap = GetHeap<int>();
            Assert.False(Heap.Contains(20));
        }

        [Test]
        public void Contains_Returns_True_When_Element_Is_Present()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Assert.True(Heap.Contains(10));
        }

        [Test]
        public void Contains_Returns_False_When_Element_Is_Not_Present()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Heap.Add(30);
            Assert.False(Heap.Contains(20));
        }

        [Test]
        public void Add_Adds_Element_To_Heap()
        {
            var Heap = GetHeap<int>();
            Heap.Add(20);
            Assert.AreEqual(1, Heap.Count);
            Assert.True(Heap.Contains(20));
        }

        [Test]
        public void Add_Adds_Three_Elements_To_Heap()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Heap.Add(20);
            Heap.Add(30);
            Assert.AreEqual(3, Heap.Count);
            Assert.True(Heap.Contains(10));
            Assert.True(Heap.Contains(20));
            Assert.True(Heap.Contains(30));
        }

        [Test]
        public void RemoveFirst_Returns_First_Element()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            var element = Heap.RemoveFirst();
            Assert.AreEqual(10, element);
        }

        [Test]
        public void RemoveFirst_Removes_First_Element()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Heap.RemoveFirst();
            Assert.False(Heap.Contains(10));
        }

        [Test]
        public void RemoveFirst_Returns_20_When_Added_10_And_Then_Added_20()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Heap.Add(20);
            var item = Heap.RemoveFirst();
            Assert.AreEqual(20, item);
        }

        [Test]
        public void RemoveFirst_Returns_20_When_Added_20_And_Then_Added_10()
        {
            var Heap = GetHeap<int>();
            Heap.Add(20);
            Heap.Add(10);
            var item = Heap.RemoveFirst();
            Assert.AreEqual(20, item);
        }

        [Test]
        public void RemoveFirst_Returns_Correct_Element_When_Called_The_Second_Time_After_Add()
        {
            var Heap = GetHeap<int>();
            Heap.Add(10);
            Heap.Add(20);
            Heap.Add(30);
            Heap.Add(40);
            Heap.RemoveFirst();
            var item = Heap.RemoveFirst();
            Assert.AreEqual(30, item);
        }

        public void RemoveFirst_Returns_Correct_Element_When_Heap_Contains_Objects()
        {
            var Heap = GetHeap<TestItem>();
            Heap.Add(new TestItem(5));
            Heap.Add(new TestItem(15));

            var expected = new TestItem(25);
            Heap.Add(expected);
            var actual = Heap.RemoveFirst();
            Assert.AreEqual(expected, actual);
        }

        private class TestItem : IComparable<TestItem>, IEquatable<TestItem>
        {
            private int value;

            public TestItem(int value)
            {
                this.value = value;
            }

            public int CompareTo(TestItem other)
            {
                if (value < other.value)
                    return -1;
                else if (value > other.value)
                    return 1;
                else
                    return 0;
            }

            public bool Equals(TestItem other)
            {
                return value.Equals(other.value);
            }
        }

        private IHeap<T> GetHeap<T>() where T : IComparable<T>
        {
            // return new Heap<T>(100);
            return new MyHeap<T>();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HeapMonitor : MonoBehaviour
    {
        public Heap<Node> heap;

        private ICollection<HeapItem<Node>[]> history;
        private int currentIndex;

        public Text heapText;

        private void Awake()
        {
            history = new List<HeapItem<Node>[]>();
            currentIndex = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Previous();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Next();
            }
        }

        private void Next()
        {
            if (currentIndex >= history.Count - 1)
                return;

            currentIndex++;
            UpdateDisplay();
        }

        private void Previous()
        {
            if (currentIndex <= 0)
                return;

            currentIndex--;
            UpdateDisplay();
        }

        private static readonly ICollection<int> endIndexes = new List<int> { 0, 2, 6, 15, 31, 63 };

        public void UpdateHeap(Heap<Node> heap)
        {
            this.heap = heap;
            history.Add(heap.Items.ToArray());
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            string heapString = string.Join(string.Empty, history.ElementAt(currentIndex)
                .Where(item => item != null)
                .Select(item =>
            {
                return endIndexes.Contains(item.Index)
                    ? $"{item.Item}{Environment.NewLine}"
                    : $"{item.Item} | ";
            }));
            heapText.text = $"{currentIndex}{Environment.NewLine}{heapString}";
        }
    }
}
﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CombinationPuzzle.DataStructures
{
     /// <summary>
    /// Heap-based resizable max-priority queue.
    /// Elements with high priority are served before elements with low priority.
    /// Priority is defined by comparing elements, so to separate priority from value use
    /// KeyValuePair or a custom class and provide corresponding Comparer.
    /// </summary>
    /// <typeparam name="T">Any comparable type, either through a specified Comparer or implementing IComparable&lt;<typeparamref name="T"/>&gt;</typeparam>
    public class PriorityQueue<T> : ICollection<T> where T: class
    {
        private readonly IComparer<T> _comparer;
        internal T[] Heap;
        private const int PhaseDefaultCapacity = 10;
        private const int PhaseShrinkRatio = 4;
        private const int PhaseResizeFactor = 2;

        private int _shrinkBound;

        // ReSharper disable once StaticFieldInGenericType
        private static readonly InvalidOperationException EmptyCollectionException = new InvalidOperationException("Collection is empty.");

        /// <summary>
        /// Create a max-priority queue with default capacity of 10.
        /// </summary>
        /// <param name="comparer">Custom comparer to compare elements. If omitted - default will be used.</param>
        public PriorityQueue(IComparer<T>? comparer = null) : this(PhaseDefaultCapacity, comparer) { }

        /// <summary>
        /// Create a max-priority queue with provided capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity</param>
        /// <param name="comparer">Custom comparer to compare elements. If omitted - default will be used.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws <see cref="ArgumentOutOfRangeException"/> when capacity is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throws <see cref="ArgumentException"/> when comparer is null and <typeparamref name="T"/> does not implement IComparable.</exception>
        public PriorityQueue(int capacity, IComparer<T>? comparer = null)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Expected capacity greater than zero.");

            // If no comparer then T must be comparable
            if (comparer == null &&
                !(typeof(IComparable).IsAssignableFrom(typeof(T)) ||
                  typeof(IComparable<T>).IsAssignableFrom(typeof(T))))
            {
                throw new ArgumentException("Expected a comparer for types, which do not implement IComparable.", nameof(comparer));
            }

            _comparer = comparer ?? Comparer<T>.Default;
            _shrinkBound = capacity / PhaseShrinkRatio;
            Heap = new T[capacity];
        }

        /// <summary>
        /// Current queue capacity
        /// </summary>
        public int Capacity => Heap.Length;

        public virtual IEnumerator<T> GetEnumerator()
        {
            var array = new T[Count];
            CopyTo(array, 0);
            return ((IEnumerable <T>)array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T item)
        {
            if (Count == Capacity) GrowCapacity();

            Heap[Count++] = item;
            // provide the index of the last item as for 1-based heap, but also set shift to -1
            Heap.Sift(Count, _comparer, shift: -1);      // move item "up" until heap principles are not met
        }

        /// <summary>
        /// Removes and returns a max element from the priority queue.
        /// </summary>
        /// <returns>Max element in the collection</returns>
        /// <exception cref="InvalidOperationException">Throws <see cref="InvalidOperationException"/> when queue is empty.</exception>
        public virtual T Take()
        {
            if (Count == 0) throw EmptyCollectionException;

            var item = Heap[0];
            Count--;
            Heap.Swap(0, Count);              // last element at count
            Heap[Count] = null!;         // release hold on the object
            // provide index of first item as for 1-based heap, but also set shift to -1
            Heap.Sink(1, Count, _comparer, shift: -1);   // move item "down" while heap principles are not met

            if (Count <= _shrinkBound && Count > PhaseDefaultCapacity)
            {
                ShrinkCapacity();
            }

            return item;
        }

        /// <summary>
        /// Returns a max element from the priority queue without removing it.
        /// </summary>
        /// <returns>Max element in the collection</returns>
        /// <exception cref="InvalidOperationException">Throws <see cref="InvalidOperationException"/> when queue is empty.</exception>
        public virtual T Peek()
        {
            if (Count == 0) throw EmptyCollectionException;
            return Heap[0];
        }

        public virtual void Clear()
        {
            Heap = new T[PhaseDefaultCapacity];
            Count = 0;
        }

        public virtual bool Contains(T item) => GetItemIndex(item) >= 0;

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Insufficient space in destination array.");

            Array.Copy(Heap, 0, array, arrayIndex, Count);

            array.HeapSort(arrayIndex, Count, _comparer);
        }

        public virtual bool Remove(T item)
        {
            var index = GetItemIndex(item);
            switch (index)
            {
                case -1:
                    return false;
                case 0:
                    Take();
                    break;
                default:
                    // provide a 1-based index of the item
                    RemoveAt(index + 1, shift: -1);
                    break;
            }

            return true;
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        /// <summary>
        /// Removes item at given index
        /// </summary>
        /// <param name="index">1-based index of the element to remove</param>
        /// <param name="shift">Shift allows to compensate and work with arrays where heap starts not from the element at position 1.
        /// Shift -1 allows to work with 0-based heap as if it was 1-based. But the main reason for this is the CopyTo method.</param>
        protected internal void RemoveAt(int index, int shift)
        {
            var itemIndex = index + shift;
            Count--;
            Heap.Swap(itemIndex, Count);       // last element at Count
            Heap[Count] = null!;          // release hold on the object
            // use a 1-based-heap index and then apply shift of -1
            var parent = index / 2 + shift;     // get parent
            // if new item at index is greater than it's parent then sift it up, else sink it down
            if (_comparer.GreaterOrEqual(Heap[itemIndex], Heap[parent]))
            {
                // provide a 1-based-heap index
                Heap.Sift(index, _comparer, shift);
            }
            else
            {
                // provide a 1-based-heap index
                Heap.Sink(index, Count, _comparer, shift);
            }
        }

        /// <summary>
        /// Returns the real index of the first occurrence of the given item or -1.
        /// </summary>
        protected internal int GetItemIndex(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_comparer.Compare(Heap[i], item) == 0) return i;
            }
            return -1;
        }


        private void GrowCapacity()
        {
            int newCapacity = Capacity * PhaseResizeFactor;
            Array.Resize(ref Heap, newCapacity);  // first element is at position 1
            _shrinkBound = newCapacity / PhaseShrinkRatio;
        }

        private void ShrinkCapacity()
        {
            int newCapacity = Capacity / PhaseResizeFactor;
            Array.Resize(ref Heap, newCapacity);  // first element is at position 1
            _shrinkBound = newCapacity / PhaseShrinkRatio;
        }
    }

    internal static class HeapMethods
    {
        internal static void Swap<T>(this T[] array, int i, int j)
        {
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }

        internal static bool GreaterOrEqual<T>(this IComparer<T> comparer, T x, T y) => comparer.Compare(x, y) >= 0;

        /// <summary>
        /// Moves the item with given index "down" the heap while heap principles are not met.
        /// </summary>
        /// <typeparam name="T">Any comparable type</typeparam>
        /// <param name="heap">Array, containing the heap</param>
        /// <param name="i">1-based index of the element to sink</param>
        /// <param name="count">Number of items in the heap</param>
        /// <param name="comparer">Comparer to compare the items</param>
        /// <param name="shift">Shift allows to compensate and work with arrays where heap starts not from the element at position 1.
        /// Shift -1 allows to work with 0-based heap as if it was 1-based. But the main reason for this is the CopyTo method.
        /// </param>
        internal static void Sink<T>(this T[] heap, int i, int count, IComparer<T> comparer, int shift)
        {
            var lastIndex = count + shift;
            while (true)
            {
                var itemIndex = i + shift;
                var leftIndex = 2 * i + shift;
                if (leftIndex > lastIndex) {
                    return;      // reached last item
                }

                var rightIndex = leftIndex + 1;
                var hasRight = rightIndex <= lastIndex;

                var item = heap[itemIndex];
                var left = heap[leftIndex];
                var right = hasRight ? heap[rightIndex] : default;

                // if item is greater than children - heap is fine, exit
                if (GreaterOrEqual(comparer, item, left) && (!hasRight || GreaterOrEqual(comparer, item, right!)))
                {
                    return;
                }

                // else exchange with greater of children
                var greaterChildIndex = !hasRight || GreaterOrEqual(comparer, left, right!) ? leftIndex : rightIndex;
                heap.Swap(itemIndex, greaterChildIndex);

                // continue at new position
                i = greaterChildIndex - shift;
            }
        }

        /// <summary>
        /// Moves the item with given index "up" the heap while heap principles are not met.
        /// </summary>
        /// <typeparam name="T">Any comparable type</typeparam>
        /// <param name="heap">Array, containing the heap</param>
        /// <param name="i">1-based index of the element to sink</param>
        /// <param name="comparer">Comparer to compare the items</param>
        /// <param name="shift">Shift allows to compensate and work with arrays where heap starts not from the element at position 1.
        /// Value -1 allows to work with 0-based heap as if it was 1-based. But the main reason for this is the CopyTo method.
        /// </param>
        internal static void Sift<T>(this T[] heap, int i, IComparer<T> comparer, int shift)
        {
            while (true)
            {
                if (i <= 1) return;           // reached root
                var parent = i / 2 + shift;   // get parent
                var index = i + shift;

                // if root is greater or equal - exit
                if (GreaterOrEqual(comparer, heap[parent], heap[index]))
                {
                    return;
                }

                heap.Swap(parent, index);
                i = parent - shift;
            }
        }

        /// <summary>
        /// Sorts the heap in descending order.
        /// </summary>
        /// <typeparam name="T">Any comparable type</typeparam>
        /// <param name="heap">Array, containing the heap</param>
        /// <param name="startIndex">Index in the array, from which the heap structure begins</param>
        /// <param name="count">Number of items in the heap</param>
        /// <param name="comparer">Comparer to compare the items</param>
        internal static void HeapSort<T>(this T[] heap, int startIndex, int count, IComparer<T> comparer)
        {
            var shift = startIndex - 1;
            var lastIndex = startIndex + count;
            var left = count;

            // take the max item and exchange it with the last one
            // decrement the count of items in the heap and sink the first item to restore the heap rules
            // repeat for every element in the heap
            while (lastIndex > startIndex)
            {
                lastIndex--;
                left--;
                heap.Swap(startIndex, lastIndex);
                heap.Sink(1, left, comparer, shift);
            }
            // when done items will be sorted in ascending order, but this is a Max-PriorityQueue, so reverse
            Array.Reverse(heap, startIndex, count);
        }
    }
}

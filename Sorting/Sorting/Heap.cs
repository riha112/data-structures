using System;

namespace Sorting
{
    /// <summary>
    /// Min Heap
    /// 
    /// Why array based approach and not object
    /// - Heap items are added sequentially, and are based on binary trees
    /// - example: 
    ///     -- tree_form:
    ///             4
    ///            / \
    ///           6   8
    ///          / \ 
    ///         7   X
    ///         
    ///     -- array_form: [4,6,8,7]
    ///     --- If we add item to this heap it will go to the last empty spot
    ///     --- If we add 9 it will go to X (in "tree_form") and "array_form" will look like: [4,6,8,7,9]
    ///     --- It's simple to access parent and children:
    ///         ---- parent: (childID - 1) / 2
    ///         ---- children:
    ///                 ----- left: 2 * parentID + 1
    ///                 ----- right: 2 * parentID + 2
    /// </summary>
    /// <typeparam name="T">Comparable array item type</typeparam>
    class Heap<T> where T : IComparable
    {
        #region Variables
        public int Length { get; private set; } = 0;
        private int Size = 10;

        private T[] Data;
        #endregion

        #region Helper functions

        private static int GetLeftChildIndex(int perentIndex) => 2 * perentIndex + 1;
        private static int GetRightChildIndex(int perentIndex) => 2 * perentIndex + 2;
        private static int GetParentIndex(int childIndex) => (childIndex - 1) / 2;


        private bool HasLeftChild(int index) => GetLeftChildIndex(index) < Length;
        private bool HasRightChild(int index) => GetRightChildIndex(index) < Length;
        private static bool HasParent(int index) => GetParentIndex(index) >= 0;

        private T Parent(int index) => Data[GetParentIndex(index)];
        private T LeftChild(int index) => Data[GetLeftChildIndex(index)];
        private T RightChild(int index) => Data[GetRightChildIndex(index)];

        public Heap()
        {
            Data = new T[Size];
        }

        private void Swap(int indexOne, int indexTwo)
        {
            T temp = Data[indexOne];
            Data[indexOne] = Data[indexTwo];
            Data[indexTwo] = temp;
        }

        #endregion


        /// <summary>
        /// Increases data array capacity
        /// </summary>
        private void EnsureExtraCapacity()
        {
            if (Length == Size)
            {
                T[] tmpItems = new T[Size * 2];
                Array.Copy(Data, tmpItems, Size);
                Size *= 2;
                Data = tmpItems;
            }
        }

        /// <summary>
        /// Returns first heap item.
        /// </summary>
        /// <returns>First item</returns>
        public T Peek()
        {
            if (Length == 0)
                throw new Exception("Empty heap");
            return Data[0];
        }

        /// <summary>
        /// Removes and returns first item from heap
        /// 
        /// Algorithm description:
        /// 1. Removes first item
        /// 2. Makes last item - first, by moving to array position 0.
        /// 3. To ensure that first item is smallest in array do HeapifyDown:
        ///     3.1. Checks if child value is smaller then parents
        ///     3.2. If parent is larger swap(child,parent)
        ///     3.3. GOTO 3.1.
        /// </summary>
        /// <returns>First item</returns>
        public T Poll()
        {
            if (Length == 0)
                throw new Exception("Empty heap");

            // Retrieves min value of heap (AKA the return value)
            T item = Data[0]; 

            // Moves last item to firsts location
            Data[0] = Data[Length - 1];

            // Shrinks heaps size, because we removed first item
            Length--;

            // Moves "last item" (new first item) to correct location
            HeapifyDown();

            return item;
        }

        public void HeapifyDown()
        {
            // When first item is removed, the last item is moved into 
            // firsts place, so:
            int index = 0;

            // If list doesn't have "left" child, then there is no "right" child either (lookup binary tree and heap adding order);
            // So this while loop means: <while element has any children>, 
            // (because otherwise it's at the end of heap AKA "tree leaf").
            while (HasLeftChild(index))
            {
                // Finds the child with smallest value
                int smallerChildIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && RightChild(index).CompareTo(LeftChild(index)) < 0)
                    smallerChildIndex = GetRightChildIndex(index);
                // --- end of smallest value ---

                // If children have larger values then item -> item is at correct location 
                if (Data[index].CompareTo(Data[smallerChildIndex]) < 0)
                    break;

                // If childern is smaller than item - swap them.
                Swap(index, smallerChildIndex);

                // When item was swapt with its child
                // it inherited its childs index, so logically:
                index = smallerChildIndex;
            }
        }

        /// <summary>
        /// Adds item to heap
        /// 
        /// Algorithm description:
        /// 1. Adds new item to end of array
        /// 2. Compares if item parent has larger value
        /// 3. If has swaps(parent, new item)
        /// 4. GOTO 2
        /// </summary>
        /// <param name="item">Item to add to heap</param>
        public void Add(T item)
        {
            // Array based list addition
            EnsureExtraCapacity();
            Data[Length] = item;    // <- Puts new item at the end of list
            Length++;
            // -- end of array based list addition --

            // Corrects position  of newly added item
            HeapifyUp();
        }

        public void HeapifyUp()
        {
            // Starting index of new item will always be:
            int index = Length - 1;

            // Swap newly added item and its parent until parents value is smaller
            // then newly added items value.
            while(HasParent(index) && Parent(index).CompareTo(Data[index]) > 0)
            {
                Swap(GetParentIndex(index), index);
                // When newly added item was swapt with its parent
                // it inherited its parents index, so logically:
                index = GetParentIndex(index);
            }
        }
    }

}

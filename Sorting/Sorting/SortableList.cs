using System;

namespace Sorting
{
    /*
    * Array based list with basic functionality
    *   - Get, Add, Pop, PopFirst;
    * and sorting:
    *   - InsertionSort, ShellSort, SelectionSort, HeapSort, MergeSort
    *   - QuickSort, BucketSort
    */

    /// <summary>
    /// Array based list with sorting capability.
    /// </summary>
    /// <typeparam name="T">Comparable array item type</typeparam>
    class SortableList<T> where T : IComparable
    {
        #region Variables

        /// <summary>
        /// Currently allocated space of the array
        /// </summary>
        public int Length { get; private set; } = 0;

        /// <summary>
        /// Real size of the array
        /// </summary>
        private int Size { get; set; } = 10;

        /// <summary>
        /// Value holder
        /// </summary>
        private T[] Data { get; set; }

        #endregion


        public SortableList()
        {
            Data = new T[Size];
        }

        public SortableList(T[] arr)
        {
            Data = arr;
            Length = arr.Length;
            if (Length >= 10)
                Size = (int)Math.Ceiling(Length / 10.0f) * 10;
        }

        #region Basic operations: Get, Add, Pop, PopFist, First, Swap

        /// <summary>
        /// Operaotr [], Get, At
        /// </summary>
        /// <param name="ID">Value location in array</param>
        /// <returns></returns>
        public T this[int ID]
        {
            get => Data[ID];
            set => Data[ID] = value;
        }

        /// <summary>
        /// Adds new value at the end of array
        /// </summary>
        /// <param name="value">Value to be added</param>
        public void Add(T value)
        {
            CheckArraySize();
            Data[Length++] = value;
        }

        public void Add(T[] arr, int from, int to)
        {
            // Bad way
            for (int i = from; i < to && i < arr.Length; i++)
                Add(arr[i]);
        }

        /// <summary>
        /// If array is full -> increases size two times
        /// </summary>
        private void CheckArraySize()
        {
            if (Length >= Size)
            {
                T[] tmp = new T[Size * 2];
                Array.Copy(Data, tmp, Size);
                Data = tmp;
                Size *= 2;
            }
        }

        /// <summary>
        /// Retrieves & removes last item from array
        /// </summary>
        /// <returns>Last item from array with type T</returns>
        public T Pop() => Data[Length--];

        /// <summary>
        /// Returns first item from array
        /// </summary>
        /// <returns>First item from array</returns>
        public T First() => Data[0];

        /// <summary>
        /// Retrieves & removes first item from array
        /// </summary>
        /// <returns>First item from array</returns>
        public T PopFirst()
        {
            // Bad way - possible solution - new variable offset first
            T result = Data[0];
            for (int i = 1; i < Length; i++)
                Data[i - 1] = Data[i];

            Length--;
            return result;
        }

        /// <summary>
        /// Swaps two elements
        /// </summary>
        /// <param name="a">ID of first element</param>
        /// <param name="b">ID of second element</param>
        private void Swap(int a, int b)
        {
            T tmp = Data[a];
            Data[a] = Data[b];
            Data[b] = tmp;
        }

        /// <summary>
        /// Removes all elements
        /// </summary>
        private void Clear()
        {
            Size = 10;
            Length = 0;
            Data = new T[Size];
        }

        #endregion

        #region Sorting

        public void InsertionSortWhile()
        {
            int i = 1;
            while(i < Length)
            {
                int j = i;
                while(j > 0 && Data[j-1].CompareTo(Data[j]) > 0)
                {
                    Swap(j, j - 1);
                    j--;
                }
                i++;
            }
        }

        public void InsertionSortFor()
        {
            for(int i = 1; i < Length; i++)
            {
                int j = i;
                T x = Data[i];
                while(j >= 1 && Data[j-1].CompareTo(x) > 0)
                {
                    Data[j] = Data[j - 1];
                    j = j - 1;
                }
                Data[j] = x;
            }
        }

        /// <summary>
        /// Algorithm description:
        /// - Insertion sort but with gaps that gets smaller with each iteration
        /// 1. Starts with largest gap
        /// 2. Compares each n*gap element
        /// 3. Shortens gap
        /// 4. GOTO 2.
        /// </summary>
        public void ShellSort()
        {
            int[] gaps = new int[] { 701, 301, 132, 57, 23, 10, 4, 1 };

            foreach(int gap in gaps)
            {
                for(int i = gap; i < Length; i++)
                {
                    T tmp = Data[i];
                    int j;
                    for(j = i; j >= gap && Data[j - gap].CompareTo(tmp) > 0; j -= gap)
                    {
                        Data[j] = Data[j - gap];
                    }
                    Data[j] = tmp;
                }
            }
        }

        /// <summary>
        /// Algorithm description:
        /// 1. Finds smallest element in array
        /// 2. Places at beginning
        /// 3. GOTO 1
        /// </summary>
        public void SelectionSort()
        {
            // Loops throug out array except last item,
            // becouse logicly speaking if all elements except last one
            // are at correct location then last item as well is correctly placed
            for(int i = 0; i < Length - 1; i++)
            {
                // Sets minimum element as current element
                int jMin = i;

                // Finds smalles element by compearing right side of array
                // with current element
                for(int j = i+1; j < Length; j++)
                {
                    if(Data[j].CompareTo(Data[jMin]) < 0)
                        jMin = j;
                }

                // If smallest element differs => changes them
                if (jMin != i)
                    Swap(i, jMin);
            }
        }

        /// <summary>
        /// Algorithm description:
        /// 1. Creates heap
        /// 2. Adds all items from array into heap
        /// 3. Pools items from heap and adds them to initial array
        /// </summary>
        public void HeapSort()
        {
            // 1. Creates heap
            Heap<T> heap = new Heap<T>();

            // 2. Puts all items into heap
            for (int i = 0; i < Length; i++)
                heap.Add(Data[i]);

            // 3. Pulls all items from heap and ads them back to array
            for (int i = 0; i < Length; i++)
                Data[i] = heap.Poll();
        }

        #region MergeSort

        /// <summary>
        /// Algorithm description:
        /// 1. Divide the unsorted list into n sublists, each containing one element
        /// 2. Repeatedly merges sublists to produce new sorted sublist until only one remains
        /// </summary>
        public void MergeSort()
        {
            Data = GetMergeSort(this).Data;
        }

        /// <summary>
        /// Sorts list using MergeSort algorithm
        /// </summary>
        /// <param name="M">List to sort</param>
        /// <returns>Sorted list</returns>
        private SortableList<T> GetMergeSort(SortableList<T> M)
        {
            // If list contains of one element -> sorted
            if (M.Length <= 1)
                return M;

            // Splits list into two half's
            SortableList<T> left = new SortableList<T>();
            SortableList<T> right = new SortableList<T>();

            for(int i = 0; i < M.Length; i++)
            {
                if (i < M.Length / 2)
                    left.Add(M[i]);
                else
                    right.Add(M[i]);
            }
            //---end of spliting into two half's-----

            // Recursively splits both halves  until each half contains one element
            left = GetMergeSort(left);
            right = GetMergeSort(right);

            // Merges two halves together - sorts two halves 
            return Merge(left, right);
        }

        /// <summary>
        /// Merges two list halves into sorted list
        /// </summary>
        /// <param name="left">List to merge A</param>
        /// <param name="right">List to merge B</param>
        /// <returns>Merged sorted list</returns>
        private SortableList<T> Merge(SortableList<T> left, SortableList<T> right)
        {
            SortableList<T> result = new SortableList<T>();

            // Sort elements in correct order until one of halves is empty
            while (left.Length > 0 && right.Length > 0)
            {
                if (left.First().CompareTo(right.First()) <= 0)
                    result.Add(left.PopFirst());
                else
                    result.Add(right.PopFirst());
            }

            // Eather left or right half may still have elements left; Add them.
            while (left.Length > 0)
                result.Add(left.PopFirst());

            while (right.Length > 0)
                result.Add(right.PopFirst());

            return result;
        }

        #endregion

        #region Quicksort
        /// <summary>
        /// Algorithm description:
        /// 1. Pick an element, called pivot from the array
        /// 2. Partitioning: 
        ///     - reorder the array so that all elements with value
        ///     - less than the pivot come before pivot, while all
        ///     - elements with values greater than the pivot comes
        ///     - after it
        /// 3. GOTO 1 with both halves of array before and after pivot.
        /// </summary>
        /// <param name="low">start sorting from</param>
        /// <param name="high">end sorting with</param>
        public void QuickSort(int low, int high)
        {
            if(low < high)
            {
                // Gets pivot point (sorted element point)
                int p = Partition(low, high);

                // Sorts rest of elements, by splitting array into two
                // halves - left of pivot point & right of pivot point
                QuickSort(low, p - 1);
                QuickSort(p + 1, high);
            }
        }

        /// <summary>
        /// Moves pivot point to correct location
        /// </summary>
        /// <param name="low">range start</param>
        /// <param name="high">range end</param>
        /// <returns>Pivot element location</returns>
        private int Partition(int low, int high)
        {
            // Selects pivot as the lest elemnt in array
            T pivot = Data[high];

            // Selects pivot point as first element location
            int i = low;

            // --- start of moving elements to the correct location ---
            // Moves elements smaller than pivot to the left side
            // and larger elements to the right side
            for(int j = low; j < high; j++)
                if(Data[j].CompareTo(pivot) < 0)
                {
                    Swap(i, j);
                    i++;
                }

            // After moving items to left and right side
            // we need to move pivot element between them, because
            // right now it's still at the end... but we have stored
            // the middle point into variable "i", which will be the
            // new pivot element location so that all elements with
            // lesser value are left of pivot point and elements with
            // greater value are right of pivot point.
            Swap(i, high);
            // --- end of moving elements to the correct location ---

            return i;
        }
        #endregion

        /// <summary>
        /// Algorithm description:
        /// 1. Set up array of initially empty "buckets"
        /// 2. Scatter: Go over the original array, putting each object/value in its bucket.
        /// 3. Sort each non-empty bucket
        /// 4. Gather: Visit the buckets in order and put all elements back into the original array
        /// 
        /// Example:
        /// 
        /// Initial array: [24, 22, 1, 45, 6, 33, 21, 44]
        /// 
        /// Buckets - (distribution of 10):
        ///     b1 = [0-9];     // Contains elements with value in range 0 to 9 including
        ///     b2 = [10-19];
        ///     b3 = [20-29];
        ///     b4 = [30-39];
        ///     b5 = [40-49];
        /// 
        /// Add elements into buckets:
        ///     b1 = [1,6]
        ///     b2 = []
        ///     b3 = [24, 22, 21]
        ///     b4 = [33]
        ///     b5 = [45, 44]
        /// 
        /// Sort each bucket separately:
        ///     b1 = [1,6]
        ///     b2 = []
        ///     b3 = [21,22,24]
        ///     b4 = [33]
        ///     b5 = [44, 45]
        /// 
        /// Output/Sorted array: b1 + b2 + b3 + b4 + b5 = [1,6,21,22,24,33,44,45]
        /// 
        /// 
        /// <param name="n">Bucket count</param>
        /// </summary>
        public void BucketSort(int n)
        {
            // 1. Creates n empty buckets
            SortableList<T>[] buckets = new SortableList<T>[n];
            for (int i = 0; i < n; i++)
                buckets[i] = new SortableList<T>();

            // 1.1 Finds largest value in array
            // in C++ you can use sizeof(arr) / sizeof(arr[0]) as base M
            dynamic M = Data[0];
            for (int i = 0; i < Length; i++)
                if (Data[i] > M)
                    M = Data[i];
            M++;

            // 2. Puts array elements into buckets
            for(int i = 0; i < Length; i++)
            {
                dynamic value = Data[i];
                int bID = (int)Math.Floor(n * value / (decimal)M);
                buckets[bID].Add(Data[i]);
            }

            // Clears current list values
            Clear();

            // 3. Sorts each bucket & adds its values to current list
            for (int i = 0; i < n; i++)
            {
                //buckets[i].SelectionSort();
                buckets[i].QuickSort(0, buckets[i].Length - 1);
                
                Add(buckets[i].Data, 0, buckets[i].Length);
            }
        }
        #endregion

        #region RadixSort
        /// <summary>
        /// Sorts by n'th places of number:
        /// Example:
        ///     arr: [22,54,1,44,10,4,1,34,801]
        ///     
        ///     1st itteration - sorts 1st place:
        ///     1(0), (1), (1), 80(1), 2(2), 5(4), 4(4), 3(4) 
        ///     
        ///     2nd itteration - sorts tens:
        ///     (0)1, (0)1, 8(0)1, (1)0, (2)2, (3)4, (4)4, (5)4
        ///     
        ///     3rd itteration - sorts hundreds
        ///     (0)01, (0)01, (0)10, (0)22, (0)34, (0)44, (0)54, (8)01
        ///     
        ///     Sorted arrey output:
        ///     [1,1,10,22,34,44,54,801]
        /// </summary>

        public void RadixSort()
        {
            // Finds max value - to know digit count
            dynamic maxValue = Data[0];
            for (int i = 1; i < Length; i++)
                if (Data[i].CompareTo(maxValue) > 0)
                    maxValue = Data[i];
            
            // Example:
            // - maxValue: 932
            // - exp: 1, 10, 100

            for(int exp = 1; maxValue / exp > 0; exp *= 10)
                RadixCountingSort(exp);
        }

        // A bit modified counting sort
        // Where (value/exponent)%10 is key(x)
        // And count represents digits: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        // More info: https://en.wikipedia.org/wiki/Counting_sort
        private void RadixCountingSort(int exponent)
        {
            int i;
            dynamic value;

            // Count of digits 0-9
            int[] count = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Counts digit occurences in array
            for (i = 0; i < Length; i++)
            {
                value = Data[i];
                count[(value / exponent) % 10]++;
            }

            for (i = 1; i < 10; i++)
                count[i] += count[i - 1];

            // Builds output array
            T[] output = new T[Length];
            for (i = Length - 1; i >= 0; i--)
            {
                value = Data[i];
                output[count[(value / exponent) % 10] - 1] = value;
                count[(value / exponent) % 10]--;
            }

            // Copies data to array
            for (i = 0; i < Length; i++)
                Data[i] = output[i];
        }
        #endregion

        /// <summary>
        /// Uses binary search algorithm to find element.
        /// </summary>
        /// <param name="searchable">Target element</param>
        /// <returns>On success returns ID of element, of fail returns -1</returns>
        public int BinarySearch(T searchable)
        {
            var range = (min: 0, max: Length - 1);
            T curr;
            
            while (range.max >= range.min)
            {
                var mid = (range.max + range.min) / 2;
                curr = Data[mid];
                
                if (curr.CompareTo(searchable) == 0)
                    return mid;

                if (curr.CompareTo(searchable) > 0)
                    range.max = mid - 1;
                else
                    range.min = mid + 1;
            }            
            
            return -1;
        }
        
        /// <summary>
        /// Merges two same type sorted list
        /// </summary>
        /// <param name="A">Sorted list A</param>
        /// <param name="B">Sorted list B</param>
        /// <returns>Merged sorted list</returns>
        public static SortableList<T> MergeSort(SortableList<T> A, SortableList<T> B)
        {
            SortableList<T> joined = new SortableList<T>();
            (var a, var b) = (0, 0);

            while (a < A.Length && b < B.Length)
            {
                if(A[a].CompareTo(B[b]) < 0)
                    joined.Add(A[a++]);
                else
                    joined.Add(B[b++]);
            }

            while (a < A.Length) joined.Add(A[a++]);
            while (b < B.Length) joined.Add(B[b++]);

            return joined;
        }
        
    }
}

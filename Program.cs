using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Projekt3
{

    struct ArrayStruct
    {
        public string ArrayName { get; }

        public int[] Array { get; }

        public ArrayStruct(string name, int[] array)
        {
            ArrayName = name;
            Array = array;
        }
        
    }
    
    class Arrays 
    {
        private int[] _constant;
        private int[] _randomized;
        private int[] _increasing;
        private int[] _decreasing;
        private int[] _vshaped;
        
        private List<ArrayStruct> _arrays = new List<ArrayStruct>();
        
        private int _maxValue;
        private int _quantity;
        
        private Random _rnd = new Random(Guid.NewGuid().GetHashCode());

        private void GenerateArrayList()
        {
            ArrayStruct constant = new ArrayStruct("constant", _constant);
            ArrayStruct randomized = new ArrayStruct("randomized", _randomized);
            ArrayStruct increasing = new ArrayStruct("increasing", _increasing);
            ArrayStruct decreasing = new ArrayStruct("decreasing", _decreasing);
            ArrayStruct vshaped = new ArrayStruct("vshaped", _vshaped);
            
            _arrays.Add(constant);
            _arrays.Add(randomized);
            _arrays.Add(increasing);
            _arrays.Add(decreasing);
            _arrays.Add(vshaped);
        }

        public List<ArrayStruct> GetArrayList()
        {
            GenerateArrayList();
            return _arrays;
        }

        public Arrays(int maxValue, int quantity)
        {
            this._maxValue = maxValue;
            this._quantity = quantity;

            _constant = new int[quantity];
            _randomized = new int[quantity];
            _increasing = new int[quantity];
            _decreasing = new int[quantity];
            _vshaped = new int[quantity];

            Console.WriteLine("Max value: " + _maxValue);
            Console.WriteLine("Quantity: " + _quantity);

            GenerateConstantArray();
            GenerateRandomArray();
            GenerateIncreasingArray();
            GenerateDecreasingArray();
            GenerateVShapedArray();
        }

        private void GenerateVShapedArray()
        {
            int[] workArray = new int[_quantity];
            Array.Copy(_decreasing, workArray, _quantity);
            

            int counter = 0;
            for (int i = 0; i < workArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    _vshaped[counter] = workArray[i];
                }
                else
                {
                    _vshaped[workArray.Length - counter - 1] = workArray[i];
                    counter++;
                }
            }
        }
        
        private void GenerateConstantArray()
        {
            int constantValue = _rnd.Next(_maxValue);
            for (int i = 0; i < _quantity; i++)
            {
                _constant[i] = constantValue;
            }
        }
        
        private void GenerateRandomArray()
        {
            for (int i = 0; i < _quantity; i++)
            {
                this._randomized[i] = _rnd.Next(_maxValue);
            }
        }

        private void GenerateIncreasingArray()
        {
            Array.Copy(_randomized, _increasing, _quantity);
            Array.Sort(this._increasing);
        }

        private void GenerateDecreasingArray()
        {
            Array.Copy(_increasing, _decreasing, _quantity);
            Array.Reverse(_decreasing);
        }
    }

    class ArraySort
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _time;
        private bool _error;
        private string _errorMessage;
        
        public long Time => _time;
        public bool Error => _error; 
        public string ErrorMessage => _errorMessage;

        public ArraySort(string sortMethod, int[] t)
        {
            switch (sortMethod)
            {
                case "InsertionSort":
                    InsertionSort(t);
                    break;
                case "SelectionSort":
                    SelectionSort(t);
                    break;
                case "HeapSort":
                    HeapSort(t);
                    break;
                case "CocktailSort":
                    CocktailSort(t);
                    break;
                default:
                    _error = true;
                    SetErrorMessage("Entered sort method [" + sortMethod + "] doesn't exists");
                    break;
            }
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
        }

        private void SetErrorMessage(string message)
        {
            _errorMessage = message;
        }
        
        // SORT METHODS: 
        
        private void SelectionSort(int[] t)
        {
            _stopwatch.Start();
            uint k;
            for (uint i = 0; i < (t.Length - 1); i++)
            {
                int Buf = t[i]; // bierzemy i-ty element
                k = i; // i jego indeks
                for (uint j = i + 1; j < t.Length; j++)
                    if (t[j] < Buf) // szukamy najmniejszego z prawej
                    {
                        k = j;
                        Buf = t[j];
                    }
                t[k] = t[i]; // zamieniamy i-ty z k-tym
                t[i] = Buf;
            }
        } /* SelectionSort() */
        
        private void InsertionSort (int[] t)
        {
            _stopwatch.Start();
            for (uint i = 1; i < t.Length; i++)
            {
                uint j = i; // elementy 0 .. i-1 są już posortowane
                int Buf = t[j]; // bierzemy i-ty (j-ty) element
                while ((j > 0) && (t[j - 1] > Buf))
                { // przesuwamy elementy
                    t[j] = t[j - 1];
                    j--;
                }
                t[j] = Buf; // i wpisujemy na docelowe miejsce
            }
        } /* InsertionSort() */
        
        private void HeapSort(int[] t)
        {
            _stopwatch.Start();
            uint left = ((uint)t.Length / 2),
                right = (uint)t.Length - 1;
            while (left > 0) // budujemy kopiec idąc od połowy tablicy
            {
                left--;
                Heapify(t, left, right);
            }
            while (right > 0) // rozbieramy kopiec
            {
                int buf = t[left];
                t[left] = t[right];
                t[right] = buf; // największy element
                right--; // kopiec jest mniejszy
                Heapify(t, left, right); // ale trzeba go naprawić
            }
        } /* HeapSort() */
        
        private void Heapify(int[] t, uint left, uint right)
        { // procedura budowania/naprawiania kopca
            uint i = left,
                j = 2 * i + 1;
            int buf = t[i]; // ojciec
            while(j <= right) // przesiewamy do dna stogu
            {
                if(j < right) // wybieramy większego syna
                    if(t[j] < t[j+1])j++;
                if(buf >= t[j])break;
                t[i] = t[j];
                i = j;
                j = 2 * i + 1; // przechodzimy do dzieci syna
            }
            t[i] = buf;
        } /* Heapify() */
        
        private void CocktailSort(int[] t)
        {
            _stopwatch.Start();
            int Left = 1, Right = t.Length - 1, k = t.Length-1;
            do
            {
                for(int j = Right; j >= Left; j--) // przesiewanie od dołu
                    if(t[j - 1] > t[j])
                    { int Buf=t[j-1]; t[j-1]=t[j]; t[j]=Buf;
                        k = j; // zamiana elementów i zapamiętanie indeksu
                    }
                Left = k + 1; // zacieśnienie lewej granicy
                for(int j = Left; j <= Right; j++) // przesiewanie od góry
                    if(t[j - 1] > t[j])
                    { int Buf=t[j-1]; t[j-1]=t[j]; t[j]=Buf;
                        k = j; // zamiana elementów i zapamiętanie indeksu
                    }
                Right = k - 1; // zacieśnienie prawej granicy
            }
            while(Left <= Right);
        } /* CocktailSort() */
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] methods = { "InsertionSort", "SelectionSort", "HeapSort", "CocktailSort" };
            int maxValue = 1000000;
            int quantity = 100000;
            
            Arrays arrays = new Arrays(maxValue, quantity);

            foreach (var item in arrays.GetArrayList())
            {
                foreach (string sortMethod in methods)
                {
                    int[] array = new int[quantity];
                    Array.Copy(item.Array, array, quantity);
                    ArraySort sort = new ArraySort(sortMethod, array);
                    if(sort.Error)
                        Console.WriteLine(sort.ErrorMessage);
                    else
                        Console.WriteLine("Time needed to sort " + item.ArrayName + " array, with method " + sortMethod + " : " + sort.Time);
                }

                Console.WriteLine("\n =----------------------------------------=");
            }
        }
    }
}
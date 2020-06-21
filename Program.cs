using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
        private int[] _ashaped;
        
        private List<ArrayStruct> _arrays = new List<ArrayStruct>();
        private int _maxValue;
        private int _quantity;
        private Random _rnd = new Random(Guid.NewGuid().GetHashCode());
        private void GenerateArrayList(string[] arrays)
        {
            foreach (string arrayName in arrays)
            {
                switch (arrayName)
                {
                    case "constant": 
                        ArrayStruct constant = new ArrayStruct("constant", _constant);
                        _arrays.Add(constant);
                        break;
                    case "randomized":
                        ArrayStruct randomized = new ArrayStruct("randomized", _randomized);
                        _arrays.Add(randomized);
                        break;
                    case "increasing":
                        ArrayStruct increasing = new ArrayStruct("increasing", _increasing);
                        _arrays.Add(increasing);
                        break;
                    case "decreasing":
                        ArrayStruct decreasing = new ArrayStruct("decreasing", _decreasing);
                        _arrays.Add(decreasing);
                        break;
                    case "vshaped":
                        ArrayStruct vshaped = new ArrayStruct("vshaped", _vshaped);
                        _arrays.Add(vshaped);
                        break;
                    case "ashaped":
                        ArrayStruct ashaped = new ArrayStruct("ashaped", _vshaped);
                        _arrays.Add(ashaped);
                        break;
                    default:
                        Console.WriteLine("THERE IS NO SUCH ARRAY!");
                        break;
                }
            }
        }
        public List<ArrayStruct> GetArrayList(string[] arrays)
        {
            GenerateArrayList(arrays);
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
            _ashaped = new int[quantity];

            Console.WriteLine("Max value: " + _maxValue);
            Console.WriteLine("Quantity: " + _quantity);

            GenerateConstantArray();
            GenerateRandomArray();
            GenerateIncreasingArray();
            GenerateDecreasingArray();
            GenerateVShapedArray();
            GenerateAShapedArray();
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
        private void GenerateAShapedArray()
        {
            int[] workArray = new int[_quantity];
            Array.Copy(_increasing, workArray, _quantity);
            int counter = 0;
            for (int i = 0; i < workArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    _ashaped[counter] = workArray[i];
                }
                else
                {
                    _ashaped[workArray.Length - counter - 1] = workArray[i];
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
        private readonly Random _rnd = new Random();
        private long _time = 0;
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
                case "QuickSortIterative":
                    QuickSortIterative(t);
                    break;
                case "QuickSortRecursive":
                    _stopwatch.Start();
                    QuickSortRecursive(t, 0, t.Length-1);
                    _time = _stopwatch.ElapsedMilliseconds;
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    break;
                case "QuickSortRecursiveFromLeft":
                    _stopwatch.Start();
                    QuickSortRecursiveFromLeft(t, 0, t.Length - 1);
                    _time = _stopwatch.ElapsedMilliseconds;
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    break;
                case "QuickSortRecursiveFromRight":
                    _stopwatch.Start();
                    QuickSortRecursiveFromRight(t, 0, t.Length - 1);
                    _time = _stopwatch.ElapsedMilliseconds;
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    break;
                case "QuickSortRecursiveFromRandom":
                    _stopwatch.Start();
                    QuickSortRecursiveFromRandom(t, 0, t.Length - 1);
                    _time = _stopwatch.ElapsedMilliseconds;
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    break;
                default:
                    _error = true;
                    SetErrorMessage("Entered sort method [" + sortMethod + "] doesn't exists");
                    break;
            }
        }
        private void SetErrorMessage(string message)
        {
            _errorMessage = message;
        }
        // SORT METHODS: 
        // QUICK SORT RECURSIVE
        private void QuickSortRecursive(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[(l+p)/2]; // (pseudo)mediana
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) QuickSortRecursive(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) QuickSortRecursive(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */
        // QUICK SORT RECURSIVE FROM LEFT
        private void QuickSortRecursiveFromLeft(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[l]; // (pseudo)mediana
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) QuickSortRecursiveFromLeft(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) QuickSortRecursiveFromLeft(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */        
        
        // QUICK SORT RECURSIVE FROM RIGHT
        private void QuickSortRecursiveFromRight(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[p]; // (pseudo)mediana
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) QuickSortRecursiveFromRight(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) QuickSortRecursiveFromRight(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */
        
        
        // QUICK SORT RECURSIVE FROM RANDOM
        private void QuickSortRecursiveFromRandom(int[] t, int l, int p)
        {
            
            int i, j, x;
            i = l;
            j = p;
            x = t[_rnd.Next(l, p)]; // (pseudo)mediana
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) QuickSortRecursiveFromRight(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) QuickSortRecursiveFromRight(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */
        
        // QUICK SORT ITERATIVE
        private void QuickSortIterative(int[] t)
        { 
            _stopwatch.Start();

            int i, j, l, p, sp;
            int[] stos_l = new int[t.Length],
                stos_p = new int[t.Length]; // przechowywanie żądań podziału
            sp=0; stos_l[sp] = 0; stos_p[sp] = t.Length - 1; // rozpoczynamy od całej tablicy
            do
            {
                l=stos_l[sp]; p=stos_p[sp]; sp--; // pobieramy żądanie podziału
                do
                { 
                    int x;
                    i=l; j=p; x=t[(l+p)/2]; // analogicznie do wersji rekurencyjnej
                    do
                    {
                        while (t[i] < x) i++;
                        while (x < t[j]) j--;
                        if (i <= j)
                        {
                            int buf = t[i]; t[i] = t[j]; t[j] = buf;
                            i++; j--;
                        }
                    } while (i <= j);
                    if(i<p) { sp++; stos_l[sp]=i; stos_p[sp]=p; } // ewentualnie dodajemy żądanie podziału
                    p=j;
                } while(l<p);
            } while(sp>=0); // dopóki stos żądań nie będzie pusty
            _stopwatch.Stop();
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
        } /* qsort() */
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
            _stopwatch.Stop();
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
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
            _stopwatch.Stop();
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
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
            _stopwatch.Stop();
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
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
            _stopwatch.Stop();
            _time = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
        } /* CocktailSort() */
    }

    internal class Program
    {
        private bool _p;
        private static Random _random = new Random();
        
        private static void GetResults(string[] methods, string[] arrs, int maxValue, int quantity)
        {
            do
            {
                Console.WriteLine("POSORTOWANE TABLICE DLA " + quantity + " elementów: ");
                Arrays arrays = new Arrays(maxValue, quantity);

                foreach (var item in arrays.GetArrayList(arrs))
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
                quantity += 10000;
            } while (quantity <= 200000);
        }
        private static void Task3()
        {
            string[] methods = { "QuickSortRecursive", "QuickSortRecursiveFromRight", "QuickSortRecursiveFromRandom" };
            string[] arrs = {"ashaped"};
            
            int maxValue = 1000000;
            int quantity = 50000;
            GetResults(methods, arrs, maxValue, quantity);
        }
        private static void Task2()
        {
            string[] methods = { "QuickSortIterative", "QuickSortRecursive" };
            string[] arrs = {"randomized"};
            
            int maxValue = 1000000;
            int quantity = 50000;

            GetResults(methods, arrs, maxValue, quantity);
        }
        private static void Task1()
        {
            string[] methods = { "InsertionSort", "SelectionSort", "HeapSort", "CocktailSort" };
            string[] arrs = {"constant", "randomized", "increasing", "decreasing", "vshaped"};
            
            int maxValue = 1000000;
            int quantity = 50000;

            GetResults(methods, arrs, maxValue, quantity);
        }
        public static void Main(string[] args)
        {
            string choice;
            ShowMenu();
            bool exitController = false;
            do
            {
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("");
                        exitController = true;
                        break;
                    case "1":
                        Task1();
                        break;
                    case "2":
                        Task2();
                        break;
                    case "3":
                        Thread TesterThread = new Thread(Program.Task3, 8 * 8192 * 8192); // utworzenie wątku
                        TesterThread.Start(); // uruchomienie wątku
                        TesterThread.Join(); // oczekiwanie na zakończenie wątku
                        break;
                    default:
                        Console.WriteLine("Nie ma takiej opcji wyboru, sprobuj ponownie");
                        break;
                }
            } while (exitController == false);
        }
        public static void ShowMenu()
        {
            Console.WriteLine("Wyjdz z programu - wybierz 0");
            Console.WriteLine("Zadanie 1 - wybierz 1");
            Console.WriteLine("Zadanie 2 - wybierz 2");
            Console.WriteLine("Zadanie 3 - wybierz 3");
        }
    }
}
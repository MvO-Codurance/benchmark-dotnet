using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace SampleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Sorting
{
    private static readonly MySorter Sorter = new MySorter();

    private readonly int[] _valuesToSort = { 5, 1, 4, 2, 8, 3, 12, 3, 8, 9, 12, 1, 5, 7, 11, 9, 6 };

    [Benchmark(Baseline = true)]
    public int[] BubbleSortBasic() => Sorter.BubbleSortBasic(_valuesToSort);
    
    [Benchmark]
    public int[] BubbleSortOptimised1() => Sorter.BubbleSortOptimised1(_valuesToSort);
    
    [Benchmark]
    public int[] BubbleSortOptimised2() => Sorter.BubbleSortOptimised2(_valuesToSort);
    
    [Benchmark]
    public int[] InsertionSortBasic() => Sorter.InsertionSortBasic(_valuesToSort);
    
    [Benchmark]
    public int[] InsertionSortRecursive() => Sorter.InsertionSortRecursive(_valuesToSort);
    
    [Benchmark]
    public int[] QuickSort() => Sorter.QuickSort(_valuesToSort);
    
    [Benchmark]
    public int[] BuiltinArraySort()
    {
        var values = _valuesToSort.ToArray();
        Array.Sort(values);
        return values;
    }
}

public class MySorter
{
    public int[] BubbleSortBasic(int[] valuesToSort)
    {
        var values = valuesToSort.ToArray();
        var n = values.Length;
        var swapped = false;
        do
        {
            swapped = false;
            for (int i = 1; i <= n - 1; i++)
            {
                if (values[i - 1] > values[i])
                {
                    // ReSharper disable once SwapViaDeconstruction
                    var temp = values[i - 1];
                    values[i - 1] = values[i];
                    values[i] = temp;
                    swapped = true;
                }
            }
        } while (swapped);

        return values;
    }
    
    public int[] BubbleSortOptimised1(int[] valuesToSort)
    {
        var values = valuesToSort.ToArray();
        var n = values.Length;
        var swapped = false;
        do
        {
            swapped = false;
            for (int i = 1; i <= n - 1; i++)
            {
                if (values[i - 1] > values[i])
                {
                    // ReSharper disable once SwapViaDeconstruction
                    var temp = values[i - 1];
                    values[i - 1] = values[i];
                    values[i] = temp;
                    swapped = true;
                }
            }
            
            // decrement n as we have already placed the n-th item in its final position 
            n--;
            
        } while (swapped);

        return values;
    }
    
    public int[] BubbleSortOptimised2(int[] valuesToSort)
    {
        var values = valuesToSort.ToArray();
        var n = values.Length;
        do
        {
            var newN = 0;
            for (int i = 1; i <= n - 1; i++)
            {
                if (values[i - 1] > values[i])
                {
                    // ReSharper disable once SwapViaDeconstruction
                    var temp = values[i - 1];
                    values[i - 1] = values[i];
                    values[i] = temp;
                    newN = i;
                }
            }
             
            n = newN;
            
        } while (n > 1);

        return values;
    }
    
    public int[] InsertionSortBasic(int[] valuesToSort)
    {
        var values = valuesToSort.ToArray();
		
        var i = 1;
        while (i < values.Length)
        {
            var x = values[i];
            var j = i - 1;
            while (j >= 0 && values[j] > x)
            {
                values[j+1] = values[j];
                j--;
            }
            values[j+1] = x;
            i++;
        }

        return values;
    }
    
    public int[] InsertionSortRecursive(int[] valuesToSort, int? n = null)
    {
        var values = valuesToSort;
        if (!n.HasValue)
        {
            values = valuesToSort.ToArray();
            n = values.Length - 1;
        }

        if (n > 0)
        {
            InsertionSortRecursive(values, n-1);
            var x = values[n.Value];
            var j = n.Value - 1;
            while (j >= 0 && values[j] > x)
            {
                values[j + 1] = values[j];
                j--;
            }
            values[j+1] = x;
        }
		
        return values;
    }
    
    public int[] QuickSort(int[] valuesToSort, int? left = null, int? right = null) 
    {
        var values = valuesToSort;
		
        if(!left.HasValue || !right.HasValue)
        {
            values = valuesToSort.ToArray();
            left = 0;
            right = values.Length - 1;
        }
				
        if (left < right)
        {
            int pivot = QuickSort_Partition(values, left.Value, right.Value);

            if (pivot > 1) 
            {
                QuickSort(values, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                QuickSort(values, pivot + 1, right);
            }
        }
		
        return values;
    }

    private int QuickSort_Partition(int[] values, int left, int right)
    {
        int pivot = values[left];
        while (true)
        {
            while (values[left] < pivot)
            {
                left++;
            }

            while (values[right] > pivot)
            {
                right--;
            }

            if (left < right)
            {
                if (values[left] == values[right])
                {
                    return right;
                }

                // ReSharper disable once SwapViaDeconstruction
                int temp = values[left];
                values[left] = values[right];
                values[right] = temp;
            }
            else
            {
                return right;
            }
        }
    }
}
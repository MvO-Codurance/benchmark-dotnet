using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace SampleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ForLoops
{
    private const int N = 100000;
    private readonly int[] _data;

    public ForLoops()
    {
        _data = new int[N];
    }

    [Benchmark(Baseline = true)]
    public int ForLoop()
    {
        int value = 0;
        for (int i = 0; i < N; i++)
        {
            value = _data[i];
        }

        return value;
    }
    
    [Benchmark]
    public int ForEachLoop()
    {
        int value = 0;
        foreach (int i in _data)
        {
            value = i;
        }

        return value;
    }
}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace SampleBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class DateParser
{
    private const string DateTimeAsString = "2022-06-01T15:31:07Z";
    private static readonly MyDateParser Parser = new MyDateParser();

    [Benchmark(Baseline = true)]
    public int GetYearFromDateTime() => Parser.GetYearFromDateTime(DateTimeAsString);
    
    [Benchmark]
    public int GetYearFromDateTimeSplit() => Parser.GetYearFromDateTimeSplit(DateTimeAsString);
    
    [Benchmark]
    public int GetYearFromDateTimeSubstring() => Parser.GetYearFromDateTimeSubstring(DateTimeAsString);
    
    [Benchmark]
    public int GetYearFromDateTimeSpan() => Parser.GetYearFromDateTimeSpan(DateTimeAsString.AsSpan());
    
    [Benchmark]
    public int GetYearFromDateTimeSpanWithManualConversion() => Parser.GetYearFromDateTimeSpanWithManualConversion(DateTimeAsString.AsSpan());
}

public class MyDateParser
{
    public int GetYearFromDateTime(string dateTimeAsString)
    {
        var dateTime = DateTime.Parse(dateTimeAsString);
        return dateTime.Year;
    }
    
    public int GetYearFromDateTimeSplit(string dateTimeAsString)
    {
        var splitOnHyphen = dateTimeAsString.Split('-');
        return int.Parse(splitOnHyphen[0]);
    }
    
    public int GetYearFromDateTimeSubstring(string dateTimeAsString)
    {
        var indexOfHyphen = dateTimeAsString.IndexOf('-');
        return int.Parse(dateTimeAsString.Substring(0, indexOfHyphen));
    }
    
    public int GetYearFromDateTimeSpan(ReadOnlySpan<char> dateTimeAsSpan)
    {
        var indexOfHyphen = dateTimeAsSpan.IndexOf('-');
        return int.Parse(dateTimeAsSpan.Slice(0, indexOfHyphen).ToString());
    }
    
    public int GetYearFromDateTimeSpanWithManualConversion(ReadOnlySpan<char> dateTimeAsSpan)
    {
        var indexOfHyphen = dateTimeAsSpan.IndexOf('-');
        var yearAsSpan = dateTimeAsSpan.Slice(0, indexOfHyphen);
        
        var temp = 0;
        for (int i = 0; i < yearAsSpan.Length; i++)
        {
            temp = temp * 10 + (yearAsSpan[i] - '0');
        }

        return temp;
    }
}
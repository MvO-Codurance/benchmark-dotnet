using BenchmarkDotNet.Running;
using SampleBenchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
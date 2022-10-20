// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Company.Framework.Benchmark;

BenchmarkRunner.Run<CoreIdBenchmarks>();

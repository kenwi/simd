``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=3914061 Hz, Resolution=255.4891 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|    Method |       Mean |    Error |   StdDev |     Median |
|---------- |-----------:|---------:|---------:|-----------:|
|       Add |   464.3 us | 17.19 us | 49.60 us |   445.1 us |
| AddNoSimd | 1,160.7 us | 18.74 us | 17.53 us | 1,155.6 us |

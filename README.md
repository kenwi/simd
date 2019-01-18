``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=3914061 Hz, Resolution=255.4891 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|         Method |     Mean |     Error |    StdDev |
|--------------- |---------:|----------:|----------:|
|            Add | 1.377 ms | 0.0271 ms | 0.0226 ms |
|      AddNoSimd | 3.018 ms | 0.0429 ms | 0.0380 ms |
|       Multiply | 1.347 ms | 0.0190 ms | 0.0177 ms |
| MultiplyNoSimd | 3.000 ms | 0.0251 ms | 0.0235 ms |

```
kenwi@i5:~$ git clone https://github.com/kenwi/simd
Cloning into 'simd'...
remote: Enumerating objects: 148, done.
remote: Counting objects: 100% (148/148), done.
remote: Compressing objects: 100% (105/105), done.
remote: Total 148 (delta 94), reused 91 (delta 41), pack-reused 0
Receiving objects: 100% (148/148), 41.50 KiB | 466.00 KiB/s, done.
Resolving deltas: 100% (94/94), done.
kenwi@i5:~$ cd simd/ && dotnet build && dotnet run
Microsoft (R) Build Engine version 15.9.20+g88f5fadfbe for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restoring packages for /home/kenwi/simd/simd.csproj...
  Generating MSBuild file /home/kenwi/simd/obj/simd.csproj.nuget.g.props.
  Generating MSBuild file /home/kenwi/simd/obj/simd.csproj.nuget.g.targets.
  Restore completed in 888.03 ms for /home/kenwi/simd/simd.csproj.
  simd -> /home/kenwi/simd/bin/Debug/netcoreapp2.2/simd.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:02.05
Running [TestCreateAndShowArray]
Creating dataset 4096x2160 = 8847360 pixels
10 first values= [208, 254, 78, 18, 185, 227, 251, 67, 28, 143]
Run Time = 00:00:00.2284070

Running [TestAddMultiply]
SIMD Addition/Multiplication
[5] first values of [a] = [15, 4, 2, 2, 8]
[5] first values of [b] = [0, 12, 4, 1, 14]
[+] [00:00:00.0293228] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0279401] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0270508] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0304233] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0262836] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0269448] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0264215] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0265795] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0274132] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0281436] [5] first values of [result] = [0, 48, 8, 2, 112]

NoSIMD Addition/Multiplication
[5] first values of [a] = [15, 4, 2, 2, 8]
[5] first values of [b] = [0, 12, 4, 1, 14]
[+] [00:00:00.0550323] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0562749] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0558395] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0551915] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0551165] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0559425] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0549396] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0553602] [5] first values of [result] = [0, 48, 8, 2, 112]
[+] [00:00:00.0674953] [5] first values of [result] = [15, 16, 6, 3, 22]
[*] [00:00:00.0507178] [5] first values of [result] = [0, 48, 8, 2, 112]
Run Time = 00:00:01.2980172

Running [TestExecuteOnSetsMethod]
[5] first values of [a] = [211, 162, 66, 79, 134]
[5] first values of [b] = [240, 197, 235, 210, 82]
[+] [00:00:00.0309690] [5] first values of [result] = [451, 359, 301, 289, 216]
[-] [00:00:00.0320615] [5] first values of [result] = [-29, -35, -169, -131, 52]
[*] [00:00:00.0316246] [5] first values of [result] = [50640, 31914, 15510, 16590, 10988]
[/] [00:00:00.0668957] [5] first values of [result] = [0, 0, 0, 0, 1]
[+] [00:00:00.0353527] [5] first values of [result] = [451, 359, 301, 289, 216]
[-] [00:00:00.0351807] [5] first values of [result] = [-29, -35, -169, -131, 52]
[*] [00:00:00.0357880] [5] first values of [result] = [50640, 31914, 15510, 16590, 10988]
[/] [00:00:00.0713379] [5] first values of [result] = [0, 0, 0, 0, 1]
[+] [00:00:00.0357276] [5] first values of [result] = [451, 359, 301, 289, 216]
[-] [00:00:00.0349470] [5] first values of [result] = [-29, -35, -169, -131, 52]
[*] [00:00:00.0356654] [5] first values of [result] = [50640, 31914, 15510, 16590, 10988]
[/] [00:00:00.0708561] [5] first values of [result] = [0, 0, 0, 0, 1]
[+] [00:00:00.0348272] [5] first values of [result] = [451, 359, 301, 289, 216]
[-] [00:00:00.0346504] [5] first values of [result] = [-29, -35, -169, -131, 52]
[*] [00:00:00.0352345] [5] first values of [result] = [50640, 31914, 15510, 16590, 10988]
[/] [00:00:00.0708327] [5] first values of [result] = [0, 0, 0, 0, 1]
[+] [00:00:00.0355395] [5] first values of [result] = [451, 359, 301, 289, 216]
[-] [00:00:00.0427322] [5] first values of [result] = [-29, -35, -169, -131, 52]
[*] [00:00:00.0308154] [5] first values of [result] = [50640, 31914, 15510, 16590, 10988]
[/] [00:00:00.0638345] [5] first values of [result] = [0, 0, 0, 0, 1]
Run Time = 00:00:01.3215721

Running [TestIntensityImage]
Run Time = 00:00:07.2244083

Running [TestFastWrite]
Run Time = 00:00:03.0428450

Running [TestWrite]
Running [<TestWrite>b__1]
Run Time = 00:00:06.0429490
```

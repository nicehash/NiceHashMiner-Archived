@echo off
IF [%1]==[] goto exit_param
IF [%2]==[] goto exit_param
IF [%3]==[] goto exit_param
set PATH=%PATH%;C:\Program Files (x86)\Windows Kits\8.1\bin\x86
mkdir NiceHashMiner_v%1
cd NiceHashMiner_v%1
copy ..\NiceHashMiner.exe NiceHashMiner.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll NiceHashMiner.exe
copy ..\Newtonsoft.Json.dll Newtonsoft.Json.dll
copy ..\cpuid.dll cpuid.dll
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll cpuid.dll
copy ..\setcpuaff.exe setcpuaff.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll setcpuaff.exe
mkdir bin
cd bin
copy ..\..\bin\ccminer_sp.exe ccminer_sp.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll ccminer_sp.exe
copy ..\..\bin\ccminer_tpruvot.exe ccminer_tpruvot.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll ccminer_tpruvot.exe
copy ..\..\bin\cpuminer_x64_AVX.exe cpuminer_x64_AVX.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll cpuminer_x64_AVX.exe
copy ..\..\bin\cpuminer_x64_AVX2.exe cpuminer_x64_AVX2.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll cpuminer_x64_AVX2.exe
copy ..\..\bin\cpuminer_x64_SSE2.exe cpuminer_x64_SSE2.exe
SignTool sign /f %2 /p %3 /t http://timestamp.globalsign.com/scripts/timestamp.dll cpuminer_x64_SSE2.exe
copy ..\..\bin\msvcr120.dll msvcr120.dll
cd ..
cd ..
echo ALL DONE!
goto:eof
:exit_param
echo Usage: [version] [cert_location] [cert_pw]
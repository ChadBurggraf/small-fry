@echo off
MSBuild Source\SmallFry.sln /p:Configuration=Debug
Lib\NUnit.Runners.2.6.1\tools\nunit-console.exe Source\SmallFry.Tests\bin\Debug\SmallFry.Tests.dll
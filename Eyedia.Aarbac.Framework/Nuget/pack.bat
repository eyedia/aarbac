set v=1.0.10
nuget pack Eyedia.Aarbac.Framework.nuspec
copy aarbac.NET.%v%.nupkg ..\..\Downloads\aarbac.NET.%v%.zip /y

rmdir .\temp /s /q

"%ProgramFiles%\WinRAR"\winrar x ..\..\Downloads\aarbac.NET.%v%.zip .\temp\aarbac.NET.%v%\

del .\temp\aarbac.NET.1.0.10\*.* /q
rmdir .\temp\aarbac.NET.1.0.10\package /s /q
rmdir .\temp\aarbac.NET.1.0.10\_rels /s /q
rmdir .\temp\aarbac.NET.1.0.10\content\aarbac.config /s /q
ren .\temp\aarbac.NET.1.0.10\content\*.pp *.cs

copy ..\..\..\aarbac\packages\EntityFramework.6.2.0\lib\net45\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\
copy ..\..\..\aarbac\packages\Microsoft.SqlServer.TransactSql.ScriptDom.14.0.3660.1\lib\net40\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\
copy ..\..\..\aarbac\packages\CommandLineParser.1.1.5\lib\net20\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\
copy ..\..\..\aarbac\packages\GenericParser.1.1.5\lib\net20\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\
copy ..\..\..\aarbac\packages\Newtonsoft.Json.10.0.3\lib\net45\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\
copy ..\..\..\aarbac\packages\FCTB.2.16.21.0\lib\*.dll .\temp\aarbac.NET.1.0.10\lib\net452\

"%ProgramFiles%\WinRAR"\winrar a -ep1 aarbac.NET.%v%.zip .\temp\aarbac.NET.%v%

copy aarbac.NET.%v%.zip ..\..\Downloads\ /y

del aarbac.NET.%v%.zip /q
rmdir .\temp /s /q
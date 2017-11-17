![Aarbac logo](https://github.com/eyedia/aarbac/blob/master/Eyedia.Aarbac.Framework/Graphics/rbac_128.png)

# aarbac Home
An Automated Role Based Access Control .NET Framework.

### Prerequisites:
1. Microsoft SQL Server. Express will do just fine!
2. You are using at least .NET 4.5.2

### Getting started with sample
1. Create an console Application
2. Get [aarbac](https://www.nuget.org/packages/aarbac.NET/) from nuget.
3. The package will have a sample "Books" application, basically just a database (.mdf file) with same valid data in it. Assume this as your own application and you want to provide row,column level permission along with screen entitlements to your users.
4. The second database will be rbac database (.mdf file) with preloaded users and 3 roles.
5. test.csv will contain few sample queries(select, insert, update, delete).
6. AarbacSamples.cs will contain the sample, and thanks to nuget, which will automatically add the cs into your project. You are all ready to go. Just type this:
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestOne();
Console.Read();
```
Above code will test just one query from test.csv, which comes with the package. This will ensure everything is good, the .mdf files are attached correctly & you are good. Mostly .mdf attachment related issues may come here, just troubleshoot as you regularly do. Alternatively you can attach the .mdfs manually into your SQL Server instance and change the connection strings in the config file.

Alright...interesting, huuuh?! Try this out then...
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestBatch();
Console.Read();
```
This code will test all queries from test.csv and put output into test_result.csv.

![Aarbac logo](https://github.com/eyedia/aarbac/blob/master/Eyedia.Aarbac.Framework/Graphics/rbac_128.png)

# Disclaimer
*11/17/2017 - I am new to github and nuget, currently working on preparing nuget release, other basic documentation on github. This document may get updated frequently. Nuget release 1.0.7 is stable, but future release may have some minor changes. Execuse me for that. I will remove this disclaimer once everything is stable.*

# aarbac Home
An Automated Role Based Access Control .NET Framework.

### Prerequisites:
1. Microsoft SQL Server. Express will do just fine!
2. You are using at least .NET 4.5.2

### Getting started with sample
1. Create an console Application
2. Get [aarbac](https://www.nuget.org/packages/aarbac.NET/) from nuget.
3. Additionally, the package will contain these:
    1. books.mdf - A sample "Books" application database with some valid data in it. Assume this as your own application and you want to provide row,column level permission along with screen entitlements to your users.
    2. rbac.mdf - The second rbac database with preloaded users and 3 roles of "Books" application.
    3. test.txt - One sample query of "Books" application.
    4. test.csv - Sample queries(select, insert, update, delete) of "Books" applicaition.
    5. AarbacSamples.cs will contain the sample code, and thanks to nuget, which will automatically add the cs into your project. You are all ready to go. Just type this:
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestOne();
Console.Read();
```
Above code will test a query from test.txt. This will ensure everything is good, the .mdf files are attached correctly. Mostly .mdf attachment related issues may come here, just troubleshoot as you regularly do. Alternatively you can attach the .mdfs manually into your SQL Server instance and change the connection strings in the config file.

Alright...interesting, huuuh?! Try this out then...
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestBatch();
Console.Read();
```
This code will test all queries from test.csv and put output into test_result.csv. You are all set to play around.


### Getting started with real world
1. Get [aarbac](https://www.nuget.org/packages/aarbac.NET/) from nuget.
2. As you may have guessed, you don't need .mdf, .ldf, test.csv, test.txt files. So feel free to delete those.
3. aarbac comes with command line utility named aarbac.exe, copy the exe and config to a suitable folder.
4. You need to create an aarbac database. Keep your SQL Server connection string handy and type following
```shell
aarbac -i <name of your application> <description> <connectionstring>
```
for example, in case of my "Books" application, I would do this:
```shell
aarbac -i "Books" "A sample books application with few sample authors, books, publishers, city, state, country, zipcode to showcase various table relationships" "Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|rbac.mdf;Initial Catalog=rbac;Integrated Security=True" 
```

![Aarbac logo](https://github.com/eyedia/aarbac/blob/master/Eyedia.Aarbac.Framework/Graphics/rbac_128.png)

# Disclaimer
*11/17/2017 - I am new to github and nuget, currently working on nuget release & other basic documentation on github. This document may get updated frequently. aarbac release 1.0.8 is stable, but future release may have some minor changes. PLease execuse me for that. I will remove this disclaimer once everything is stable.*

# What is aarbac?
An Automated Role Based Access Control .NET framework which can automatically apply row & column level permissions on your SELECT,INSERT,UPDATE & DELETE queries.For example, a read (or select) operation like the following …

```sql
select * from Author
```
automatically may get converted to…

```sql
select AuthorId, Name, ZipCodeId from Author inner join Zipcode zc on zc.ZipCodeId = Author.ZipCodeId inner join City c on c.CityId = zc.CityId where c.Name = 'New York'
```

assuming user belongs to a role which allows him to see only 3 columns from author table and only allowed to see authors from New York city.

And an update query like the following…

```sql
update Author set Name = 'Eyedia', SSN = '999-99-9999' where AuthorId = 9999
```
may hit exception like 

```diff
User ‘abc’ does have permission to update table ‘Author’ but does not have permission to update column ‘SSN’
```

### Prerequisites:
1. Microsoft SQL Server.
2. .NET 4.5.2+

### Getting started with sample
1. Create an console Application
2. Get [aarbac](https://www.nuget.org/packages/aarbac.NET/) from nuget.
3. Additionally, the package will contain these:
    1. books.mdf - A sample "Books" application database with some valid data in it. Assume this as your own application and you want to provide row,column level permission along with screen entitlements to your users.
    2. rbac.mdf - The rbac database with 20 users and 3 roles of "Books" application preloaded.
    3. test.txt - One sample query of "Books" application.
    4. test.csv - Few sample queries(select, insert, update, delete) of "Books" applicaition.
    5. AarbacSamples.cs will contain the sample code, and thanks to nuget, which will automatically add the cs into your project. You are all set, just type this:
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestOne();
Console.Read();
```
Above code will test a query from test.txt. This will ensure everything is good, the .mdf files are attached correctly. Mostly .mdf attachment related issues may occure here, just troubleshoot as you regularly do. Alternatively you can attach the .mdfs manually into your SQL Server instance and change the connection strings in the config file. Check out the test_result.txt (the output) along with test.txt (the input).

Alright...interesting, huuuh?! Try this out then...
```cs
new TestAarbac.Samples.AarbacSamples().BookStoreTestBatch();
Console.Read();
```
This code will parse all queries from test.csv and store parsed queries into test_result.csv. Study all the input queries and parsed queries to get feel of aarbac framework. Start playing around.

[See WIKI](https://github.com/eyedia/aarbac/wiki)

### Getting started with real world
1. Get [aarbac](https://www.nuget.org/packages/aarbac.NET/) from nuget.
2. As you may have guessed, you don't need .mdf, .ldf, test.csv, test.txt files. So feel free to delete those, or keep those as is.
3. aarbac comes with command line utility named aarbac.exe. Go to command prompt of the package folder and run various commands to ferform following:

    1. Create aarbac database
        1. You can create 
    
    
[See WIKI](https://github.com/eyedia/aarbac/wiki)

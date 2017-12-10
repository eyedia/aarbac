These are formatted test results of [Books](https://github.com/eyedia/aarbac/wiki/Samples-Overview) application. You can test these queries by executing samples included with aarbac [nuget](https://www.nuget.org/packages/aarbac.NET/) package.

See [csv](https://github.com/eyedia/aarbac/blob/master/Eyedia.Aarbac.Command/Samples/Books/tests_result.csv) format
---1. **Wildcard select, the user can read Author table, cannot read SSN column and can see authors from New York and Charlotte**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId FROM Author   
inner join [ZipCode] [t1] on [t1].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t2] on [t2].CityId = [t1].CityId 
WHERE t2.Name
 in ('New York','Charlotte')  
```
***

2. **Specific columns the user can read Author table, cannot read SSN column and can see authors from New York and Charlotte**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select name,ssn from Author
```
```
Parsed Query:
```
```sql
SELECT name   FROM Author   
inner join [ZipCode] [t5] on [t5].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t6] on [t6].CityId = [t5].CityId 
WHERE t6.Name
 in ('New York','Charlotte')  
```
```diff
- Ambiguous column name 'name'.

```
***

3. **Specific columns select, the user cannot read SSN column & can see authors from NY and Charlotte**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author a
```
```
Parsed Query:
```
```sql
SELECT a.AuthorId , a.Name , a.ZipCodeId FROM Author a   
inner join [ZipCode] [t9] on [t9].ZipCodeId = [a].ZipCodeId   
inner join [City] [t10] on [t10].CityId = [t9].CityId 
WHERE t10.Name
 in ('New York','Charlotte')  
```
***

4. **One specific column select, the user does not have permission to see the SSN column**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select SSN from Author
```
```diff
- RBAC.Core - The query returned 0(zero) column!
```
***

5. **Wildcard select, there is no restriction on Book table, user should see everything**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Book
```
```
Parsed Query:
```
```sql
select Book.BookId,Book.Title,Book.Subject,Book.Price,Book.Isbn13,Book.Isbn10,Book.PublisherId from Book
```
***

6. **Wildcard select with alias**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select a.* from Author a
```
```
Parsed Query:
```
```sql
SELECT a.AuthorId , a.Name , a.ZipCodeId FROM Author a   
inner join [ZipCode] [t18] on [t18].ZipCodeId = [a].ZipCodeId   
inner join [City] [t19] on [t19].CityId = [t18].CityId 
WHERE t19.Name
 in ('New York','Charlotte')  
```
***

7. **Wildcard select with only table alias**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author a
```
```
Parsed Query:
```
```sql
SELECT a.AuthorId , a.Name , a.ZipCodeId FROM Author a   
inner join [ZipCode] [t22] on [t22].ZipCodeId = [a].ZipCodeId   
inner join [City] [t23] on [t23].CityId = [t22].CityId 
WHERE t23.Name
 in ('New York','Charlotte')  
```
***

8. **Wildcard muliselect with table alias**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select a.*, a.* from Author a
```
```
Parsed Query:
```
```sql
SELECT a.AuthorId , a.Name , a.ZipCodeId , a.AuthorId , a.Name , a.ZipCodeId FROM Author a   
inner join [ZipCode] [t26] on [t26].ZipCodeId = [a].ZipCodeId   
inner join [City] [t27] on [t27].CityId = [t26].CityId 
WHERE t27.Name
 in ('New York','Charlotte')  
```
***

9. **Complex wildcard select**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author 
where Author.ZipCodeId
 in (select zc.ZipCodeId from ZipCode zc 
where zc.CityId
 in (select c.CityId from City c 
where c.StateId
 in (select StateId from State 
where ShortName
 in ('NY', 'NC'))))
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId FROM Author   
inner join [ZipCode] [t30] on [t30].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t31] on [t31].CityId = [t30].CityId 
WHERE (Author.ZipCodeId
 in (SELECT zc.ZipCodeId  FROM ZipCode zc  
WHERE zc.CityId
 in (SELECT c.CityId  FROM City c  
WHERE c.StateId
 in (SELECT StateId  FROM State  
WHERE ShortName
 in ('NY'  , 'NC' ) ) ) )) AND (t31.Name
 in ('New York','Charlotte'))  
```
***

10. **select non-permitted table with wildcard, user does not have permission to read from Country table**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Country
```
```diff
- RBAC.Core - The query returned 0(zero) column!
```
***

11. **select non-permitted table with specific columns, user does not have permission to read from Country table**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select countryId,Name from Country
```
```diff
- RBAC.Core - The query returned 0(zero) column!
```
***

12. **select permitted & non-permitted tables with specific columns, user does not have permission to read from Country table**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select Country.CountryId, dbo.Country.Code, dbo.Country.Name, dbo.State.ShortName, dbo.State.Name AS [StateName]
from dbo.Country 

inner join dbo.State ON dbo.Country.CountryId = dbo.State.CountryId
```
```
Parsed Query:
```
```sql
select dbo.State.ShortName, dbo.State.Name AS [StateName] from dbo.Country 

inner join dbo.State ON dbo.Country.CountryId = dbo.State.CountryId
```
***

13. **Select into wildcard**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * into Author2 from Author
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId  into Author2  FROM Author   
inner join [ZipCode] [t34] on [t34].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t35] on [t35].CityId = [t34].CityId 
WHERE t35.Name
 in ('New York','Charlotte')  
```
***

14. **select into specific columns**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select name,ssn into Author2 from Author
```
```
Parsed Query:
```
```sql
SELECT name    into Author2  FROM Author   
inner join [ZipCode] [t38] on [t38].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t39] on [t39].CityId = [t38].CityId 
WHERE t39.Name
 in ('New York','Charlotte')  
```
```diff
- Ambiguous column name 'name'.

```
***

15. **select into specific columns recommended way**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select Author.name, Author.ssn into Author2 from Author
```
```
Parsed Query:
```
```sql
SELECT Author.name    into Author2  FROM Author   
inner join [ZipCode] [t42] on [t42].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t43] on [t43].CityId = [t42].CityId 
WHERE t43.Name
 in ('New York','Charlotte')  
```
```diff
- There is already an object named 'Author2' in the database.

```
***

16. **Select using inner join 1**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select a.AuthorId, a.Name as [AuthorName], a.ZipCodeId, c.Name as City from Author a
inner join Zipcode zc on zc.ZipCodeId = a.ZipCodeId
inner join City c on c.CityId = zc.CityId
where c.Name = 'Charlotte'
```
```
Parsed Query:
```
```sql
SELECT a.AuthorId , a.Name as [AuthorName] , a.ZipCodeId , c.Name as City  FROM Author a 
inner join Zipcode zc on zc.ZipCodeId = a.ZipCodeId 
inner join City c on c.CityId = zc.CityId   
WHERE c.Name
 in ('New York','Charlotte')  
```
***

17. **Select using inner join 2**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select zc.ZipCode, a.Name from ZipCode zc 
inner join Author a on a.ZipCodeId = zc.ZipCodeId 
where zc.ZipCodeId = 12
```
```
Parsed Query:
```
```sql
SELECT zc.ZipCode , a.Name  FROM ZipCode zc 
inner join Author a on a.ZipCodeId = zc.ZipCodeId    
inner join [City] [t48] on [t48].CityId = [zc].CityId 
WHERE (zc.ZipCodeId = 12) AND (t48.Name
 in ('New York','Charlotte'))  
```
***

18. **aarbac recommends to use table or alias prefix, this query will parse good, but will throw error while executing ```Ambiguous column name 'ZipCodeId'```**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author 
where ZipCodeId
 in (select zc.ZipCodeId from ZipCode zc 
where zc.CityId
 in (select c.CityId from City c 
where c.StateId
 in (select StateId from State 
where ShortName
 in ('NY', 'NC'))))
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId FROM Author   
inner join [ZipCode] [t51] on [t51].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t52] on [t52].CityId = [t51].CityId 
WHERE (ZipCodeId
 in (SELECT zc.ZipCodeId  FROM ZipCode zc  
WHERE zc.CityId
 in (SELECT c.CityId  FROM City c  
WHERE c.StateId
 in (SELECT StateId  FROM State  
WHERE ShortName
 in ('NY'  , 'NC' ) ) ) )) AND (t52.Name
 in ('New York','Charlotte'))  
```
```diff
- Ambiguous column name 'ZipCodeId'.

```
***

19. **aarbac recommended column usage**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from Author 
where Author.ZipCodeId
 in (select zc.ZipCodeId from ZipCode zc 
where zc.CityId
 in (select c.CityId from City c 
where c.StateId
 in (select StateId from State 
where ShortName
 in ('NY', 'NC'))))
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId FROM Author   
inner join [ZipCode] [t55] on [t55].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t56] on [t56].CityId = [t55].CityId 
WHERE (Author.ZipCodeId
 in (SELECT zc.ZipCodeId  FROM ZipCode zc  
WHERE zc.CityId
 in (SELECT c.CityId  FROM City c  
WHERE c.StateId
 in (SELECT StateId  FROM State  
WHERE ShortName
 in ('NY'  , 'NC' ) ) ) )) AND (t56.Name
 in ('New York','Charlotte'))  
```
***

20. **incorrect query**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select
```
```
Parsed Query:
```
```sql
select
```
```diff
- Incorrect syntax near select.Error:Incorrect syntax near select. at line nr:1 column:1 
```
***

21. **incorrect query**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
abc
```
```diff
- RBAC.Core - Invalid query type!
```
***

22. **incorrect query**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * 
```
```
Parsed Query:
```
```sql
select * 
```
```diff
- Must specify table to select from.

```
***

23. **incorrect query**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from 
```
```
Parsed Query:
```
```sql
select * from 
```
```diff
- Unexpected end of file occurred.Error:Unexpected end of file occurred. at line nr:1 column:15 
```
***

24. **incorrect query**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from book 
where
```
```
Parsed Query:
```
```sql
select * from book 
where
```
```diff
- Unexpected end of file occurred.Error:Unexpected end of file occurred. at line nr:1 column:25 
```
***

25. **incorrect table**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from abc
```
```diff
- RBAC.PRS - The referred table abc was not found in meta data!
```
***

26. **Simple insert into permissible table, user can insert record into Book**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
INSERT INTO [dbo].[Book] ([Title],[Subject],[Price],[PublisherId]) VALUES
           ('A new Book'
           ,'aarbac - An Automated Role Based Access Control'
           ,5.50          
           ,11)
```
```
Parsed Query:
```
```sql
INSERT INTO [dbo].[Book] ([Title],[Subject],[Price],[PublisherId]) VALUES
           ('A new Book'
           ,'aarbac - An Automated Role Based Access Control'
           ,5.50          
           ,11)
```
***

27. **Simple Insert**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
insert into Author values ('','',1)
```
```diff
- RBAC.PRS - User 'Lashawn' does not have permission to insert record into the table 'Author'!
```
***

28. **Simple insert with specific column names**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
insert into Author(Name,SSN,ZipCodeId) values ('','',1)
```
```diff
- RBAC.PRS - User 'Lashawn' does not have permission to insert record into the table 'Author'!
```
***

29. **Simple update, Lashawn has permission to update Name**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
update Author set Name='abc' 
where AuthorId = 1
```
```
Parsed Query:
```
```sql
update Author set Name='abc' 
where AuthorId = 1
```
***

30. **Simple update, Lashawn does not have permission to update SSN**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
update Author set SSN='abc' 
where AuthorId = 1
```
```diff
- RBAC.PRS - User 'Lashawn' has permission to update table 'Author', however has no permission to update column 'SSN'!
```
***

31. **Update with Join Clause**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
UPDATE a 
SET a.Name = 'abc' 
FROM dbo.Author AS a
INNER JOIN dbo.ZipCode AS zc 
       ON a.ZipCodeId = a.ZipCodeId

WHERE zc.ZipCode = '00000' 
```
```
Parsed Query:
```
```sql
UPDATE a 
SET a.Name = 'abc' 
FROM dbo.Author AS a
INNER JOIN dbo.ZipCode AS zc 
       ON a.ZipCodeId = a.ZipCodeId

WHERE zc.ZipCode = '00000' 
```
***

32. **simple delete**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
delete from author 
where name = 'abc'
```
```
Parsed Query:
```
```sql
delete from author 
where name = 'abc'
```
***


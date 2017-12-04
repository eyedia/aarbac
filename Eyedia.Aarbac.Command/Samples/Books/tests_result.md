1. **Simple inner join**  
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

2. **simple non scalar**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
select * from book
```
```
Parsed Query:
```
```sql
select book.BookId,book.Title,book.Subject,book.Price,book.Isbn13,book.Isbn10,book.PublisherId from book
```
***

3. **non scalar complex**  
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
inner join [ZipCode] [t4] on [t4].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t5] on [t5].CityId = [t4].CityId 
WHERE (Author.ZipCodeId
 in (SELECT zc.ZipCodeId  FROM ZipCode zc  
WHERE zc.CityId
 in (SELECT c.CityId  FROM City c  
WHERE c.StateId
 in (SELECT StateId  FROM State  
WHERE ShortName
 in ('NY'  , 'NC' ) ) ) )) AND (t5.Name
 in ('New York','Charlotte'))  
```
***

4. ****  
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
inner join [City] [t8] on [t8].CityId = [zc].CityId 
WHERE (zc.ZipCodeId = 12) AND (t8.Name
 in ('New York','Charlotte'))  
```
***

5. **aarbac recommends to use table or alias prefix, this query will parse good, but will throw error while executing ```Ambiguous column name 'ZipCodeId'```**  
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
inner join [ZipCode] [t11] on [t11].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t12] on [t12].CityId = [t11].CityId 
WHERE (ZipCodeId
 in (SELECT zc.ZipCodeId  FROM ZipCode zc  
WHERE zc.CityId
 in (SELECT c.CityId  FROM City c  
WHERE c.StateId
 in (SELECT StateId  FROM State  
WHERE ShortName
 in ('NY'  , 'NC' ) ) ) )) AND (t12.Name
 in ('New York','Charlotte'))  
```
```diff
- Ambiguous column name 'ZipCodeId'.

```
***

6. **incorrect query**  
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

7. **incorrect query**  
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

8. **incorrect query**  
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

9. **incorrect query**  
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

10. **incorrect query**  
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

11. **incorrect table**  
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

12. **Simple Insert**  
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

13. **Simple insert with specific column names**  
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

14. **Simple update, Lashawn has permission to update Name**  
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

15. **Simple update, Lashawn does not have permission to update SSN**  
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

16. **Update with Join Clause**  
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

17. **Select wildcard**  
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
inner join [ZipCode] [t15] on [t15].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t16] on [t16].CityId = [t15].CityId 
WHERE t16.Name
 in ('New York','Charlotte')  
```
***

18. **Select wildcard**  
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
inner join [ZipCode] [t19] on [t19].ZipCodeId = [a].ZipCodeId   
inner join [City] [t20] on [t20].CityId = [t19].CityId 
WHERE t20.Name
 in ('New York','Charlotte')  
```
***

19. **Select wildcard**  
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
inner join [ZipCode] [t23] on [t23].ZipCodeId = [a].ZipCodeId   
inner join [City] [t24] on [t24].CityId = [t23].CityId 
WHERE t24.Name
 in ('New York','Charlotte')  
```
***

20. **Select wildcard**  
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
inner join [ZipCode] [t27] on [t27].ZipCodeId = [a].ZipCodeId   
inner join [City] [t28] on [t28].CityId = [t27].CityId 
WHERE t28.Name
 in ('New York','Charlotte')  
```
***

21. **Select wildcard**  
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
inner join [ZipCode] [t31] on [t31].ZipCodeId = [a].ZipCodeId   
inner join [City] [t32] on [t32].CityId = [t31].CityId 
WHERE t32.Name
 in ('New York','Charlotte')  
```
***

22. **simple delete**  
Rbac: books  
User: Lashawn  
Role: [role_city_mgr](https://raw.githubusercontent.com/eyedia/aarbac/master/Eyedia.Aarbac.Command/Samples/Books/role_city_mgr.xml)  
Query:  
```sql
delete from author 
where name = 'abc'
```
```diff
- RBAC.PRS - User 'Lashawn' does not have permission to delete record from table 'Author'!
```
***


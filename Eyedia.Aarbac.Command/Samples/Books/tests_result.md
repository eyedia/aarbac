1. Simple inner join
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
WHERE City.Name
 in ('New York','Charlotte')  
```
```
Record Count(s):Errored
```
```diff
- The multi-part identifier "City.Name" could not be bound.
The multi-part identifier "City.Name" could not be bound.

```
***

2. simple non scalar
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select * from book
```
```
Parsed Query:
```
```sql
select book.BookId,book.Title,book.Subject,book.Price,book.Isbn13,book.Isbn10,book.PublisherId from book
```
```
Record Count(s):10 record(s)
```
***

3. non scalar complex
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):10 record(s)
```
***

4. 
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):0 record(s)
```
***

5. aarbac recommends to use table or alias prefix, this query will parse good, but will throw error while executing ```Ambiguous column name 'ZipCodeId'```
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):Errored
```
```diff
- Ambiguous column name 'ZipCodeId'.

```
***

6. incorrect query
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select
```
```
Parsed Query:
```
```sql
select
```
```
Record Count(s):Errored
```
```diff
- Incorrect syntax near select.Error:Incorrect syntax near select. at line nr:1 column:1 
Incorrect syntax near '10'.

```
***

7. incorrect query
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
abc
```
```diff
- RBAC.Core - Invalid query type!
```
***

8. incorrect query
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select * 
```
```diff
- Object reference not set to an instance of an object.
```
***

9. incorrect query
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select * from 
```
```
Parsed Query:
```
```sql
select * from 
```
```
Record Count(s):Errored
```
```diff
- Unexpected end of file occurred.Error:Unexpected end of file occurred. at line nr:1 column:15 
Incorrect syntax near 'from'.

```
***

10. incorrect query
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):Errored
```
```diff
- Unexpected end of file occurred.Error:Unexpected end of file occurred. at line nr:1 column:25 
Incorrect syntax near 'where'.

```
***

11. incorrect table
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select * from abc
```
```diff
- RBAC.PRS - The referred table abc was not found in meta data!
```
***

12. Simple Insert
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
insert into Author values ('','',1)
```
```diff
- RBAC.PRS - User 'Lashawn' does not have permission to insert record into the table 'Author'!
```
***

13. Simple insert with specific column names
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
insert into Author(Name,SSN,ZipCodeId) values ('','',1)
```
```diff
- RBAC.PRS - User 'Lashawn' does not have permission to insert record into the table 'Author'!
```
***

14. Simple update, Lashawn has permission to update Name
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
update Author set Name='abc' 
where AuthorId = 1
```
```diff
- Could not find table name in referred tables!
```
***

15. Simple update, Lashawn does not have permission to update SSN
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
update Author set SSN='abc' 
where AuthorId = 1
```
```diff
- Could not find table name in referred tables!
```
***

16. Update with Join Clause
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):Errored
```
```diff
- Incorrect syntax near '10'.

```
***

17. Select wildcard
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):10 record(s)
```
***

18. Select wildcard
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):10 record(s)
```
***

19. Select wildcard
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
```sql
select * from Author a
```
```
Parsed Query:
```
```sql
SELECT Author.AuthorId , Author.Name , Author.ZipCodeId FROM Author a   
inner join [ZipCode] [t23] on [t23].ZipCodeId = [Author].ZipCodeId   
inner join [City] [t24] on [t24].CityId = [t23].CityId 
WHERE t24.Name
 in ('New York','Charlotte')  
```
```
Record Count(s):Errored
```
```diff
- The multi-part identifier "Author.ZipCodeId" could not be bound.
The multi-part identifier "Author.AuthorId" could not be bound.
The multi-part identifier "Author.Name" could not be bound.
The multi-part identifier "Author.ZipCodeId" could not be bound.

```
***

20. Select wildcard
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):10 record(s)
```
***

21. Select wildcard
```
Rbac:books
User:Lashawn
Role:role_city_mgr
Query:
```
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
```
Record Count(s):10 record(s)
```
***


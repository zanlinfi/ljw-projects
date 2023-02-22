# ljw-projects
ljw's projects
### database connection: write in secret.json
```
"ConnectionStrings": {
    "ConnStr": "Server=.;Database=Demo5_Feb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;Packet Size=512"
  } 
```
### stored procedures
```
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Title]
      ,[Price]
      ,[AuthorName]
  FROM [Demo5_Feb].[dbo].[TBook]

  --create a stored procedure without parameters named GetBookAll
create procedure GetBookAll
as
begin
    select *
    from TBook
end

drop procedure GetBookAll
--execute GetBookAll 
execute GetBookAll

-- create a stored procedure named GetBookById
if (object_id('GetBookById', 'P') is not null)
    drop proc GetBookById
go
create proc GetBookById(@Id int)
as
    select * from TBook where Id=@Id;
go

execute GetBookById 2;

-- create a stored procedure named AddBook
if (object_id('AddBook', 'P') is not null)
    drop proc AddBook
go
create proc AddBook(@Title nvarchar(max) , @Price float, @AuthorName nvarchar(max))
as
begin
    insert into TBook values(@Title,@Price,@AuthorName)
end
go

execute AddBook 'book24', 19,  'me'

-- create a stored procedure named UpdateBookById
if (object_id('UpdateBookById', 'P') is not null)
    drop proc UpdateBookById
go
create proc UpdateBookById(@Id int,@Title nvarchar(max) , @Price float, @AuthorName nvarchar(max))
as
begin
    update TBook Set Title=@Title, Price=@Price, AuthorName=@AuthorName where Id=@Id
end
go
execute UpdateBookById 26, 'book26', 26,  'person'

SELECT TOP (1000) [Id]
      ,[Title]
      ,[Price]
      ,[AuthorName]
  FROM [Demo5_Feb].[dbo].[TBook]

  -- create a stored procedure named DeleteById
if (object_id('DeleteById', 'P') is not null)
    drop proc DeleteById
go
create proc DeleteById(@Id int)
as
begin
    DELETE FROM TBook WHERE Id = @Id;
end
go
execute DeleteById 29

-- show all stored procedure
select * from sys.objects where type='P'
```
![Database & Tables structure](https://github.com/zanlinfi/ljw-projects/blob/main/Database%20%26%20Tables%20structure.png)
![Project structure introduction](https://github.com/zanlinfi/ljw-projects/blob/main/Project%20structure%20introduction.png)
  

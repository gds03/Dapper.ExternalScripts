create database Sales
go
use [Sales]

create table Product
(
	id int primary key identity(1, 1),
	name nvarchar(256) not null,
	quantity int not NULL
)

insert into Product([name], [quantity])
select 'Pen', 10 union all
select 'Pencil', 55 union all
select 'Eraser', 40 union all
select 'Paper', 4 union all
select 'Board', 1
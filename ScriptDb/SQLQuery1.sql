create database Shoes

use Shoes


create table Brand(
BrandId int IDENTITY primary key,
Name varchar(50)
)


create table Shoes(
ShoesId int IDENTITY primary key,
Name varchar(50),
BrandId int,
foreign key (BrandId) References Brand(BrandId)
)


select * from Shoes;

select * from Brand;

insert into Brand(Name)
values('Nike'),
('Adidas'),
('Puma'),
('Fila'),
('Saucony');



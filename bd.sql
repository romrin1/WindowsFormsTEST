
create database test
go
USE [test];
CREATE TABLE department
    (id int, name varchar(100))


CREATE TABLE employee
    (id int, department_id int, chief_id int, name varchar(100), salary int)
;

INSERT INTO department
    (id, name)
VALUES
    (1, 'D1'),
    (2, 'D2'),
    (3, 'D3')
;

INSERT INTO employee
    (id, department_id, chief_id, name, salary)
VALUES
    (1, 1, 5, 'John', 100),
    (2, 1, 5, 'Misha', 600),
    (3, 2, 6, 'Eugen', 300),
    (4, 2, 6, 'Tolya', 400),
    (5, 3, 7, 'Stepan', 500),
	(6, 3, 7, 'Alex', 1000),
	(7, 3, NULL, 'Ivan', 1100);
go
CREATE PROCEDURE [dbo].[sel_dep_im]
--declare
	@id int,
	@ch int
AS
BEGIN
 IF @id=0
 BEGIN 
	IF @ch=0
		SELECT 
			[Департамент]                                                  = d.name,
			[Зарплата]														=sum(salary)
		FROM department d join employee e on d.id=e.department_id
		GROUP BY d.name
	ELSE
		SELECT 
			[Департамент]                                                  = d.name,
			[Руководитель]												   =case when e2.name is null then '' else e2.name end,
			[Зарплата]														=sum(e.salary)
		FROM department d join employee e on d.id=e.department_id
		left join employee e2 on e.chief_id=e2.id
		GROUP BY d.name, e2.name

 END
 ELSE
	IF @id=1
	BEGIN
		SELECT 
			[Департамент]                                                  = d.name,
			[Зарплата]														=max(salary)
		FROM department d join employee e ON d.id=e.department_id
		WHERE salary in (SELECT max(salary) FROM employee)
		GROUP BY d.name
	END
	ELSE
	BEGIN
		SELECT 
			[Департамент]                                                  = d.name,
			[Сотрудник]														=e.name,
			[Зарплата]														=salary
		FROM department d join employee e ON d.id=e.department_id
		WHERE e.id in (SELECT chief_id FROM employee) 
		ORDER BY salary DESC


 END
END
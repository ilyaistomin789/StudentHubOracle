CREATE DATABASE StudentHub
GO
USE StudentHub

-- ---
-- Table 'User'
-- 
-- ---

DROP TABLE IF EXISTS Users;
		
CREATE TABLE Users (
  UserId INTEGER PRIMARY KEY NOT NULL IDENTITY(1,1),
  UserName NVARCHAR(30) NOT NULL ,
  UserPassword NVARCHAR(30) NOT NULL,
);

-- ---
-- Table 'Student'
-- 
-- ---

DROP TABLE IF EXISTS Student;

CREATE TABLE Student (
  StudentId INTEGER NOT NULL IDENTITY(1000,1),
  UserId INTEGER NOT NULL constraint STUDENT_USER_FK foreign key references Users(UserId),
  StudentName NVARCHAR(50) NULL DEFAULT NULL,
  StudentStatus NVARCHAR(20) NULL DEFAULT 'Student',
  Course INTEGER NULL DEFAULT NULL,
  GroupId INTEGER NULL DEFAULT NULL,
  Specialization NVARCHAR(20) NULL DEFAULT NULL,
  Faculty NVARCHAR(30) NULL DEFAULT NULL,
  Birthday DATE NULL DEFAULT NULL,
  Email NVARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY(StudentId)
);

-- ---
-- Table 'subject'
-- 
-- ---

DROP TABLE IF EXISTS Subject;

CREATE TABLE Subject (
  Subject NVARCHAR(20) NULL DEFAULT NULL,
  SubjectName NVARCHAR(50) NOT NULL,
  CONSTRAINT PK_Subject PRIMARY KEY (SubjectName)
);

-- ---
-- Table 'progress'
-- 
-- ---

DROP TABLE IF EXISTS Progress;

CREATE TABLE Progress (
  ProgressId INTEGER NOT NULL IDENTITY(1,1),
  StudentId INTEGER NOT NULL constraint PROGRESS_STUDENT_FK foreign key references Student(StudentId),
  SubjectName NVARCHAR(50) NULL DEFAULT NULL,
  Note INTEGER NULL DEFAULT NULL,
  PDate DATE NULL DEFAULT NULL,
  PRIMARY KEY (ProgressId, StudentId)
);

-- ---
-- Table 'admin'
-- 
-- ---

DROP TABLE IF EXISTS Admin;

CREATE TABLE Admin (
  AdminId INTEGER NOT NULL IDENTITY(1,1),
  AdminName NVARCHAR(80) NULL DEFAULT NULL,
  UserId INTEGER constraint ADMIN_USER_FK foreign key references Users(UserId)
);

-- ---
-- Table 'adjustment'
-- 
-- ---
DROP TABLE IF EXISTS Adjustment;

CREATE TABLE Adjustment (
  AdjustmentId INTEGER NOT NULL IDENTITY(1,1),
  StudentId INTEGER NOT NULL constraint ADJUSTMENT_STUDENT_FK foreign key references Student(StudentId),
  SubjectName NVARCHAR(50) NULL DEFAULT NULL,
  AdjustmentStatus INTEGER CHECK(AdjustmentStatus IN (0,1,2)) DEFAULT 0,
  ADate DATE NULL DEFAULT NULL,
  PRIMARY KEY (AdjustmentId)
);

-- ---
-- Table 'Retake'
-- 
-- ---

DROP TABLE IF EXISTS Retake;

CREATE TABLE Retake (
  RetakeId INTEGER NOT NULL IDENTITY(1,1),
  StudentId INTEGER NOT NULL constraint RETAKE_STUDENT_FK foreign key references Student(StudentId),
  SubjectName NVARCHAR(50) NULL DEFAULT NULL,
  RetakeStatus INT CHECK(RetakeStatus IN (0,1,2)) DEFAULT 0,
  RDate DATE NULL DEFAULT NULL,
  PRIMARY KEY (RetakeId)
);
-- ---
-- Table 'SubjectGaps'
-- 
-- ---
DROP TABLE IF EXISTS SubjectGaps;
CREATE TABLE SubjectGaps (
  StudentId INTEGER constraint SUBJECTGAPS_STUDENT_FK foreign key references Student(StudentId),
  SubjectName NVARCHAR(50) NULL DEFAULT NULL,
  GapsCount INTEGER NULL DEFAULT NULL)
-- ---
-- Table 'Faculty'
-- 
-- ---
DROP TABLE IF EXISTS Faculty;
create table Faculty
  (    Faculty      char(10)   constraint  FACULTY_PK primary key,
       FacultyName varchar(50) default '???'
  );
-- ---
-- Table 'Subject'
-- 
-- ---
  DROP TABLE IF EXISTS Subject;
create table Subject
    (     Subject      char(10)   constraint SUBJECT_PK  primary key, 
     SubjectName varchar(100) unique,
     Faculty       char(10)    constraint SUBJECT_FACULTY_FK foreign key 
                         references Faculty(Faculty)   
     );
-- ---
-- Table 'Specialization'
-- 
-- ---
DROP TABLE IF EXISTS Specialization;
CREATE TABLE Specialization (
  Specialization NVARCHAR(20),
  Faculty char(10) constraint SPEC_FACULTY foreign key references Faculty(Faculty)
)

-- ---
-- Table 'BadStudent'
-- 
-- ---
DROP TABLE IF EXISTS BadStudent;
CREATE TABLE BadStudent (
	StudentId INTEGER constraint BADSTUDENT_STUDENT_FK foreign key references Student(StudentId),
	Gaps INTEGER NOT NULL
)
GO






CREATE PROCEDURE ADD_USER
@UserName NVARCHAR(30),
@UserPassword NVARCHAR(30)
AS
BEGIN
	INSERT INTO Users(UserName,UserPassword) VALUES (@UserName,@UserPassword);
	DECLARE @userId INTEGER;
	SET @userId = (SELECT MAX(UserId) FROM Users);
	INSERT INTO Student(UserId,StudentName,Course,GroupId,Specialization,Faculty,Birthday,Email) VALUES (@userId,'undefined',1,1,'undefined','undefined','12-12-2020','none');
END

CREATE PROCEDURE GET_USERNAME AS
BEGIN
SELECT UserName FROM Users;
END

CREATE PROCEDURE GET_USER AS
BEGIN
SELECT * FROM Users;
END
GO

CREATE PROCEDURE IS_ADMIN
@UserId INTEGER
AS
BEGIN
SELECT UserId FROM Admin where UserId = @UserId;
END

CREATE PROCEDURE GET_STUDENT_FIELDS
@UserId INTEGER
AS
BEGIN
SELECT * FROM Student where UserId = @UserId
END

CREATE PROCEDURE SET_STUDENT_FIELDS
@StudentId INTEGER,
@StudentName NVARCHAR(50),
@Course INTEGER,
@GroupId INTEGER,
@Specialization NVARCHAR(20),
@Faculty NVARCHAR(30),
@Birthday DATE
AS
BEGIN
UPDATE Student SET StudentName = @StudentName, Course = @Course, 
GroupId = @GroupId, Specialization = @Specialization, Faculty = @Faculty, Birthday = @Birthday WHERE StudentId = @StudentId
SELECT * FROM Student WHERE StudentId = @StudentId
END

CREATE PROCEDURE ADD_ADJUSTMENT
@StudentId INTEGER,
@SubjectName NVARCHAR(50),
@ADate DATE
AS
BEGIN
INSERT INTO Adjustment(StudentId,SubjectName,ADate) VALUES(@StudentId,@SubjectName,@ADate)
END

CREATE PROCEDURE ADD_RETAKE
@StudentId INTEGER,
@SubjectName NVARCHAR(50),
@RDate DATE
AS
BEGIN
INSERT INTO Retake(StudentId,SubjectName,RDate) VALUES(@StudentId,@SubjectName,@RDate)
END


CREATE PROCEDURE SET_PROGRESS
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@Note INTEGER,
@PDate DATE
AS
BEGIN
DECLARE @StudentId INTEGER;
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName);
INSERT INTO Progress(StudentId,SubjectName,Note,PDate) VALUES (@StudentId,@SubjectName,@Note,@PDate);
END

CREATE PROCEDURE SET_GAPS
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@Gaps INTEGER
AS
BEGIN
DECLARE @StudentId INTEGER;
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName);
INSERT INTO SubjectGaps(StudentId,SubjectName,GapsCount) VALUES (@StudentId,@SubjectName,@Gaps);
if (select sum(GapsCount) from SubjectGaps where StudentId = @StudentId) > 30
	if EXISTS (select * from BadStudent where StudentId = @StudentId)
		update BadStudent set Gaps = (select sum(GapsCount) from SubjectGaps where StudentId = @StudentId) where StudentId = @StudentId
	else
	insert into BadStudent values (@StudentId, (select sum(GapsCount) from SubjectGaps where StudentId = @StudentId))

END


CREATE PROCEDURE GET_SUBJECTS
@Faculty NVARCHAR(10)
AS
BEGIN
SELECT Subject FROM Subject where Faculty = @Faculty
END

CREATE PROCEDURE ADMIN_GET_ADJUSTMENT
AS
BEGIN
select s.StudentName [Student], s.Faculty, s.Specialization, s.Course, s.GroupId, a.SubjectName [Subject], convert(varchar,a.ADate,104) [Date],
CASE
when a.AdjustmentStatus = 0 then 'In processing'
when a.AdjustmentStatus = 1 then 'Request rejected'
when a.AdjustmentStatus = 2 then 'Request accepted'
END [Status]
from Adjustment a inner join Student s on a.StudentId = s.StudentId
order by s.Faculty
END

CREATE PROCEDURE ADMIN_GET_RETAKE
AS
BEGIN
select s.StudentName [Student], s.Faculty, s.Specialization, s.Course, s.GroupId, r.SubjectName [Subject], convert(varchar,r.RDate,104) [Date],
CASE
when r.RetakeStatus = 0 then 'In processing'
when r.RetakeStatus = 1 then 'Request rejected'
when r.RetakeStatus = 2 then 'Request accepted'
END [Status]
from Retake r inner join Student s on r.StudentId = s.StudentId
order by s.Faculty
END

CREATE PROCEDURE ADMIN_GET_GAPS
AS
BEGIN
select s.StudentName [Student], s.Faculty, s.Specialization, s.Course, s.GroupId, g.SubjectName [Subject] , g.GapsCount [Gaps]
from SubjectGaps g inner join Student s on g.StudentId = s.StudentId
order by s.Faculty, s.StudentName
END

CREATE PROCEDURE ADMIN_ACCEPT_ADJUSTMENT
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@ADate DATE
AS
BEGIN
DECLARE @StudentId INTEGER
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName)
UPDATE Adjustment SET AdjustmentStatus = 2 WHERE StudentId = @StudentId AND SubjectName = @SubjectName AND ADate = @ADate
END


CREATE PROCEDURE ADMIN_DECLINE_ADJUSTMENT
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@ADate DATE
AS
BEGIN
DECLARE @StudentId INTEGER
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName)
UPDATE Adjustment SET AdjustmentStatus = 1 WHERE StudentId = @StudentId AND SubjectName = @SubjectName AND ADate = @ADate
END

CREATE PROCEDURE ADMIN_ACCEPT_RETAKE
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@RDate DATE
AS
BEGIN
DECLARE @StudentId INTEGER
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName)
UPDATE Retake SET RetakeStatus = 2 WHERE StudentId = @StudentId AND SubjectName = @SubjectName AND RDate = @RDate
END

CREATE PROCEDURE ADMIN_DECLINE_RETAKE
@StudentName NVARCHAR(50),
@SubjectName NVARCHAR(50),
@RDate DATE
AS
BEGIN
DECLARE @StudentId INTEGER
SET @StudentId = (SELECT StudentId FROM Student WHERE StudentName = @StudentName)
UPDATE Retake SET RetakeStatus = 1 WHERE StudentId = @StudentId AND SubjectName = @SubjectName AND RDate = @RDate
END

CREATE PROCEDURE ADMIN_GET_BADSTUDENTS
AS
SELECT s.StudentName [Student], s.Faculty, s.Specialization, s.Course, s.GroupId, b.Gaps FROM BadStudent b
INNER JOIN Student s ON b.StudentId = s.StudentId


CREATE PROCEDURE SEARCH_STUDENT
@StudentName NVARCHAR(50)
AS
BEGIN
DECLARE @LIKE_FIELD NVARCHAR(50) = '%' + @StudentName + '%'
SELECT s.StudentName [Student], s.Course, s.GroupId [Group], s.Specialization, s.Faculty,p.SubjectName, convert(varchar,p.PDate,104) [Date], p.Note FROM Student s 
INNER JOIN Progress p ON s.StudentId = p.StudentId
WHERE s.StudentName LIKE @LIKE_FIELD
END
EXEC SEARCH_STUDENT 'I'
CREATE PROCEDURE SEARCH_STUDENT_FIELDS
@StudentName NVARCHAR(50)
AS
BEGIN
DECLARE @LIKE_FIELD NVARCHAR(50) = '%' + @StudentName + '%'
SELECT StudentName [Student],Course,GroupId [Group], Faculty, Specialization, convert(varchar,Birthday,104) [Birthday] FROM Student 
WHERE StudentName LIKE @LIKE_FIELD
END
drop procedure SEARCH_STUDENT_FIELDS

CREATE PROCEDURE UPDATE_EMAIL
@StudentName NVARCHAR(50),
@Course INT,
@GroupId INT,
@Faculty NVARCHAR(30),
@Email NVARCHAR(50)
AS
BEGIN
UPDATE Student SET Email = @Email where StudentName = @StudentName and Course = @Course and GroupId = @GroupId and Faculty = @Faculty
END
SELECT * FROM Student WHERE StudentName LIKE '%' + 'def' + '%' 

use StudentHub
select * from Student
select * from Users
select * from admin
select * from Retake
select * from Adjustment
select * from Progress
select * from SubjectGaps
select * from Subject
select * from Faculty
select * from Specialization
select * from BadStudent
insert into users values('admin','ISMvKXpXpadDiUoOSoAfww==')
insert into admin values('admin',3)
select * from admin inner join users on users.UserId = admin.UserId
SELECT StudentName from Student where Course = 2 and GroupId = 6 and Specialization = 'ПОИТ' and Faculty = 'ИТ'
SELECT SubjectName [Subject], convert(varchar,PDate,104) [Date of issue], Note FROM Progress where StudentId = 1003
SELECT COUNT(*) FROM Retake where RetakeStatus = 1 OR RetakeStatus = 2 
                SELECT COUNT(*) FROM Adjustment where AdjustmentStatus = 1 OR AdjustmentStatus = 2 
             SELECT COUNT(*) FROM Adjustment where AdjustmentStatus = 0
           SELECT COUNT(*) FROM Retake where RetakeStatus = 1 OR RetakeStatus = 2
             SELECT COUNT(*) FROM Retake where RetakeStatus = 0
             SELECT COUNT(*) FROM Users
insert into Faculty(Faculty, FacultyName )
	values	('ХТиТ', 'Химическая технология и техника'),
			('ЛХФ', 'Лесохозяйственный факультет'),
			('ИЭФ', 'Инженерно-экономический факультет'),
			('ТТЛП', 'Технология и техника лесной промышленности'),
			('ТОВ', 'Технология органических веществ'),
			('ИТ', 'Факультет информационных технологий'),
			('ИДиП', 'Издательское дело и полиграфия');
insert into Subject (Subject, SubjectName, Faculty)
	values	('СУБД', 'Системы управления базами данных', 'ИТ'),
			('ООТПиСП', 'Объектно-ориентированные технологии программирования и структуры проектирвоания', 'ИТ'),
			('СТПВI', 'Современные технологии программирования в Internet', 'ИТ'),
			('БД', 'Базы данных','ИТ'),
			('ИНФ', 'Информационные технологии','ИТ'),
			('ОАиП', 'Основы алгоритмизации и программирования', 'ИТ'),
			('ПЗ', 'Представление знаний в компьютерных системах', 'ИТ'),
			('ПСП', 'Программирование сетевых приложений','ИТ'),
			('МСОИ', 'Моделирование систем обработки информации', 'ИТ'),
			('ПИС', 'Проектирование информационных систем', 'ИТ'),
			('КГ', 'Компьютерная геометрия ','ИТ'),
			('ПМАПЛ', 'Полиграф. машины, автоматы и поточные линии', 'ИДиП'),
			('КМС', 'Компьютерные мультимедийные системы', 'ИТ'),
			('ОПП', 'Организация полиграф. производства', 'ИДиП'),
			('ДМ', 'Дискретная математика', 'ИТ'),
			('МП', 'Математическое программирование','ИТ'),  
			('ЛЭВМ', 'Логические основы ЭВМ',  'ИТ'),                 
			('ООП', 'Объектно-ориентированное программирование', 'ИТ'),
			('ЭП', 'Экономика природопользования','ИЭФ'),
			('ЭТ', 'Экономическая теория','ИЭФ'),
			('БЛЗиПсOO','Биология лесных зверей и птиц с осн. охотов.','ЛХФ'),
			('ОСПиЛПХ','Основы садово-паркового и лесопаркового хозяйства',  'ЛХФ'),
			('ИГ', 'Инженерная геодезия ','ЛХФ'),
			('ЛВ', 'Лесоводство', 'ЛХФ'), 
			('ОХ', 'Органическая химия', 'ТОВ'),   
			('ВТЛ', 'Водный транспорт леса','ТТЛП'),
			('ТиОЛ', 'Технология и оборудование лесозаготовок', 'ТТЛП'), 
			('ТОПИ', 'Технология обогащения полезных ископаемых ','ХТиТ');
insert into Specialization values	('ПОИТ','ИТ'),
									('ИСИТ','ИТ'),
									('ДЭИВИ','ИТ'),
									('ПОИБМС','ИТ'),
									('ИД','ИДиП'),
									('ПОиСОИ','ИДиП'),
									('КиПИИКМ','ХТиТ'),             
									('МиАХПиПСМ','ХТиТ'),
									('ЛХ','ЛХФ'),
									('СПС','ЛХФ'),
									('ТиП','ЛХФ'),
									('ЭиУНП','ИЭФ'),
									('БУАиА','ИЭФ'),                   
									('МиОЛК','ТТЛП'),
									('ЛД','ТТЛП'),
									('ХТОВ','ТОВ'),          
									('ХТПД','ТОВ'),
									('ФХМиПККП','ТОВ');
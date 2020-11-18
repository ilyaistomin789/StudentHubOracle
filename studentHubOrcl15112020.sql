---
-- Globals
---

-- SQLINES DEMO *** AUTO_VALUE_ON_ZERO";
-- SQLINES DEMO *** HECKS=0;

---
-- Table 'users'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE users';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE users (
id NUMBER(10) DEFAULT NULL NULL,
login VARCHAR2(40) DEFAULT NULL NULL,
password VARCHAR2(50) DEFAULT NULL NULL,
role VARCHAR2(20) DEFAULT NULL NULL,
PRIMARY KEY (id)
);
alter table users add constraint role check ( role in ('student','deanery','teacher') );

-- Generate ID using sequence and trigger
CREATE SEQUENCE users_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER users_seq_tr
BEFORE INSERT ON users FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT users_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
//


--student info

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE student_info';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


CREATE TABLE student_info (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
student_name VARCHAR2(60) DEFAULT NULL NULL,
status varchar2(20) DEFAULT NULL NULL,
course NUMBER(10) DEFAULT NULL NULL,
num_group NUMBER(2) DEFAULT NULL NULL,
specialization VARCHAR2(20) DEFAULT NULL NULL,
faculty VARCHAR2(20) DEFAULT NULL NULL,
birthday DATE DEFAULT NULL NULL,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE student_info_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER student_info_seq_tr
BEFORE INSERT ON student_info FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT student_info_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/


---
-- Table 'admin_info'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE deanery_info';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE deanery_info (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
deanery_name varchar2(70) DEFAULT NULL NULL,
telephone VARCHAR2(20) DEFAULT NULL NULL,
faculty char(10) default null null,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE deanery_info_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER deanery_info_seq_tr
BEFORE INSERT ON deanery_info FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT deanery_info_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE teacher_info';
EXCEPTION
WHEN OTHERS THEN NULL;
END;

CREATE TABLE teacher_info (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
teacher_name varchar2(70) DEFAULT NULL NULL,
telephone VARCHAR2(20) DEFAULT NULL NULL,
faculty char(10) default null null,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE teacher_info_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER teacher_info_seq_tr
BEFORE INSERT ON teacher_info FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT teacher_info_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;



-----
-- SQLINES DEMO *** ogress'
--
-----

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE student_progress';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE
student_progress (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
subject CHAR(20) DEFAULT NULL NULL,
note NUMBER(2) DEFAULT NULL NULL,
progress_date DATE DEFAULT NULL NULL,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE student_progress_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER student_progress_seq_tr
BEFORE INSERT ON student_progress FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT student_progress_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

---
-- Table 'subjects'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE subjects';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE subjects (
id number,
subject CHAR(20) DEFAULT NULL NULL,
name VARCHAR2(50) DEFAULT NULL NULL,
faculty CHAR(10) DEFAULT NULL NULL,
PRIMARY KEY (subject)
);
-- Generate ID using sequence and trigger
CREATE SEQUENCE subjects_seq START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER subjects_seq_tr
BEFORE INSERT ON subjects FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT subjects_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/


---
-- Table 'faculty_specialization'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE faculty_specialization';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE faculty_specialization (
id number DEFAULT NULL NULL,
faculty char(10) DEFAULT NULL NULL,
specialization varchar2(20) DEFAULT NULL NULL,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE faculty_specialization_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER faculty_specialization_seq_tr
BEFORE INSERT ON faculty_specialization FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT faculty_specialization_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

---
-- Table 'faculty'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE faculty';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE faculty (
id number,
faculty CHAR(10) DEFAULT NULL NULL,
name NUMBER(10) DEFAULT NULL NULL,
PRIMARY KEY (faculty)
);
-- Generate ID using sequence and trigger
CREATE SEQUENCE faculty_seq START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER faculty_seq_tr
BEFORE INSERT ON faculty FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT faculty_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

---
-- SQLINES DEMO *** tion'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE specialization';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE specialization (
specialization VARCHAR2(20) DEFAULT NULL NULL,
name VARCHAR2(50) DEFAULT NULL NULL,
PRIMARY KEY (specialization)
);
-- Generate ID using sequence and trigger
CREATE SEQUENCE specialization_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER specialization_seq_tr
BEFORE INSERT ON specialization FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT specialization_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

---
-- Table 'adjustments'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE adjustments';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE adjustments (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
teacher_id NUMBER(10) DEFAULT NULL NULL,
subject CHAR(20) DEFAULT NULL NULL,
status varchar2(20) DEFAULT NULL NULL,
adjustment_date DATE DEFAULT NULL NULL,
access_date DATE DEFAULT NULL NULL,
img blob,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE adjustments_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER adjustments_seq_tr
BEFORE INSERT ON adjustments FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT adjustments_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

---
-- Table 'retakes'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE retakes';
EXCEPTION
WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE retakes (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
teacher_id NUMBER(10) DEFAULT NULL NULL,
subject CHAR(20) DEFAULT NULL NULL,
status varchar2(20) DEFAULT NULL NULL,
retake_date DATE DEFAULT NULL NULL,
access_date DATE DEFAULT NULL NULL,
img blob,
PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE retakes_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER retakes_seq_tr
BEFORE INSERT ON retakes FOR EACH ROW
WHEN (NEW.id IS NULL)
BEGIN
SELECT retakes_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/
-- ---
-- Table 'gaps'
--
-- ---

BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE gaps';
EXCEPTION
   WHEN OTHERS THEN NULL;
END;
/

CREATE TABLE gaps (
  id NUMBER(10) DEFAULT NULL NULL,
  user_id NUMBER(10) DEFAULT NULL NULL,
  subject CHAR(20) DEFAULT NULL NULL,
  gap_date DATE DEFAULT NULL NULL,
  PRIMARY KEY (id)
);

-- Generate ID using sequence and trigger
CREATE SEQUENCE gaps_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER gaps_seq_tr
 BEFORE INSERT ON gaps FOR EACH ROW
 WHEN (NEW.id IS NULL)
BEGIN
 SELECT gaps_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/
---
-- Table 'Timetable'
---
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE gaps';
EXCEPTION
   WHEN OTHERS THEN NULL;
END;/

CREATE TABLE timetable (
  id NUMBER(10) DEFAULT NULL NULL,
  teacher_id NUMBER(10) DEFAULT NULL NULL,
  subject CHAR(20) DEFAULT NULL NULL,
  start_time varchar2(4) DEFAULT NULL NULL,
  end_time varchar2(4) DEFAULT NULL NULL,
  PRIMARY KEY (id)
);
-- Generate ID using sequence and trigger
CREATE SEQUENCE timetable_seq START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER timetable_seq_tr
 BEFORE INSERT ON timetable FOR EACH ROW
 WHEN (NEW.id IS NULL)
BEGIN
 SELECT timetable_seq.NEXTVAL INTO :NEW.id FROM DUAL;
END;
/

-- ---
-- Foreign Keys
-- ---
ALTER TABLE deanery_info ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE deanery_info ADD FOREIGN KEY (faculty) REFERENCES faculty (faculty);
ALTER TABLE teacher_info ADD FOREIGN KEY (faculty) REFERENCES faculty (faculty);
ALTER TABLE teacher_info ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE student_info ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE student_info ADD FOREIGN KEY (specialization) REFERENCES specialization (specialization);
ALTER TABLE student_progress ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE student_progress ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE subjects ADD FOREIGN KEY (faculty) REFERENCES faculty (faculty);
ALTER TABLE faculty ADD FOREIGN KEY (specialization) REFERENCES specialization (specialization);
ALTER TABLE adjustments ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE adjustments ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE adjustments ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE retakes ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE retakes ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE retakes ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE gaps ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE gaps ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE timetable ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE faculty_specialization ADD FOREIGN KEY (faculty) references faculty (faculty);
ALTER TABLE faculty_specialization ADD FOREIGN KEY (specialization) references specialization (specialization) ;
-- ----
-- FUNCTIONS AND PROCEDURES
-- ----

CREATE OR REPLACE FUNCTION get_user_cursor(in_login in varchar2, in_password in varchar2)
return sys_refcursor
as
user_cur sys_refcursor;
    begin
    open user_cur for select * from users where login = in_login and password = in_password;
    return user_cur;
    end get_user_cursor;

CREATE OR REPLACE PROCEDURE findUser (in_login in users.login%TYPE, in_password in users.password%TYPE,user_cur out sys_refcursor)
IS
invalid_user EXCEPTION;
check_count number;
BEGIN
SELECT COUNT(*) into check_count from users where login = in_login and password = in_password;
    if check_count != 0 then user_cur := get_user_cursor(in_login, in_password);
    else raise invalid_user;
    end if;
    DBMS_OUTPUT.ENABLE();
    DBMS_SQL.RETURN_RESULT(user_cur);
    exception
    when invalid_user then
    DBMS_OUTPUT.PUT_LINE('Please, check that the information you entered is correct');
end findUser;

--add hash password
CREATE PROCEDURE addUser(in_login in users.login%TYPE, in_password in users.password%TYPE)
IS
user_exists number;
user_id users.id%TYPE;
curr_user_exists exception;
begin
    SELECT COUNT(*) into user_exists from users where users.login = in_login;
    if user_exists != 0 then raise curr_user_exists;
        else insert into users (login,password,role) values (in_login,in_password,'student');
    end if;
    SELECT MAX(id) into user_id from users;
    ADDEMPTYSTUDENT(user_id);
    exception
    when curr_user_exists then
    DBMS_OUTPUT.PUT_LINE('This user is exists');
end addUser;
drop procedure addUser;

CREATE OR REPLACE PROCEDURE addEmptyStudent(in_id in users.id%TYPE)
IS
begin
    insert into student_info(user_id, student_name, status, course, num_group, specialization, faculty, birthday) VALUES
    (in_id, null, 'student', null,null,null,null,null);
    commit;
end;

CREATE PROCEDURE findStudent(in_user_id in users.id%type, student out sys_refcursor)
IS
student_exists number;
curr_student_not_exists exception;
begin
select count(*) into student_exists from student_info where user_id = in_user_id;
if  student_exists != 0 then open student for select * from student_info where user_id = in_user_id;
else raise curr_student_not_exists;
end if;
exception when curr_student_not_exists then DBMS_OUTPUT.PUT_LINE('error when searching for information');
end findStudent;


select * from users;
select * from student_info;
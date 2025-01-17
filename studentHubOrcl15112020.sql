---
-- Globals
---

---
-- Table 'users'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE users';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


CREATE TABLE users (
id NUMBER(10) DEFAULT NULL NULL,
login VARCHAR2(40) DEFAULT NULL NULL,
password VARCHAR2(2000) DEFAULT NULL NULL,
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
faculty CHAR(10) DEFAULT NULL NULL,
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



---
-- Table 'deanery_info'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE deanery_info';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


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


---
-- Table 'subjects'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE subjects';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


CREATE TABLE subjects (
id number,
subject CHAR(20) DEFAULT NULL NULL,
name VARCHAR2(200) DEFAULT NULL NULL,
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



---
-- Table 'faculty_specialization'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE faculty_specialization';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


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


---
-- Table 'faculty'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE faculty';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


CREATE TABLE faculty (
id number,
faculty CHAR(10) DEFAULT NULL NULL,
name varchar2(80) DEFAULT NULL NULL,
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


---
-- SQLINES DEMO *** tion'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE specialization';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


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
filing_date DATE default null null,
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


---
-- Table 'retakes'
--
---

BEGIN
EXECUTE IMMEDIATE 'DROP TABLE retakes';
EXCEPTION
WHEN OTHERS THEN NULL;
END;


CREATE TABLE retakes (
id NUMBER(10) DEFAULT NULL NULL,
user_id NUMBER(10) DEFAULT NULL NULL,
teacher_id NUMBER(10) DEFAULT NULL NULL,
subject CHAR(20) DEFAULT NULL NULL,
status varchar2(20) DEFAULT NULL NULL,
retake_date DATE DEFAULT NULL NULL,
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

-- ---
-- Table 'gaps'
--
-- ---

BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE gaps';
EXCEPTION
   WHEN OTHERS THEN NULL;
END;


CREATE TABLE gaps (
  id NUMBER(10) DEFAULT NULL NULL,
  user_id NUMBER(10) DEFAULT NULL NULL,
  subject CHAR(20) DEFAULT NULL NULL,
  gap_date DATE DEFAULT NULL NULL,
  gaps_count number,
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

---
-- Table 'Timetable'
---
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE gaps';
EXCEPTION
   WHEN OTHERS THEN NULL;
END;

CREATE TABLE timetable (
  id NUMBER(10) DEFAULT NULL NULL,
  teacher_id NUMBER(10) DEFAULT NULL NULL,
  subject CHAR(20) DEFAULT NULL NULL,
  start_time varchar2(30) DEFAULT NULL NULL,
  end_time varchar2(30) DEFAULT NULL NULL,
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
ALTER TABLE adjustments ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE adjustments ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE adjustments ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE retakes ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE retakes ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE retakes ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE gaps ADD FOREIGN KEY (user_id) REFERENCES users (id);
ALTER TABLE gaps ADD FOREIGN KEY (subject) REFERENCES subjects (subject);
ALTER TABLE timetable ADD FOREIGN KEY (teacher_id) REFERENCES users (id);
ALTER TABLE faculty_specialization add foreign key (specialization) references specialization(specialization);
ALTER TABLE faculty_specialization add foreign key (faculty) references faculty(faculty);
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
encode_key varchar2(2000) := 'StudentHub123456';
encode_mode number;
encode_pass raw(2000);
BEGIN
encode_mode := DBMS_CRYPTO.ENCRYPT_AES128 + DBMS_CRYPTO.CHAIN_CBC + DBMS_CRYPTO.PAD_PKCS5;
encode_pass := DBMS_CRYPTO.ENCRYPT(utl_i18n.string_to_raw (in_password, 'AL32UTF8'), encode_mode,
    utl_i18n.string_to_raw (encode_key, 'AL32UTF8'));

SELECT COUNT(*) into check_count from users where login = in_login and password = encode_pass;
    if check_count != 0 then user_cur := get_user_cursor(in_login, encode_pass);
    else raise invalid_user;
    end if;
    DBMS_OUTPUT.ENABLE();
    DBMS_SQL.RETURN_RESULT(user_cur);
    exception
    when invalid_user then
    RAISE_APPLICATION_ERROR(-20005, 'Please, check that the information you entered is correct');
end findUser;

CREATE or replace PROCEDURE addUser(in_login in users.login%TYPE, in_password in users.password%TYPE)
IS
user_exists number;
user_id users.id%TYPE;
curr_user_exists exception;
encode_key varchar2(2000) := 'StudentHub123456';
encode_mode number;
encode_pass raw(2000);
begin
    encode_mode := DBMS_CRYPTO.ENCRYPT_AES128 + DBMS_CRYPTO.CHAIN_CBC + DBMS_CRYPTO.PAD_PKCS5;
    SELECT COUNT(*) into user_exists from users where users.login = in_login;
    encode_pass := DBMS_CRYPTO.ENCRYPT(utl_i18n.string_to_raw (in_password, 'AL32UTF8'),
        encode_mode, utl_i18n.string_to_raw (encode_key, 'AL32UTF8'));

    if user_exists != 0 then raise curr_user_exists;
        else insert into users (login,password,role) values (in_login,encode_pass,'student');
    end if;
    SELECT MAX(id) into user_id from users;
    ADDEMPTYSTUDENT(user_id);
    exception
    when curr_user_exists then
    raise_application_error(-20000, 'This user is exists');
end addUser;

CREATE OR REPLACE PROCEDURE addEmptyStudent(in_id in users.id%TYPE)
IS
begin
    insert into student_info(user_id, student_name, status, course, num_group, specialization, faculty, birthday) VALUES
    (in_id, null, 'student', null,null,null,null,null);
    commit;
end;

CREATE or replace PROCEDURE findStudent(in_user_id in users.id%type, student out sys_refcursor)
IS
student_exists number;
curr_student_not_exists exception;
begin
select count(*) into student_exists from student_info where user_id = in_user_id;
if  student_exists != 0 then open student for select * from student_info where user_id = in_user_id;
else raise curr_student_not_exists;
end if;
exception when curr_student_not_exists then
    RAISE_APPLICATION_ERROR(-20007, 'Error when searching for information');
end findStudent;

CREATE OR REPLACE PROCEDURE updateStudent(in_user_id users.id%type, in_student_name student_info.student_name%type,
in_course student_info.course%type, in_num_group student_info.num_group%type, in_specialization student_info.specialization%type, in_faculty student_info.faculty%type,
in_birthday student_info.birthday%type, new_user out sys_refcursor)
is
begin
    update student_info set student_name = in_student_name, course = in_course, num_group = in_num_group, specialization = in_specialization, faculty = in_faculty,
    birthday = in_birthday where user_id = in_user_id;
    commit;
    open new_user for select * from student_info where user_id = in_user_id;
end updateStudent;


create or replace procedure addAdjustment(in_user_id users.id%type, in_teacher_name teacher_info.teacher_name%type,
in_subject subjects.subject%type, in_filing_date
adjustments.filing_date%type, in_img blob)
IS
    curr_teacher_id number;
    check_adj number;
    adjustment_exists exception;
BEGIN
    SELECT user_id into curr_teacher_id from teacher_info where teacher_name = in_teacher_name;
        SELECT count(*) into check_adj from adjustments where user_id = in_user_id and
                                                              teacher_id = curr_teacher_id
                                                          and subject = in_subject and filing_date = in_filing_date;
    if check_adj = 0 then
    insert into adjustments(user_id, teacher_id, subject, status, filing_date, img) values(in_user_id, curr_teacher_id,
                                                                                           in_subject, 'in processing',in_filing_date,in_img);
    commit;
    else raise adjustment_exists;
    end if;
    exception when adjustment_exists then RAISE_APPLICATION_ERROR(-20001, 'This Adjustment is exists');
end;

create or replace procedure addRetake(in_user_id users.id%type,
in_teacher_name teacher_info.teacher_name%type, in_subject subjects.subject%type, in_retake_date
retakes.retake_date%type, in_img blob)
IS
    curr_teacher_id number;
    check_ret number;
    retake_exists exception;
BEGIN
    SELECT user_id into curr_teacher_id from teacher_info where teacher_name = in_teacher_name;
        SELECT count(*) into check_ret from retakes where user_id = in_user_id and
                                                          teacher_id = curr_teacher_id and subject = in_subject and retake_date = in_retake_date;
    if check_ret = 0 then
    insert into retakes(user_id, teacher_id, subject, status, retake_date, img) values(in_user_id, curr_teacher_id,
                                                                                           in_subject, 'in processing',in_retake_date,in_img);
    commit;
    else raise retake_exists;
    end if;
    exception when retake_exists then RAISE_APPLICATION_ERROR(-20002, 'This Retake is exists');
end;

create or replace procedure findDeanery(in_user_id users.id%type, deanery out sys_refcursor)
is
    deanery_exists number;
    deanery_exception exception;
begin
    select count(*) into deanery_exists from deanery_info where user_id = in_user_id;
    if deanery_exists != 0 then open deanery for select * from deanery_info where user_id = in_user_id;
    else raise deanery_exception;
    end if;
    exception when deanery_exception then RAISE_APPLICATION_ERROR(-20008, 'error when searching for information');
end;

create or replace procedure findTeacher(in_user_id users.id%type, teacher out sys_refcursor)
is
    teacher_exists number;
    teacher_exception exception;
begin
    select count(*) into teacher_exists from teacher_info where user_id = in_user_id;
    if teacher_exists != 0 then open teacher for select * from teacher_info where user_id = in_user_id;
    else raise teacher_exception;
    end if;
    exception when teacher_exception then RAISE_APPLICATION_ERROR(-20008, 'error when searching for information');
end;

create or replace procedure setGaps(in_student_name student_info.student_name%type,in_subject subjects.subject%type, in_gaps_count gaps.gaps_count%type,
in_gap_date gaps.gap_date%type)
is
    student_id number;
begin
    select user_id into student_id from student_info where student_name = in_student_name;
    insert into gaps (user_id, subject, gap_date, GAPS_COUNT) values (student_id, in_subject, in_gap_date, in_gaps_count);
end setGaps;

create or replace procedure accept_decline_Adjustment(in_student_name student_info.student_name%type, in_subject subjects.subject%type,
in_filing_date adjustments.filing_date%type, action boolean)
is
    curr_user_id users.id%type;
begin
    select user_id into curr_user_id from student_info where student_name = in_student_name;
    if action = true then
        update adjustments set status = 'accept/decline' where user_id = curr_user_id and subject = in_subject
        and filing_date = in_filing_date;
    else
        update adjustments set status = 'decline' where user_id = curr_user_id and subject = in_subject
        and filing_date = in_filing_date;
    end if;
end;

create or replace procedure accept_decline_Retake(in_student_name student_info.student_name%type, in_subject subjects.subject%type,
in_retake_date retakes.retake_date%type, action boolean)
is
    curr_user_id users.id%type;
begin
    select user_id into curr_user_id from student_info where student_name = in_student_name;
    if action = true then
        update retakes set status = 'accept' where user_id = curr_user_id and subject = in_subject
        and retake_date = in_retake_date;
    else
        update retakes set status = 'decline' where user_id = curr_user_id and subject = in_subject
        and retake_date = in_retake_date;
    end if;
end;
    select * from retakes;

create or replace procedure setRatings(in_student_name student_info.student_name%type,in_subject subjects.subject%type, in_note student_progress.note%type,
in_progress_date student_progress.progress_date%type, in_comment student_progress.feedback%type)
is
    curr_user_id users.id%type;
begin
    select user_id into curr_user_id from student_info where student_name = in_student_name;
    insert into student_progress(user_id, subject, note, progress_date,FEEDBACK) values (curr_user_id, in_subject, in_note, in_progress_date, in_comment);
end setRatings;


create or replace procedure accept_decline_Adjustment_Teacher(in_student_name student_info.student_name%type, in_subject subjects.subject%type, in_teacher_id
users.id%type, in_adjustment_date adjustments.adjustment_date%type, in_filing_date adjustments.filing_date%type, action boolean)
is
    curr_student_id number;
begin
    select user_id into curr_student_id from student_info where student_name = in_student_name;
    if action = true then
    update adjustments set status = 'accept/accept', access_date = TO_CHAR(SYSDATE, 'DD.MM.YYYY'), adjustment_date = in_adjustment_date
    where subject = in_subject and teacher_id = in_teacher_id and
    user_id = curr_student_id and filing_date = in_filing_date;
    else
    update adjustments set status = 'accept/decline', access_date = null
    where subject = in_subject and teacher_id = in_teacher_id and
    user_id = curr_student_id and filing_date = in_filing_date;
    end if;
    commit;
end accept_decline_Adjustment_Teacher;
    select * from student_info;
select a.img from adjustments a inner join student_info s on a.user_id = s.user_id where
s.student_name = :in_student_name and s.faculty = :in_faculty and a.subject = :in_subject;

create or replace procedure deleteAdjustment(in_student_name student_info.student_name%type, in_subject subjects.subject%type,
in_filing_date adjustments.filing_date%type)
is
    curr_user_id users.id%type;
begin
    select user_id into curr_user_id from student_info where student_name = in_student_name;
    delete adjustments where user_id = curr_user_id and subject = in_subject and filing_date = in_filing_date;
    commit;
end;
-- ----
-- GENERATE
-- ----
declare
    i number(8):=0;
 procedure generateUsers
is
begin
    while i< 100000
    loop
        addUser('user'||i,'user'||i);
        i:= i+1;
    end loop;
    commit;
    exception when others then rollback;
end generateUsers;
begin
generateUsers;
end;

-- ----
-- XML
-- ----

CREATE OR REPLACE DIRECTORY UTLDATA AS 'A:/XML';
DROP DIRECTORY UTLDATA;


CREATE OR REPLACE PACKAGE XML_PACKAGE IS
  PROCEDURE EXPORT_USERS_TO_XML;
  PROCEDURE IMPORT_USERS_FROM_XML;
END XML_PACKAGE;

CREATE OR REPLACE PACKAGE BODY XML_PACKAGE IS

PROCEDURE EXPORT_USERS_TO_XML
IS
  DOC  DBMS_XMLDOM.DOMDocument;
  XDATA  XMLTYPE;
  CURSOR XMLCUR IS
    SELECT XMLELEMENT("USERS",
      XMLAttributes('http://www.w3.org/2001/XMLSchema' AS "xmlns:xsi",
      'http://www.oracle.com/Employee.xsd' AS "xsi:nonamespaceSchemaLocation"),
      XMLAGG(XMLELEMENT("USER",
        XMLELEMENT("ID",U.ID),
        XMLELEMENT("LOGIN",U.LOGIN),
        XMLELEMENT("PASSWORD",U.PASSWORD),
        XMLELEMENT("ROLE",U.ROLE)
      ))
) FROM USERS U;
BEGIN
  OPEN XMLCUR;
    LOOP
      FETCH XMLCUR INTO XDATA;
    EXIT WHEN XMLCUR%NOTFOUND;
    END LOOP;
  CLOSE XMLCUR;
  DOC := DBMS_XMLDOM.NewDOMDocument(XDATA);
  DBMS_XMLDOM.WRITETOFILE(DOC, 'UTLDATA/users.xml');
END EXPORT_USERS_TO_XML;

PROCEDURE IMPORT_USERS_FROM_XML
IS
  L_CLOB CLOB;
  L_BFILE  BFILE := BFILENAME('UTLDATA', 'users.xml');

  L_DEST_OFFSET   INTEGER := 1;
  L_SRC_OFFSET    INTEGER := 1;
  L_BFILE_CSID    NUMBER  := 0;
  L_LANG_CONTEXT  INTEGER := 0;
  L_WARNING       INTEGER := 0;

  P                DBMS_XMLPARSER.PARSER;
  V_DOC            DBMS_XMLDOM.DOMDOCUMENT;
  V_ROOT_ELEMENT   DBMS_XMLDOM.DOMELEMENT;
  V_CHILD_NODES    DBMS_XMLDOM.DOMNODELIST;
  V_CURRENT_NODE   DBMS_XMLDOM.DOMNODE;

  U USERS%ROWTYPE;
BEGIN
  DBMS_LOB.CREATETEMPORARY (L_CLOB, TRUE);
  DBMS_LOB.FILEOPEN(L_BFILE, DBMS_LOB.FILE_READONLY);

  DBMS_LOB.LOADCLOBFROMFILE (DEST_LOB => L_CLOB, SRC_BFILE => L_BFILE, AMOUNT => DBMS_LOB.LOBMAXSIZE,
    DEST_OFFSET => L_DEST_OFFSET, SRC_OFFSET => L_SRC_OFFSET, BFILE_CSID => L_BFILE_CSID,
    LANG_CONTEXT => L_LANG_CONTEXT, WARNING => L_WARNING);
  DBMS_LOB.FILECLOSE(L_BFILE);
  COMMIT;

   P := DBMS_XMLPARSER.NEWPARSER;

   DBMS_XMLPARSER.PARSECLOB(P,L_CLOB);

   V_DOC := DBMS_XMLPARSER.GETDOCUMENT(P);

   V_ROOT_ELEMENT := DBMS_XMLDOM.Getdocumentelement(v_Doc);

   V_CHILD_NODES := DBMS_XMLDOM.GETCHILDRENBYTAGNAME(V_ROOT_ELEMENT,'*');

   FOR i IN 0 .. DBMS_XMLDOM.GETLENGTH(V_CHILD_NODES) - 1
   LOOP

      V_CURRENT_NODE := DBMS_XMLDOM.ITEM(V_CHILD_NODES,i);

      DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
        'ID/text()',U.ID);
      DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
        'LOGIN/text()',U.LOGIN);
      DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
        'PASSWORD/text()',U.PASSWORD);
      DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
        'ROLE/text()',U.ROLE);

      INSERT INTO USERS(id, login, password, role)
        VALUES(u.id,u.login,u.password,u.role) ;
   END LOOP;

  DBMS_LOB.FREETEMPORARY(L_CLOB);
  DBMS_XMLPARSER.FREEPARSER(P);
  DBMS_XMLDOM.FREEDOCUMENT(V_DOC);
  COMMIT;
EXCEPTION
  WHEN OTHERS THEN
  DBMS_LOB.FREETEMPORARY(L_CLOB);
  DBMS_XMLPARSER.FREEPARSER(P);
  DBMS_XMLDOM.FREEDOCUMENT(V_DOC);
  RAISE_APPLICATION_ERROR(-20101, 'IMPORT XML ERROR'|| SQLERRM);
END IMPORT_USERS_FROM_XML;

END XML_PACKAGE;

declare
begin
    XML_PACKAGE.EXPORT_USERS_TO_XML();
    --XML_PACKAGE.IMPORT_USERS_FROM_XML();
end;

-- ----
-- INDEXES
-- ----

create index userid_student on student_info(user_id);
create index teacher_name_index on teacher_info(teacher_name);

-- ----
-- INSERTS
-- ----
insert all
    into  Faculty(Faculty, name) values	('ХТиТ', 'Химическая технология и техника')
	into  Faculty(Faculty, name) values ('ЛХФ', 'Лесохозяйственный факультет')
	into  Faculty(Faculty, name) values	('ИЭФ', 'Инженерно-экономический факультет')
	into  Faculty(Faculty, name) values('ТТЛП', 'Технология и техника лесной промышленности')
	into  Faculty(Faculty, name) values('ТОВ', 'Технология органических веществ')
	into  Faculty(Faculty, name) values('ИТ', 'Факультет информационных технологий')
	into  Faculty(Faculty, name) values('ИДиП', 'Издательское дело и полиграфия')
	SELECT * from dual;

insert all
    into subjects (subject, name, faculty) values ('СУБД', 'Системы управления базами данных', 'ИТ')
	into subjects (subject, name, faculty) values ('ООТПиСП', 'Объектно-ориентированные технологии программирования и структуры проектирвоания', 'ИТ')
	into subjects (subject, name, faculty) values ('СТПВI', 'Современные технологии программирования в Internet', 'ИТ')
	into subjects (subject, name, faculty) values ('БД', 'Базы данных','ИТ')
	into subjects (subject, name, faculty) values ('ИНФ', 'Информационные технологии','ИТ')
	into subjects (subject, name, faculty) values ('ОАиП', 'Основы алгоритмизации и программирования', 'ИТ')
	into subjects (subject, name, faculty) values ('ПЗ', 'Представление знаний в компьютерных системах', 'ИТ')
	into subjects (subject, name, faculty) values ('ПСП', 'Программирование сетевых приложений','ИТ')
	into subjects (subject, name, faculty) values ('МСОИ', 'Моделирование систем обработки информации', 'ИТ')
	into subjects (subject, name, faculty) values ('ПИС', 'Проектирование информационных систем', 'ИТ')
	into subjects (subject, name, faculty) values ('КГ', 'Компьютерная геометрия ','ИТ')
	into subjects (subject, name, faculty) values ('ПМАПЛ', 'Полиграф. машины, автоматы и поточные линии', 'ИДиП')
	into subjects (subject, name, faculty) values ('КМС', 'Компьютерные мультимедийные системы', 'ИТ')
	into subjects (subject, name, faculty) values ('ОПП', 'Организация полиграф. производства', 'ИДиП')
	into subjects (subject, name, faculty) values ('ДМ', 'Дискретная математика', 'ИТ')
	into subjects (subject, name, faculty) values ('МП', 'Математическое программирование','ИТ')
	into subjects (subject, name, faculty) values ('ЛЭВМ', 'Логические основы ЭВМ',  'ИТ')
	into subjects (subject, name, faculty) values ('ООП', 'Объектно-ориентированное программирование', 'ИТ')
	into subjects (subject, name, faculty) values ('ЭП', 'Экономика природопользования','ИЭФ')
	into subjects (subject, name, faculty) values ('ЭТ', 'Экономическая теория','ИЭФ')
	into subjects (subject, name, faculty) values ('БЛЗиПсOO','Биология лесных зверей и птиц с осн. охотов.','ЛХФ')
	into subjects (subject, name, faculty) values ('ОСПиЛПХ','Основы садово-паркового и лесопаркового хозяйства',  'ЛХФ')
	into subjects (subject, name, faculty) values ('ИГ', 'Инженерная геодезия ','ЛХФ')
	into subjects (subject, name, faculty) values ('ЛВ', 'Лесоводство', 'ЛХФ')
	into subjects (subject, name, faculty) values ('ОХ', 'Органическая химия', 'ТОВ')
	into subjects (subject, name, faculty) values ('ВТЛ', 'Водный транспорт леса','ТТЛП')
	into subjects (subject, name, faculty) values ('ТиОЛ', 'Технология и оборудование лесозаготовок', 'ТТЛП')
	into subjects (subject, name, faculty) values ('ТОПИ', 'Технология обогащения полезных ископаемых ','ХТиТ')
	select * from dual;

insert all
    into specialization(specialization, name) values ('ПОИТ',null)
	into specialization(specialization, name) values ('ИСИТ',null)
	into specialization(specialization, name) values ('ДЭИВИ',null)
	into specialization(specialization, name) values ('ПОИБМС',null)
	into specialization(specialization, name) values ('ИД',null)
	into specialization(specialization, name) values ('ПОиСОИ',null)
	into specialization(specialization, name) values ('КиПИИКМ',null)
	into specialization(specialization, name) values ('МиАХПиПСМ',null)
	into specialization(specialization, name) values ('ЛХ',null)
	into specialization(specialization, name) values ('СПС',null)
	into specialization(specialization, name) values ('ТиП',null)
	into specialization(specialization, name) values ('ЭиУНП',null)
	into specialization(specialization, name) values ('БУАиА',null)
	into specialization(specialization, name) values ('МиОЛК',null)
	into specialization(specialization, name) values ('ЛД',null)
	into specialization(specialization, name) values ('ХТОВ',null)
	into specialization(specialization, name) values ('ХТПД',null)
	into specialization(specialization, name) values ('ФХМиПККП',null)
	SELECT * from dual;
insert all
    into faculty_specialization(specialization, faculty) values	('ПОИТ','ИТ')
	into faculty_specialization(specialization, faculty) values	('ИСИТ','ИТ')
	into faculty_specialization(specialization, faculty) values	('ДЭИВИ','ИТ')
	into faculty_specialization(specialization, faculty) values	('ПОИБМС','ИТ')
	into faculty_specialization(specialization, faculty) values	('ИД','ИДиП')
	into faculty_specialization(specialization, faculty) values	('ПОиСОИ','ИДиП')
	into faculty_specialization(specialization, faculty) values	('КиПИИКМ','ХТиТ')
	into faculty_specialization(specialization, faculty) values	('МиАХПиПСМ','ХТиТ')
	into faculty_specialization(specialization, faculty) values	('ЛХ','ЛХФ')
	into faculty_specialization(specialization, faculty) values	('СПС','ЛХФ')
	into faculty_specialization(specialization, faculty) values	('ТиП','ЛХФ')
	into faculty_specialization(specialization, faculty) values	('ЭиУНП','ИЭФ')
	into faculty_specialization(specialization, faculty) values	('БУАиА','ИЭФ')
	into faculty_specialization(specialization, faculty) values	('МиОЛК','ТТЛП')
	into faculty_specialization(specialization, faculty) values	('ЛД','ТТЛП')
	into faculty_specialization(specialization, faculty) values	('ХТОВ','ТОВ')
	into faculty_specialization(specialization, faculty) values	('ХТПД','ТОВ')
	into faculty_specialization(specialization, faculty) values	('ФХМиПККП','ТОВ')
	select * from dual;


select * from users;
select * from teacher_info;
select * from student_info;
select * from student_progress;
select * from deanery_info;
select * from adjustments;
select * from retakes;
select * from subjects;
select * from faculty;
select * from gaps;
select * from specialization;
select * from faculty_specialization;


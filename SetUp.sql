Drop TABLE TEACHERS CASCADE;
DROP TABLE CLASSES CASCADE;
DROP TABLE COURSES CASCADE;
DROP  TABLE STUDENTS CASCADE;
DROP TABLE STUDENT_COURSES CASCADE;


Create TABLE TEACHERS
(
    ID        varchar(64) PRIMARY KEY,
    NAME      varchar(64),
    FIRSTNAME varchar(64),
    BDATE     Timestamp,
    GENDER    integer,
    SALARY    integer,
    HDATE     timestamp
);

CREATE TABLE CLASSES(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    KTEACHER varchar(64)  references TEACHERS(ID)

);

CREATE TABLE COURSES(
    ID varchar(64) PRIMARY KEY ,
    HACTIVE integer,
    NAME varchar(64),
    KTEACHER varchar(64)  references TEACHERS(ID)

);

Create TABLE STUDENTS(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    FIRSTNAME varchar(64),
    BDATE Timestamp,
    GENDER integer,
    GRADE int,
    KCLASS varchar(64) Null
    );

Create TABLE STUDENT_COURSES(
    KSTUDENT varchar(64) REFERENCES STUDENTS(id) ,
    KCOURSE varchar(64) REFERENCES COURSES(id)
    );

SELECT * From STUDENT_COURSES

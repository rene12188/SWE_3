Drop TABLE TEACHERS CASCADE ;
DROP TABLE CLASSES CASCADE ;
DROP TABLE COURSES CASCADE ;
DROP  TABLE STUDENT CASCADE ;
DROP TABLE STUDENT_COURSES CASCADE ;

CREATE TYPE gender AS ENUM ('Male', 'Female');

Create TABLE TEACHERS
(
    ID        varchar(64) PRIMARY KEY,
    NAME      varchar(64),
    FIRSTNAME varchar(64),
    BDATE     Timestamp,
    ISMALE    boolean,
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



Create TABLE STUDENT(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    FIRSTNAME varchar(64),
    BDATE Timestamp,
    ISMALE boolean,
    SALARY integer,
    GRADE int,
    KCLASS varchar(64)
    );

Create TABLE STUDENT_COURSES(
    KSTUDENT varchar(64),
    KCOURSE varchar(64)
    );
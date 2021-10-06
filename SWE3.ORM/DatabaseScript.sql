DROP TABLE Person CASCADE ;
DROP TABLE Teacher CASCADE ;
DROP TABLE Student CASCADE ;
DROP TABLE Class CASCADE ;
DROP TABLE COURSE CASCADE ;
DROP TABLE Student_Course CASCADE ;


Create TABLE Person(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    FIRSTNAME varchar(64),
    GENDER integer,
    BDATE Timestamp
);

create Table Teacher(
    KPERSON varchar(64) REFERENCES Person(ID) PRIMARY KEY ,
    HDATE timestamp,
    SALARY int
);

create table Class(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    KTEACHER varchar(64) REFERENCES Teacher(KPERSON)

);

create Table Student(
    KPERSON varchar(64) references Person(ID) PRIMARY KEY ,
    KCLASS varchar(64) REFERENCES Class(ID),
    GRADE int
);




create table COURSE(
    ID varchar(64) PRIMARY KEY,
    HACTIVE int,
    NAME varchar(64),
    KTEACHER varchar(64) REFERENCES Teacher(KPERSON)

);

create table Student_Course(
    ID SERIAL PRIMARY KEY,
    KSTUDENT varchar(64) REFERENCES Student(KPERSON),
    KCOURS varchar(64) REFERENCES  COURSE(ID)


);

INSERT INTO Person(ID, NAME,FIRSTNAME,GENDER,BDATE)VALUES ('if19b097',' Hallo', 'Person', 0, now())
INSERT INTO PERSON (ID,Name,FirstName,BDATE) Values ('if19b09888','Mr Placeholder','John','17.09.2021 00:00:00') ;
Select * from Person;
Select * from Teacher;
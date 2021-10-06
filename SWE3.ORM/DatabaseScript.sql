DROP TABLE Persons CASCADE ;
DROP TABLE Teacher CASCADE ;
DROP TABLE Student CASCADE ;
DROP TABLE Class CASCADE ;

Create TABLE Persons(
    ID serial PRIMARY KEY ,
    NAME varchar(64),
    FIRSTNAME varchar(64),
    GENDER integer,
    BDATE Timestamp
);

create Table Teacher(
    KPERSON varchar(64) PRIMARY KEY ,
    HDATE timestamp,
    SALARY int,
    ID int REFERENCES Persons(ID)
);

create table Class(
    ID varchar(64) PRIMARY KEY ,
    NAME varchar(64),
    KTEACHER varchar(64) REFERENCES Teacher(KPERSON)

);

create Table Student(
    KPERSON varchar(64) PRIMARY KEY ,
    KCLASS varchar(64) REFERENCES Class(ID),
    GRADE int,
    ID int  REFERENCES Persons(ID)

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


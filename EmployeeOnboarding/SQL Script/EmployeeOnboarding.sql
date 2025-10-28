CREATE DATABASE EmployeesOnBoardingProcess;

--Use Database
USE EmployeesOnBoardingProcess;


--Department Table
CREATE TABLE Department(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	DepartmentName VARCHAR(20) NOT NULL,
);

--Location Table
CREATE TABLE [Location](
	Id INT IDENTITY(1,1) PRIMARY KEY,
	LocationName VARCHAR(20) NOT NULL
);

--Role Table
CREATE TABLE [Role](
	Id INT IDENTITY(1,1) PRIMARY KEY,
	DepartmentId INT NOT NULL,
	RoleName VARCHAR(40) NOT NULL,
	CONSTRAINT Fk_Role_Department FOREIGN KEY (DepartmentId) REFERENCES Department(Id)
);

--Employee Table
CREATE TABLE Employee(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	SubmissionId INT NOT NULL,
	FirstName VARCHAR(20) NOT NULL,
	MiddleName VARCHAR(20) NULL,
	LastName VARCHAR(20) NOT NULL,
	Email VARCHAR(30) NOT NULL UNIQUE,
	PhoneNumber VARCHAR(10) NOT NULL,
	DepartmentId INT NOT NULL,
	RoleId INT NOT NULL,
	LocationId INT NOT NULL,
	Experience INT NOT NULL,
	JoiningDate DATE NOT NULL,
	CTC INT NOT NULL,
	[Status] VARCHAR(30) NULL,
	ProbationEndDate Date NOT NULL,
	CONSTRAINT Fk_Employee_Department FOREIGN KEY (DepartmentId) REFERENCES Department(Id),
	CONSTRAINT Fk_Employee_Location FOREIGN KEY (LocationId) REFERENCES [Location](Id),
	CONSTRAINT Fk_Employee_Role FOREIGN KEY (RoleId) REFERENCES [Role](Id)
);


--Meta Log Table
CREATE TABLE MetaLog(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeId INT NOT NULL,
	DepartmentId INT NOT NULL,
	RoleId INT NOT NULL,
	JoiningDate DATE NOT NULL,
	CONSTRAINT Fk_MetaLog_Department FOREIGN KEY (DepartmentId) REFERENCES Department(Id),
	CONSTRAINT Fk_MetaLog_Role FOREIGN KEY (RoleId) REFERENCES [Role](Id)
);

--Hod Table
CREATE TABLE Hod(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(20) NOT NULL,
	MiddleName VARCHAR(20) NULL,
	LastName VARCHAR(20) NOT NULL,
	Email VARCHAR(30) NOT NULL UNIQUE,
	PhoneNumber VARCHAR(10) NOT NULL,
	DepartmentId INT NOT NULL,
	LocationId INT NOT NULL,
	Experience INT NOT NULL,
	JoiningDate DATE NOT NULL,
	CONSTRAINT Fk_Hod_Department FOREIGN KEY (DepartmentId) REFERENCES Department(Id),
	CONSTRAINT Fk_Hod_Location FOREIGN KEY (LocationId) REFERENCES [Location](Id),
);


-- Master Data
INSERT INTO Department (DepartmentName) VALUES ('IT'),('HR'),('Finace'),('Marketing');

INSERT INTO [Role] (DepartmentId, RoleName) VALUES (1,'Developer'),(1,'Tester'),(1,'DevOps');

INSERT INTO [Role] (DepartmentId, RoleName) VALUES (2,'Recruiter');

INSERT INTO [Role] (DepartmentId, RoleName) VALUES (3,'Payroll Executive'),(3,'Accounts Officer');

INSERT INTO [Role] (DepartmentId, RoleName) VALUES (4,'Marketing Analyst'),(4,'Business Consultant');

INSERT INTO [Location] (LocationName) VALUES ('Bangalore'),('Pune'),('Hyderabad'),('Delhi'),('Chennai');

INSERT INTO [Employee] (SubmissionId, FirstName, MiddleName, LastName, Email, PhoneNumber, DepartmentId, RoleId, LocationId, Experience,
 JoiningDate, CTC, [Status], ProbationEndDate ) VALUES
(1001, 'Imran', NULL, 'Khan', 'imran2@example.com', '9876543210', 1, 1, 1, 2, '2023-08-01', 600000, 'Active', '2024-02-01');

INSERT INTO Hod (FirstName, MiddleName, LastName, Email, PhoneNumber, DepartmentId, LocationId, Experience, JoiningDate)
VALUES
('Ram', NULL, 'Sundar', 'ram@dept1.com', '987456321', 1, 1, 10, '2015-06-01'),
('Adil', NULL, 'Basha', 'adil@dept1.com', '987456321', 1, 2, 8, '2016-05-01'),

('Santhosh', NULL, 'Kumar', 'santhosh@dept2.com', '987456321', 2, 2, 9, '2014-05-01'),
('Gokul', NULL, 'Raja', 'gokul@dept2.com', '987456321', 2, 3, 7, '2017-05-01'),

('Karthik', NULL, 'Ram', 'karthik@dept3.com', '987456321', 3, 3, 11, '2013-05-01'),
('Murugan', NULL, 'Durai', 'durai@dept3.com', '987456321', 3, 4, 9, '2015-05-01'),

('Sri', NULL, 'Ram', 'sriram@dept4.com', '987456321', 4, 4, 10, '2012-05-01'),
('Ashik', NULL, 'Jamsheer', 'ashik@dept4.com', '987456321', 4, 5, 8, '2016-05-01');

-- Select
SELECT * FROM [Employee];
SELECT * FROM [MetaLog];
SELECT * FROM [Location];
SELECT * FROM [Hod];
SELECT * FROM [Role];
SELECT * FROM [Department];
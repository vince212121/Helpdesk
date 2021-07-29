-- create new query in VS2019 SQL Server Object Explorer
-- on the existing helpdeskDb before proceeding

DROP TABLE Problems
GO
DROP TABLE Employees
GO
DROP TABLE Departments
GO
-- uncomment these lines if you need to reset call data
/*
DROP TABLE Calls
*/

-- Problem Table with Constraints
CREATE TABLE Problems
( Id INT IDENTITY(1,1) NOT NULL,
  Description VARCHAR(50),
  Timer ROWVERSION,
  CONSTRAINT PK_Problem PRIMARY KEY(Id)  
)
GO

-- Let's load problem with some data (identity means we don't load id)
INSERT INTO Problems (Description) VALUES ('Device Not Plugged In')
INSERT INTO Problems (Description) VALUES ('Device Not Turned On')
INSERT INTO Problems (Description) VALUES ('Hard Drive Failure')
INSERT INTO Problems (Description) VALUES ('Memory Failure')
INSERT INTO Problems (Description) VALUES ('Power Supply Failure')
INSERT INTO Problems (Description) VALUES ('Password fails due to Caps Lock being on')
INSERT INTO Problems (Description) VALUES ('Network Card Faulty')
INSERT INTO Problems (Description) VALUES ('Cpu Fan Failure')
INSERT INTO Problems (Description) VALUES ('Memory Upgrade')
INSERT INTO Problems (Description) VALUES ('Graphics Upgrade')
INSERT INTO Problems (Description) VALUES ('Needs software upgrade')
GO

-- Department Table with Constraints
CREATE TABLE Departments
( Id INT IDENTITY(100,100) NOT NULL,
  DepartmentName VARCHAR(50),
  Timer ROWVERSION,
  CONSTRAINT PK_Department PRIMARY KEY(Id)  
)
GO

-- add the department data
INSERT INTO Departments (DepartmentName) VALUES ('Administration')
INSERT INTO Departments (DepartmentName) VALUES ('Sales')
INSERT INTO Departments (DepartmentName) VALUES ('Food Services')
INSERT INTO Departments (DepartmentName) VALUES ('Lab')
INSERT INTO Departments (DepartmentName) VALUES ('Maintenance')
GO

-- Employees Table with Constraints
CREATE TABLE Employees
( Id INT IDENTITY(1,1) NOT NULL,
  Title VARCHAR(4),
  FirstName VARCHAR(50),
  LastName VARCHAR(50),
  PhoneNo VARCHAR(25),
  Email VARCHAR(50),
  DepartmentId INT NOT NULL,
  IsTech BIT NULL,
  StaffPicture VARBINARY(MAX) NULL,
  Timer ROWVERSION,
  CONSTRAINT PK_Employee PRIMARY KEY(Id),
  CONSTRAINT FK_EmployeeInDept FOREIGN KEY(DepartmentId) REFERENCES Departments(Id)
)
GO

-- add initial data
INSERT INTO Employees (Title,FirstName,LastName,PhoneNo,Email,DepartmentId) VALUES ('Mr.','David','Williams','(555) 555-5551','pg@abc.com',100)
INSERT INTO Employees (Title,FirstName,LastName,PhoneNo,Email,DepartmentId) VALUES ('Mrs.','Penny','Johnson','(555) 555-5551','pj@abc.com',100)
INSERT INTO Employees (Title,FirstName,LastName,PhoneNo,Email,DepartmentId) VALUES ('Mr.','John','Smith','(555) 555-5552','js@abc.com',200)
INSERT INTO Employees (Title,FirstName,LastName,PhoneNo,Email,DepartmentId) VALUES ('Mr.','Evan','Smith','(555) 555-5552','es@abc.com',200)

-- Call Table with Constraints
CREATE TABLE Calls
( Id INT IDENTITY(100,100) NOT NULL, -- start at 100, increment by 100
  EmployeeId INT NOT NULL,
  ProblemId INT NOT NULL,
  TechId INT NOT NULL,
  DateOpened SMALLDATETIME NOT NULL,
  DateClosed SMALLDATETIME,
  OpenStatus BIT NOT NULL,
  Notes VARCHAR(250) NOT NULL,
  Timer ROWVERSION,
  CONSTRAINT PK_Call PRIMARY KEY(Id),
  CONSTRAINT FK_CallHasTech FOREIGN KEY(TechId) REFERENCES Employees(Id),
  CONSTRAINT FK_CallHasEmployee FOREIGN KEY(EmployeeId) REFERENCES Employees(Id),
  CONSTRAINT FK_CallHasProblem FOREIGN KEY(ProblemId) REFERENCES Problems(Id)  
)
GO
SELECT COUNT(*) AS Problems FROM Problems
GO
SELECT COUNT(*) Departments FROM Departments
GO
SELECT COUNT(*) AS Employees FROM Employees
GO
SELECT COUNT(*) AS Calls FROM Calls










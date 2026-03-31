USE master

USE MediRecordsTestDB

SELECT * FROM [User];
SELECT * FROM UserRole


INSERT INTO UserRole (Name)
VALUES
    ('FrontDesk'),
    ('Nurse'),
    ('Physician'),
    ('LabTech'),
    ('Admin');

INSERT INTO [User] (Name, RoleId, Email, Phone, Password, [Status])
VALUES
    ('Ramesh', 5,'ramesh@test.com', '1234567890', 'hashed_password', 1);

INSERT INTO [User] (Name, RoleId, Email, Phone, Password, [Status])
VALUES
    ('Rani', 1,'rani@test.com', '0987654321', 'rani_hashed_password', 1);


UPDATE [User]
SET RoleId = 5
WHERE UserId = 2 AND RoleId = 1;

SELECT u.UserId, u.Name, ur.Name AS RoleName
FROM [User] u
JOIN UserRole ur ON u.RoleId = ur.RoleId;

DROP DATABASE IF EXISTS MediRecordsTestDB;
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

INSERT INTO Patient
(MRN, Name, DOB, Gender, ContactInfo, Status, PrimaryProviderId)
VALUES
('MRN004', 'Akhila', '1998-05-10', 'Female', '8888888888', 'Active', 3),
('MRN005', 'Rahul',  '1995-02-15', 'Male',   '7777777777', 'Active', 3),
('MRN006', 'Sneha',  '2000-08-20', 'Female', '6666666666', 'Active', 3);
INSERT INTO ProviderSchedule
(ProviderId, DayOfWeek, StartTime, EndTime, SlotDuration, Status)
VALUES
-- Monday
(3, 'Monday', '2026-04-06 09:00:00', '2026-04-06 12:00:00', 30, 1),

-- Tuesday
(3, 'Tuesday', '2026-04-07 10:00:00', '2026-04-07 13:00:00', 30, 1),

-- Wednesday
(3, 'Wednesday', '2026-04-08 09:00:00', '2026-04-08 11:00:00', 20, 1);
INSERT INTO ProviderSchedule
(ProviderId, DayOfWeek, StartTime, EndTime, SlotDuration, Status)
VALUES


-- Provider 2
(2, 'Monday',    '2026-04-06 08:00:00', '2026-04-06 11:00:00', 30, 1),
(2, 'Thursday',  '2026-04-09 09:00:00', '2026-04-09 12:00:00', 20, 1),

-- Provider 1
(1, 'Tuesday',   '2026-04-07 14:00:00', '2026-04-07 17:00:00', 30, 1),
(1, 'Friday',    '2026-04-10 09:00:00', '2026-04-10 12:00:00', 30, 1);
INSERT INTO Patient
(MRN, Name, DOB, Gender, ContactInfo, Status, PrimaryProviderId)
VALUES
('MRN007', 'Vikram', '1992-11-12', 'Male',   '9999999999', 'Active', 2),
('MRN008', 'Priya',  '1989-03-25', 'Female', '5555555555', 'Inactive', 2),
('MRN009', 'Arjun',  '1997-07-30', 'Male',   '4444444444', 'Active', 1),
('MRN010', 'Meera',  '2001-01-18', 'Female', '3333333333', 'Active', 1);


SELECT * from dbo.Patient
SELECT * from dbo.ProviderSchedule
SELECT * from dbo.Appointment
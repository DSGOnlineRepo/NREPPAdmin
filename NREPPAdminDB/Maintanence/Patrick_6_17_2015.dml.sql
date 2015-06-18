GO

DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer'),
		@ROLEID9 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'PreScreener')

INSERT INTO Permissions (Id, PermissionName) VALUES (9, 'ScreenInterv')


INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (9, @ROLEID5, 4, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (8, @ROLEID9, 2, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (7, @ROLEID5, 3, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (7, @ROLEID5, 4, cast(1 as Bit))

GO


SET IDENTITY_INSERT Answer ON

INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (39, 'Posttest', 'Post-Test')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (40, 'LastFollowup', 'Last Follow-up')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (41, '>2', '4  = >2 years')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (42, '1-2', '3 = 1-2 years')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (43, '<1', '2 = < 1 year')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (44, 'posttest', '1 = Posttest')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (45, 'FullSample', 'Full Sample')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (46, 'SubMale', 'Sub - Male')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (47, 'SubFemale', 'Sub - Female')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (48, 'SubJuvenile', 'Sub - Juveniles')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (49, 'SubAdults', 'Sub - Adults')

SET IDENTITY_INSERT Answer OFF

GO

SET IDENTITY_INSERT Category ON

INSERT INTO Category (Id, CategoryName) VALUES (12, 'AssessmentPd')
INSERT INTO Category (Id, CategoryName) VALUES (13, 'LongestFollowup')
INSERT INTO Category (Id, CategoryName) VALUES (14, 'FullSample')

SET IDENTITY_INSERT Category OFF

INSERT INTO Answer_Category (CategoryID, AnswerID) SELECT 12, Id from Answers where Id BETWEEN 39 AND 40
INSERT INTO Answer_Category (CategoryID, AnswerID) SELECT 13, Id from Answers where Id BETWEEN 41 AND 44
INSERT INTO Answer_Category (CategoryID, AnswerID) SELECT 14, Id from Answers where Id BETWEEN 45 AND 49

GO
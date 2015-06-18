/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Data Entry');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Assigner');							   
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Principal Investigator');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Lit Review');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Review Coordinator');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'DSG PRM');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Mathematica Assigner');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Reviewer');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'PreScreener');


-- Statuses

SET IDENTITY_INSERT InterventionStatus ON

INSERT INTO InterventionStatus (Id, StatusName) VALUES (1, 'Pending Submission')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (2, 'Pre-Screening')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (3, 'Awaiting Assignment')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (13, 'Awaiting Mathematica Screened')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (4, 'Being Screened')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (5, 'PRM Screening')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (92, 'Rejected From Pre-screening')


SET IDENTITY_INSERT InterventionStatus OFF

-- Answers, Categories, Mapping
-- TODO: Index off Version

SET IDENTITY_INSERT Answers ON

INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (-1, ' ', 'None')
Insert INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (1, 'Y', 'Yes')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (2, 'N', 'No')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (3, 'NR', 'Not Reported')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (4, 'ProgEval', 'Program Evaluation')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (5, 'Dissem', 'Dissemination Materials')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (6, 'Support', 'Other Supporting Materials')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (7, 'RCT', 'RCT')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (8, 'QED/Propen', 'QED/Propensity-score Matched')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (9, 'QED/Matched', 'QED/Matches')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (10, 'QED/Other', 'QED/Other')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (11, 'Time Series Multi', 'Time Series (Multiple-Group)')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (12, 'Time Series Single', 'Time Series (Single-Group)')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (13, 'Y&Priority', 'Yes & Priority')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (14, 'Foreign Language', 'Study not in English')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (15, 'Too Old', 'Study Published before 1990')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (16, 'Not Impact', 'Not an impact evaluation')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (17, 'Study Design', 'Not an eligible study design')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (18, 'Comp Effective', 'Comparative Effectiveness Study')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (19, 'Compare Group', 'Inappropriate Comparison Group')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (20, 'Information', 'Insufficient Information')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (21, 'Not SAMSHA', 'No SAMSHA Population or Outcome')

-- Answers for Attrition Questions

INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (22, 'Not Available', 'No, data not available')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (23, 'Assignment Calculation', 'Assignment group can be calculated')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (24, 'Pretest Calculation', 'Pretest can be calculated')

-- SAMHSA Population Answers
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (25, 'Ind', 'Indicated')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (26, 'Sel', 'Selective')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (27, 'Univ', 'Universal')

-- SAMHSA Outcomes
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (28, 'BH', 'Behavioral Health')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (29, 'R/P', 'Risk/Protective')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (30, 'Oth', 'Other')

-- Treatment Size
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (31, 'No effect', 'No effect size or other effect size reported')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (32, 'Odds', 'Odds Ratio')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (33, 'Cohen', 'Cohen''s d')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (34, 'Hedges', 'Hedges''s g')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (35, 'Eta', 'Eta Squared')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (36, 'f2', 'f squared')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (37, 'Rphi', 'Rphi')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (38, 'Rel Risk', 'Relative Risk')


SET IDENTITY_INSERT Answers OFF

-- Categories

SET IDENTITY_INSERT Category ON

INSERT INTO Category (Id, CategoryName) VALUES (1, 'Yes/No')
INSERT INTO Category (Id, CategoryName) VALUES (2, 'Y/N/NR')
INSERT INTO Category (Id, CategoryName) VALUES (3, 'DocumentType')
INSERT INTO Category (Id, CategoryName) VALUES (4, 'StudyDesign')
INSERT INTO Category (Id, CategoryName) VALUES (5, 'YPYN')
INSERT INTO Category (Id, CategoryName) VALUES (6, 'Exclusions')
INSERT INTO Category (Id, CategoryName) VALUES (7, 'AreasExpertise')
INSERT INTO Category (Id, CategoryName) VALUES (8, 'AttritionAnswer')
INSERT INTO Category (Id, CategoryName) VALUES (9, 'SAMHSAPop')
INSERT INTO Category (Id, CategoryName) VALUES (10, 'SAMHSAOutcome')
INSERT INTO Category (Id, CategoryName) VALUES (11, 'TreatCompare')

SET IDENTITY_INSERT Category OFF

INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (1, 1)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (2, 1)

INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (1, 2)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (2, 2)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (3, 2)

-- Document Types

INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (4, 3)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (5, 3)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (6, 3)

-- Study Designs
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (7, 4)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (8, 4)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (9, 4)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (10, 4)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (11, 4)
INSERT INTO Answer_Category(AnswerID, CategoryID) VALUES (12, 4)

-- Yes, Yes & Priority, No
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (1, 5)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (2, 5)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (13, 5)

-- Exclusions
INSERT INTO Answer_Category (AnswerId, CategoryId) SELECT ID, 6 from Answers where Id BETWEEN 14 AND 21
INSERT INTO Answer_Category (AnswerId, CategoryID) VALUES (-1, 6)

-- Attrition Answers
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (22, 8)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (23, 8)
INSERT INTO Answer_Category (AnswerID, CategoryID) VALUES (24, 8)

INSERT INTO Answer_Category (AnswerId, CategoryId) SELECT ID, 9 from Answers where Id BETWEEN 25 AND 27
INSERT INTO Answer_Category (AnswerId, CategoryId) SELECT ID, 10 from Answers where Id BETWEEN 28 AND 30
INSERT INTO Answer_Category (AnswerId, CategoryId) SELECT ID, 11 from Answers where Id BETWEEN 31 AND 38

-- Masks
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (1, 'Mental Health Promotion', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (2, 'Mental Health Treatment', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (3, 'Substance Use Prevention', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (4, 'Substance Abuse Treatment', 'ProgramType')

-- Pre-Screening
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (1, 'Do you have an evaluation study that assess mental health or substance abuse outcomes?', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (2, 'Do you have an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (3, 'Has the effectiveness of the intervention been assessed with at least one experiment or quasi-experimental design, with a comparison group? (Studies with single-group, pretest-posttest designs do not meet this requirement.)', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (4, 'Have the results of these studies been published in a peer reviewed journal, other professional publication, or documented in a comprehensive evaluation report dated 1990 or later?', 'PreScreen')

-- Taxonomic Outcome Stuff

INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines) VALUES ('Outcome One', 'Some guidelines go here soon')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines) VALUES ('Outcome Two', 'Some guidelines go here later')

-- Application Settings
INSERT INTO AppSettings (SettingID, Value) VALUES ('AppName', 'NREPPAdmin')
INSERT INTO AppSettings (SettingID, Value) VALUES ('CurrentPreScreen', NULL)
INSERT INTO AppSettings (SettingID, Value) VALUES ('NextPreScreen', NULL)


/*
Is there a study that assesses mental health or substance use outcome?
Is there an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?
Has the effectiveness of the intervention been assessed with at least one experiment or quasi-experimental design, with a comparison group?
Have the results of tehse studies been published in a peer reviewed journal, other professional publication, or documentmented in a comprehensive evaluation report?
*/

-- Routing Stuff


DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')

INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (1, @ROLEID5, 2)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, @ROLEID2, 3)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, @ROLEID5, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, @ROLEID4, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, @ROLEID5, 2)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, @ROLEID2, 3)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, @ROLEID5, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, null, 92)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (4, @ROLEID2, 3)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (4, @ROLEID6, 5)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, @ROLEID7, 13)


-- Permissions Stuff

INSERT INTO Permissions (Id, PermissionName) VALUES (1, 'TestPermission')
INSERT INTO Permissions (Id, PermissionName) VALUES (2, 'EditBaseSubmission')
INSERT INTO Permissions (Id, PermissionName) VALUES (3, 'UploadDocs')
INSERT INTO Permissions (Id, PermissionName) VALUES (4, 'SeeRCDocs')
INSERT INTO Permissions (Id, PermissionName) VALUES (5, 'UserPreScreen')
INSERT INTO Permissions (Id, PermissionName) VALUES (6, 'CanLitReview')
INSERT INTO Permissions (Id, PermissionName) VALUES (7, 'AssignLitReview')

INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, @ROLEID8, NULL, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (4, @ROLEID5, NULL, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, @ROLEID8, 3, CAST(0 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, @ROLEID8, 1, CAST(1 as BIT))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (3, @ROLEID3, 1, 1)
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (2, @ROLEID3, NULL, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (5, @ROLEID3, 1, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (7, @ROLEID2, 3, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (7, @ROLEID2, 4, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (7, @ROLEID2, 4, CAST(1 AS BIT))

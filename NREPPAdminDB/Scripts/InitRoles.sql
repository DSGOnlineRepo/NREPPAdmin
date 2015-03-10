﻿/*
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

SET IDENTITY_INSERT Roles ON

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(1, 'Data Entry', 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(2, 'Assigner', 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0);
							   
INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(3, 'Principal Investigator', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(4, 'Lit Review', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(5, 'Review Coordinator', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(6, 'DSG PRM', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(12, 'Mathematica Assigner', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (Id, RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					(7, 'Reviewer', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

SET IDENTITY_INSERT Roles OFF

-- Statuses

SET IDENTITY_INSERT InterventionStatus ON

INSERT INTO InterventionStatus (Id, StatusName) VALUES (1, 'Submitted')
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
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (14, 'Not Impact', 'Not an impact evaluation')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (15, 'Not Quant', 'Not Quantitative')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (16, 'Pre-experimental', 'Pre-experimental study design')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (17, 'pretest/posttest', '< 3 pretests/posttests, single time series design')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (18, 'Comp Effective', 'Comparative Effectiveness Study')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (19, 'Not Article', 'Not a journal article or comp eval report')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (20, 'Not English', 'Study Not in English')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (21, 'Not SAMSHA', 'No SAMSHA Population or Outcome')

SET IDENTITY_INSERT Answers OFF

-- Categories

SET IDENTITY_INSERT Category ON

INSERT INTO Category (Id, CategoryName) VALUES (1, 'Yes/No')
INSERT INTO Category (Id, CategoryName) VALUES (2, 'Y/N/NR')
INSERT INTO Category (Id, CategoryName) VALUES (3, 'DocumentType')
INSERT INTO Category (Id, CategoryName) VALUES (4, 'StudyDesign')
INSERT INTO Category (Id, CategoryName) VALUES (5, 'YPYN')
INSERT INTO Category (Id, CategoryName) VALUES (6, 'Exclusions')

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

-- Masks

INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (1, 'Mental Health Promotion', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (2, 'Mental Health Treatment', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (3, 'Substance Use Prevention', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (4, 'Mental Health Treatment', 'ProgramType')

-- Routing Stuff

INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, 5, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, 4, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (3, 5, 2)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, 2, 3)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, 5, 4)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, null, 92)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (4, 2, 3)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (4, 6, 5)
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus) VALUES (2, 12, 13)

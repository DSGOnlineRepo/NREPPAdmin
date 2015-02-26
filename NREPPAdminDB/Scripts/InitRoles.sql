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

-- Roles TODO: Identity insert off version

INSERT INTO Roles (RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					('Data Entry', 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					('Editor', 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0);
							   
INSERT INTO Roles (RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					('Principal Investigator', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0);

INSERT INTO Roles (RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					('Quality Control', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO Roles (RoleName, ViewInterventions, ViewAllUsers,
					ViewPendingInterventions, CreateUser, CreateIntervention, CreateReview,
					AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
					ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram) VALUES
					('Review Coordinator', 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0);

-- Statuses

INSERT INTO InterventionStatus (StatusName) VALUES ('Submitted')
INSERT INTO InterventionStatus (StatusName) VALUES ('Under Review')
INSERT INTO InterventionStatus (StatusName) VALUES ('Awaiting Assignment')
INSERT INTO InterventionStatus (StatusName) VALUES ('Being Screened')

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

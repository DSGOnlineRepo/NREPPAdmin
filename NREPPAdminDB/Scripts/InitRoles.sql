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
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (23, 'Assignment Calculation', 'Attrition group from assignment can be calculated')
INSERT INTO Answers (Id, ShortAnswer, LongAnswer) VALUES (24, 'Pretest Calculation', 'Attrition from pretest can be calculated')

/*
•	Co-occurring Disorders
•	Homelessness
•	Mental Health Promotion
•	Mental Health Treatment – Adults
•	Mental Health Treatment – Children
•	Older Adults
•	Suicide Prevention	•	Adult/Workplace
•	Childhood/School
•	Community Prevention
•	Environmental/Systems/Policy
•	Family
•	Older Adult
•	Violence Prevention
•	Youth/Adolescent/School	•	Adolescents
•	Adult, General Population
•	Co-occurring Disorders
•	Smoking Cessation

*/

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

-- Masks

INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (1, 'Mental Health Promotion', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (2, 'Mental Health Treatment', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (3, 'Substance Use Prevention', 'ProgramType')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (4, 'Substance Abuse Treatment', 'ProgramType')

-- Pre-Screening
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (1, 'Is there a study that assesses mental health or substance use outcome?', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (2, 'Is there an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (3, 'Has the effectiveness of the intervention been assessed with at least one experiment or quasi-experimental design, with a comparison group?', 'PreScreen')
INSERT INTO MaskList (MaskPower, MaskValueName, MaskCategory) VALUES (4, 'Have the results of tehse studies been published in a peer reviewed journal, other professional publication, or documentmented in a comprehensive evaluation report?', 'PreScreen')

-- Taxonomic Outcome Stuff

INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines) VALUES ('Outcome One', 'Some guidelines go here soon')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines) VALUES ('Outcome Two', 'Some guidelines go here later')

/*
Is there a study that assesses mental health or substance use outcome?
Is there an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?
Has the effectiveness of the intervention been assessed with at least one experiment or quasi-experimental design, with a comparison group?
Have the results of tehse studies been published in a peer reviewed journal, other professional publication, or documentmented in a comprehensive evaluation report?
*/

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


-- Permissions Stuff

INSERT INTO Permissions (Id, PermissionName) VALUES (1, 'TestPermission')

INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, 7, NULL, cast(1 as Bit))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, 7, 3, CAST(0 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (1, 7, 1, CAST(1 as BIT))

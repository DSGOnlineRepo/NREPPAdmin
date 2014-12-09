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

-- Roles

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

-- Statuses

INSERT INTO InterventionStatus (StatusName) VALUES ('Submitted')
INSERT INTO InterventionStatus (StatusName) VALUES ('Under Review')
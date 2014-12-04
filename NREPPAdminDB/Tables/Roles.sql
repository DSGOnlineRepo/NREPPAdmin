CREATE TABLE [dbo].[Roles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoleName] VARCHAR(50) NULL, 
    [ViewInterventions] BIT NULL DEFAULT 0, 
    [ViewAllUsers] BIT NULL DEFAULT 0, 
    [ViewPendingInterventions] BIT NULL DEFAULT 0, 
    [CreateUser] BIT NULL DEFAULT 0, 
    [CreateIntervention] BIT NULL DEFAULT 0, 
    [CreateReview] BIT NULL DEFAULT 0, 
    [AccesPIComments] BIT NULL DEFAULT 0, 
    [EmailEditor] BIT NULL DEFAULT 0, 
    [EmailPI] BIT NULL DEFAULT 0, 
    [EmailQC] BIT NULL DEFAULT 0, 
    [EmailSAMHSA] BIT NULL DEFAULT 0, 
    [CanBeContacted] BIT NULL DEFAULT 0, 
    [ChangeProgStatus] BIT NULL DEFAULT 0, 
    [ChangeAccess] BIT NULL DEFAULT 0, 
    [AssignStaff] BIT NULL DEFAULT 0, 
    [DeleteProgram] BIT NULL DEFAULT 0
)

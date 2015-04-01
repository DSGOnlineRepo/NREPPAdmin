CREATE TABLE [dbo].[Role_Permissions]
(
	[PermissionID] INT NOT NULL,
	[RoleID] INT NOT NULL,
	[StatusID] INT,
	[Allowed] BIT NOT NULL DEFAULT 0
)

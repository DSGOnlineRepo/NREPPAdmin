CREATE TABLE [dbo].[Role_Permissions]
(
	[PermissionID] INT NOT NULL,
	[RoleID] NVARCHAR(128) NOT NULL,
	[StatusID] INT,
	[Allowed] BIT NOT NULL DEFAULT 0
)

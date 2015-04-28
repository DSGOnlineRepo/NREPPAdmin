CREATE TABLE [dbo].[Inter_User_Roles]
(
	[InterventionId] INT NOT NULL, 
    [UserId] nvarchar(128) NOT NULL, 
    [WkRoleId] nvarchar(128) NOT NULL, 
    CONSTRAINT [FK_Inter_User_Roles_Interv] FOREIGN KEY ([InterventionId]) REFERENCES [Interventions]([Id]),
	CONSTRAINT [FK_Inter_User_Roles_User] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]),
	CONSTRAINT [FK_Inter_User_Roles_Roles] FOREIGN KEY ([WkRoleId]) REFERENCES [AspNetRoles]([Id])
)

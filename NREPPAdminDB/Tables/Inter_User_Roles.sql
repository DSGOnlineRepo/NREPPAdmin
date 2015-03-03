CREATE TABLE [dbo].[Inter_User_Roles]
(
	[InterventionId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [WkRoleId] INT NOT NULL, 
    CONSTRAINT [FK_Inter_User_Roles_Interv] FOREIGN KEY ([InterventionId]) REFERENCES [Interventions]([Id]),
	CONSTRAINT [FK_Inter_User_Roles_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_Inter_User_Roles_Roles] FOREIGN KEY ([WkRoleId]) REFERENCES [Roles]([Id])
)

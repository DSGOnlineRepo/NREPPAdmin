CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] VARCHAR(30) NOT NULL, 
    [Firstname] VARCHAR(50) NULL, 
    [Lastname] VARCHAR(50) NULL, 
    [hash] VARCHAR(100) NOT NULL, 
    [RoleID] INT NOT NULL, 
    [salt] VARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_Users_ToRole] FOREIGN KEY ([RoleID]) REFERENCES [Roles]([Id])
)

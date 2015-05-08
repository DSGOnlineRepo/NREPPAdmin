CREATE TABLE [dbo].[MaskList]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [MaskPower] INT NOT NULL, 
<<<<<<< HEAD
    [MaskValueName] VARCHAR(350) NULL, 
=======
    [MaskValueName] VARCHAR(500) NULL, 
>>>>>>> dev
    [MaskCategory] VARCHAR(50) NULL
)

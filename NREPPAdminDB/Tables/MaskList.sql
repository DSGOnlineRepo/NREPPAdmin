CREATE TABLE [dbo].[MaskList]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [MaskPower] INT NOT NULL, 
    [MaskValueName] VARCHAR(200) NULL, 
    [MaskCategory] VARCHAR(50) NULL
)

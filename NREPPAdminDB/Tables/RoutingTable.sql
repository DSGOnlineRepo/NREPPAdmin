CREATE TABLE [dbo].[RoutingTable]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [CurrentStatus] INT NULL, 
    [DestStatus] INT NULL, 
    [DestUserRole] INT NULL
)

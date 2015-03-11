﻿CREATE TABLE [dbo].[RoutingTable]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [CurrentStatus] INT NULL, 
    [DestStatus] INT NULL, 
    [DestUserRole] INT NULL, 
    CONSTRAINT [FK_RoutingTable_CurentStatus] FOREIGN KEY ([CurrentStatus]) REFERENCES [InterventionStatus]([Id]), 
    CONSTRAINT [FK_RoutingTable_DestStatus] FOREIGN KEY ([DestStatus]) REFERENCES [InterventionStatus]([Id]), 
    CONSTRAINT [FK_RoutingTable_Roles] FOREIGN KEY ([DestUserRole]) REFERENCES [Roles]([Id])
)
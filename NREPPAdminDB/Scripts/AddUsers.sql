/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- This script adds users and roles on the database itself

-- TODO: Get these to work right (Pre-Deployment Script)?

CREATE LOGIN nrAdmin WITH PASSWORD = 'nr!Admin123456';
GO

CREATE LOGIN nrAgent WITH PASSWORD = 'nr!Agent123456';
GO

CREATE USER [nrAdmin] FOR LOGIN nrAdmin
EXEC sp_addrolemember N'db_owner', N'nrAdmin'
GO

CREATE USER [nrAgent] FOR LOGIN nrAgent;
GRANT EXECUTE ON SCHEMA :: dbo To nrAgent;
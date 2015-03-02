/*
 * Creates database users. You will need to run this manually for the time being.
*/

CREATE LOGIN nrAdmin WITH PASSWORD = 'nr!Admin123456';
CREATE LOGIN nrAgent WITH PASSWORD = 'nr!Agent123456';
GO

use NREPPAdmin;
CREATE USER [nrAdmin] FOR LOGIN nrAdmin
EXEC sp_addrolemember N'db_owner', N'nrAdmin'
GO

CREATE USER [nrAgent] FOR LOGIN nrAgent;
GRANT EXECUTE ON SCHEMA :: dbo To nrAgent;
# NREPPAdmin Project

### Setup

The following steps should be performed when locally deploying in order to make sure everything works:
 1. Publish the NreppAdminDB Project (default DB name is NREPPAdmin)
 1. Run the DBUserSetup script. (_Note:_ if you're RE-deploying, you only need to run the second half of the script)
   ```
  use NREPPAdmin;
  CREATE USER [nrAdmin] FOR LOGIN nrAdmin
  EXEC sp_addrolemember N'db_owner', N'nrAdmin'
  GO

  CREATE USER [nrAgent] FOR LOGIN nrAgent;
  GRANT EXECUTE ON SCHEMA :: dbo To nrAgent;
  ```
 1. Go into the application and create two _Review Coordinator_ users.
 1. Insert the users into the AppSettings table as the CurrentPreScreen and NextPreScreen (see example below)
  ```
  update AppSettings
  SET Value = '6da271c6-90e2-4726-bd51-56b98a7917e1'
  WHERE SettingID = 'NextPreScreen'

  update AppSettings
  SET Value = 'd1cdaecb-f56d-4f80-9818-f9c71288cf77'
  WHERE SettingID = 'CurrentPreScreen'
  ```
  _Note:_ this will change slightly once I fix the SP to use UserNames
  
Once these steps are complete, you should be able to submit items into Pre-Screening with no issue.

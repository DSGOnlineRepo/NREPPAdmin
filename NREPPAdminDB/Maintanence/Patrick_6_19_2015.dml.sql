GO

delete from Role_Permissions
WHERE PermissionId in (select Id from Permissions where PermissionName = 'EditBaseSubmission') AND
RoleID in (SELECT Id from AspNetRoles WHERE Name = 'Principal Investigator')

GO
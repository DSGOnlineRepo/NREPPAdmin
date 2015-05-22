using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web;
using NREPPAdminSite.Models;
using NREPPAdminSite.Constants;

namespace NREPPAdminSite.Security
{
    public class NREPPRoleProvider : System.Web.Security.SqlRoleProvider
    {
        // Exceptions:
        //   System.Web.HttpException:
        //     An attempt was made to set the System.Web.Security.SqlRoleProvider.ApplicationName
        //     property by a caller that does not have System.Web.AspNetHostingPermissionLevel.High
        //     ASP.NET hosting permission.
        //
        //   System.Configuration.Provider.ProviderException:
        //     An attempt was made to set the System.Web.Security.SqlRoleProvider.ApplicationName
        //     to a string that is longer than 256 characters.
        /// <summary>
        /// Gets or sets the name of the application for which to store and retrieve role information.
        /// <remarks>The name of the application for which to store and retrieve role information.
        ///     The default is the System.Web.HttpRequest.ApplicationPath property value
        ///     for the current System.Web.HttpContext.Request.
        /// </remarks>
        /// </summary>
        public override string ApplicationName { get; set; }

        // Summary:
        //     Adds the specified user names to each of the specified roles.
        //
        // Parameters:
        //   usernames:
        //     A string array of user names to be added to the specified roles.
        //
        //   roleNames:
        //     A string array of role names to add the specified user names to.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     One of the roles in roleNames is null.-or-One of the users in usernames is
        //     null.
        //
        //   System.ArgumentException:
        //     One of the roles in roleNames is an empty string or contains a comma.-or-One
        //     of the users in usernames is an empty string or contains a comma.-or-One
        //     of the roles in roleNames is longer than 256 characters.-or-One of the users
        //     in usernames is longer than 256 characters.-or-roleNames contains a duplicate
        //     element.-or-usernames contains a duplicate element.
        //
        //   System.Configuration.Provider.ProviderException:
        //     One or more of the specified role names was not found.-or- One or more of
        //     the specified user names was not found.-or- One or more of the specified
        //     user names is already associated with one or more of the specified role names.-or-
        //     An unknown error occurred while communicating with the database.
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException("AddUsersToRoles(string[] usernames, string[] roleNames)");
        }
        Dictionary<string, List<string>> userRoles = new Dictionary<string, List<string>>();
        //
        // Summary:
        //     Adds a new role to the role database.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to create.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     roleName is null.
        //
        //   System.ArgumentException:
        //     roleName is an empty string or contains a comma.-or-roleName is longer than
        //     256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     roleName already exists in the database.-or- An unknown error occurred while
        //     communicating with the database.
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException("CreateRole(string roleName)");
        }

        //
        // Summary:
        //     Removes a role from the role database.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to delete.
        //
        //   throwOnPopulatedRole:
        //     If true, throws an exception if roleName has one or more members.
        //
        // Returns:
        //     true if the role was successfully deleted; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     roleName is null (Nothing in Visual Basic).
        //
        //   System.ArgumentException:
        //     roleName is an empty string or contains a comma.-or-roleName is longer than
        //     256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     roleName has one or more members and throwOnPopulatedRole is true.-or- An
        //     unknown error occurred while communicating with the database.
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException("DeleteRole(string roleName, bool throwOnPopulatedRole)");
        }

        //
        // Summary:
        //     Gets an array of user names in a role where the user name contains the specified
        //     user name to match.
        //
        // Parameters:
        //   roleName:
        //     The role to search in.
        //
        //   usernameToMatch:
        //     The user name to search for.
        //
        // Returns:
        //     A string array containing the names of all the users where the user name
        //     matches usernameToMatch and the user is a member of the specified role.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     roleName is null (Nothing in Visual Basic).-or-usernameToMatch is null.
        //
        //   System.ArgumentException:
        //     roleName is an empty string or contains a comma.-or-usernameToMatch is an
        //     empty string.-or-roleName is longer than 256 characters.-or-usernameToMatch
        //     is longer than 256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     roleName was not found in the database.-or- An unknown error occurred while
        //     communicating with the database.
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException("FindUsersInRole(string roleName, string usernameToMatch)");
        }

        //
        // Summary:
        //     Gets a list of all the roles for the application.
        //
        // Returns:
        //     A string array containing the names of all the roles stored in the database
        //     for a particular application.
        //
        // Exceptions:
        //   System.Configuration.Provider.ProviderException:
        //     An unknown error occurred while communicating with the database.
        public override string[] GetAllRoles()
        {
           return Enum.GetNames(typeof(SystemRoles));           
        }

        //
        // Summary:
        //     Gets a list of the roles that a user is in.
        //
        // Parameters:
        //   username:
        //     The user to return a list of roles for.
        //
        // Returns:
        //     A string array containing the names of all the roles that the specified user
        //     is in.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     username is null.
        //
        //   System.ArgumentException:
        //     username contains a comma.-or-username is longer than 256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     An unknown error occurred while communicating with the database.
        public override string[] GetRolesForUser(string username)
        {
            NrepServ nrepServ = new NrepServ(NrepServ.ConnString);
            return nrepServ.GetRolesForUser(username);
        }

        //
        // Summary:
        //     Gets a list of users in the specified role.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to get the list of users for.
        //
        // Returns:
        //     A string array containing the names of all the users who are members of the
        //     specified role.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     roleName is null.
        //
        //   System.ArgumentException:
        //     roleName is an empty string or contains a comma.-or-roleName is longer than
        //     256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     rolename was not found in the database.-or- An unknown error occurred while
        //     communicating with the database.
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException("GetUsersInRole(string roleName)");
        }

        private StringDictionary nreppRoles = new StringDictionary();

        //
        /// Summary:
        ///     Initializes the SQL Server role provider with the property values specified
        ///     in the ASP.NET application's configuration file. This method is not intended
        ///     to be used directly from your code.
        ///
        // Parameters:
        //   name:
        //     The name of the System.Web.Security.SqlRoleProvider instance to initialize.
        //
        //   config:
        //     A System.Collections.Specialized.NameValueCollection that contains the names
        //     and values of configuration options for the role provider.
        //
        // Exceptions:
        //   System.Web.HttpException:
        //     The ASP.NET application is not running at System.Web.AspNetHostingPermissionLevel.Medium
        //     trust or higher.
        //
        //   System.ArgumentNullException:
        //     config is null.
        //
        //   System.Configuration.Provider.ProviderException:
        //     The connectionStringName attribute is empty or does not exist in the application
        //     configuration file for this System.Web.Security.SqlRoleProvider instance.-or-The
        //     applicationName attribute exceeds 256 characters.-or-The application configuration
        //     file for this System.Web.Security.SqlRoleProvider instance contains an unrecognized
        //     attribute.
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            var roles = GetAllRoles();
            foreach (var r in roles)
                nreppRoles.Add(r, r);

            //throw new NotImplementedException("Initialize(string name, NameValueCollection config)" + name);
        }

        //
        // Summary:
        //     Removes the specified user names from the specified roles.
        //
        // Parameters:
        //   usernames:
        //     A string array of user names to be removed from the specified roles.
        //
        //   roleNames:
        //     A string array of role names to remove the specified user names from.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     One of the roles in roleNames is null.-or-One of the users in usernames is
        //     null.
        //
        //   System.ArgumentException:
        //     One of the roles in roleNames is an empty string or contains a comma.-or-One
        //     of the users in usernames is an empty string or contains a comma.-or-One
        //     of the roles in roleNames is longer than 256 characters.-or-One of the users
        //     in usernames is longer than 256 characters.-or-roleNames contains a duplicate
        //     element.-or-usernames contains a duplicate element.
        //
        //   System.Configuration.Provider.ProviderException:
        //     One or more of the specified user names was not found.-or- One or more of
        //     the specified role names was not found.-or- One or more of the specified
        //     user names is not associated with one or more of the specified role names.-or-
        //     An unknown error occurred while communicating with the database.
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException("RemoveUsersFromRoles(string[] usernames, string[] roleNames)");
        }

        //
        // Summary:
        //     Gets a value indicating whether the specified role name already exists in
        //     the role database.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to search for in the database.
        //
        // Returns:
        //     true if the role name already exists in the database; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     roleName is null.
        //
        //   System.ArgumentException:
        //     roleName is an empty string or contains a comma.-or-roleName is longer than
        //     256 characters.
        //
        //   System.Configuration.Provider.ProviderException:
        //     An unknown error occurred while communicating with the database.
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException("RoleExists(string roleName)");
        }

    }

}

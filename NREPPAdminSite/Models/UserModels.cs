using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Script.Serialization;

namespace NREPPAdminSite.Models
{
    public class Role
    {
        public int RoleId {get; set;}
        public string RoleName { get; set; }
        //private Dictionary<string, bool> permissions = new Dictionary<string, bool>();

        /*public string RoleName
        {
            get { return roleName; }
        }*/

        public Role(int id, string name)
        {
            RoleId = id;
            RoleName = name;
            //permissions = permList;
        }


        /*public bool hasPermission(string inAction)
        {
            return permissions[inAction];
        }

        public Dictionary<string, bool> RoleList
        {
            get
            {
                return permissions;
            }
        }*/
    }

    public class NreppUser
    {
        public int Id {get; set;}

        private Role userRole;
        private bool isAuth = false;

        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RoleName
        {
            get
            {
                return userRole.RoleName;
            }
        }

        public Role UserRole
        {
            get
            {
                return userRole;
            }
        }

        public NreppUser(int id, Role inRole, string uname, string fname, string lname)
        {
            userRole = inRole;
            Id = id;
            Firstname = fname;
            UserName = uname;
            Lastname = lname;
        }

        public NreppUser()
        {
            userRole = null;
            Id = -1;
            Firstname = "";
            Lastname = "";
        }

        public NreppUser(int id, string uname, string fname, string lname)
        {
            userRole = null;
            Id = id;
            Firstname = fname;
            UserName = uname;
            Lastname = lname;
        }

        /*public bool hasPermission(string permission)
        {
            return userRole.hasPermission(permission);
        }*/


        public void Authenticate(bool setStatus) // function callback? or have I been writing too much JS?
        {
            isAuth = setStatus;
        }

        public void setRole(int RoleId, string RoleName)
        {
            userRole = new Role(RoleId, RoleName);
        }

        public string MakeJSON()
        {
            string JSONString = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JSONString = serializer.Serialize(this);

            return JSONString;
        }

    }

    /// <summary>
    /// Reviewer Class
    /// </summary>
    public class Reviewer
    {
        public int ReviewerId { get; set; }
        public int UserId { get; set; }
        public int Degree { get; set; }
        public int ReviewerType { get; set; }
        public bool isActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string FaxNumber { get; set; }
        public string Employer { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }

        public string WorkStreetAddress { get; set; }
        public string WorkPhone { get; set; }
        public string WorkState { get; set; }
        public string WorkZIP { get; set; }
        public string WorkFaxNumber { get; set; }
        public string WorkEmployer { get; set; }
        public string WorkEmail { get; set; }
        public string ExpSummary { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage="You most provide a user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Choose a Password")]
        public string Password1 { get; set; }

        [Compare("Password1")]
        [Display(Name = "Confirm Password")]
        public string Password2 { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Enter your password")]
        public string Password { get; set; }
    }
}
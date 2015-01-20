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
        private int roleId;
        private string roleName;
        private Dictionary<string, bool> permissions = new Dictionary<string, bool>();

        public string RoleName
        {
            get { return roleName; }
        }

        public Role(int id, string name, Dictionary<string, bool>permList)
        {
            roleId = id;
            roleName = name;
            permissions = permList;
        }

        public bool hasPermission(string inAction)
        {
            return permissions[inAction];
        }

        public Dictionary<string, bool> RoleList
        {
            get
            {
                return permissions;
            }
        }
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

        public bool hasPermission(string permission)
        {
            return userRole.hasPermission(permission);
        }


        public void Authenticate(bool setStatus) // function callback? or have I been writing too much JS?
        {
            isAuth = setStatus;
        }

        public string MakeJSON()
        {
            string JSONString = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JSONString = serializer.Serialize(this);

            return JSONString;
        }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Choose a Password")]
        public string Password1 { get; set; }
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
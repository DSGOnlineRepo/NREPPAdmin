using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


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
    }

    public class NreppUser
    {
        private int Id;
        private Role userRole;

        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public NreppUser(int id, Role inRole, string uname, string fname, string lname)
        {
            userRole = inRole;
            Id = id;
            Firstname = fname;
            UserName = uname;
            Lastname = lname;
        }

        public bool hasPermission(string permission)
        {
            return userRole.hasPermission(permission);
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
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Choose a Password")]
        public string Password { get; set; }
    }
}
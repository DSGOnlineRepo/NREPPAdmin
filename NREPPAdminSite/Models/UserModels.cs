using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;

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
        public string ReviewerId { get; set; }
        public string UserId { get; set; }
        public string Degree { get; set; }
        public string  ReviewerType { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomeAddressLine1 { get; set; }
        public string HomeAddressLine2 { get; set; }
        public string HomeCity { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Employer { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }

        public string WorkAddressLine1 { get; set; }
        public string WorkAddressLine2 { get; set; }
        public string WorkCity { get; set; }
        public string WorkState { get; set; }
        public string WorkZip { get; set; }
        public string WorkPhone { get; set; }
        public string WorkFaxNumber { get; set; }
        public string WorkEmployer { get; set; }
        public string WorkEmail { get; set; }
        public string ExpSummary { get; set; }

        public Reviewer(string reviewerId, string userId, string degree, string reviewerType, string firstName, string lastName, string department)
        {
            ReviewerId = reviewerId;
            UserId = userId;
            Degree = degree;
            ReviewerType = reviewerType;            
            FirstName = firstName;
            LastName = lastName;
            Department = department;

        }
       
    }

    public class ReviewersWrapper
    {

        private List<Reviewer> Reviewers;


        #region Constructors

        public ReviewersWrapper()
        {
            Reviewers = new List<Reviewer>();

        }

        public ReviewersWrapper(List<Reviewer> inOutcomes)
        {
            Reviewers = inOutcomes;
        }

        #endregion

        public List<Reviewer> OutcomesReviewers;
        
    }

    public class ReviewerWrapper
    {

        public Reviewer Reviewer;       

    }
    

    public class UserProfileModel
    {
        [Required(ErrorMessage = "You most provide a user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
    }

    public class ChangePasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }


    public class RegisterViewModel
    {
        [Required(ErrorMessage="You most provide a user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public sealed class ExtendedUser : IdentityUser
    {
        public override string Id { get; set; }
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        [StringLength(128)]
        public string Degree { get; set; }
        [StringLength(128)]
        public string ReviewerType { get; set; }
        [StringLength(128)]
        public string HomeAddressLine1 { get; set; }
        [StringLength(128)]
        public string HomeAddressLine2 { get; set; }
        [StringLength(50)]
        public string HomeCity { get; set; }
        [StringLength(30)]
        public string HomeState { get; set; }
        [StringLength(11)]
        public string HomeZip { get; set; }
        [StringLength(15)]
        public string HomeFaxNumber { get; set; }
        [StringLength(128)]
        public string Employer { get; set; }
        [StringLength(128)]
        public string Department { get; set; }
        [StringLength(128)]
        public string WorkEmail { get; set; }
        [StringLength(128)]
        public string WorkAddressLine1 { get; set; }
        [StringLength(128)]
        public string WorkAddressLine2 { get; set; }
        [StringLength(50)]
        public string WorkCity { get; set; }
        [StringLength(30)]
        public string WorkState { get; set; }
        [StringLength(11)]
        public string WorkZip { get; set; }
        [StringLength(15)]
        public string WorkPhoneNumber { get; set; }
        [StringLength(15)]
        public string WorkFaxNumber { get; set; }
        public string ExperienceSummary { get; set; }

        public ExtendedUser() { }

        public ExtendedUser(string username, string email, string firstName, string lastName,
            bool lockoutEnabled)
        {
            UserName = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            LockoutEnabled = lockoutEnabled;
        }

        public ExtendedUser(RegisterViewModel registerViewModel)
        {
            UserName = registerViewModel.UserName;
            Email = registerViewModel.Email;
            FirstName = registerViewModel.FirstName;
            LastName = registerViewModel.LastName;
            PhoneNumber = registerViewModel.PhoneNumber;
        }
        

    }
}
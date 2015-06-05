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
    public class GenericUser : Base
    {

        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(128)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Fax Number")]
        [Phone]
        [StringLength(15)]
        public string FaxNumber { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(128)]
        public string HomeAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(128)]
        public string HomeAddressLine2 { get; set; }

        [Required]
        [Display(Name = "City", Prompt = "City")]
        [StringLength(50)]
        public string HomeCity { get; set; }

        [Required]
        [Display(Name = "State", Prompt = "State")]
        [StringLength(2, ErrorMessage = "Please enter state code")]
        public string HomeState { get; set; }

        [Required]
        [Display(Name = "Zip")]
        [StringLength(11)]
        public string HomeZip { get; set; }

        [Display(Name = "Employer")]
        [StringLength(128)]
        public string Employer { get; set; }

        [Display(Name = "Department")]
        [StringLength(128)]
        public string Department { get; set; }

        [Display(Name = "Address Line 1")]
        [StringLength(128)]
        public string WorkAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(128)]
        public string WorkAddressLine2 { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string WorkCity { get; set; }

        [Display(Name = "State")]
        [StringLength(2, ErrorMessage = "Please enter state code")]
        public string WorkState { get; set; }

        [Display(Name = "Zip")]
        public string WorkZip { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(11)]
        public string WorkPhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        [StringLength(11)]
        public string WorkFaxNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(128)]
        public string WorkEmail { get; set; }

        public bool IsLocked { get; set; }

        public string UserName { get; set; }
       
    }

    public class Reviewer : GenericUser
    {
        [Display(Name = "User Id")]
        [StringLength(128)]
        public string UserId { get; set; }

        [Display(Name = "Degree")]
        [StringLength(50)]
        public string Degree { get; set; }

        [Display(Name = "Reviewer Type")]
        [StringLength(50)]
        public string ReviewerType { get; set; }

        [Display(Name = "Experience Summary")]
        public string ExperienceSummary { get; set; }

        public string WkRoleId { get; set; }

    }

    public class RegisterViewModel : Reviewer
    {

        [Required(ErrorMessage = "You most provide a user name.")]
        [Display(Name = "User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        [StringLength(128)]
        public string ConfirmEmail { get; set; }

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
        [StringLength(256)]
        public string Role { get; set; }

        public string CapImage { get; set; }
        [Required(ErrorMessage = "Varification code is required.")]
        [Compare("CapImageText", ErrorMessage = "Captcha code Invalid")]
        public string CaptchaCodeText { get; set; }
        public string CapImageText { get; set; }
    }

    public class ReviewerSearchResult
    {
        public List<Reviewer> Reviewers = new List<Reviewer>();
        public int TotalSearchCount;
    }

    public class UsersSearchResult
    {
        public List<GenericUser> UserList = new List<GenericUser>();
        public List<Role> Roles = new List<Role>();
        public int TotalSearchCount;
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
        [Required]
        [Display(Name = "First Name")]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(128)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        [Phone]
        [StringLength(15)]
        public string FaxNumber { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(128)]
        public string HomeAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(128)]
        public string HomeAddressLine2 { get; set; }

        [Required]
        [Display(Name = "City", Prompt = "City")]
        [StringLength(50)]
        public string HomeCity { get; set; }

        [Required]
        [Display(Name = "State", Prompt = "State")]
        [StringLength(2, ErrorMessage = "Please enter state code")]
        public string HomeState { get; set; }

        [Required]
        [Display(Name = "Zip")]
        [StringLength(11)]
        public string HomeZip { get; set; }

        [Display(Name = "Employer")]
        [StringLength(128)]
        public string Employer { get; set; }

        [Display(Name = "Department")]
        [StringLength(128)]
        public string Department { get; set; }

        [Display(Name = "Address Line 1")]
        [StringLength(128)]
        public string WorkAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(128)]
        public string WorkAddressLine2 { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string WorkCity { get; set; }

        [Display(Name = "State")]
        [StringLength(2, ErrorMessage = "Please enter state code")]
        public string WorkState { get; set; }

        [Display(Name = "Zip")]
        public string WorkZip { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(11)]
        public string WorkPhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        [StringLength(11)]
        public string WorkFaxNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(128)]
        public string WorkEmail { get; set; }
    }
}
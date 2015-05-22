using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NREPPAdminSite.Models
{

    #region Lookup Page Model

    public class LookupPageModel
    {
        private List<Answer> firstLookup = new List<Answer>();
        private List<Answer> secondLookup = new List<Answer>();
        private List<MaskValue> maskList = new List<MaskValue>();

        public List<Answer> FirstLookup
        {
            get { return firstLookup; }
        }

        public List<Answer> SecondLookup
        {
            get { return secondLookup; }
        }

        public List<MaskValue> MaskList
        {
            get { return maskList; }
        }

        public LookupPageModel() { }

        public LookupPageModel(IEnumerable<Answer> inFirstLookup,
            IEnumerable<Answer> inSecondLookup, IEnumerable<MaskValue> inMasks)
        {
            firstLookup = inFirstLookup.ToList<Answer>();
            //secondLookup = inSecondLookup.ToList<Answer>();
            maskList = inMasks.ToList<MaskValue>();
        }
    }

    #endregion

    #region Intervention View Page Model

    public class IntervPageModel : NREPPPermissions
    {
        private Intervention theIntervention;
        private List<InterventionDoc> documents = new List<InterventionDoc>();

        private List<MaskValue> programType = new List<MaskValue>();
        private List<MaskValue> prescreen = new List<MaskValue>();
        private List<MaskValue> userprescreen = new List<MaskValue>();
        private List<Answer> documentType = new List<Answer>();
        private List<Destination> dests = new List<Destination>();
        private List<RCDocument> rcDocuments = new List<RCDocument>();

        private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>();

        #region Constructors

        public IntervPageModel() { } // Default Constructor

        /// <summary>
        /// This constructor sets up the lookup info
        /// </summary>
        /// <param name="inDocuments"></param>
        /// <param name="inProgTypes"></param>
        /// <param name="inDocTypes"></param>
        public IntervPageModel(List<InterventionDoc> inDocuments, List<MaskValue> inProgTypes, List<Answer> inDocTypes)
        {
            documents = inDocuments;
            programType = inProgTypes;
            documentType = inDocTypes;
        }

        public IntervPageModel(List<InterventionDoc> inDocuments, List<MaskValue> inProgTypes, List<Answer> inDocTypes,
            List<Destination> inDests, List<MaskValue> preScreen, List<MaskValue> userPreScreen)
        {
            documents = inDocuments;
            programType = inProgTypes;
            documentType = inDocTypes;
            dests = inDests;
            prescreen = preScreen;
            userprescreen = userPreScreen;
        }

        public IntervPageModel(List<InterventionDoc> inDocuments, List<RCDocument> rcDocs, List<MaskValue> inProgTypes, List<Answer> inDocTypes,
            List<Destination> inDests, List<MaskValue> preScreen, List<MaskValue> userPreScreen)
        {
            documents = inDocuments;
            programType = inProgTypes;
            documentType = inDocTypes;
            dests = inDests;
            prescreen = preScreen;
            userprescreen = userPreScreen;
            rcDocuments = rcDocs;
        }

        #endregion

        #region Accessors

        public Intervention TheIntervention
        {
            get { return theIntervention; }
            set { theIntervention = value; }
        }

        public List<InterventionDoc> Documents
        {
            get { return documents; }
        }

        public List<MaskValue> ProgramType
        {
            get { return programType; }
        }

        public List<MaskValue> PreScreen
        {
            get { return prescreen; }
        }

        public List<MaskValue> UserPreScreen
        {
            get { return userprescreen;}
        }

        public List<Answer> DocumentType
        {
            get { return documentType; }
        }

        public List<Destination> TheDestinations
        {
            get { return dests; }
        }

        public Dictionary<string, bool> PermissionsList
        {
            get { return permissionsList; }
        }

        public List<RCDocument> RCDocuments
        {
            get { return rcDocuments; }
        }

        #endregion

        /// <summary>
        /// Add a Document to the intervention
        /// </summary>
        /// <param name="inDoc"></param>
        public void AddDocument(InterventionDoc inDoc)
        {
            documents.Add(inDoc);
        }

        /// <summary>
        /// Remove a document from the intervention
        /// </summary>
        /// <param name="index"></param>
        public void DeleteDocument(int index)
        {
            documents.RemoveAt(index);
        }

        public void AddRCDocument(RCDocument inRCDoc)
        {
            rcDocuments.Add(inRCDoc);
        }

        public void DeleteRCDocument(int index)
        {
            rcDocuments.RemoveAt(index);
        }

    }

    #endregion

    #region Screening Model

    public class ScreeningModel : NREPPPermissions
    {
        private List<Study> studyDocuments = new List<Study>();
        private Intervention intervention = new Intervention();
        private List<Answer> studyDesigns;
        private List<Answer> ypyn;
        private List<Answer> exclusions;
        private List<Answer> attritionAnswers;
        private List<RCDocument> theDocuments;
        private List<Destination> theDestinations;
        private List<Answer> taxonomyOutcomes;
        private List<Answer> samhsaOutcomes;
        private List<Answer> effectReports;
        private List<Answer> samhsaPops;

        private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>();


        public OutcomesWrapper Outcomes { get; set; }

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScreeningModel() { }

        public ScreeningModel(List<Study> inDocs, Intervention interV, List<Answer> inStudyDesigns, List<Answer> inYPYN, List<Answer> inExclusions,
            OutcomesWrapper OW)
        {
            studyDocuments = inDocs;
            intervention = interV;
            studyDesigns = inStudyDesigns;
            ypyn = inYPYN;
            exclusions = inExclusions;
            Outcomes = OW;
        }

        public ScreeningModel(List<Study> inDocs, Intervention interV, List<Answer> inStudyDesigns, List<Answer> inYPYN, List<Answer> inExclusions,
            OutcomesWrapper OW, List<RCDocument> someDocs, List<Answer> inAttrAnswers, List<Answer> insamhsaPop, List<Answer> insamhsaOut,
            List<Answer> ineffectReports, List<Answer> TaxonomyOuts)
            : this(inDocs, interV, inStudyDesigns, inYPYN, inExclusions, OW)
        {
            theDocuments = someDocs;
            attritionAnswers = inAttrAnswers;
            samhsaPops = insamhsaPop;
            samhsaOutcomes = insamhsaOut;
            effectReports = ineffectReports;
            taxonomyOutcomes = TaxonomyOuts;
        }

        #endregion

        public IEnumerable<Study> StudyDocuments
        {
            get
            {
                return studyDocuments;
            }
        }

        public Intervention TheIntervention
        {
            get { return intervention; }
        }

        public IEnumerable<Answer> StudyDesigns
        {
            get { return studyDesigns; }
        }

        public IEnumerable<Answer> YPYN
        {
            get { return ypyn; }
        }

        public IEnumerable<Answer> Exclusions
        {
            get { return exclusions; }
        }

        public IEnumerable<RCDocument> TheDocuments
        {
            get { return theDocuments; }
        }

        public IEnumerable<Destination> TheDestinations
        {
            get {return theDestinations;}
        }

        public IEnumerable<Answer> AttritionAnswers
        {
            get { return attritionAnswers; }
        }

        public IEnumerable<Answer> EffectReports
        {
            get { return effectReports; }
        }

        public IEnumerable<Answer> TaxonomyOutcomes
        {
            get { return taxonomyOutcomes; }
        }

        public IEnumerable<Answer> SAMHSAOutcomes
        {
            get { return samhsaOutcomes; }
        }

        public IEnumerable<Answer> SAMHSAPops
        {
            get { return samhsaPops;  }
        }

        public Dictionary<string, bool> PermissionsList
        {
            get
            {
                return permissionsList;
            }
        }

        /// <summary>
        /// Add a Study Document
        /// </summary>
        /// <param name="inStudy">The Study Info to Add To the List</param>
        public void AddStudyDocument(Study inStudy)
        {
            studyDocuments.Add(inStudy);
        }

        /// <summary>
        /// Remove a document from the study list
        /// </summary>
        /// <param name="index">The index of the study document record to delete</param>
        public void DeleteStudyDocument(int index)
        {
            studyDocuments.RemoveAt(index);
        }

        public void AddDests(IEnumerable<Destination> inDests) // stopgap
        {
            theDestinations = inDests.ToList();
        }
    }

    #endregion

    public class JoinProgramModel
    {
        private Reviewer rev;
        private Intervention theIntervention;

        public Reviewer Reviewer
        {
            get { return rev; }
        }

        public Intervention TheIntervention
        {
            get { return theIntervention; }
        }

        public bool CanAccess { get; set; }

        public JoinProgramModel(Reviewer inReviewer, Intervention inInterv)
        {
            rev = inReviewer;
            theIntervention = inInterv;
        }

        public JoinProgramModel()
        {
            rev = null;
            theIntervention = null;
        }
    }

    #region Reviewer models

    public class ReviewersPageModel : NREPPPermissions
    {
        public ReviewersWrapper Outcomes { get; set; }
    }

    public class ReviewerPageModel : NREPPPermissions
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Degree { get; set; }
        [Display(Name = "Reviewer Type")]
        public string ReviewerType { get; set; }
        [Display(Name = "Home Address Line 1")]
        public string HomeAddressLine1 { get; set; }
        [Display(Name = "Home Address Line 2")]
        public string HomeAddressLine2 { get; set; }
        [Display(Name = "Home City")]
        public string HomeCity { get; set; }
        [Display(Name = "Home State")]
        public string HomeState { get; set; }
        [Display(Name = "Home Zip")]
        public string HomeZip { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Fax Number")]
        public string FaxNumber { get; set; }
        public string Email { get; set; }

        public string Employer { get; set; }
        public string Department { get; set; }
        [Display(Name = "Work Address Line 1")]
        public string WorkAddressLine1 { get; set; }
        [Display(Name = "Work Address Line 2")]
        public string WorkAddressLine2 { get; set; }
        [Display(Name = "Work City")]
        public string WorkCity { get; set; }
        [Display(Name = "Work State")]
        public string WorkState { get; set; }
        [Display(Name = "Work Zip")]
        public string WorkZip { get; set; }
        [Display(Name = "Work Phone Number")]
        public string WorkPhoneNumber { get; set; }
        [Display(Name = "Work Fax Number")]
        public string WorkFaxNumber { get; set; }
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }
        [Display(Name = "Experience Summary")]
        public string ExperienceSummary { get; set; }

        public bool IsActive { get; set; }

        public ReviewerWrapper Outcomes { get; set; }
    }

    #endregion

    #region UserPageModel

    public class UserPageModel : NREPPPermissions
    {
        public ExtendedUser User { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string UserRole { get; set; }

        [Required(ErrorMessage = "You most provide a user name.")]
        [Display(Name = "User Name")]
        [StringLength(50)]
        public string UserName { get; set; }
    }

    #endregion

    public class AssignmentPageModel : NREPPPermissions
    {
        private List<Destination> theDests = new List<Destination>();
        private List<Destination> theLitReviews = new List<Destination>();

        public AssignmentPageModel(List<Destination> Dests, List<Destination> LitReview)
        {
            theDests = Dests;
            theLitReviews = LitReview;
        }

        public IEnumerable<Destination> TheDests
        {
            get { return theDests; }
        }

        public IEnumerable<Destination> TheLitReviews
        {
            get { return theLitReviews; }
        }
    }

    #region Extra Stuff

    public class NREPPPermissions
    {
        //private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>();
        private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>();

        public void SetPermissions(IEnumerable<string> lookingFor, string UserName, int? interVentionId)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            foreach (string permission in lookingFor)
            {
                permissionsList.Add(permission, localService.CanDo(permission, UserName, interVentionId));
            }
        }

        public bool CanDo(string permission)
        {
            if (permissionsList.ContainsKey(permission))
                return permissionsList[permission];
            else return false;
        }
    }

    #endregion
}
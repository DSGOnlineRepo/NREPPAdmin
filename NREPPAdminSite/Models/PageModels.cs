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
            set { theDocuments = value.ToList(); }
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

        public IEnumerable<Answer> AssessmentPd { get; set; }
        public IEnumerable<Answer> LongestFollowup { get; set; }
        public IEnumerable<Answer> FullSample { get; set; }

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

    #region Join Program Model

    public class JoinProgramModel
    {
        private Intervention theIntervention;

        public bool loginAccountExists = false;

        private List<RCDocument> documents;

        public Intervention TheIntervention
        {
            get { return theIntervention; }
        }

        public List<RCDocument> Documents
        {
            get { return documents; }
        }

        public int CanAccess { get; set; }

        public string UserId { get; set; }

        public JoinProgramModel(List<RCDocument> Docs, Intervention inInterv)
        {
            documents = Docs;
            theIntervention = inInterv;
        }

        public JoinProgramModel()
        {
            theIntervention = null;
        }
    }

    #endregion

    #region Document Upload

    public class DocUploadPage : NREPPPermissions
    {
        private List<InterventionDoc> documents = new List<InterventionDoc>();
        private List<Answer> documentType = new List<Answer>();
        private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>();

        #region Accessors

        public string InterventionName { get; set; }
        public int InterventionId { get; set; }

        [Required(ErrorMessage = "You must choose a document type!")]
        [Display (Name = "Document Type")]
        public string UploadDocType { get; set; }

        [Required (ErrorMessage = "You must provide a Title for Your Document!")]
        [Display (Name = "Document Title")]
        public string UploadDocTitle { get; set; }

        [Display (Name = "Document Description")]
        public string UploadDescription { get; set; }

        public List<InterventionDoc> Documents
        {
            get { return documents; }
        }

        public List<Answer> DocumentType
        {
            get { return documentType; }
        }

        public Dictionary<string, bool> PermissionsList
        {
            get { return permissionsList; }
        }

        #endregion

        #region Constructors

        public DocUploadPage()
        {
            documents = new List<InterventionDoc>();
            documentType = new List<Answer>();
        }

        public DocUploadPage(List<InterventionDoc> inDocs, List<Answer> inDocTypes)
        {
            documents = inDocs;
            documentType = inDocTypes;
        }

        #endregion

    }

    #endregion

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

    #region AssignmentPageModel

    public class AssignmentPageModel : NREPPPermissions
    {
        private List<Destination> theDests = new List<Destination>();
        private List<Destination> theLitReviews = new List<Destination>();

        public bool HasReviewer { get; set; }
        public string LitReviewer { get; set; }
        public int InvId { get; set; }
        public string InterventionStatus { get; set; }

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

    #endregion

    #region Assign Reviewer Page Models

    public class AssignReviewModel : NREPPPermissions
    {
        private List<ReviewerOnInterv> assignedReviewers = new List<ReviewerOnInterv>();

        public int InterventionId { get; set; }

        public List<ReviewerOnInterv> AssignedReviewers
        {
            get {return assignedReviewers; }
        }

        public void AddAssignedReviewer(ReviewerOnInterv rev)
        {
            assignedReviewers.Add(rev);
        }
    }

    #region Reviewer Rigor Page

    public class ReviewerRigorPage : ReviewInstrumentPage
    {
        #region Private Members
        
        private Dictionary<string, bool> permissionsList = new Dictionary<string, bool>(); // Permissions from NreppPermissions model
        private OutcomesWrapper ow = new OutcomesWrapper();
        private List<RigorQuestion> questions = new List<RigorQuestion>();
        private List<RigorAnswer> answers = new List<RigorAnswer>();


        #endregion

        #region Accessors

        public IEnumerable<RigorQuestion> Questions
        {
            get { return questions; }
        }

        public List<RigorAnswer> Answers
        {
            get { return answers; }
        }

        public OutcomesWrapper Outcomes
        {
            get { return ow; }
        }

        #endregion

        #region Constructors

        public ReviewerRigorPage() { }

        public ReviewerRigorPage(ReviewerDocsWrapper w) : base(w) { }

        public ReviewerRigorPage(ReviewerDocsWrapper w, OutcomesWrapper inOw) : base(w)
        {
            ow = inOw;
        }

        public ReviewerRigorPage(ReviewerDocsWrapper w, OutcomesWrapper inOw, List<RigorAnswer> inAns, List<RigorQuestion> inQues)
            : this(w, inOw)
        {
            questions = inQues;
            answers = inAns;
 
        }

        /// <summary>
        /// Adds a Question
        /// </summary>
        /// <param name="questionText">Question Text</param>
        /// <param name="Id">Question Id</param>
        public void AddQuestion(string questionText, int Id)
        {
            questions.Add(new RigorQuestion(questionText, Id));
        }


        /// <summary>
        /// Adds an Answer to the dictionary
        /// </summary>
        /// <param name="ans">RigorAnswer Object containing the information</param>
        /*public void AddAnswer(RigorAnswer ans)
        {
            AddAnswer(ans.outcomeMeasureId, ans);
        }

        /// <summary>
        /// Adds an Answer to the Dictionary
        /// </summary>
        /// <param name="MeasureId">Outcome Measure Id</param>
        /// <param name="questionId">QuestionId</param>
        /// <param name="chosen">Chosen Answer</param>
        public void AddAnswer(int MeasureId, int questionId, string chosen)
        {
            AddAnswer(new RigorAnswer() { qId = questionId, outcomeMeasureId = MeasureId, chosenAnswer = chosen });
        }*/

        public void SetOutcomesWrapper(OutcomesWrapper inWrapper)
        {
            ow = inWrapper;
        }

        
        #endregion
    }

    #endregion

    public class ConsensusModel
    {
        private List<QoRAnswerType> answerTypes;
        private List<QoRAnswer> answerList;
        private OutcomesWrapper ow;

        public ConsensusModel(List<QoRAnswer> inAnswers, List<QoRAnswerType> inAnswerType)
        {
            answerTypes = inAnswerType;
            answerList = inAnswers;
        }

        public ConsensusModel(List<QoRAnswer> inAnswers, List<QoRAnswerType> inAnswerType, OutcomesWrapper inOW)
            : this(inAnswers, inAnswerType)
        {
            ow = inOW;
        }

        public IEnumerable<QoRAnswerType> AnswerTypes
        {
            get { return answerTypes; }
        }

        public IEnumerable<QoRAnswer> AnswerList
        {
            get { return answerList; }
        }

        public OutcomesWrapper OW
        {
            get { return ow; }
        }

        public IEnumerable<Answer> AssessmentPd { get; set; }
        public IEnumerable<Answer> SampleSize { get; set; }
    }


    #region Reviewer Instrument Page Parent Class

    public class ReviewInstrumentPage : NREPPPermissions
    {
        private ReviewerDocsWrapper reviewerDocs;

        #region Accessors

        public ReviewerDocsWrapper ReviewerDocs
        {
            get { return reviewerDocs; }
        }

        #endregion

        #region Constructors

        public ReviewInstrumentPage()
        {
            reviewerDocs = new ReviewerDocsWrapper();
        }

        public ReviewInstrumentPage(ReviewerDocsWrapper w)
        {
            reviewerDocs = w;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets up the reviewer docs
        /// </summary>
        /// <param name="inWrapper">Reviewer Document Wrapper</param>
        public void SetReviewerDocs(ReviewerDocsWrapper inWrapper)
        {
            reviewerDocs = inWrapper;
        }

        #endregion
    }

    #endregion

    public class RigorQuestion
    {
        private int questionId;
        private string questionString;
        private List<string> answerCodes = new List<string>() { "3", "2", "1", "99", "98-do not rate" }; // These can go in the databse later
        private string questionVar;

        public string QuestionGroup { get; set; }

        public IEnumerable<string> AnswerCodes
        {
            get { return answerCodes; }
        }

        public string QuestionString
        {
            get { return questionString; }
        }

        public int QuestionId
        {
            get { return questionId; }
        }

        public string QuestionVar
        {
            get { return questionVar; }
        }

        public RigorQuestion(string inQuestion, int qId)
        {
            questionString = inQuestion;
            questionId = qId;
        }

        /// <summary>
        /// Works
        /// </summary>
        /// <param name="inQuestion"></param>
        /// <param name="qId">Question Id</param>
        /// <param name="inCodes">Answer Codes</param>
        public RigorQuestion(string inQuestion, int qId, List<string> inCodes)
        {
            questionString = inQuestion;
            questionId = qId;
            answerCodes = inCodes;
        }

        public RigorQuestion(string inQuestion, int qId, List<string> inCodes, string qVar) : this(inQuestion, qId, inCodes)
        {
            questionVar = qVar;
        }
    }

    // TODO: Convert me to a class with an XML serializer
    public struct RigorAnswer
    {
        public int qId; // could use the unique string here instead I suppose
        public int outcomeMeasureId;
        public string chosenAnswer;
    }

    #endregion

    #region Extra Stuff

    public class ReviewerDocsWrapper
    {
        private List<RCDocument> qoRDocs = new List<RCDocument>();
        private List<RCDocument> supplements = new List<RCDocument>();

        public IEnumerable<RCDocument> QoRDocs
        {
            get { return qoRDocs; }
        }

        public IEnumerable<RCDocument> Supplements
        {
            get { return supplements; }
        }

        public ReviewerDocsWrapper() { }

        public ReviewerDocsWrapper(List<RCDocument> inQoR, List<RCDocument> inSupp)
        {
            qoRDocs = inQoR;
            supplements = inSupp;
        }
    }

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
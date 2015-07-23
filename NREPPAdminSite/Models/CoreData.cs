using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NREPPAdminSite.Models
{
    public class Intervention
    {
        //private int id;
        
        [Display(Name = "Program Name")]
        public string Title { get; set; }

        [Display(Name = "Previous Program Name")]
        public string PrevProgName { get; set; }

        public string SubmitterId { get; set; }

        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }

        [Display(Name = "Submitter")]
        public string Submitter { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        public int StatusId { get; set; }

        [Display(Name = "Program Type")]
        public int ProgramType { get; set; }

        [Display(Name = "Program Acronym")]
        public string Acronym { get; set; }

        [Display(Name = "From Literature Search?")]
        public bool FromLitSearch { get; set; }

        [Display(Name = "Materials List")]
        public string MaterialsList { get; set; }

        public bool HaveMaterials { get; set; }

        public bool IsLive { get; set; }

        public int Id {get; set;}

        #region Contact Info

        [Display(Name = "Primary Contact")]
        public string PrimaryName {get; set;}
		
        [Display(Name = "Organization")]
        public string PrimaryOrg {get; set;}

        [Display(Name = "Title")]
		public string PrimaryTitle {get; set;}
		
        [Display (Name = "Address")]
        public string PrimaryAddressLine1 {get; set;}

        public string PrimaryAddressLine2 {get; set;}
		
        [Display (Name = "City")]
        public string PrimaryCity {get; set;}

        [Display(Name = "State")]
		public string PrimaryState {get; set;}
		
        [Display(Name = "Zip")]
        public string PrimaryZip {get; set;}

        [Display (Name = "Phone Number")]
        public string PrimaryPhoneNumber {get; set;}
		
        [Display (Name = "Fax Number")]
        public string PrimaryFaxNumber {get; set;}

        [Display(Name = "Primary Contact Email")]
        public string PrimaryEmail { get; set; }

        [Display(Name = "Secondary Contact")]
        public string SecondaryName { get; set; }

        [Display(Name = "Organization")]
        public string SecondaryOrg { get; set; }

        [Display(Name = "Title")]
        public string SecondaryTitle { get; set; }

        [Display(Name = "Address")]
        public string SecondaryAddressLine1 { get; set; }

        public string SecondaryAddressLine2 { get; set; }

        [Display(Name = "City")]
        public string SecondaryCity { get; set; }

        [Display(Name = "State")]
        public string SecondaryState { get; set; }

        [Display(Name = "Zip")]
        public string SecondaryZip { get; set; }

        [Display(Name = "Phone Number")]
        public string SecondaryPhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        public string SecondaryFaxNumber { get; set; }

        [Display(Name = "Secondary Contact Email")]
        public string SecondaryEmail { get; set; }

        public string DummyColumnForFilter { get; set; }

        #endregion
        /*{
            get { return id; }
        }*/

        public int PreScreenMask { get; set; }
        public int UserPreScreenMask { get; set; }
        public bool LitReviewDone { get; set; }

        [Display(Name = "Notes from Screening")]
        public string ScreeningNotes { get; set; }

        public Intervention()
        {
            Id = -1;
            Title = "";
            FullDescription = "";
            Submitter = "";
            SubmitterId = "";
            PublishDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Status = "Pending Submission";
            StatusId = 1;
            ProgramType = 0;
            Acronym = "";
            FromLitSearch = false;
        }

        public Intervention(int inId, string title, string fullDescription, string submitter, DateTime? publishDate, DateTime updateDate,
            string submitterId, string status, int statusId, int programType, string acronym, bool fromLitSearch)
        {
            Id = inId;
            Title = title;
            FullDescription = fullDescription;
            Submitter = submitter;
            PublishDate = publishDate;
            UpdatedDate = updateDate;
            SubmitterId = submitterId;
            Status = status;
            StatusId = statusId;
            ProgramType = programType;
            Acronym = acronym;
            FromLitSearch = fromLitSearch;
        }

        // private int submitterId; Is this a thing?
    }

    public class InterventionDoc
    {
        [Display(Name = "Description")]
        [Required]
        public string FileDescription { get; set; }
        public string Link { get; set; }

        [Display(Name = "Uploaded By")]
        public string Uploader { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        protected string documentTypeName;
        protected int documentType;
        protected int docId;

        [Display(Name = "Document Type")]
        public string DocumentTypeName {

            get { return documentTypeName; }
            
            set {documentTypeName = value;}
        }

        public int DocumentType
        {
            get { return documentType; }
        }

        public int DocId
        {
            get { return docId; }
        }

        /// <summary>
        /// Ensure that you set Document Type Name and Document Type at the same time
        /// </summary>
        /// <param name="inDocType"></param>
        /// <param name="inDocTypeName"></param>
        public void SetDocType(int inDocType, string inDocTypeName)
        {
            documentType = inDocType;
            documentTypeName = inDocTypeName;
        }

        public InterventionDoc() { }

        public InterventionDoc(int inId)
        {
            docId = inId;
        }

    }

    public class RCDocument : InterventionDoc
    {
        private int rcdocId;

        public int RCDocId
        {
            get { return rcdocId; }
        }

        public string Reference { get; set; }
        public string RCName { get; set; }
        public int? PubYear { get; set; }
        public bool AddToReview { get; set; }

        public RCDocument(int inDocId, int rcDocId) // Can you do this with : base(id)?
        {
            rcdocId = rcDocId;
            docId = inDocId;
        }

    }

    public class Outcome
    {
        public int Id { get; set; }
        public int IntervId { get; set; }
        public string OutcomeName { get; set; }
    }

    public class OutcomeMeasure
    {
        public int Id { get; set; }
        public int OutcomeId { get; set; }
        public int OutcomeMeasureId { get; set; }

        [Display(Name = "Measure Reported in Study")]
        public string OutcomeMeasureName { get; set; }
        public string Citation { get; set; } // Hmm... Store this for display
        public bool OverallAttrition { get; set; }
        public bool DiffAttrition { get; set; }
        public bool EffectSize { get; set; }
        public bool GroupFavored { get; set; }

        [Display(Name = "Describe Evaluated Population")]
        public string PopDescription { get; set; }

        [Display(Name = "SAMHSA-related population")]
        public int SAMHSAPop { get; set; }

        [Display(Name = "SAMHSA-related Outcome")]
        public int SAMHSAOutcome { get; set; }
        public int EffectReport { get; set; }

        public int DocumentId { get; set; }
        public int StudyId { get; set; }
        public int TaxOutcome { get; set; }
        public bool RecommendReview { get; set; }
        public string InstrumentSource { get; set; }
        public string EffectSource { get; set; }
        public string GeneralDescription { get; set; }

        public int AssessmentPd { get; set; }
        public int LongestFollowup { get; set; }
        public int FullSample { get; set; }
    }

    public class OutcomesWrapper
    {
        private List<OutcomeMeasure> outcomes;
        private List<Outcome> studyOutcomes;

        #region Constructors

        public OutcomesWrapper()
        {
            outcomes = new List<OutcomeMeasure>();
            studyOutcomes = new List<Outcome>();
        }

        public OutcomesWrapper(List<OutcomeMeasure> inOutcomes, List<Outcome> inStudyOutcomes)
        {
            outcomes = inOutcomes;
            studyOutcomes = inStudyOutcomes;
        }

        #endregion

        public List<OutcomeMeasure> OutcomesMeasures
        {
            get { return outcomes; }
        }

        public List<Outcome> MacroOutcomes
        {
            get { return studyOutcomes; }
        }
    }

    public class Study
    {
        public int Id { get; set;}
        public int StudyId { get; set; }
        public string Reference { get; set; }
        public bool inLitSearch { get; set; }
        public int Exclusion1 { get; set; }
        public int Exclusion2 { get; set; }
        public int Exclusion3 { get; set; }
        public int StudyDesign { get; set; }
        // public string GroupSize { get; set; } // I am not sure that this is right?
        public string BaselineEquiv { get; set; }
        public bool UseMultivariate { get; set; }
        public string LongestFollowup {get; set;}
        public int SAMSHARelated { get; set; }
        // public int TargetPop { get; set; }
        // public int ListOfOutcomes { get; set; }
        public string TotalSampleSize { get; set; }
        public bool AuthorQueryNeeded { get; set; }
        public bool RecommendReview { get; set; }
        public string Notes { get; set; }
        public int DocumentId { get; set; }
        public int DocOrdinal { get; set; }
        public int OverallAttrition { get; set; }
        public int DiffAttrition { get; set; }
    }

    public class Study_Outcome
    {
        public int StudyId { get; set; }
        public  int OutcomeId { get; set; }
        public int DocumentId { get; set; }
    }

    public class something
    {
        Study_Outcome so = new Study_Outcome() { StudyId = 1, OutcomeId = 2 };
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public string LongAnswer { get; set; }
        public string ShortAnswer { get; set; }

        public Answer(int Id, string Long, string Short)
        {
            AnswerId = Id;
            LongAnswer = Long;
            ShortAnswer = Short;
        }

        public Answer()
        {
            AnswerId = -1;
        }
    }

    public class Destination
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DestDescription { get; set; }
    }

    public class MaskValue
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int Value { get; set; }
        public bool Selected { get; set; }

        public static IEnumerable<MaskValue> SplitMask(List<MaskValue> inList, int inMask)
        {
            List<MaskValue> outList = new List<MaskValue>();

            int principal = inMask;

            for (int i = 0; i < inList.Count; i++ )
            {
                int someOtherVal = (int)Math.Pow(2, inList[i].Value);
                bool someValue = (principal & someOtherVal) > 0;

                outList.Add(new MaskValue()
                {
                    Name = inList[i].Name,
                    Value = inList[i].Value,
                    Selected =
                        someValue
                });
            }

                return outList;
        }
    }

    public struct SubmissionPd
    {
        public DateTime StartDate;
        public DateTime EndDate;
    }

    public struct ReviewerOnInterv
    {
        public string UserId;
        public string Name;
        public string ReviewerStatus;
    }

    
    /// <summary>
    /// Wrapper structure for Taxonomic outcomes
    /// </summary>
    public struct TaxOutcome
    {
        public int Id;
        public string OutcomeName;
        public string Guidelines;
        public string GroupName;
        public string NotInclude;
    }

    public class QoRAnswer
    {

        public int OutcomeId { get; set; }
        public string TaxOutcomeName { get; set; }
        public int StudyId { get; set; } // have to extrapolate this?
        public int AnswerTypeId { get; set; } // Enumeration lives in the database
        public string AnswerTypeName { get; set; }
        public string FixedAnswer { get; set; }
        public string CalcAnswer { get; set; }
        //public Reviewer Reviewer { get; set; }
        public string ReviewerName { get; set; }

        public string IdString() 
        {
            string returnString = "";

            returnString = string.Format("{0}_{1}_{2}", OutcomeId, StudyId, AnswerTypeId);

            return returnString;
        }
    }

    public class QoRAnswerType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Comparison { get; set; }
    }

}
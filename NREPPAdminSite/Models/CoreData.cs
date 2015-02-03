using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NREPPAdminSite.Models
{
    public class Intervention
    {
        //private int id;
        
        [Display(Name = "Title")]
        public string Title { get; set; }
        public int SubmitterId { get; set; }

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

        [Display(Name = "Acronym")]
        public string Acronym { get; set; }

        [Display(Name = "From Literature Search?")]
        public bool FromLitSearch { get; set; }

        public int Id {get; set;}
        /*{
            get { return id; }
        }*/

        public Intervention()
        {
            Id = -1;
            Title = "";
            FullDescription = "";
            Submitter = "";
            SubmitterId = 0;
            PublishDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Status = "Submitted";
            StatusId = 1;
            ProgramType = 0;
            Acronym = "";
            FromLitSearch = false;
        }

        public Intervention(int inId, string title, string fullDescription, string submitter, DateTime? publishDate, DateTime updateDate,
            int submitterId, string status, int statusId, int programType, string acronym, bool fromLitSearch)
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
        public string FileDescription { get; set; }
        public string Link { get; set; }

        [Display(Name = "Uploaded By")]
        public string Uploader { get; set; }

        private string documentTypeName;
        private int documentType;
        private int docId;

        [Display(Name = "Document Type")]
        public string DocumentTypeName
        {
            get { return documentTypeName; }
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

    public class Outcome
    {
        public int Id { get; set; }

        [Display(Name = "Measure Reported in Study")]
        public string OutcomeMeasure { get; set; }
        public string Citation { get; set; }
        public bool OverallAttrition { get; set; }
        public bool DiffAttrition { get; set; }
        public bool EffectSize { get; set; }
        public int BaselineEquiv { get; set; }
        public int SignificantImpact { get; set; }
        public bool GroupFavored { get; set; }

        [Display(Name = "Describe Evaluated Population")]
        public string PopDescription { get; set; }

        [Display(Name = "SAMSHA-related population")]
        public int SAMHSAPop { get; set; }
        public bool PrimaryOutcome { get; set; }
        public int Priority { get; set; }
        public int DocumentId { get; set; }
    }

    public class OutcomesWrapper
    {
        private List<Outcome> outcomes;
        private List<Study_Outcome> studyOutcomes;

        #region Constructors

        public OutcomesWrapper()
        {
            outcomes = new List<Outcome>();
            studyOutcomes = new List<Study_Outcome>();
        }

        public OutcomesWrapper(List<Outcome> inOutcomes, List<Study_Outcome> inStudyOutcomes)
        {
            outcomes = inOutcomes;
            studyOutcomes = inStudyOutcomes;
        }

        #endregion

        public List<Outcome> Outcomes
        {
            get { return outcomes; }
        }

        public List<Study_Outcome> StudyOutcomes
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
        // public int LongestFollowup {get; set;}
        public int SAMSHARelated { get; set; }
        // public int TargetPop { get; set; }
        // public int ListOfOutcomes { get; set; }
        public bool AuthorQueryNeeded { get; set; }
        public bool RecommendReview { get; set; }
        public string Notes { get; set; }
        public int DocumentId { get; set; }
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

}
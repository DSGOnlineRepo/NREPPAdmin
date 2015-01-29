using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    public class IntervPageModel
    {
        private Intervention theIntervention;
        private List<InterventionDoc> documents = new List<InterventionDoc>();

        private List<MaskValue> programType = new List<MaskValue>();
        private List<Answer> documentType = new List<Answer>();

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

        public List<Answer> DocumentType
        {
            get { return documentType; }
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
    }

    #endregion

    #region Screening Model

    public class ScreeningModel
    {
        private List<Study> studyDocuments = new List<Study>();
        private Intervention intervention = new Intervention();
        private List<Answer> studyDesigns;
        private List<Answer> ypyn;

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScreeningModel() { }

        public ScreeningModel(List<Study> inDocs, Intervention interV, List<Answer> inStudyDesigns, List<Answer> inYPYN)
        {
            studyDocuments = inDocs;
            intervention = interV;
            studyDesigns = inStudyDesigns;
            ypyn = inYPYN;
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
    }

    #endregion
}
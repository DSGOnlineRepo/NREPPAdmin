using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NREPPAdminSite.Models
{
    public class ProgramDetailModel
    {
        


        public ProgramDetailModel()
        {
            /*
             * Ques2A:CheckBoxQuestion2A
             * Ques2B:CheckBoxQuestion2B
             * Ques3:CheckBoxQuestion3
             * Ques4:CheckBoxQuestion4
             * Ques5:CheckBoxQuestion5
             * Table2:Implementation Training Type Of Option List
             * Table3:Types of Training Available  Training Activities List
             * Table5:Program Specific Dissemination Information Types of dissemination information List
             * Table6:Continued Learning and Quality Assurance Materials Type of item/activity List
             */
            //CheckBoxQueston 2A Options List



            //List of the check box questions
            Ques = new List<Questions>();
            Ques.Add(new Questions { id = 1, Question = "2. Who is the program designed to be provided by? (e.g., individual counselor, multiservice team, or teachers.) Check all that apply."});
            Ques.Add(new Questions { id = 2, Question = "b. Is there a specified staff ratio of providers to clients of the program (i.e., teacher/student per student, social workers to clients)?"});
            Ques.Add(new Questions { id = 3, Question = "3. What is the average length of time needed for a provider to implement the program as designed (include time to cover delivery, training, supervision, preparation, and travel)?"});
            Ques.Add(new Questions { id = 4, Question = "4. What is the usual size group of providers who are trained to implement the program? " });
            Ques.Add(new Questions { id = 5, Question = "5. What previous education and/or experience is required for line staff before they are able to provide the program?  Check all that apply."});

            //CheckBoxQueston 2A Options List
            Ques2A = new List<CheckBoxQuestions2A>();
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 1, name = "Individuals (e.g., case manager or counselor) Specify", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 2, name = "A team of people (e.g., multidisciplinary team) Specify", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 3, name = "An agency (e.g., school system) Specify", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 4, name = "Multiple agencies (e.g., police department and school collaboratively) Specify", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 5, name = "The program is to be provided by software or via the internet Specify", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2A.Add(new CheckBoxQuestions2A { chkBoxid = 6, name = "Other (specify):", isSelected = false, specify = true, txtBoxSpecify = "" });

            //CheckBoxQueston 2B Options List
            Ques2B = new List<CheckBoxQuestions2B>();
            Ques2B.Add(new CheckBoxQuestions2B { chkBoxid = 1, name = "Yes (specify):", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques2B.Add(new CheckBoxQuestions2B { chkBoxid = 2, name = "No", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques2B.Add(new CheckBoxQuestions2B { chkBoxid = 3, name = "Not applicable", isSelected = false, specify = false, txtBoxSpecify = "" });

            //CheckBoxQueston 3 Options List
            Ques3 = new List<CheckBoxQuestions3>();
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 1, name = "1 week or less", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 2, name = "1 month or less", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 3, name = "1 to 6 months", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 4, name = "6 months to 1 year", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 5, name = "Greater than 1 year", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques3.Add(new CheckBoxQuestions3 { chkBoxid = 6, name = "Other (specify):", isSelected = false, specify = true, txtBoxSpecify = "" });

            //CheckBoxQueston 4 Options List
            Ques4 = new List<CheckBoxQuestions4>();
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 1, name = "1 – 12 individuals per training ", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 2, name = "13 – 30 individuals per training ", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 3, name = "31—50 individuals per training", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 4, name = "> 51 individuals per training ", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 5, name = "No training required ", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 6, name = "Self-training completed by individuals", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 7, name = "Other (specify):", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques4.Add(new CheckBoxQuestions4 { chkBoxid = 8, name = "Not applicable", isSelected = false, specify = false, txtBoxSpecify = "" });

            //CheckBoxQueston 5 Options List
            Ques5 = new List<CheckBoxQuestions5>();
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 1, name = "Master’s degree", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 2, name = "Bachelor’s degree", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 3, name = "High school diploma or GED ", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 4, name = "Specific license or credential (e.g., M.D., LADAC)", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 5, name = "Lived experience: ", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 6, name = "Other (specify): ", isSelected = false, specify = true, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 7, name = "None", isSelected = false, specify = false, txtBoxSpecify = "" });
            Ques5.Add(new CheckBoxQuestions5 { chkBoxid = 8, name = "Not applicable", isSelected = false, specify = false, txtBoxSpecify = "" });

            //Table2: Implementation Training Type Of Option List
            Table2 = new List<ImplementationTraining>();
            Table2.Add(new ImplementationTraining { id = 1, name = "Manual" });
            Table2.Add(new ImplementationTraining { id = 2, name = "Textbook" });
            Table2.Add(new ImplementationTraining { id = 3, name = "Book chapter" });
            Table2.Add(new ImplementationTraining { id = 4, name = "Article" });
            Table2.Add(new ImplementationTraining { id = 5, name = "Presentation " });
            Table2.Add(new ImplementationTraining { id = 6, name = "Video" });
            Table2.Add(new ImplementationTraining { id = 7, name = "Handouts" });
            Table2.Add(new ImplementationTraining { id = 8, name = "Webinar" });
            Table2.Add(new ImplementationTraining { id = 9, name = "Online training" });
            Table2.Add(new ImplementationTraining { id = 10, name = "Case studies" });
            Table2.Add(new ImplementationTraining { id = 11, name = "Implementation guide" });
            Table2.Add(new ImplementationTraining { id = 12, name = "Lesson-based curriculum" });
            Table2.Add(new ImplementationTraining { id = 13, name = "Other (specify)" });

            //Table3: Types of Training Available  Training Activities List
            Table3 = new List<TypesofTrainingAvailable>();
            Table3.Add(new TypesofTrainingAvailable { id = 1, name = "Line staff/Direct service training" });
            Table3.Add(new TypesofTrainingAvailable { id = 2, name = "Supervisory training" });
            Table3.Add(new TypesofTrainingAvailable { id = 3, name = "Overview training" });
            Table3.Add(new TypesofTrainingAvailable { id = 4, name = "Booster training" });
            Table3.Add(new TypesofTrainingAvailable { id = 5, name = "Other (specify)" });

            //Table4:  Program Specific Technical Assistance
            Table4 = new List<ProgramSpecificTA>();
            Table4.Add(new ProgramSpecificTA { id = 1, name = "Describe TA" });

            //Table5:  Program Specific Dissemination Information Types of dissemination information List
            Table5 = new List<ProgramSpecificDisseminationInfo>();
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 1, name = "Brochures" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 2, name = "Videos" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 3, name = "Handouts" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 4, name = "Presentations" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 5, name = "Testimonials" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 6, name = "Announcements" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 7, name = "Press release" });
            Table5.Add(new ProgramSpecificDisseminationInfo { id = 8, name = "Other (specify)" });


            //Table6: Continued Learning and Quality Assurance Materials Type of item/activity List   
            Table6 = new List<ContinuedLearningQualityAssuranceMaterials>();
            Table6.Add(new ContinuedLearningQualityAssuranceMaterials { id = 1, name = "Learning community/Peer to peer" });
            Table6.Add(new ContinuedLearningQualityAssuranceMaterials { id = 2, name = "Certification" });
            Table6.Add(new ContinuedLearningQualityAssuranceMaterials { id = 3, name = "Observation" });
            Table6.Add(new ContinuedLearningQualityAssuranceMaterials { id = 4, name = "SFidelity assessment"});
            Table6.Add(new ContinuedLearningQualityAssuranceMaterials { id = 5, name = "Other (specify)" });

        }

       DateTime _ProgramDate =DateTime.Now;
        [Display(Name = "Date:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:MM-dd-yyyy}")]
        public DateTime ProgramDate { get { return (_ProgramDate == DateTime.Now ? DateTime.Now : _ProgramDate);} set { _ProgramDate = value; } }
        [Display(Name = "Name of program:")]
        public string ProgramName { get; set; }
        [Display(Name = "Name of program contact completing this form:")]
        public string ProgramContact { get; set; }
        [EmailAddress]
        [Display(Name = "Email of program contact: ")]
        public string Email { get; set; }
        [Display(Name = "Program website(s):")]
        public string ProgramWebsite { get; set; }
        public int ProgramId { get; set; }

        //List to store the checkbox options
        public List<CheckBoxQuestions2A> Ques2A { get; set; }
        public List<CheckBoxQuestions2B> Ques2B { get; set; }
        public List<CheckBoxQuestions3> Ques3 { get; set; }
        public List<CheckBoxQuestions4> Ques4 { get; set; }
        public List<CheckBoxQuestions5> Ques5 { get; set; }

        //List to store the checkbox question
        public List<Questions> Ques { get; set; }

        //List to store the table items
        public List<ImplementationTraining> Table2 { get; set; }
        public List<TypesofTrainingAvailable> Table3 { get; set; }
        public List<ProgramSpecificTA> Table4 { get; set; }
        public List<ProgramSpecificDisseminationInfo> Table5 { get; set; }
        public List<ContinuedLearningQualityAssuranceMaterials> Table6 { get; set; }

        public List<string> txtBoxImpTraining { get; set; }
        public List<string> txtBoxTypesofTraining { get; set; }
        public List<string> txtBoxPrgmDiss { get; set; }
        public List<string> txtBoxQltyMat { get; set; }

        //List to store the checkbox textbox data
        public List<string> txtBoxSpecify2A { get; set; }
        public List<string> txtBoxSpecify2B { get; set; }
        public List<string> txtBoxSpecify3 { get; set; }
        public List<string> txtBoxSpecify4 { get; set; }
        public List<string> txtBoxSpecify5 { get; set; }
        
        //List for the tables
        public List<ImplementationLocation> table1List { get; set; }
        public List<ImplementationTrainings> table2List { get; set; }
        public List<TypeofTrngAvailable> table3List { get; set; }
        public List<ProgramSpecificTechnicalAssistance> table4List { get; set; }
        public List<PrgmSpecificDisseminationInformation> table5List { get; set; }
        public List<ContinuedLrngQualityAssuranceMaterials> table6List { get; set; }
        public List<ChkBoxAnswers> chkBoxAnsList { get; set; }

    }

}
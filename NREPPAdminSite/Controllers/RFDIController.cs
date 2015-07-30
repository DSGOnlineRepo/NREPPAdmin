using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NREPPAdminSite.Context;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Controllers
{
    public class RFDIController : Controller
    {
        //
        // GET: /Home/

        /// <summary>
        /// Start method which will insert multiple records and return values to view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProgramDetailModel objPgrmDetail = new ProgramDetailModel();
            try
            {
                //implementation of IDatabaseInitializer that will delete, recreate,
                //and optionally re-seed the database with data only if the model has changed since the database was created. 
                //This implementation require you to use the type of the Database Context because it’s a generic class. 
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RFDIEntityContext>());
                return View(objPgrmDetail);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult InsertProgramDetail(ProgramDetailModel pgrmdetail)
        {
            var programID = 0;            
            try
            {
                var prgmDetails = new ProgramDetail()
                   {
                       Id=pgrmdetail.ProgramId,
                       ProgramDate = pgrmdetail.ProgramDate,
                       ProgramName = pgrmdetail.ProgramName,
                       ProgramContact = pgrmdetail.ProgramContact,
                       Email = pgrmdetail.Email,
                       ProgramWebsite = pgrmdetail.ProgramWebsite
                   };                       
                using (var context = new RFDIEntityContext())
                {
                    if (prgmDetails.Id == 0)
                    {
                        context.programDetails.Add(prgmDetails);
                        context.SaveChanges();
                        programID = prgmDetails.Id;
                    }
                    else
                    {
                        programID = prgmDetails.Id;
                        context.programDetails.Attach(prgmDetails);                           
                        context.Entry(prgmDetails).State = System.Data.Entity.EntityState.Modified;                          
                        context.SaveChanges();
                        programID = prgmDetails.Id;
                    }
                }

                if (programID != null)
                {                                      
                    ProgramDetailModel objPgrmModel = new ProgramDetailModel();
                    var context = new RFDIEntityContext();
                   var size = 0;
                    dynamic count = null;
                    dynamic objpgrmdetail = null;
                    dynamic txtSpecify=null;
                    dynamic objtxtSpecify = null;
                    context.chkboxAns.RemoveRange(context.chkboxAns.Where(m => m.ProgramId == pgrmdetail.ProgramId));
                    for (int j = 0; j <= objPgrmModel.Ques.Count-1;j++ )
                    {
                        //For checkbox question 2A
                         if (j == 0)
                         {
                             size = objPgrmModel.Ques2A.Count;
                             count = objPgrmModel.Ques2A;
                             objpgrmdetail = pgrmdetail.Ques2A;
                             objtxtSpecify = pgrmdetail.txtBoxSpecify2A;
                              
                             
                         }
                         //For checkbox question 2B
                         if (j == 1)
                         {
                             size = objPgrmModel.Ques2B.Count;
                             count = objPgrmModel.Ques2B;
                             objpgrmdetail = pgrmdetail.Ques2B;
                             objtxtSpecify = pgrmdetail.txtBoxSpecify2B;
                              
                         }
                         //For checkbox question 3
                         if (j == 2)
                         {
                             size = objPgrmModel.Ques3.Count;
                             count = objPgrmModel.Ques3;
                             objpgrmdetail = pgrmdetail.Ques3;
                             objtxtSpecify = pgrmdetail.txtBoxSpecify3;
                         }
                         //For checkbox question 4
                         if (j == 3)
                         {
                             size = objPgrmModel.Ques4.Count;
                             count = objPgrmModel.Ques4;
                             objpgrmdetail = pgrmdetail.Ques4;
                             objtxtSpecify = pgrmdetail.txtBoxSpecify4;
                         }
                         //For checkbox question 5
                         if (j == 4)
                         {
                             size = objPgrmModel.Ques5.Count;
                             count = objPgrmModel.Ques5;
                             objpgrmdetail = pgrmdetail.Ques5;
                             objtxtSpecify = pgrmdetail.txtBoxSpecify5;
                         }
                        //To store all the checkbox answers
                             int cnttxtbox=0;
                             for (int i = 0; i <size; i++)
                             {

                                 if (count[i].specify == true)
                                 {
                                     txtSpecify = objpgrmdetail[i];
                                     cnttxtbox++;
                                 }
                                 if (objpgrmdetail[i].isSelected == true)
                                     {
                                         var chkboxAnswers = new ChkBoxAnswers()
                                         {
                                             id=objpgrmdetail[i].id,
                                             ProgramId = programID,
                                             QuestionId = j+1,
                                             CheckBoxId = objpgrmdetail[i].chkBoxid,
                                             SpecifiedAnswer =objpgrmdetail[i].txtBoxSpecify ,
                                             IsChecked=true,
                                         };
                                         txtSpecify = "";
                                         if (pgrmdetail.ProgramId == 0)
                                         {
                                             context.chkboxAns.Add(chkboxAnswers);
                                             context.SaveChanges();
                                         }
                                         else
                                         {
                                             context.chkboxAns.Add(chkboxAnswers);
                                             context.SaveChanges();
                                         }
                                     }                                 
                             }              
                    }
                            SaveImplementationLocation(pgrmdetail, programID);
                            SaveImplementationTrainings(pgrmdetail, programID);
                            SaveTypeOfTrainings(pgrmdetail, programID);
                            SavePrgmSpecificDisseminationInformation(pgrmdetail, programID);
                            SaveContinuedLrngQualityAssuranceMaterials(pgrmdetail, programID);
                            SaveProgramSpecificTA(pgrmdetail, programID);
                            context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
               return Json(ex.Message) ;
            }
            return RedirectToAction("DisplayProgramDetails", "Home");
        }

        //Store the data related to the Table1 i.e Implementatio Location
        public void SaveImplementationLocation(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {              
                foreach (var item in pgrmDetail.table1List)
                {
                    var impLocation = new ImplementationLocation()
                    {
                        Id=item.Id,
                        ProgramId = programID,
                        NumberofIndividuals = item.NumberofIndividuals,
                        ProgramLocations = item.ProgramLocations,
                        ProgramStartedYear = item.ProgramStartedYear,
                        ActiveInLocation = item.ActiveInLocation,                       
                    };
                    if (pgrmDetail.ProgramId == 0 || (pgrmDetail.ProgramId > 0 && item.Id==0))  //To insert new Record
                    {
                        context.implemantationLocationDetails.Add(impLocation);
                        context.SaveChanges();
                    }
                    else
                    {

                        context.implemantationLocationDetails.Attach(impLocation);  //To update existing record
                        context.Entry(impLocation).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    
                }

            }
        }

        //Store the data related to the Table2 i.e Implementatio Training
        public void SaveImplementationTrainings(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {
                int itemid = 1;
                foreach (var item in pgrmDetail.table2List)
                {
                    var impTraining = new ImplementationTrainings()
                    {
                        Id = item.Id,
                        ProgramId = programID,
                        TypeofItemId = itemid,
                        Nameofitem = item.Nameofitem,
                        IntendedAudience = item.IntendedAudience,
                        Whereavailable = item.Whereavailable,
                        AvailableLaunguages = item.AvailableLaunguages,
                        Required = item.Required,
                        TypicalCost = item.TypicalCost,
                    };
                    itemid++;
                    if (pgrmDetail.ProgramId == 0)   //To insert new Record
                    {
                        context.impleTraining.Add(impTraining);
                        context.SaveChanges();
                    }
                    else
                    {

                        context.impleTraining.Attach(impTraining);   //To update existing record
                        context.Entry(impTraining).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }                   
                }

            }
        }

        //Store the data related to the Table3 i.e Type Of Trainings Available
        public void SaveTypeOfTrainings(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {
                int itemid = 1;
                foreach (var item in pgrmDetail.table3List)
                {
                    var typesOfTraining = new TypeofTrngAvailable()
                    {
                        Id = item.Id,
                        ProgramId = programID,
                        TrngId = itemid,
                        Nameoftrng = item.Nameoftrng,
                        Trngdeliverymethod = item.Trngdeliverymethod,
                        Whereavailable = item.Whereavailable,
                        AvailableLang = item.AvailableLang,
                        RequiredTraining = item.RequiredTraining,
                        DurationTraining = item.DurationTraining,
                        TypicalTrainingCostPerUnit = item.TypicalTrainingCostPerUnit,
                    };
                    itemid++;
                    if (pgrmDetail.ProgramId == 0)   //To insert new Record
                    {
                        context.typeofTrngAvailable.Add(typesOfTraining);
                        context.SaveChanges();
                    }
                    else
                    {

                        context.typeofTrngAvailable.Attach(typesOfTraining);  //To update existing record
                        context.Entry(typesOfTraining).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
        }

        //Store the data related to the Table4 i.e Program Specific Technical Assistance
        public void SaveProgramSpecificTA(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {
                int itemid = 1;
                foreach (var item in pgrmDetail.table4List)
                {
                    var pgrmSpecTA = new ProgramSpecificTechnicalAssistance()
                    {
                        Id = item.Id,
                        ProgramId = programID,
                        TATypeId = itemid,
                        NameofTA = item.NameofTA,
                        TAdeliverymethod = item.TAdeliverymethod,
                        Accessibleformats = item.Accessibleformats,
                        RequiredTA = item.RequiredTA,
                        Duration = item.Duration,
                        TypicalCostPerUnit = item.TypicalCostPerUnit,
                    };
                    itemid++;
                    if (pgrmDetail.ProgramId == 0)   //To insert new Record
                    {
                        context.programSpecificTA.Add(pgrmSpecTA);
                        context.SaveChanges();
                    }
                    else
                    {

                        context.programSpecificTA.Attach(pgrmSpecTA);  //To update existing record
                        context.Entry(pgrmSpecTA).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
        }


        //Store the data related to the Table5 i.e Program Specific Dissemination Information
        public void SavePrgmSpecificDisseminationInformation(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {
                int itemid = 1;
                foreach (var item in pgrmDetail.table5List)
                {
                    var pgrmSpecDissInfo = new PrgmSpecificDisseminationInformation()
                    {
                        Id = item.Id,
                        ProgramId = programID,
                        TypeofdisseminationinformationId = itemid,
                        NameofDissItem = item.NameofDissItem,
                        Disseminationformat = item.Disseminationformat,
                        DisseminationWhereavailable = item.DisseminationWhereavailable,
                        DisseminationAvailableLang = item.DisseminationAvailableLang,
                        DisseminationRequired = item.DisseminationRequired,
                        DisseminationTypicalCostPerUnit = item.DisseminationTypicalCostPerUnit,
                    };
                    itemid++;
                    if (pgrmDetail.ProgramId == 0)    //To insert new Record
                    {
                        context.prgmDissInfo.Add(pgrmSpecDissInfo);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.prgmDissInfo.Attach(pgrmSpecDissInfo);    //To update existing record
                        context.Entry(pgrmSpecDissInfo).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
        }

        //Store the data related to the Table6 i.e Continued Learning and Quality Assurance Materials    
        public void SaveContinuedLrngQualityAssuranceMaterials(ProgramDetailModel pgrmDetail, int programID)
        {
            using (var context = new RFDIEntityContext())
            {
                int itemid = 1;
                foreach (var item in pgrmDetail.table6List)
                {
                    var conLrngAssMat = new ContinuedLrngQualityAssuranceMaterials()
                    {
                        Id = item.Id,
                        ProgramId = programID,
                        TypeofQualityAssuranceitem = itemid,
                        NameofQualityAssuranceItem = item.NameofQualityAssuranceItem,
                        QualityAssuranceformat = item.QualityAssuranceformat,
                        QualityAssuranceWhereavailable = item.QualityAssuranceWhereavailable,
                        QualityAssuranceAvailableLang = item.QualityAssuranceAvailableLang,
                        QualityAssuranceRequired = item.QualityAssuranceRequired,
                        QualityAssuranceDuration = item.QualityAssuranceDuration,
                        QualityAssuranceTypicalCostPerUnit = item.QualityAssuranceTypicalCostPerUnit,
                    };
                    itemid++;
                    if (pgrmDetail.ProgramId == 0)                     //To insert new record
                    {
                        context.conLrngQltyMat.Add(conLrngAssMat);
                        context.SaveChanges();
                    }
                    else
                    {

                        context.conLrngQltyMat.Attach(conLrngAssMat);               //To update existing record
                        context.Entry(conLrngAssMat).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
        }

        //Displays all the PRogram Details in a table
        public ActionResult DisplayProgramDetails() 
        {
            var dbContext = new RFDIEntityContext();
            var programList = from program in dbContext.programDetails select program;
            dynamic programs = new List<GetProgramDetails>();
            if (programList.Any())
            {
                foreach (var program in programList)
                {
                    programs.Add(new GetProgramDetails() { ProgramId=program.Id,ProgramName = program.ProgramName,ProgramContact=program.ProgramContact,Email=program.Email,ProgramWebsite=program.ProgramWebsite});
                }
            }
            return View(programs);
        }

        //Gets the data of the selected program 
        public ActionResult editProgramDetails(int? id)
        {
            var programs = new ProgramDetailModel();
            var dbContext = new RFDIEntityContext();
            var programDetails = dbContext.programDetails.FirstOrDefault(m => m.Id == id);
            programs.table1List= dbContext.implemantationLocationDetails.Where(m=>m.ProgramId==id).ToList();
            programs.table2List = dbContext.impleTraining.Where(m => m.ProgramId == id).ToList();
            programs.table3List = dbContext.typeofTrngAvailable.Where(m => m.ProgramId == id).ToList();
            programs.table4List = dbContext.programSpecificTA.Where(m => m.ProgramId == id).ToList();
            programs.table5List = dbContext.prgmDissInfo.Where(m => m.ProgramId == id).ToList();
            programs.table6List = dbContext.conLrngQltyMat.Where(m => m.ProgramId == id).ToList();
            if (programDetails != null)
            {
                programs.ProgramId = programDetails.Id;
                programs.ProgramDate = programDetails.ProgramDate;//string.Format("{0:MM/dd/yyyy}",DateTime.Now)
                programs.ProgramName = programDetails.ProgramName;
                programs.ProgramContact = programDetails.ProgramContact;
                programs.Email = programDetails.Email;
                programs.ProgramWebsite = programDetails.ProgramWebsite;               
            }

            //Gets the answers related to the question2A
            var chkAnswers2A = dbContext.chkboxAns.Where(m => m.ProgramId == id && m.QuestionId == 1).ToList();

            for (int i = 0; i < programs.Ques2A.Count; i++)
            {
                for (int j = 0; j < chkAnswers2A.Count; j++)
                {
                    if (programs.Ques2A[i].chkBoxid == chkAnswers2A[j].CheckBoxId)
                    {
                        programs.Ques2A[i].id = chkAnswers2A[j].id;
                        programs.Ques2A[i].chkBoxid = chkAnswers2A[j].CheckBoxId;
                        programs.Ques2A[i].ProgramId = chkAnswers2A[j].ProgramId;
                        programs.Ques2A[i].isSelected = chkAnswers2A[j].IsChecked;
                        programs.Ques2A[i].txtBoxSpecify = chkAnswers2A[j].SpecifiedAnswer;
                    }
                }
                
            }

            //Gets the answers related to the question2B
            var chkAnswers2B = dbContext.chkboxAns.Where(m => m.ProgramId == id && m.QuestionId == 2).ToList();

            for (int i = 0; i < programs.Ques2B.Count; i++)
            {
                for (int j = 0; j < chkAnswers2B.Count; j++)
                {
                    if (programs.Ques2B[i].chkBoxid == chkAnswers2B[j].CheckBoxId)
                    {
                        programs.Ques2B[i].id = chkAnswers2B[j].id;
                        programs.Ques2B[i].chkBoxid = chkAnswers2B[j].CheckBoxId;
                        programs.Ques2B[i].ProgramId = chkAnswers2B[j].ProgramId;
                        programs.Ques2B[i].isSelected = chkAnswers2B[j].IsChecked;
                        programs.Ques2B[i].txtBoxSpecify = chkAnswers2B[j].SpecifiedAnswer;
                    }
                }

            }

            //Gets the answers related to the question3
            var chkAnswers3 = dbContext.chkboxAns.Where(m => m.ProgramId == id && m.QuestionId == 3).ToList();

            for (int i = 0; i < programs.Ques3.Count; i++)
            {
                for (int j = 0; j < chkAnswers3.Count; j++)
                {
                    if (programs.Ques3[i].chkBoxid == chkAnswers3[j].CheckBoxId)
                    {
                        programs.Ques3[i].id = chkAnswers3[j].id;
                        programs.Ques3[i].chkBoxid = chkAnswers3[j].CheckBoxId;
                        programs.Ques3[i].ProgramId = chkAnswers3[j].ProgramId;
                        programs.Ques3[i].isSelected = chkAnswers3[j].IsChecked;
                        programs.Ques3[i].txtBoxSpecify = chkAnswers3[j].SpecifiedAnswer;
                    }
                }

            }
            //Gets the answers related to the question4
            var chkAnswers4 = dbContext.chkboxAns.Where(m => m.ProgramId == id && m.QuestionId == 4).ToList();
            for (int i = 0; i < programs.Ques4.Count; i++)
            {
                for (int j = 0; j < chkAnswers4.Count; j++)
                {
                    if (programs.Ques4[i].chkBoxid == chkAnswers4[j].CheckBoxId)
                    {
                        programs.Ques4[i].id = chkAnswers4[j].id;
                        programs.Ques4[i].chkBoxid = chkAnswers4[j].CheckBoxId;
                        programs.Ques4[i].ProgramId = chkAnswers4[j].ProgramId;
                        programs.Ques4[i].isSelected = chkAnswers4[j].IsChecked;
                        programs.Ques4[i].txtBoxSpecify = chkAnswers4[j].SpecifiedAnswer;
                    }
                }

            }
            //Gets the answers related to the question5
            var chkAnswers5 = dbContext.chkboxAns.Where(m => m.ProgramId == id && m.QuestionId == 5).ToList();

            for (int i = 0; i < programs.Ques5.Count; i++)
            {
                for (int j = 0; j < chkAnswers5.Count; j++)
                {
                    if (programs.Ques5[i].chkBoxid == chkAnswers5[j].CheckBoxId)
                    {
                        programs.Ques5[i].id = chkAnswers5[j].id;
                        programs.Ques5[i].chkBoxid = chkAnswers5[j].CheckBoxId;
                        programs.Ques5[i].ProgramId = chkAnswers5[j].ProgramId;
                        programs.Ques5[i].isSelected = chkAnswers5[j].IsChecked;
                        programs.Ques5[i].txtBoxSpecify = chkAnswers5[j].SpecifiedAnswer;
                    }
                }

            }
            return View("Index", programs);
        }
    }
}

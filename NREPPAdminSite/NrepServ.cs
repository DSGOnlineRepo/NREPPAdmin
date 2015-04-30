using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using NREPPAdminSite.Models;
using System.Security.Principal;
using System.Web.Script.Serialization;
using System.Data.SqlTypes;
using System.IO;
using NREPPAdminSite.Constants;

namespace NREPPAdminSite
{
    public class NrepServ
    {
        private SqlConnection conn; // There is a better way to do this for the time being, but let's assume that this will be replaced with something much better.

        #region Constructors

        public NrepServ() // Dummy Method
        {
            conn = null;
        }

        public NrepServ(string ConnectionString)
        {
            conn = new SqlConnection(ConnectionString);
        }

        #endregion

        #region Service-Like Methods

        #region Misc Functionality

        public void ChangeStatus(int inId, int inUser, int ToStatus)
        {
            SqlCommand cmd = new SqlCommand("SPChangeInterventionStatus", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@IntervId", inId));
            cmd.Parameters.Add(new SqlParameter("@User", inUser));
            cmd.Parameters.Add(new SqlParameter("@DestUser", inUser));
            cmd.Parameters.Add(new SqlParameter("@DestStatus", ToStatus));

            try
            {
                CheckConn();

                cmd.ExecuteNonQuery();

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<Answer> GetTaxonomicOutcomes(int? inId)
        {
            List<Answer> TheOutcomes = new List<Answer>();

            // SQL Stuff
            SqlCommand GetTaxonomy = new SqlCommand("SPGetTaxOutcomes", conn);

            GetTaxonomy.CommandType = CommandType.StoredProcedure;
            GetTaxonomy.Parameters.Add(new SqlParameter("@TaxId", inId));

            SqlDataAdapter da = new SqlDataAdapter(GetTaxonomy);


            try
            {
                CheckConn();

                DataTable dt = new DataTable();


                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    TheOutcomes.Add(new Answer((int)dr["Id"], dr["Guidelines"].ToString(), dr["OutcomeName"].ToString()));
                }

            }
            catch (Exception ex)
            {
                Answer nAnswer = new Answer();
                nAnswer.ShortAnswer = "Error!";
                nAnswer.LongAnswer = ex.Message;
                nAnswer.AnswerId = -1;
                TheOutcomes.Add(nAnswer);
            }

            return TheOutcomes;
        }
        
        /// <summary>
        /// Gets a an IEnumberable of Answers from the provided Category
        /// </summary>
        /// <param name="inCategory">The Category's Name</param>
        /// <returns>The Answers as an IEnumerable</returns>
        public IEnumerable<Answer> GetAnswersByCategory(string inCategory)
        {
            List<Answer> OutAnswers = new List<Answer>();

            // SQL Stuff
            SqlCommand GetAnswersByCategory = new SqlCommand("SPGetAnswersByCategory", conn);

            GetAnswersByCategory.CommandType = CommandType.StoredProcedure;
            GetAnswersByCategory.Parameters.Add(new SqlParameter("@InCategoryName", inCategory));

            SqlDataAdapter da = new SqlDataAdapter(GetAnswersByCategory);
         

            try
            {
                CheckConn();

                DataTable dt = new DataTable();
                

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    OutAnswers.Add(new Answer((int)dr["AnswerId"], dr["ShortAnswer"].ToString(), dr["LongAnswer"].ToString()));
                }

            } catch (Exception ex)
            {
                Answer nAnswer = new Answer();
                nAnswer.ShortAnswer = "Error!";
                nAnswer.LongAnswer = ex.Message;
                nAnswer.AnswerId = -1;
                OutAnswers.Add(nAnswer);
            }

            return OutAnswers;
        }

        public IEnumerable<MaskValue> GetMaskList(string inMaskName)
        {
            List<MaskValue> OutAnswers = new List<MaskValue>();

            // SQL Stuff
            SqlCommand cmdGetMasks = new SqlCommand("SPGetMasksByCategory", conn);

            cmdGetMasks.CommandType = CommandType.StoredProcedure;
            cmdGetMasks.Parameters.Add(new SqlParameter("@InCategory", inMaskName));

            SqlDataAdapter da = new SqlDataAdapter(cmdGetMasks);


            try
            {
                CheckConn();

                DataTable dt = new DataTable();


                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    OutAnswers.Add(new MaskValue() { Name = dr["MaskValueName"].ToString(), Value = (int)dr["MaskPower"], Selected = false });
                }

            }
            catch (Exception ex)
            {
                MaskValue nAnswer = new MaskValue();
                //nAnswer.Name = "Error!";
                nAnswer.Name = ex.Message;
                nAnswer.Value = -1;
                nAnswer.Selected = false;
                OutAnswers.Add(nAnswer);
            }

            return OutAnswers;
        }

        #endregion

        #region Intervention Functionality

        /// <summary>
        /// Generically gets the interventions list
        /// </summary>
        /// <returns></returns>
        public List<Intervention> GetInterventions() // This needs to take some parameters, so there should be a bunch of functions for it
        {
            List<SqlParameter> nullParams = new List<SqlParameter> { new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = null } };
            return this.GetInterventions(nullParams);
        }

        /// <summary>
        /// Gets the interventions list based on role
        /// </summary>
        /// <param name="RoleId">User's Role Id</param>
        /// <returns></returns>
        public List<Intervention> GetInterventions(string RoleName) // This needs to take some parameters, so there should be a bunch of functions for it
        {
            List<SqlParameter> nullParams = new List<SqlParameter> { new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = null } };
            nullParams.Add(new SqlParameter() { ParameterName = "@RoleName", SqlDbType = SqlDbType.NVarChar, Value = RoleName });
            return this.GetInterventions(nullParams);
        }


        // TODO: Generalize the parameter passing better? I need something that will be easier to maintain

        /// <summary>
        /// Gets a specific list of interventions
        /// </summary>
        /// <param name="parameters">A List of parameters to govern which records you want</param>
        /// <returns>A List if intervention objects</returns>
        public List<Intervention> GetInterventions(List<SqlParameter> parameters)
        {
            List<Intervention> interventions = new List<Intervention>();
            SqlCommand cmdGetInterventions = new SqlCommand("SPGetInterventionList", conn);
            cmdGetInterventions.CommandType = CommandType.StoredProcedure;
            Intervention inv;

            foreach (SqlParameter param in parameters)
                cmdGetInterventions.Parameters.Add(param);

            try
            {
                CheckConn();
                DataTable interVs = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdGetInterventions);
                da.Fill(interVs);

                foreach (DataRow dr in interVs.Rows)
                {
                    inv = new Intervention((int)dr["InterventionId"], dr["Title"].ToString(), dr["FullDescription"].ToString(), dr["Submitter"].ToString(), NullDate(dr["PublishDate"]),
                        Convert.ToDateTime(dr["UpdateDate"]), dr["SubmitterId"].ToString(), dr["StatusName"].ToString(), (int)dr["StatusId"],
                        dr["ProgramType"] == DBNull.Value ? 0 : (int)dr["ProgramType"], dr["Acronym"].ToString(), false);

                    inv.PreScreenMask = (int)dr["PreScreenAnswers"];
                    inv.UserPreScreenMask = dr.IsNull("UserPreScreenAnswer") ? 0 :  (int)dr["UserPreScreenAnswer"];
                    inv.ScreeningNotes = dr.IsNull("ScreeningNotes")? "" : dr["ScreeningNotes"].ToString();

                    interventions.Add(inv);

                    
                }

            }
            catch (Exception ex)
            {
                //TODO: Why are we adding dummy intervention in case of a error
                //interventions.Add(new Intervention(-1, "Error!", ex.Message, "", DateTime.Now, DateTime.Now, User, "Submitted", 1, 0, "", false));
            }

            return interventions;
        }

        /// <summary>
        /// Gets documents associated with either an intervention or a Reviewer
        /// </summary>
        /// <param name="InterventionId">Intervention Id (nullable)</param>
        /// <param name="ReviewerId">Reviewer Id (nullable)</param>
        /// <returns></returns>
        public IEnumerable<InterventionDoc> GetDocuments(int? InterventionId, int? ReviewerId, int? DocId)
        {
            List<InterventionDoc> documents = new List<InterventionDoc>();
            SqlCommand cmdGetDocuments = new SqlCommand("SPGetDocuments", conn);
            cmdGetDocuments.CommandType = CommandType.StoredProcedure;

            cmdGetDocuments.Parameters.Add(new SqlParameter("@InvId", InterventionId));
            cmdGetDocuments.Parameters.Add(new SqlParameter("@ReviewerId", ReviewerId));
            cmdGetDocuments.Parameters.Add(new SqlParameter("@DocumentId", DocId));


            try
            {
                CheckConn();
                DataTable documentz = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdGetDocuments);
                da.Fill(documentz);

                foreach (DataRow dr in documentz.Rows)
                {
                    InterventionDoc doc = new InterventionDoc((int)dr["DocId"]);
                    doc.FileDescription = dr["Description"].ToString();
                    doc.Link = dr["FileName"].ToString(); // This needs some work
                    doc.Uploader = dr["Uploader"].ToString();
                    doc.SetDocType((int)dr["TypeOfDocument"], dr["Document Type Name"].ToString());

                    documents.Add(doc);
                }

            }
            catch (Exception ex)
            {
                //TODO: Why are we adding dummy intervention in case of a error
                //documents.Add(new InterventionDoc() { FileDescription = ex.Message, Link = "Error" });
            }

            return documents;
        }

        /// <summary>
        /// Save or Create a New Intervention
        /// </summary>
        /// <param name="inData">The intervention to be saved or added to the database</param>
        /// <returns>True if save is valid, false if it is not</returns>
        public int SaveIntervention(Intervention inData) {

            int returnValue = -1;
            SqlCommand cmdUpdate = new SqlCommand("SPUpdateIntervention", conn);
            cmdUpdate.CommandType = CommandType.StoredProcedure;

            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@IntervId", SqlDbType = SqlDbType.Int, Value = inData.Id });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = inData.Title });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@fulldescription", SqlDbType = SqlDbType.NText, Value = inData.FullDescription });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@submitterId", SqlDbType = SqlDbType.NVarChar, Value = inData.SubmitterId });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@updateDate", SqlDbType = SqlDbType.DateTime, Value = inData.UpdatedDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@publishDate", SqlDbType = SqlDbType.DateTime, Value = inData.PublishDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@status", SqlDbType = SqlDbType.Int, Value = inData.StatusId > 0 ? inData.StatusId : 1 });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@programType", SqlDbType = SqlDbType.Int, Value = inData.ProgramType });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@Acronym", SqlDbType = SqlDbType.VarChar, Value = inData.Acronym });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@IsLitSearch", SqlDbType = SqlDbType.Bit, Value = inData.FromLitSearch });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@PreScreenAnswers", SqlDbType = SqlDbType.Int, Value = inData.PreScreenMask });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@UserPreScreenAnswer", SqlDbType = SqlDbType.Int, Value = inData.UserPreScreenMask });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@ScreeningNotes", SqlDbType = SqlDbType.VarChar, Value = string.IsNullOrEmpty(inData.ScreeningNotes) ? "" : inData.ScreeningNotes });

            SqlParameter OutPut = new SqlParameter("@Output", -1);
            OutPut.Direction = ParameterDirection.Output;
            cmdUpdate.Parameters.Add(OutPut);

            try
            {
                CheckConn();
                cmdUpdate.ExecuteNonQuery();

                returnValue = (int)cmdUpdate.Parameters["@Output"].Value;
                



            } catch (Exception ex) // Just in here for error handling reasons
            {
                throw new Exception(ex.Message);
            }


            //cmdUpdate.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.Output;

            return returnValue;
        }

        public int SaveFileToDB(byte[] inData, string fileName, string uploaderName, string MIMEType, int IntervId, bool isDelete, int ItemId, string DisplayName,
            int documentType)
        {

            SqlCommand cmdSaveFile = new SqlCommand("SPAddOrRemoveDoc", conn);
            int retValue = -1;
            cmdSaveFile.CommandType = CommandType.StoredProcedure;

            cmdSaveFile.Parameters.Add(new SqlParameter("@IntervId", IntervId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@Description", DisplayName));
            cmdSaveFile.Parameters.Add(new SqlParameter("@MIMEType", MIMEType));
            cmdSaveFile.Parameters.Add(new SqlParameter("@IsDelete", isDelete));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ItemId", ItemId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@UploaderName", uploaderName));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ReviewerId", null));
            cmdSaveFile.Parameters.Add(new SqlParameter("@DocumentTypeId", documentType));
            //cmdSaveFile.Parameters.Add(new SqlParameter("@OutPut", null));

            SqlParameter OutPut = new SqlParameter("@Output", -1);
            OutPut.Direction = ParameterDirection.Output;
            cmdSaveFile.Parameters.Add(OutPut);

            try
            {
                // TODO: file name collision

                //string nFileName = ConfigurationManager.AppSettings["fileLocation"] + "\\" + IntervId.ToString() + "\\" + fileName;
                string nDirectory = ConfigurationManager.AppSettings["fileLocation"] + "\\" + IntervId.ToString() + "\\";
                string nFileName = nDirectory + fileName;

                if (!Directory.Exists(nDirectory))
                {
                    Directory.CreateDirectory(nDirectory);
                }


                FileStream someStream = new FileStream(nFileName, FileMode.OpenOrCreate);
                someStream.Write(inData, 0, inData.Length);
                someStream.Close();

                // If we successfully save the file...
                CheckConn();

                cmdSaveFile.Parameters.Add(new SqlParameter("@FileName", nFileName));
                retValue = cmdSaveFile.ExecuteNonQuery();

                // TODO: delete the file if the transaction failed.

            }
            catch (Exception) { }
            finally {
                conn.Close();
            }

            if (retValue >= 0)
                return (int)cmdSaveFile.Parameters["@Output"].Value;
            else
                return -1;
        }

        public byte[] GetFileFromDB(int fileNum)
        {
            byte[] outFile = null;
            SqlCommand cmdFileInfo = new SqlCommand("SPGetAFileFromDB", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmdFileInfo);
            DataTable dt = new DataTable();

            cmdFileInfo.CommandType = CommandType.StoredProcedure;
            cmdFileInfo.Parameters.Add(new SqlParameter("@FileId", fileNum));


            try
            {
                CheckConn();
                da.Fill(dt);
                string fileName = dt.Rows[0]["FileName"].ToString();
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                fs.Read(outFile, 0, Convert.ToInt32(fs.Length));

                fs.Close();

            } catch (Exception ex)
            {
                outFile = Encoding.ASCII.GetBytes(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return outFile;

        }

        public int DeleteDocument(int DocId, int theUser)
        {
            SqlCommand cmdSaveFile = new SqlCommand("SPAddOrRemoveDoc", conn);
            int retValue = -1;
            cmdSaveFile.CommandType = CommandType.StoredProcedure;

            cmdSaveFile.Parameters.Add(new SqlParameter("@IntervId", -1));
            //cmdSaveFile.Parameters.Add(new SqlParameter("@Description", DisplayName));
            cmdSaveFile.Parameters.Add(new SqlParameter("@MIMEType", ""));
            cmdSaveFile.Parameters.Add(new SqlParameter("@IsDelete", true));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ItemId", DocId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@UploaderId", theUser));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ReviewerId", null));
            //cmdSaveFile.Parameters.Add(new SqlParameter("@OutPut", null));

            SqlParameter OutPut = new SqlParameter("@Output", -1);
            OutPut.Direction = ParameterDirection.Output;
            cmdSaveFile.Parameters.Add(OutPut);

            List<InterventionDoc> documents = new List<InterventionDoc>();
            SqlCommand cmdGetDocuments = new SqlCommand("SPGetDocuments", conn);
            cmdGetDocuments.CommandType = CommandType.StoredProcedure;

            cmdGetDocuments.Parameters.Add(new SqlParameter("@InvId", null));
            cmdGetDocuments.Parameters.Add(new SqlParameter("@ReviewerId", null));
            cmdGetDocuments.Parameters.Add(new SqlParameter("@DocumentId", DocId));

            try
            {
                CheckConn();

                // 
                DataTable documentz = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdGetDocuments);
                da.Fill(documentz);

                foreach (DataRow dr in documentz.Rows)
                {
                    InterventionDoc doc = new InterventionDoc((int)dr["DocId"]);
                    doc.FileDescription = dr["Description"].ToString();
                    doc.Link = dr["FileName"].ToString(); // This needs some work
                    doc.Uploader = dr["Uploader"].ToString();
                    doc.SetDocType((int)dr["TypeOfDocument"], dr["Document Type Name"].ToString());

                    documents.Add(doc);
                }

                File.Delete(documents[0].Link); // This might not work

                CheckConn();

                cmdSaveFile.ExecuteNonQuery();

            }
            catch (Exception) {
                retValue = -2; // Indicates some other exception
            }
            finally
            {
                conn.Close();
            }

            return retValue;
        }

        /// <summary>
        /// Deletes a study record with the supplied ID
        /// </summary>
        /// <param name="RecordId"></param>
        public void DeleteStudyRecord(int RecordId)
        {
            SqlCommand cmd = new SqlCommand("SPDeleteStudyRecord", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StudyRecordID", RecordId));

            try
            {
                CheckConn();
                cmd.ExecuteNonQuery();

            } catch (Exception)
            {

            }
            finally { conn.Close(); }

            return;
        }

        public int AddStudyRecord(Study inStudy)
        {
            int returnValue = 0;
            SqlCommand cmd = new SqlCommand("SPAddOrUpdateStudyRecord", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(new SqlParameter() { ParameterName = "RETURN_VALUE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = inStudy.Id });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@StudyId", SqlDbType = SqlDbType.Int, Value = inStudy.StudyId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Reference", SqlDbType = SqlDbType.VarChar, Value = "Not used at the moment" });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@InLitSearch", SqlDbType = SqlDbType.Bit, Value = inStudy.inLitSearch });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Exclusion1", SqlDbType = SqlDbType.Int, Value = inStudy.Exclusion1 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@StudyDesign", SqlDbType = SqlDbType.Int, Value = inStudy.StudyDesign });
            //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@BaselineEquiv", SqlDbType = SqlDbType.VarChar, Value = inStudy.BaselineEquiv });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@UseMultivariate", SqlDbType = SqlDbType.Bit, Value = inStudy.UseMultivariate == null ? false : inStudy.UseMultivariate });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SAMSHARelated", SqlDbType = SqlDbType.Int, Value = inStudy.SAMSHARelated });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@AuthorQueryNeeded", SqlDbType = SqlDbType.Bit, Value = inStudy.AuthorQueryNeeded });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@RecommendReview", SqlDbType = SqlDbType.Bit, Value = inStudy.RecommendReview });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Notes", SqlDbType = SqlDbType.VarChar, Value = inStudy.Notes });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocumentId", SqlDbType = SqlDbType.Int, Value = inStudy.DocumentId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocOrdinal", SqlDbType = SqlDbType.Int, Value = inStudy.DocOrdinal });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@IDOut", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DiffAttr", SqlDbType = SqlDbType.Int, Value = inStudy.DiffAttrition });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OverallAttr", SqlDbType = SqlDbType.Int, Value = inStudy.OverallAttrition });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@TotalSampleSize", SqlDbType = SqlDbType.VarChar, Value = inStudy.TotalSampleSize });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@LongestFollowup", SqlDbType = SqlDbType.VarChar, Value = inStudy.LongestFollowup });

            try
            {
                CheckConn();

                int someVal = cmd.ExecuteNonQuery();
                returnValue = (int)cmd.Parameters["RETURN_VALUE"].Value;

            } catch (Exception)
            {
                returnValue = -1;
            }
            finally { conn.Close(); }

            return returnValue;
        }

        public IEnumerable<Study> GetStudiesByIntervention(int IntervId)
        {
            List<Study> OutList = new List<Study>();
            SqlCommand cmd = new SqlCommand("SPGetStudiesByIntervention", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@IntervId", IntervId));

            try
            {
                CheckConn();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    OutList.Add(new Study() { Id = (int)dr["Id"], Notes = dr["Notes"].ToString(), StudyId = (int)dr["StudyId"],
                        Reference = dr["Reference"].ToString(), DocumentId = (int)dr["DocumentId"], Exclusion1 = (int)dr["Exclusion1"], StudyDesign = (int)dr["StudyDesign"],
                    RecommendReview = (bool)dr["RecommendReview"], DocOrdinal = (int)dr["DocOrdinal"],
                    OverallAttrition = dr.IsNull("OverallAttritionAvail") ? 0 : (int)dr["OverallAttritionAvail"], TotalSampleSize = dr["TotalSampleSize"].ToString(),
                    DiffAttrition = dr.IsNull("DiffAttritionAvail") ? 0 : (int)dr["DiffAttritionAvail"], LongestFollowup = dr.IsNull("LongestFollowup") ? "" : dr["LongestFollowup"].ToString()
                    });
                }

            } catch (Exception ex)
            {
                OutList.Add(new Study() { Notes = ex.Message, Id = -1 });
            }
            finally
            {
                conn.Close();
            }

            return OutList;
        }

        public OutcomesWrapper GetOutcomesByIntervention(int IntervId)
        {
            OutcomesWrapper OutList;
            List<OutcomeMeasure> outcomeList = new List<OutcomeMeasure>();
            List<Outcome> studyOutcomeList = new List<Outcome>();
            SqlCommand cmd = new SqlCommand("SPGetOutcomesByInterventionId", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@InterventionId", IntervId));

            /*
             * [Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[OutcomeId] INT NOT NULL,
	[StudyId] INT NOT NULL,
	[OutcomeMeasure] VARCHAR(50) NULL, 
    [GroupFavored] BIT NULL DEFAULT 0, 
    [PopDescription] VARCHAR(50) NULL, 
    [SAMHSAPop] INT NULL,
	[SAMHSAOutcome] INT NULL,
	[EffectReport] INT NULL,
    [DocId] INT NULL, 
    [RecommendReview] BIT NULL DEFAULT 0, 
    [TaxonomyOutcome] INT NULL, 
             */

            try
            {
                CheckConn();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    outcomeList.Add(new OutcomeMeasure() { Id = (int)dr["OutcomeMeasureId"], OutcomeMeasureName = dr["OutcomeMeasure"].ToString(),
                    SAMHSAOutcome = dr.IsNull("SAMHSAOutcome") ? 0 : (int)dr["SAMHSAOutcome"], GroupFavored = (bool)dr["GroupFavored"], PopDescription = dr["PopDescription"].ToString(),
                    SAMHSAPop = (int)dr["SAMHSAPop"], TaxOutcome = (int)dr["TaxonomyOutcome"], EffectReport = dr.IsNull("EffectReport") ? 0 : (int)dr["EffectReport"],
                    StudyId = (int)dr["StudyId"], DocumentId = (int)dr["DocumentId"], OutcomeId = (int)dr["OutcomeId"], RecommendReview = (bool)dr["RecommendReview"]});
                }

                // TODO: Document Association

                foreach (DataRow dr in ds.Tables[1].Rows)
                    studyOutcomeList.Add(new Outcome() { IntervId = IntervId, OutcomeName = dr["OutcomeName"].ToString(), Id = (int)dr["Id"] });

            }
            catch (Exception ex)
            {
                outcomeList.Add(new OutcomeMeasure() { OutcomeMeasureName = ex.Message, Id = -1 });
            }
            finally
            {
                OutList = new OutcomesWrapper(outcomeList, studyOutcomeList);
                conn.Close();
            }

            return OutList;
        }

        /// <summary>
        /// Update the doc
        /// </summary>
        /// <param name="RCDocId">The ID of the Review Coordinator Record</param>
        /// <param name="DocId">The DocumentId</param>
        /// <param name="Reference">Reference Text</param>
        /// <param name="RCDocName">Review Coordinator Name for the Document</param>
        /// <returns></returns>
        public int UpdateRCDocInfo(int RCDocId, int DocId, string Reference, string RCDocName, int? pubYear)
        {
            int retValue = 0;
            SqlCommand cmd = new SqlCommand("SPAddOrUpdateDocTags", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "RETURN_VALUE", Direction = ParameterDirection.ReturnValue, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Id", Value = RCDocId  });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocId", DbType = DbType.Int32, Value = DocId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Reference", DbType = DbType.String, Value = Reference });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@RCName", DbType = DbType.String, Value = RCDocName });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@PubYear", Value = pubYear });

            try
            {
                CheckConn();

                retValue = cmd.ExecuteNonQuery();

            } catch (Exception)
            {
                retValue = -1;
            }
            finally
            {
                conn.Close();
            }

            return retValue;
        }

        public List<RCDocument> GetRCDocuments(int? inRCDocID, int IntervId)
        {
            List<RCDocument> outList = new List<RCDocument>();
            SqlCommand cmd = new SqlCommand("SPGetDocsWithTagsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocId", Value = inRCDocID });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@InterventionId", Value = IntervId });

            try
            {
                CheckConn();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                RCDocument doc;

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    doc = new RCDocument((int)dr["DocumentId"], dr.IsNull("RCId") ? -1 : (int)dr["RCId"]);
                    doc.Reference = dr["Reference"].ToString();
                    doc.RCName = dr["RCName"].ToString();
                    doc.FileDescription = dr["Description"].ToString();
                    doc.PubYear = dr.IsNull("PubYear") ? null : (int?)dr["PubYear"];
                    outList.Add(doc);
                }

            } catch (Exception ex)
            {
                RCDocument ADocument  = new RCDocument(-1, -1);
                ADocument.FileDescription = ex.Message;
                outList.Add(ADocument);
            }
            finally
            {
                conn.Close();
            }

            return outList;
        }

        public int AddOrUpdateOutcomeMeasure(OutcomeMeasure om, int InterventionId, string NewOutcome)
        {
            int retValue = 0;
            SqlCommand cmd = new SqlCommand("SPAddOrUpdateOutcomeMeasure", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "RETURN_VALUE", Direction = ParameterDirection.ReturnValue, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OutcomeId", Value = om.OutcomeId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OutcomeMeasureId", DbType = DbType.Int32, Value = om.Id });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OutcomeMeasure", DbType = DbType.String, Value = om.OutcomeMeasureName });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@InterventionId", DbType = DbType.Int32, Value = InterventionId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@StudyId", DbType = DbType.Int32, Value = om.StudyId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OutcomeName", DbType = DbType.String, Value = NewOutcome });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@GroupFavored", Value = om.GroupFavored });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@PopDescription", Value = om.PopDescription });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SAMHSAPop", Value = om.SAMHSAPop });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SAMHSAOutcome", Value = om.SAMHSAOutcome });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@EffectReport", Value = om.EffectReport });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@RecommendReview", Value = om.RecommendReview });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocId", Value = om.DocumentId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@TaxonomyOutcome", Value = om.TaxOutcome });

            try
            {
                CheckConn();

                cmd.ExecuteNonQuery();

                retValue = (int)cmd.Parameters["RETURN_VALUE"].Value;

            }
            catch (Exception)
            {
                retValue = -1;
            }
            finally
            {
                conn.Close();
            }

            return retValue;
        }

        public bool DeleteOutcomeMeasure(int MeasureId)
        {
            bool retValue = true;

            SqlCommand cmd = new SqlCommand("SPDeleteOutcomeMeasure", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "RETURN_VALUE", Direction = ParameterDirection.ReturnValue, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@OutcomeMeasureId", Value = MeasureId });
            

            try
            {
                CheckConn();

                cmd.ExecuteNonQuery();

                retValue = (int)cmd.Parameters["RETURN_VALUE"].Value == 0;

            }
            catch (Exception)
            {
                retValue = false;
            }
            finally
            {
                conn.Close();
            }

            return retValue;
        }

        #endregion

        #region Workflow Functionality

        /// <summary>
        /// Chages the status of an Intervention
        /// </summary>
        /// <param name="InterventionId">Id of the intervention to change</param>
        /// <param name="ToStatus">Destination status</param>
        /// <param name="UserId">User performing the transition</param>
        /// <param name="DestUser">Destination user</param>
        /// <returns></returns>
        public bool ChangeStatus(int InterventionId, int ToStatus, int UserId, int? DestUser)
        {
            SqlCommand cmd = new SqlCommand("SPChangeInterventionStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@IntervId", SqlDbType = SqlDbType.Int, Value = InterventionId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@User", SqlDbType = SqlDbType.Int, Value = UserId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DestStatus", SqlDbType = SqlDbType.Int, Value = ToStatus });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DestUser", SqlDbType = SqlDbType.Int, Value = DestUser });

            try
            {
                CheckConn();

                cmd.ExecuteNonQuery();

            } catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }

            return true;
        }

        public IEnumerable<Destination> GetDestinations(int IntervId)
        {
            List<Destination> outList = new List<Destination>();
            SqlCommand cmd = new SqlCommand("SPGetDestinations", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@IntervId", Value = IntervId });

            try
            {
                CheckConn();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //Answer ans;

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {

                    outList.Add(new Destination() { UserId = dr.IsNull("UserId") ? -1 : (int)dr["UserId"], StatusId = (int)dr["StatusId"], StatusName = dr["StatusName"].ToString(),
                        UserName = dr["UserName"].ToString(), RoleName = dr["RoleName"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                //ADocument.FileDescription = ex.Message;
                outList.Add(new Destination() { StatusId = -1, UserName = ex.Message, StatusName = "Error" });
            }
            finally
            {
                conn.Close();
            }

            return outList;
        }

        public bool CanDo(string Permission, string UserName, int? InterventionId)
        {
            bool result = false;

            //SqlCommand cmd = new SqlCommand("SELECT from dbo.FNHavePermission(@inPermission, @UserId,@InterventionId)", conn);
            
            //cmd.CommandType = CommandType.Text;
            SqlCommand cmd = new SqlCommand("FNHavePermission", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@inPermission", Permission));
            cmd.Parameters.Add(new SqlParameter("@InterventionId", InterventionId));
            cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Ret", Direction = ParameterDirection.ReturnValue, SqlDbType = SqlDbType.Bit });

            try
            {
                CheckConn();
                cmd.ExecuteNonQuery();
                result = (bool)cmd.Parameters["@Ret"].Value;
            } catch (Exception) // Somehow we need to recover the error
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region Other User Functionality

        

        #endregion

        #endregion

        #region Helper Functions

        /// <summary>
        /// Check to see if the connection is open. If it is not, it opens it
        /// </summary>
        protected void CheckConn()
        {
            if (conn == null)
                throw new Exception("There is no SQL Connection String!"); // TODO: Make own version of an exception to go here

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
        }

        public static string ConnString
        {
            get
            {
                string currentEnv = ConfigurationManager.AppSettings[SystemConstants.ENV_SETTING];
                return ConfigurationManager.ConnectionStrings[currentEnv].ConnectionString;

            }
        }

        public DateTime? NullDate(object inObj)
        {
            if (inObj == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(inObj);
        }

        #endregion
    }

    #region Other Global Tools

    public class PasswordHash
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        //public const int PBKDF2_ITERATIONS = 1000;

        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;

        public const string AES_KEY = "o/W9j1SmE74zzvvxfswBxmyMhl/CsLHJJQt2i8U17Tk=";
        public const string AES_IV = "XNuf36OTWCn5fjtvi20h0Q==";
        //public const int PBKDF2_INDEX = 2;

        /// <summary>
        /// Gets a salt for a hash algorithm
        /// </summary>
        /// <returns></returns>
        public static byte[] getSalt()
        {
            byte[] osalt = new byte[SALT_BYTE_SIZE];
            RNGCryptoServiceProvider csprov = new RNGCryptoServiceProvider();
            csprov.GetNonZeroBytes(osalt);

            return osalt;
        }

        public static string HashMe(string inPass, string inSalt)
        {
            //string oPass = "";
            SHA256 Hasher = SHA256.Create();
            string salted = string.Concat(inSalt, inPass);
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] oPassBytes = Hasher.ComputeHash(encoder.GetBytes(salted));

            return Convert.ToBase64String(oPassBytes);
        }

        /// <summary>
        /// Validates a hash if provided with the plain text and salt
        /// </summary>
        /// <param name="inText">Plain Text pwd</param>
        /// <param name="hash">Hash from DB</param>
        /// <param name="salt">Salt from DB</param>
        /// <returns></returns>
        public static bool ValidateMe(string inText, string hash, string salt)
        {
            SHA256 Hasher = SHA256.Create();
            string salted = string.Concat(salt, inText);
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] tPassBytes = Hasher.ComputeHash(encoder.GetBytes(salted));
            string result = Convert.ToBase64String(tPassBytes);

            return result.Equals(hash);
        }

        public static string AESCrypt(string plainText)
        {
            string retstring = string.Empty;
            byte[] Key = Convert.FromBase64String(PasswordHash.AES_KEY);
            byte[] IV = Convert.FromBase64String(PasswordHash.AES_IV);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        retstring = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return retstring;
        }

       public static string AESDecrypt(string cipherText)
        {
           string retString = string.Empty;
           byte[] bitText = Convert.FromBase64String(cipherText);
           byte[] Key = Convert.FromBase64String(PasswordHash.AES_KEY);
           byte[] IV = Convert.FromBase64String(PasswordHash.AES_IV);

           using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
           {
               aesAlg.Key = Key;
               aesAlg.IV = IV;

               ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

               using (MemoryStream msDecrypt = new MemoryStream(bitText))
               {
                   using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                   {
                       using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                       {
                           retString = srDecrypt.ReadToEnd();
                       }
                   }
               }
           }

           return retString;
        }
    }

    #endregion

    #region Constants

    #endregion
}
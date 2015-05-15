using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using DataTables.Mvc;
using NREPPAdminSite.Constants;
using NREPPAdminSite.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;

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

        public NrepServ(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        #endregion

        #region Service-Like Methods

        #region Misc Functionality

        public void ChangeStatus(int inId, string inUser, int toStatus)
        {
            SqlCommand cmd = new SqlCommand("SPChangeInterventionStatus", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@IntervId", inId));
            cmd.Parameters.Add(new SqlParameter("@User", inUser));
            cmd.Parameters.Add(new SqlParameter("@DestUser", inUser));
            cmd.Parameters.Add(new SqlParameter("@DestStatus", toStatus));

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
            List<Answer> theOutcomes = new List<Answer>();

            // SQL Stuff
            SqlCommand getTaxonomy = new SqlCommand("SPGetTaxOutcomes", conn);

            getTaxonomy.CommandType = CommandType.StoredProcedure;
            getTaxonomy.Parameters.Add(new SqlParameter("@TaxId", inId));

            SqlDataAdapter da = new SqlDataAdapter(getTaxonomy);


            try
            {
                CheckConn();

                DataTable dt = new DataTable();


                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    theOutcomes.Add(new Answer((int)dr["Id"], dr["Guidelines"].ToString(), dr["OutcomeName"].ToString()));
                }

            }
            catch (Exception ex)
            {
                Answer nAnswer = new Answer();
                nAnswer.ShortAnswer = "Error!";
                nAnswer.LongAnswer = ex.Message;
                nAnswer.AnswerId = -1;
                theOutcomes.Add(nAnswer);
            }

            return theOutcomes;
        }
        
        /// <summary>
        /// Gets a an IEnumberable of Answers from the provided Category
        /// </summary>
        /// <param name="inCategory">The Category's Name</param>
        /// <returns>The Answers as an IEnumerable</returns>
        public IEnumerable<Answer> GetAnswersByCategory(string inCategory)
        {
            List<Answer> outAnswers = new List<Answer>();

            // SQL Stuff
            SqlCommand getAnswersByCategory = new SqlCommand("SPGetAnswersByCategory", conn);

            getAnswersByCategory.CommandType = CommandType.StoredProcedure;
            getAnswersByCategory.Parameters.Add(new SqlParameter("@InCategoryName", inCategory));

            SqlDataAdapter da = new SqlDataAdapter(getAnswersByCategory);
         

            try
            {
                CheckConn();

                DataTable dt = new DataTable();
                

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    outAnswers.Add(new Answer((int)dr["AnswerId"], dr["ShortAnswer"].ToString(), dr["LongAnswer"].ToString()));
                }

            } catch (Exception ex)
            {
                Answer nAnswer = new Answer();
                nAnswer.ShortAnswer = "Error!";
                nAnswer.LongAnswer = ex.Message;
                nAnswer.AnswerId = -1;
                outAnswers.Add(nAnswer);
            }

            return outAnswers;
        }

        public IEnumerable<MaskValue> GetMaskList(string inMaskName)
        {
            List<MaskValue> outAnswers = new List<MaskValue>();

            // SQL Stuff
            SqlCommand cmdGetMasks = new SqlCommand("SPGetMasksByCategory", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmdGetMasks.Parameters.Add(new SqlParameter("@InCategory", inMaskName));

            SqlDataAdapter da = new SqlDataAdapter(cmdGetMasks);


            try
            {
                CheckConn();

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    outAnswers.Add(new MaskValue() { Name = dr["MaskValueName"].ToString(), Value = (int)dr["MaskPower"], Selected = false });
                }

            }
            catch (Exception ex)
            {
                MaskValue nAnswer = new MaskValue();
                //nAnswer.Name = "Error!";
                nAnswer.Name = ex.Message;
                nAnswer.Value = -1;
                nAnswer.Selected = false;
                outAnswers.Add(nAnswer);
            }

            return outAnswers;
        }

        public SubmissionPd GetCurrentSubmissionPd()
        {
            SubmissionPd pd = new SubmissionPd();
            SqlCommand cmd = new SqlCommand("SPGetRecentSubPd");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                CheckConn();
                DataTable dt = new DataTable();
                da.Fill(dt);
                pd.StartDate = Convert.ToDateTime(dt.Rows[0]["StartDate"]);
                pd.EndDate = Convert.ToDateTime(dt.Rows[0]["EndDate"]);

            } catch(Exception) {
                pd.StartDate = new DateTime(1901, 1, 1);
                pd.EndDate = new DateTime(1901, 1, 1);
            }


            return pd;
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
            return GetInterventions(nullParams);
        }

        /// <summary>
        /// Gets the interventions list based on role
        /// </summary>
        /// <returns></returns>
        public List<Intervention> GetInterventions(string roleName) // This needs to take some parameters, so there should be a bunch of functions for it
        {
            List<SqlParameter> nullParams = new List<SqlParameter> { new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = null } };
            nullParams.Add(new SqlParameter() { ParameterName = "@RoleName", SqlDbType = SqlDbType.NVarChar, Value = roleName });
            return GetInterventions(nullParams);
        }

        public List<Intervention> GetInterventions([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string roleName)
        {
            List<Intervention> interventions = new List<Intervention>();

            string title = null;
            string fullDescription = null;
            string submitter = null;
            string updatedDate = null;

            var reviewerSearchResult = new ReviewerSearchResult();
            var reviewerList = new List<Reviewer>();
            SqlCommand cmdGetReviewerList = new SqlCommand("SPGetInterventionList", conn);
            cmdGetReviewerList.CommandType = CommandType.StoredProcedure;

            var filteredColumns = requestModel.Columns.GetFilteredColumns();
            foreach (var filter in filteredColumns)
            {
                switch (filter.Data)
                {
                    case "Title":
                        title = filter.Search.Value;
                        break;
                    case "FullDescription":
                        fullDescription = filter.Search.Value;
                        break;
                    case "Submitter":
                        submitter = filter.Search.Value;
                        break;
                    case "UpdatedDate":
                        updatedDate = filter.Search.Value;
                        break;
                                    
                }
            }
            List<SqlParameter> paramsList = new List<SqlParameter> { new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = null } };
            paramsList.Add(new SqlParameter() { ParameterName = "@RoleName", SqlDbType = SqlDbType.NVarChar, Value = roleName });
            paramsList.Add(new SqlParameter() { ParameterName = "@Title", SqlDbType = SqlDbType.NVarChar, Value = title });
            paramsList.Add(new SqlParameter() { ParameterName = "@FullDescription", SqlDbType = SqlDbType.NVarChar, Value = fullDescription });
            paramsList.Add(new SqlParameter() { ParameterName = "@UpdatedDate", Value = updatedDate });
            paramsList.Add(new SqlParameter("@Submitter", submitter));
            paramsList.Add(new SqlParameter("@Page", requestModel.Start + 1));
            paramsList.Add(new SqlParameter("@PageLength", requestModel.Length));
            return GetInterventions(paramsList);
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
                    //inv.FullDescription

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
                    doc.Title = dr["Title"].ToString();
                    

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
            int documentType, string Title)
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
            cmdSaveFile.Parameters.Add(new SqlParameter("@Title", Title));
            //cmdSaveFile.Parameters.Add(new SqlParameter("@OutPut", null));

            SqlParameter OutPut = new SqlParameter("@Output", -1);
            OutPut.Direction = ParameterDirection.Output;
            cmdSaveFile.Parameters.Add(OutPut);

            try
            {
                // TODO: file name collision

                //string nFileName = ConfigurationManager.AppSettings["fileLocation"] + "\\" + IntervId.ToString() + "\\" + fileName;
                string nDirectory = ConfigurationManager.AppSettings["fileLocation"].Trim('\\') + "\\" + IntervId.ToString() + "\\";
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
            
            cmdSaveFile.Parameters.Add(new SqlParameter("@MIMEType", ""));
            cmdSaveFile.Parameters.Add(new SqlParameter("@IsDelete", true));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ItemId", DocId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@UploaderId", theUser));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ReviewerId", null));
            

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

                    outList.Add(new Destination() { UserId = dr["UserId"].ToString(), StatusId = (int)dr["StatusId"], StatusName = dr["StatusName"].ToString(),
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

        public ReviewersWrapper GetOutComesReviewers(int? id)
        {
            ReviewersWrapper rw = new ReviewersWrapper();
            try
            {
                //TODO: Remove Later
                //rw.OutcomesReviewers = GetReviewers();
            }
            catch (Exception ex)
            {
                
            }          
            return rw;
        }


        public ReviewerWrapper GetOutComesReviewer(int? id)
        {
            ReviewerWrapper reviewerWrapper = new ReviewerWrapper();
            try
            {
                reviewerWrapper.Reviewer = GetReviewer(id);
            }
            catch (Exception ex)
            {

            }
            return reviewerWrapper;
        }


        public Reviewer GetReviewer(int? id)
        {
            Reviewer reviewer = null; //= new Reviewer();
            SqlCommand cmdGetReviewerList = new SqlCommand("SPGetReviewerList", conn);
            cmdGetReviewerList.CommandType = CommandType.StoredProcedure;


            try
            {
                CheckConn();
                DataTable reviewers = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdGetReviewerList);
                da.Fill(reviewers);
                DataRow dr = reviewers.Rows[0];
                reviewer = new Reviewer {
                    Id = dr["Id"].ToString(), 
                    UserId = dr["UserId"].ToString(), 
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    Employer = dr["Employer"].ToString(),
                    Department = dr["Department"].ToString(),
                    ReviewerType = dr["ReviewerType"].ToString(),
                    Degree = dr["Degree"].ToString()
                };
                return reviewer;
            }
            catch (Exception ex)
            {

            }

            return reviewer;

        }

        public ReviewerSearchResult GetReviewers([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            string id = null;
            string firstName = null;
            string lastName = null;
            string employer = null;
            string department = null;
            string reviewerType = null;
            string degree = null;

            var reviewerSearchResult = new ReviewerSearchResult();
            var reviewerList = new List<Reviewer>();
            SqlCommand cmdGetReviewerList = new SqlCommand("SPGetReviewers", conn);
            cmdGetReviewerList.CommandType = CommandType.StoredProcedure;

            var filteredColumns = requestModel.Columns.GetFilteredColumns();
            foreach (var filter in filteredColumns)
            {
                switch (filter.Data)
                {
                    case "Id":
                        id = filter.Search.Value;
                        break;
                    case "FirstName":
                        firstName = filter.Search.Value;
                        break;
                    case "LastName":
                        lastName = filter.Search.Value;
                        break;
                    case "Employer":
                        employer = filter.Search.Value;
                        break;
                    case "Department":
                        department = filter.Search.Value;
                        break;
                    case "ReviewerType":
                        reviewerType = filter.Search.Value;
                        break;
                    case "Degree":
                        degree = filter.Search.Value;
                        break;
                }
            }
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@Id", Utilities.ToDbNull(id)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@FirstName", Utilities.ToDbNull(firstName)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@LastName", Utilities.ToDbNull(lastName)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@Employer", Utilities.ToDbNull(employer)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@Department", Utilities.ToDbNull(department)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@ReviewerType", Utilities.ToDbNull(reviewerType)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@Degree", Utilities.ToDbNull(degree)));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@Page", requestModel.Start + 1));
            cmdGetReviewerList.Parameters.Add(new SqlParameter("@PageLength", requestModel.Length));
            try
            {
                CheckConn();
                DataTable reviewers = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmdGetReviewerList);
                da.Fill(reviewers);

                reviewerList.AddRange(from DataRow dr in reviewers.Rows
                    select new Reviewer
                    {
                        Id = dr["Id"].ToString(), UserId = dr["UserId"].ToString(), FirstName = dr["FirstName"].ToString(), 
                        LastName = dr["LastName"].ToString(), Employer = dr["Employer"].ToString(), 
                        Department = dr["Department"].ToString(), ReviewerType = dr["ReviewerType"].ToString(), 
                        Degree = dr["Degree"].ToString()
                    });
                if (reviewers.Rows.Count > 0)
                {
                    reviewerSearchResult.Reviewers = reviewerList;
                    reviewerSearchResult.TotalSearchCount = Convert.ToInt16(reviewers.Rows[0]["searchTotal"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return reviewerSearchResult;
        }

        public bool AddReviewer(RegisterViewModel model)
        {
            SqlCommand cmd = new SqlCommand("SPAddReviewer", conn) {CommandType = CommandType.StoredProcedure};
            cmd.Parameters.Add(new SqlParameter() {ParameterName = "@UserId", SqlDbType = SqlDbType.NVarChar, Value = model.UserId});
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@FirstName", SqlDbType = SqlDbType.NVarChar, Value = model.FirstName });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = model.LastName });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Degree", SqlDbType = SqlDbType.VarChar, Value = model.Degree });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ReviewerType", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.ReviewerType) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@HomeAddressLine1", SqlDbType = SqlDbType.VarChar, Value = model.HomeAddressLine1 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@HomeAddressLine2", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.HomeAddressLine2) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@HomeCity", SqlDbType = SqlDbType.VarChar, Value = model.HomeCity });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@HomeState",  Value = model.HomeState });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@HomeZip", SqlDbType = SqlDbType.VarChar, Value = model.HomeZip });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@PhoneNumber", SqlDbType = SqlDbType.VarChar, Value = model.PhoneNumber });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@FaxNumber", SqlDbType = SqlDbType.VarChar, Value = model.FaxNumber });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Value = model.Email });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Employer", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.Employer) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Department", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.Department) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkAddressLine1", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkAddressLine1) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkAddressLine2", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkAddressLine2) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkCity", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkCity) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkState",  Value = Utilities.ToDbNull(model.WorkState) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkZip", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkZip) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkPhoneNumber", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkPhoneNumber) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkFaxNumber", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.WorkFaxNumber) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@WorkEmail", Value = Utilities.ToDbNull(model.WorkEmail) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ExperienceSummary", SqlDbType = SqlDbType.VarChar, Value = Utilities.ToDbNull(model.ExperienceSummary) });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Active", SqlDbType = SqlDbType.Bit, Value = true });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CreatedBy", SqlDbType = SqlDbType.NVarChar, Value = model.CreatedBy });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", SqlDbType = SqlDbType.NVarChar, Value = model.ModifiedBy });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CreatedOn", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ModifiedOn", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now });

           try
            {
                CheckConn();

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public UsersSearchResult GetUsers([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            string userName = null;
            string firstName = null;
            string lastName = null;
            string email = null;
            bool lockoutEnabled = false;
            bool isStatus=false;
            var usersSearchResult = new UsersSearchResult();
            var userList = new List<GenericUser>();
            SqlCommand cmdGetUserList = new SqlCommand("SPGetUsers", conn);
            cmdGetUserList.CommandType = CommandType.StoredProcedure;

            var filteredColumns = requestModel.Columns.GetFilteredColumns();
            foreach (var filter in filteredColumns)
            {
                switch (filter.Data)
                {
                    case "UserName":
                        userName = filter.Search.Value;
                        break;
                    case "FirstName":
                        firstName = filter.Search.Value;
                        break;
                    case "LastName":
                        lastName = filter.Search.Value;
                        break;
                    case "Email":
                        email = filter.Search.Value;
                        break;        
                }
            }
            cmdGetUserList.Parameters.Add(new SqlParameter("@UserName", Utilities.ToDbNull(userName)));
            cmdGetUserList.Parameters.Add(new SqlParameter("@FirstName", Utilities.ToDbNull(firstName)));
            cmdGetUserList.Parameters.Add(new SqlParameter("@LastName", Utilities.ToDbNull(lastName)));
            cmdGetUserList.Parameters.Add(new SqlParameter("@Email", Utilities.ToDbNull(email)));
            cmdGetUserList.Parameters.Add(new SqlParameter("@Page", requestModel.Start + 1));
            cmdGetUserList.Parameters.Add(new SqlParameter("@PageLength", requestModel.Length));
            CheckConn();
            DataTable users = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmdGetUserList);
            da.Fill(users);

            foreach (DataRow dr in users.Rows)
            {
                userList.Add(new GenericUser
                {
                    Id = dr["Id"].ToString(),
                    UserName = dr["UserName"].ToString(), 
                    Email = dr["Email"].ToString(),
                    FirstName = dr["FirstName"].ToString(), 
                    LastName = dr["LastName"].ToString(),
                    HomeAddressLine1 = dr["HomeAddressLine1"].ToString(),
                    HomeAddressLine2 = dr["HomeAddressLine2"].ToString(),
                    HomeCity = dr["HomeCity"].ToString(),
                    HomeState = dr["HomeState"].ToString(),
                    HomeZip = dr["HomeZip"].ToString(),
                    PhoneNumber = dr["PhoneNumber"].ToString(),
                    FaxNumber = dr["FaxNumber"].ToString(),
                    Employer = dr["Employer"].ToString(),
                    Department = dr["Department"].ToString(),
                    WorkAddressLine1 = dr["WorkAddressLine1"].ToString(),
                    WorkAddressLine2 = dr["WorkAddressLine2"].ToString(),
                    WorkCity = dr["WorkCity"].ToString(),
                    WorkState = dr["WorkState"].ToString(),
                    WorkZip = dr["WorkZip"].ToString(),
                    WorkPhoneNumber = dr["WorkPhoneNumber"].ToString(),
                    WorkFaxNumber = dr["WorkFaxNumber"].ToString(),
                    WorkEmail = dr["WorkEmail"].ToString(),
                    IsLocked = dr["IsUserLocked"].ToString() == "1" ? true : false
                });
            }
            if (users.Rows.Count > 0)
            {
                usersSearchResult.UserList = userList;
                usersSearchResult.TotalSearchCount = Convert.ToInt16(users.Rows[0]["searchTotal"].ToString());
            }

            return usersSearchResult;
        }

        public ExtendedUser GetUser(string id)
        {
            ExtendedUser reviewer =  new ExtendedUser();
            SqlCommand cmdGetReviewerList = new SqlCommand("SPGetReviewerList", conn);
            cmdGetReviewerList.CommandType = CommandType.StoredProcedure;


            try
            {
                CheckConn();
                
                SqlDataReader dr = cmdGetReviewerList.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        reviewer.Id = Convert.ToString(dr["Id"]);
                    }
                }
               
                return reviewer;

            }
            catch (Exception ex)
            {

            }

            return reviewer;
        }        

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

            if (conn.State != ConnectionState.Open)
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

        public bool IsInSubPd(SubmissionPd pd)
        {
            return pd.StartDate < DateTime.Now && pd.EndDate > DateTime.Now;
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
            byte[] Key = Convert.FromBase64String(AES_KEY);
            byte[] IV = Convert.FromBase64String(AES_IV);

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
           byte[] Key = Convert.FromBase64String(AES_KEY);
           byte[] IV = Convert.FromBase64String(AES_IV);

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

    public class NreppServiceResponseBase
    {
        public NreppServiceResponseBase()
        {
            Errors = new List<string>();
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public bool HasErrors { get { return Errors != null && Errors.Count > 0; } }

        public void ResponseSet(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public void ErrorsAdd(string error)
        {
            if (Errors == null)
                Errors = new List<string>();
            Errors.Add(error);
        }
    }

    public class CaptchaUtil
    {
        public byte[] GetCaptchaImage(string checkCode)
        {
            Bitmap image = new Bitmap(Convert.ToInt32(Math.Ceiling((decimal)(checkCode.Length * 15))), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                g.Clear(Color.AliceBlue);
                Font font = new Font("Comic Sans MS", 14, FontStyle.Bold);
                string str = "";
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);               
                HatchBrush hatchBrush = new HatchBrush(HatchStyle.Shingle, Color.LightGray, Color.White);
                g.FillRectangle(hatchBrush, rect);
                for (int i = 0; i < checkCode.Length; i++)
                {
                    str = str + checkCode.Substring(i, 1);
                }
                g.DrawString(str, font, new SolidBrush(Color.Green), 0, 0);
                g.Flush();
                

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public byte[] VerificationTextGenerator()
        {           
            string randomCode = "";        
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = new char[7];
            Random rd = new Random();
            for (int i = 0; i < 7; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            randomCode= new string(chars);
            GetCaptchaImage(randomCode);
            HttpContext.Current.Session["Captcha"] = randomCode;
            return GetCaptchaImage(randomCode);
        }

    }
    #endregion

    #region Constants

    #endregion
}
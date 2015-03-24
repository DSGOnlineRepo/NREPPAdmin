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

        #region Login Functionality

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="uName">User Name</param>
        /// <param name="fname">First Name (null allowed)</param>
        /// <param name="lname">Last Name (null allowed)</param>
        /// <param name="passwd">Password Desired</param>
        /// <param name="RoleId">RoleId</param>
        /// <returns></returns>
        public int registerUser(string uName, string fname, string lname, string passwd, int RoleId)
        {
            CheckConn();

            SqlCommand cmd = new SqlCommand("SPAdminRegisterUser", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            Tuple<string, string> saltedResult = DoHash(passwd);
            
            // Parameters
            cmd.Parameters.Add("@userName", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@fname", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@lname", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@hash", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@salt", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@RoleId", System.Data.SqlDbType.Int);

            cmd.Parameters["@userName"].Value = uName;
            cmd.Parameters["@fname"].Value = fname;
            cmd.Parameters["@lname"].Value = lname;
            cmd.Parameters["@hash"].Value = saltedResult.Item2;
            cmd.Parameters["@salt"].Value = saltedResult.Item1;
            cmd.Parameters["@RoleId"].Value = RoleId;
            
            int returnValue = cmd.ExecuteNonQuery();

            conn.Close();

            return returnValue;
        }

        /// <summary>
        /// Logs in user and obtains their user information if successful.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataSet LoginUser(string username, string password)
        {
            DataSet outUser = null;
            SqlCommand cmdGetCreds = new SqlCommand("SPGetLogonCreds", conn);
            SqlCommand cmdGetUser = new SqlCommand("SPGetUser", conn);

            try 
            {
                CheckConn(); // Check to make sure that the connection is open

                // Get the user's login credentials

                cmdGetCreds.Parameters.Add("@userName", SqlDbType.VarChar);
                cmdGetCreds.Parameters["@userName"].Value = username;
                cmdGetCreds.CommandType = System.Data.CommandType.StoredProcedure;

                DataTable hashStuff = new DataTable();
                SqlDataReader dr = cmdGetCreds.ExecuteReader();
                dr.Read();

                
                if (PasswordHash.ValidateMe(password, dr["hash"].ToString(), dr["salt"].ToString()))
                {
                    dr.Close();
                    outUser = new DataSet();
                    cmdGetUser.Parameters.Add("@userName", SqlDbType.VarChar);
                    cmdGetUser.Parameters["@userName"].Value = username;
                    cmdGetUser.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmdGetUser;
                    da.Fill(outUser);
                }
                else
                {
                    throw new Exception("Invalid username/password combination");
                }

            } catch (Exception ex) {
                outUser = new DataSet();
                outUser.Tables.Add(Constants.ERR_TABLE);
                DataColumn col = new DataColumn(Constants.ERR_MSG_COL, System.Type.GetType("System.String"));
                outUser.Tables[Constants.ERR_TABLE].Columns.Add(col);
                DataRow errRow = outUser.Tables[Constants.ERR_TABLE].NewRow();
                errRow[Constants.ERR_MSG_COL] = ex.Message;
                outUser.Tables[Constants.ERR_TABLE].Rows.Add(errRow);
            }
            finally
            {
                conn.Close();
            }

            return outUser;
        }

        /// <summary>
        /// Hashes Passwords during Registration
        /// </summary>
        /// <param name="inPwd">Desired Password to be hashed</param>
        /// <returns></returns>
        public Tuple<string, string> DoHash(string inPwd)
        {
            string someSalt = Convert.ToBase64String(PasswordHash.getSalt());
            return Tuple.Create(someSalt, PasswordHash.HashMe(inPwd, someSalt));
        }

        public NreppUser LoginComplete(string username, string password)
        {
            DataSet rawUser = LoginUser(username, password); // TODO: What happens when this fails?
            Dictionary<string, bool> roleStatus = new Dictionary<string, bool>();
            DataRow UserRow = rawUser.Tables[0].Rows[0];
            NreppUser currentUser;

            if (rawUser.Tables[0].Columns.Contains(Constants.ERR_MSG_COL))
            {
                currentUser = new NreppUser(-1, rawUser.Tables[0].Rows[0][Constants.ERR_MSG_COL].ToString(), "Failed", "Login");
            }
            else
            {

                DataRow RoleRow = rawUser.Tables[1].Rows[0];

                foreach (DataColumn col in rawUser.Tables[1].Columns)
                {
                    if (col.ColumnName != "RoleId" && col.ColumnName != "RoleName")
                        roleStatus.Add(col.ColumnName, (bool)RoleRow[col]); // This is totally clear. :D
                }

                //Role userRole = new Role((int)RoleRow["RoleId"], RoleRow["RoleName"].ToString(), roleStatus);
                Role userRole = new Role((int)RoleRow["RoleId"], RoleRow["RoleName"].ToString());

                currentUser = new NreppUser((int)UserRow["Id"], userRole, UserRow["Username"].ToString(), UserRow["Firstname"].ToString(), UserRow["Lastname"].ToString());

                currentUser.Authenticate(true);
            }

            return currentUser;

        }


        public static NreppUser GetFromCookie(HttpCookie inCookie)
        {
            return (new JavaScriptSerializer()).Deserialize<NreppUser>(inCookie.Value);
        }

        #endregion

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
        public List<Intervention> GetInterventions(int RoleId) // This needs to take some parameters, so there should be a bunch of functions for it
        {
            List<SqlParameter> nullParams = new List<SqlParameter> { new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = null } };
            nullParams.Add(new SqlParameter() { ParameterName = "@UserRoleId", SqlDbType = SqlDbType.Int, Value = RoleId });
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
                        Convert.ToDateTime(dr["UpdateDate"]), (int)dr["SubmitterId"], dr["StatusName"].ToString(), (int)dr["StatusId"],
                        dr["ProgramType"] == DBNull.Value ? 0 : (int)dr["ProgramType"], dr["Acronym"].ToString(), false);

                    inv.PreScreenMask = (int)dr["PreScreenAnswers"];

                    interventions.Add(inv);

                    
                }

            }
            catch (Exception ex)
            {
                interventions.Add(new Intervention(-1, "Error!", ex.Message, "", DateTime.Now, DateTime.Now, -1, "Submitted", 1, 0, "", false));
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
                //interventions.Add(new Intervention(-1, "Error!", ex.Message, "", DateTime.Now, DateTime.Now, -1, "Submitted", 1, 0));
                documents.Add(new InterventionDoc() { FileDescription = ex.Message, Link = "Error" });
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
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@submitter", SqlDbType = SqlDbType.Int, Value = inData.SubmitterId });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@updateDate", SqlDbType = SqlDbType.DateTime, Value = inData.UpdatedDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@publishDate", SqlDbType = SqlDbType.DateTime, Value = inData.PublishDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@status", SqlDbType = SqlDbType.Int, Value = inData.StatusId > 0 ? inData.StatusId : 1 });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@programType", SqlDbType = SqlDbType.Int, Value = inData.ProgramType });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@Acronym", SqlDbType = SqlDbType.VarChar, Value = inData.Acronym });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@IsLitSearch", SqlDbType = SqlDbType.Bit, Value = inData.FromLitSearch });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@PreScreenAnswers", SqlDbType = SqlDbType.Int, Value = inData.PreScreenMask });

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

        public int SaveFileToDB(byte[] inData, string fileName, int theUser, string MIMEType, int IntervId, bool isDelete, int ItemId, string DisplayName,
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
            cmdSaveFile.Parameters.Add(new SqlParameter("@UploaderId", theUser));
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
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@BaselineEquiv", SqlDbType = SqlDbType.VarChar, Value = inStudy.BaselineEquiv });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@UseMultivariate", SqlDbType = SqlDbType.Bit, Value = inStudy.UseMultivariate });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SAMSHARelated", SqlDbType = SqlDbType.Int, Value = inStudy.SAMSHARelated });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@AuthorQueryNeeded", SqlDbType = SqlDbType.Bit, Value = inStudy.AuthorQueryNeeded });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@RecommendReview", SqlDbType = SqlDbType.Bit, Value = inStudy.RecommendReview });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Notes", SqlDbType = SqlDbType.VarChar, Value = inStudy.Notes });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocumentId", SqlDbType = SqlDbType.Int, Value = inStudy.DocumentId });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@IDOut", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

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
                        Reference = dr["Reference"].ToString(), DocumentId = (int)dr["DocumentId"], Exclusion1 = (int)dr["Exclusion1"],
                    Exclusion2 = (int)dr["Exclusion2"], Exclusion3 = (int)dr["Exclusion3"], StudyDesign = (int)dr["StudyDesign"],
                    RecommendReview = (bool)dr["RecommendReview"], DocOrdinal = (int)dr["DocOrdinal"],
                    OverallAttrition = dr.IsNull("OverallAttritionAvail") ? 0 : (int)dr["OverallAttritionAvail"],
                    DiffAttrition = dr.IsNull("DiffAttritionAvail") ? 0 : (int)dr["DiffAttritionAvail"]
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

            try
            {
                CheckConn();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    outcomeList.Add(new OutcomeMeasure() { Id = (int)dr["OutcomeMeasureId"], OutcomeMeasureName = dr["OutcomeMeasure"].ToString(),
                    SignificantImpact = (int)dr["SignificantImpact"], GroupFavored = (bool)dr["GroupFavored"], PopDescription = dr["PopDescription"].ToString(),
                    SAMHSAPop = (int)dr["SAMHSAPop"], PrimaryOutcome = (bool)dr["PrimaryOutcome"], Priority = (int)dr["Priority"],
                    StudyId = (int)dr["StudyId"], DocumentId = (int)dr["DocumentId"], OutcomeId = (int)dr["OutcomeId"]});
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
            /*cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DiffAttrition", Value = om.DiffAttrition });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@EffectSize", Value = om.EffectSize });*/
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@BaselineEquiv", Value = om.BaselineEquiv });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SignificantImpact", Value = om.SignificantImpact });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@GroupFavored", Value = om.GroupFavored });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@PopDescription", Value = om.PopDescription });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SAMHSAPop", Value = om.SAMHSAPop });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@PrimaryOutcome", Value = om.PrimaryOutcome });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Priority", Value = om.Priority });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DocId", Value = om.DocumentId });

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
                string currentEnv = ConfigurationManager.AppSettings[Constants.ENV_SETTING];
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
    }

    #endregion

    #region Constants

    public class Constants
    {
        public const string ENV_SETTING = "Environment";
        public const string LOCAL_ENV = "LocalDev";
        public const string REMOTE_ENV = "RemoteDev";
        public const string ERR_TABLE = "Errors";
        public const string ERR_MSG_COL = "ErrMsg";
        public const string USR_COOKIE = "currentUser";
    }

    #endregion
}
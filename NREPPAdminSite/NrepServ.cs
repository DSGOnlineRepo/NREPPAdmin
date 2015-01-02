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

                Role userRole = new Role((int)RoleRow["RoleId"], RoleRow["RoleName"].ToString(), roleStatus);

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
                    interventions.Add(new Intervention((int)dr["InterventionId"], dr["Title"].ToString(), dr["FullDescription"].ToString(), dr["Submitter"].ToString(), NullDate(dr["PublishDate"]),
                        Convert.ToDateTime(dr["UpdateDate"]), (int)dr["SubmitterId"], dr["StatusName"].ToString(), (int)dr["StatusId"] ));
                }

            }
            catch (Exception ex)
            {
                interventions.Add(new Intervention(-1, "Error!", ex.Message, "", DateTime.Now, DateTime.Now, -1, "Submitted", 1));
            }

            return interventions;
        }

        /// <summary>
        /// Save or Create a New Intervention
        /// </summary>
        /// <param name="inData">The intervention to be saved or added to the database</param>
        /// <returns>True if save is valid, false if it is not</returns>
        public bool SaveIntervention(Intervention inData) {

            bool returnValue = false;
            SqlCommand cmdUpdate = new SqlCommand("SPUpdateIntervention", conn);
            cmdUpdate.CommandType = CommandType.StoredProcedure;

            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@IntervId", SqlDbType = SqlDbType.Int, Value = inData.Id });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = inData.Title });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@fulldescription", SqlDbType = SqlDbType.NText, Value = inData.FullDescription });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@submitter", SqlDbType = SqlDbType.Int, Value = inData.SubmitterId });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@updateDate", SqlDbType = SqlDbType.DateTime, Value = inData.UpdatedDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@publishDate", SqlDbType = SqlDbType.DateTime, Value = inData.PublishDate });
            cmdUpdate.Parameters.Add(new SqlParameter() { ParameterName = "@status", SqlDbType = SqlDbType.Int, Value = inData.StatusId });

            try
            {
                CheckConn();
                returnValue = cmdUpdate.ExecuteNonQuery() == 0;

            } catch (Exception ex) // Just in here for error handling reasons
            {
                throw new Exception(ex.Message);
            }


            //cmdUpdate.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.Output;

            return returnValue;
        }

        public int SaveFileToDB(byte[] inData, string fileName, int theUser, string MIMEType, int IntervId, bool isDelete, int ItemId, string DisplayName)
        {

            SqlCommand cmdSaveFile = new SqlCommand("SPAddOrRemoveDoc", conn);
            int retValue = -1;
            cmdSaveFile.CommandType = CommandType.StoredProcedure;

            cmdSaveFile.Parameters.Add(new SqlParameter("@IntervId", IntervId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@DisplayName", DisplayName));
            cmdSaveFile.Parameters.Add(new SqlParameter("@MIMEType", MIMEType));
            cmdSaveFile.Parameters.Add(new SqlParameter("@IsDelete", isDelete));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ItemId", ItemId));
            cmdSaveFile.Parameters.Add(new SqlParameter("@UploaderId", theUser));
            cmdSaveFile.Parameters.Add(new SqlParameter("@ReviewerId", null));
            //cmdSaveFile.Parameters.Add(new SqlParameter("@OutPut", null));

            SqlParameter OutPut = new SqlParameter("@Output", -1);
            OutPut.Direction = ParameterDirection.Output;
            cmdSaveFile.Parameters.Add(OutPut);

            try
            {
                // TODO: file name collision

                string nFileName = ConfigurationManager.AppSettings["fileLocation"] + fileName;
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
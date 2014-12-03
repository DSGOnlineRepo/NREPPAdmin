using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

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

            return returnValue;
        }

        public Tuple<string, string> DoHash(string inPwd)
        {
            string someSalt = Convert.ToBase64String(PasswordHash.getSalt());
            return Tuple.Create(someSalt, PasswordHash.HashMe(inPwd, someSalt));
        }

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

        #endregion
    }

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

        public bool ValidateMe(string inText, string hash, string salt)
        {
            SHA256 Hasher = SHA256.Create();
            string salted = string.Concat(salt, inText);
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] tPassBytes = Hasher.ComputeHash(encoder.GetBytes(salted));
            string result = Convert.ToBase64String(tPassBytes);

            return result.Equals(hash);
        }
    }

    #region Constants

    public class Constants
    {
        public const string ENV_SETTING = "Environment";
        public const string LOCAL_ENV = "LocalDev";
        public const string REMOTE_ENV = "RemoteDev";
    }

    #endregion
}
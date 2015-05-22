using System;

namespace NREPPAdminSite.Constants
{
    public static class DBFunctions
    {
        public static object ToDbNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
    }
}
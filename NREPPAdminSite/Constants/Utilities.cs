using System;

namespace NREPPAdminSite.Constants
{
    public static class Utilities
    {
        public static object ToDbNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB_API.DL
{
    public class BaseDL
    {
        public static string connection = ConfigurationManager.AppSettings["Conn"];
        static BaseDL()
        {
            if (string.IsNullOrEmpty(DataManager.DbConnectionString) && !string.IsNullOrEmpty(connection))
            {
                DataManager.DbConnectionString = connection;
            }
        }
    }
}

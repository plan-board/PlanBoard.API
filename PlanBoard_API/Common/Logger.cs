using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PlanBoard_API.Common
{
    public class Logger
    {
        public static void LogError(Exception exception, object obj = null)
        {
            var serializer = new JavaScriptSerializer();
            var datastring = serializer.Serialize(obj);
            var siteLocation = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var folderName = ConfigurationManager.AppSettings["JsonFolder"];
            var path = siteLocation + @"\" + folderName;
            var filePath = string.Format(path + @"\{0}.txt", "ErrorLog");
            using (var aFile = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            using (var sw = new StreamWriter(aFile))
            {
                sw.WriteLine(exception.Message);
                sw.WriteLine("<><>");
                sw.WriteLine(datastring);
                sw.WriteLine("<><>");
                sw.WriteLine(DateTime.Now);
            }
        }
    }
}
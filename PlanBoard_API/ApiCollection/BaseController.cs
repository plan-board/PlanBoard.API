using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Http;
using PlanBoard_API.Common;
using Logger = LoggerLibray.Logger;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing;
using System.Drawing.Imaging;

namespace PlanBoard_API.ApiCollection
{
    public class BaseController : ApiController
    {
        public void WriteTrace()
        {
            var uri = DateTime.Now + " Request " + Request.RequestUri;
            WriteTrace(uri);
        }

        public void WriteTrace(string message)
        {
            if (ConfigurationManager.AppSettings["Logger:IsTraceEnabled"].ToLower() == "yes")
            {
                try
                {
                    throw new CustomException("Trace");
                }
                catch (Exception ex2)
                {
                    Logger.WriteLog(ex2, "Trace-->" + message);
                }
            }
        }
    }



}

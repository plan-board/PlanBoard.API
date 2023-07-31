using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PlanBoard_API.Common
{
    public class Helper
    {
        public static string SeriliseData(object obj)
        {
            try
            {
                var serilizer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                return serilizer.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
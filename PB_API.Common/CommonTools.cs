using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PB_API.Common
{
    public class CommonTools
    {
        public interface IVisitorsGeographicalLocation
        {
            DataTable GetLocation(string strIPAddress);
        }
        public class ClsVisitorsGeographicalLocation : IVisitorsGeographicalLocation
        {
            public DataTable GetLocation(string strIPAddress)
            {
                //Create a WebRequest with the current Ip
                WebRequest _objWebRequest =
                    WebRequest.Create("http://freegeoip.appspot.com/xml/" //http://ipinfodb.com/ip_query.php?ip=
                                      + strIPAddress);
                //Create a Web Proxy
                WebProxy _objWebProxy =
                    new WebProxy("http://freegeoip.appspot.com/xml/"
                                 + strIPAddress, true);

                //Assign the proxy to the WebRequest
                _objWebRequest.Proxy = _objWebProxy;

                //Set the timeout in Seconds for the WebRequest
                _objWebRequest.Timeout = 2000;

                try
                {
                    //Get the WebResponse 
                    WebResponse _objWebResponse = _objWebRequest.GetResponse();
                    //Read the Response in a XMLTextReader
                    XmlTextReader _objXmlTextReader
                        = new XmlTextReader(_objWebResponse.GetResponseStream());

                    //Create a new DataSet
                    DataSet _objDataSet = new DataSet();
                    //Read the Response into the DataSet
                    _objDataSet.ReadXml(_objXmlTextReader);

                    return _objDataSet.Tables[0];
                }

                catch
                {

                    return null;

                }
            }
        }
    }
}

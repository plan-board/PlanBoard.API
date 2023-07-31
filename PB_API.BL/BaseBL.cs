using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using PB_API.Common;
using PB_API.DL;
using PB_API.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PB_API.BL
{
    public class BaseBL
    {
        /// <summary>
        /// Sets the connection.
        /// </summary>
        /// <param name="conString">The con string.</param>
        public void SetConnection(string conString)
        {
            if (string.IsNullOrEmpty(DataManager.DbConnectionString) && !string.IsNullOrEmpty(conString))
            {
                DataManager.DbConnectionString = conString;
            }
        }

        /// <summary>
        /// Gets the size of obejct.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public long GetSizeOfObejct(object obj)
        {
            var serilizer = new JavaScriptSerializer();
            var data = serilizer.Serialize(obj);
            var size = Encoding.UTF8.GetByteCount(data);     // giving half of size
            return size;
            //var bytes = new byte[data.Length * sizeof(char)]; // giving full size not sure 
            //return bytes.Length; //1/Constants.Bytes;
        }


        public long GetSizeOfObejct<T>(List<T> list)
        {
            if (!list.Any())
            {
                return Constants.Zero;
            }
            var obj = list.FirstOrDefault();
            var serilizer = new JavaScriptSerializer();
            var serilizedData = serilizer.Serialize(obj);
            var size = Encoding.UTF8.GetByteCount(serilizedData);     // giving half of size
            return size * list.Count;
        }

        /// <summary>
        /// Wriles the json file.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="subFolder">The sub folder.</param>
        /// <returns></returns>
        //public Response<string> WrileJsonFile(object obj, string fileName, string subFolder)
        //{
        //    var response = new Response<string>();
        //    try
        //    {
        //        var serialiser = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
        //        var filedata = serialiser.Serialize(obj);
        //        var folderName = ConfigurationManager.AppSettings["JsonFolder"];
        //        if (string.IsNullOrEmpty(folderName))
        //        {
        //            response.IsSuccess = true;
        //            response.Message = Constants.JsonFolderMissing;
        //            return response;
        //        }

        //        var siteLocation = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        //        var path = siteLocation + @"\" + folderName;
        //        if (!string.IsNullOrEmpty(subFolder))
        //            path += @"\" + subFolder;

        //        var filePath = string.Format(path + @"\{0}.json", fileName);

        //        if (!File.Exists(filePath))
        //            File.Create(filePath).Close();

        //        File.WriteAllText(filePath, filedata);
        //        var file = new FileInfo(filePath);
        //        var size = file.Length;
        //        response.IsSuccess = true;
        //        response.DataSize = size / (1024.0);
        //        if (!string.IsNullOrEmpty(subFolder))
        //            folderName = folderName + "/" + subFolder;

        //        response.FilePath = string.Format("/" + folderName + "/{0}.json", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        //response.Entity = ex.Message;
        //        response.IsSuccess = false;
        //        return response;
        //    }

        //    return response;
        //}

    }

    public class EntityConvert<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            T doc = (T)value;
            // Create a JObject from the document, respecting existing JSON attribs
            JObject jo = JObject.FromObject(value);
            // Write out the JSON
            jo.WriteTo(writer);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

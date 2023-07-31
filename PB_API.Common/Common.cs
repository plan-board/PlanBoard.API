using LoggerLibray;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace PB_API.Common
{
    public class Common
    {
        public static string Base64ToImage(string base64String, int countValue, int maxValue, string savePath, string filePrefix, string fileName)
        {
            string imagePath = string.Empty;

            try
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String.Replace(" ", "+"));
                //  byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0,
                    imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                if (countValue > 0 && maxValue > 0)
                {
                    imagePath = (maxValue + "_" + filePrefix + countValue + ".png");
                }
                else
                {
                    imagePath = ((filePrefix != "" ? "_" : "") + fileName + ".png");
                }
                image.Save((savePath + imagePath), ImageFormat.Png);
                return Convert.ToString(imagePath);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return "fail";
            }
        }
    }
}

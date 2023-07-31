using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using LoggerLibray;

namespace PB_API.Common
{
    public class SendMail
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ToCCAddress { get; set; }
        public string ToBCCAddress { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public string Sender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        ////public string HostName { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool DefaultCredentials { get; set; }
        public System.Net.Mail.SmtpDeliveryMethod DeliveryMethod { get; set; }
        public int Timeout { get; set; }



        public string SmtpServer { get; set; }

        public string Attachments { get; set; }

        public Dictionary<bool, string> SendingMail()
        {
            Dictionary<bool, string> responseList = new Dictionary<bool, string>();
            try
            {
                if (!string.IsNullOrEmpty(Sender) && !string.IsNullOrEmpty(FromAddress) && !string.IsNullOrEmpty(ToAddress))
                {
                    if (!string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body))
                    {
                        using (SmtpClient SmtpServer = new SmtpClient(Sender, Port))
                        {
                            using (MailMessage mail = new MailMessage())
                            {
                                try
                                {


                                    mail.Subject = Subject;
                                    mail.Body = Body;
                                    mail.IsBodyHtml = true;
                                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                                    mail.BodyEncoding = System.Text.Encoding.UTF8;

                                    mail.From = new MailAddress(FromAddress);
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(Attachments);
                                    mail.Attachments.Add(attachment);
                                    mail.To.Add(ToAddress);

                                    //SmtpServer.Host = Sender;
                                    SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                                    //SmtpServer.EnableSsl = EnableSsl;
                                    SmtpServer.Port = Port;
                                    //SmtpServer.UseDefaultCredentials = DefaultCredentials;
                                    SmtpServer.Timeout = Timeout;

                                    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                                    //System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    //System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    //System.Net.Security.SslPolicyErrors sslPolicyErrors)
                                    //{
                                    //    return true;
                                    //};

                                    SmtpServer.Send(mail);
                                    responseList.Add(true, "Mail Sent Sucessfully");
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteLog(ex, "Exception for Sending Mail");
                                    responseList.Add(false, ex.Message);
                                }
                                finally
                                {
                                    mail.Dispose();
                                    SmtpServer.Dispose();
                                }

                            }
                        }
                    }
                    else
                    {
                        responseList.Add(false, "Subject/Body is blank");
                    }

                }
                else
                {
                    responseList.Add(false, "Sender Name is blank");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, "Exception for Sending Mail");
                responseList.Add(false, ex.Message);
            }
            return responseList;
        }
    }
}

using System;
using System.Data;
using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace ChapterPortal
{
    public class EmailModule
    {
        public virtual void SendInvoiceEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string emailSender, string subject, string body, string attachmentName, byte[] content)
        {
            MailAddress from = new MailAddress(fromEmail, emailSender);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            MemoryStream ms = new MemoryStream(content);

            ContentType ct = new ContentType();
            ct.MediaType = MediaTypeNames.Application.Pdf;
            ct.Name = attachmentName + ".pdf";

            Attachment attach = new Attachment(ms, ct);
            message.Attachments.Add(attach);

            SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
        }

        public virtual void SendRegistrationEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string emailBody)
        {
            MailAddress from = new MailAddress(fromEmail);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = "Chapter Portal Officer Info Updates";
            message.Body = emailBody;

            message.BodyEncoding = System.Text.Encoding.UTF8;

            SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
        }

        public virtual void SendRegistrationEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string emailBody, byte[] content)
        {
            MailAddress from = new MailAddress(fromEmail);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = "AIRP 2011 Registration";
            message.Body = emailBody;

            message.BodyEncoding = System.Text.Encoding.UTF8;

            MemoryStream ms = new MemoryStream(content);
            ContentType ct = new ContentType();
            ct.MediaType = MediaTypeNames.Application.Pdf;
            ct.Name = "AIRP_INVOICE.pdf";
            Attachment attach = new Attachment(ms, ct);
            message.Attachments.Add(attach);

            SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
        }

        public virtual void SendRegistrationEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string emailSender, string subject, string emailBody, byte[] content)
        {
            MailAddress from = new MailAddress(fromEmail, emailSender);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = emailBody;

            message.BodyEncoding = System.Text.Encoding.UTF8;

            MemoryStream ms = new MemoryStream(content);
            ContentType ct = new ContentType();
            ct.MediaType = MediaTypeNames.Application.Pdf;
            ct.Name = "AIRP_INVOICE.pdf";
            Attachment attach = new Attachment(ms, ct);
            message.Attachments.Add(attach);

            SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
        }

        public virtual void SendResidentAdminEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string ccEmail, string emailSender, IList<string> bccAllEmail, IList<int> ToCaseId, string subject, string body)
        {
            MailAddress from = new MailAddress(fromEmail, emailSender);

            int m = 0;
            for (int idx = 0; idx < bccAllEmail.Count; idx++)
            {
                try
                {
                    string bccEmail = bccAllEmail[idx];
                    MailMessage message = new MailMessage(from, from);

                    if (!string.IsNullOrEmpty(ccEmail) && idx < 1)
                    {
                        string[] ccEmails = ccEmail.Split(new char[] { ';', ',' });
                        for (int j = 0; j < ccEmails.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(ccEmails[j].Trim()))
                                message.CC.Add(ccEmails[j].Trim());
                        }
                    }

                    if (!string.IsNullOrEmpty(bccEmail))
                    {
                        string[] bccEmails = bccEmail.Split(new char[] { ';', ',' });
                        for (int k = 0; k < bccEmails.Length; k++)
                        {
                            if (!string.IsNullOrEmpty(bccEmails[k].Trim()))
                                message.Bcc.Add(bccEmails[k].Trim());
                            if (m < ToCaseId.Count)
                            {

                            }
                            m++;
                        }
                    }

                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    message.Body = body;
                    message.BodyEncoding = System.Text.Encoding.UTF8;

                    SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
                    message.Dispose();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw new Exception(idx.ToString());
                }
            }
        }

        public virtual void SendRegistrationEmail(string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword, string fromEmail, string toEmail, string emailSender, string subject, string emailBody)
        {
            MailAddress from = new MailAddress(fromEmail, emailSender);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = emailBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            SendEmail(message, smtpServer, smtpDomain, smtpLogin, smtpPassword);
        }

        private static void SendEmail(MailMessage message, string smtpServer, string smtpDomain, string smtpLogin, string smtpPassword)
        {
            SmtpClient client = new SmtpClient(smtpServer);
            client.Timeout = 1200000;

            if (string.IsNullOrEmpty(smtpDomain) || string.IsNullOrEmpty(smtpLogin) || string.IsNullOrEmpty(smtpPassword))
                client.UseDefaultCredentials = true;
            else
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpLogin, smtpPassword, smtpDomain);
            }

            client.Send(message);
            client.Dispose();
        }
    }

    /// <summary>
    /// Summary description for Utility
    /// </summary>
    public class Utility
    {
        public string strEncCusToken;

        public static string _PersonifySSOWebService_Default = null;
        public static string PersonifySSOWebService_Default
        {
            get { return _PersonifySSOWebService_Default; }
            set
            {
                if (value != null)
                    _PersonifySSOWebService_Default = value;
            }
        }

        public static string _PIWSAcc_Login = null;
        public static string PIWSAcc_Login
        {
            get { return _PIWSAcc_Login; }
            set
            {
                if (value != null)
                    _PIWSAcc_Login = value;
            }
        }

        public static string _PIWSAcc_Password = null;
        public static string PIWSAcc_Password
        {
            get { return _PIWSAcc_Password; }
            set
            {
                if (value != null)
                    _PIWSAcc_Password = value;
            }
        }

        public static string _Pws_Acc_OrgId = null;
        public static string Pws_Acc_OrgId
        {
            get { return _Pws_Acc_OrgId; }
            set
            {
                if (value != null)
                    _Pws_Acc_OrgId = value;
            }
        }

        public static string _Pws_Acc_OrgUnitId = null;
        public static string Pws_Acc_OrgUnitId
        {
            get { return _Pws_Acc_OrgUnitId; }
            set
            {
                if (value != null)
                    _Pws_Acc_OrgUnitId = value;
            }
        }

        static string timssSessionId = null;

        public static string TimssSessionId
        {
            get
            {
                if (timssSessionId == null)
                    return (string)Get_TIMSS_SessionID().Result;
                else return timssSessionId;
            }
        }

        public static async Task<string> Get_TIMSS_SessionID()
        {
            try
            {
                var basicHttpBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                var endpointAddress = new EndpointAddress(new Uri(Utility.PersonifySSOWebService_Default));

                PersonifySSOWebService.SimpleWebServiceSoapClient client = new PersonifySSOWebService.SimpleWebServiceSoapClient(basicHttpBinding, endpointAddress);
                if (client.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    client.Abort();
                    client = new PersonifySSOWebService.SimpleWebServiceSoapClient(basicHttpBinding, endpointAddress);
                }

                string login = Utility.PIWSAcc_Login;
                string passwd = Utility.PIWSAcc_Password;
                string orgid = Utility.Pws_Acc_OrgId;
                string unitid = Utility.Pws_Acc_OrgUnitId;
                var ret = await client.ConnectAsync(login, passwd, orgid, unitid).ConfigureAwait(false);
                client.Abort();
                return ret.Token;
            }
            catch (Exception e)
            {

                string msg = e.Message;
                throw;
            }
        }

        public static async Task<DataSet> AMS_Query_Result(string spName, string paramList, string paramValueList) //, string PersonifySSOWebService)
        {
            PersonifySSOWebService.SimpleWebServiceSoapClient client = null;
            DataSet myDataSet = new DataSet();

            try
            {
                var basicHttpBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                var endpointAddress = new EndpointAddress(new Uri(Utility.PersonifySSOWebService_Default));

                client = new PersonifySSOWebService.SimpleWebServiceSoapClient(basicHttpBinding, endpointAddress);
                if (client.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    client.Abort();
                    client = new PersonifySSOWebService.SimpleWebServiceSoapClient(basicHttpBinding, endpointAddress);
                }
                string timssSessionId = TimssSessionId;
                var procedure = await client.ExecuteStoredProcedureAsync(timssSessionId, spName, paramList, paramValueList).ConfigureAwait(true);
                if (procedure.Status)
                {
                    XmlDocument xdocRes = new XmlDocument();
                    xdocRes.LoadXml(procedure.ResultSet);

                    myDataSet.ReadXml(new XmlNodeReader(xdocRes));
                }
                client.Abort();
                client = null;
                return myDataSet;
            }
            catch (TimeoutException ex)
            {
                string error = ex.Message;
                if (client != null)
                    client.Abort();
                throw;
            }
            catch (FaultException<string> ex)
            {
                string error = ex.Message;
                if (client != null)
                    client.Abort();
                throw;
            }
            catch (FaultException ex)
            {
                string error = ex.Message;
                if (client != null)
                    client.Abort();
                throw;
            }
            catch (CommunicationException ex)
            {
                string error = ex.Message + ex.StackTrace;
                if (client != null)
                    client.Abort();
                throw;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (client != null)
                    client.Abort();
                throw;
            }
        }
    }
}
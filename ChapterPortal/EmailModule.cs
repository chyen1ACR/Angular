using System;
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
}

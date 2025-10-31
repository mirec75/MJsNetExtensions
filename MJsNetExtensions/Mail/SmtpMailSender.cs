using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectValidation;

namespace MJsNetExtensions.Mail
{
    /// <summary>
    /// Summary description for SmtpMailSender
    /// </summary>
    public static class SmtpMailSender
    {
        #region API - Public Methods

        /// <summary>
        /// Send an Email 
        /// </summary>
        /// <param name="emailMessage"><see cref="SmtpMailMessage"/></param>
        /// <param name="settings"><see cref="SmtpClientSettings"/></param>
        /// <returns><see cref="OperationResult"/></returns>
        public static OperationResult SendMail(this SmtpMailMessage emailMessage, SmtpClientSettings settings)
        {
            return settings.SendMail(emailMessage);
        }

        /// <summary>
        /// Send an Email 
        /// </summary>
        /// <param name="settings"><see cref="SmtpClientSettings"/></param>
        /// <param name="emailMessage"><see cref="SmtpMailMessage"/></param>
        /// <returns><see cref="OperationResult"/></returns>
        public static OperationResult SendMail(this SmtpClientSettings settings, SmtpMailMessage emailMessage)
        {
            // validate params:
            OperationResult operationResult = OperationResult.GetFailureIfNullOrInvalid(settings, nameof(settings));
            if (operationResult != null) { return operationResult; }

            operationResult = OperationResult.GetFailureIfNullOrInvalid(emailMessage, nameof(emailMessage));
            if (operationResult != null) { return operationResult; }

            Exception catchedEx = null;
            try
            {
                using SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = settings.SmtpHost;
                smtpClient.Port = settings.Port;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = settings.EnableSsl;

                smtpClient.Credentials = settings.Credentials;

                using MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailMessage.From);
                mailMessage.To.AddAllComaOrSemicolonSeparatedToAdresses(emailMessage.To);
                mailMessage.CC.AddAllComaOrSemicolonSeparatedToAdresses(emailMessage.CC);
                mailMessage.Bcc.AddAllComaOrSemicolonSeparatedToAdresses(emailMessage.Bcc);
                mailMessage.ReplyToList.AddAllComaOrSemicolonSeparatedToAdresses(emailMessage.ReplyTo);

                mailMessage.BodyEncoding = emailMessage.BodyEncoding;
                mailMessage.SubjectEncoding = emailMessage.SubjectEncoding;
                mailMessage.Priority = emailMessage.Priority;
                mailMessage.IsBodyHtml = emailMessage.IsBodyHtml;

                mailMessage.Subject = emailMessage.Subject;
                mailMessage.Body = emailMessage.Body;

                if (emailMessage.AttachmentFilePaths?.Any() ?? false)
                {
                    foreach (string attachmentFilePath in emailMessage.AttachmentFilePaths)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachmentFilePath));
                    }
                }

                if (emailMessage.Attachments?.Any() ?? false)
                {
                    foreach (Attachment attachment in emailMessage.Attachments)
                    {
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                smtpClient.Send(mailMessage);
            }
            catch (ObjectDisposedException ex)
            {
                catchedEx = ex;
            }
            catch (SmtpException ex)
            {
                catchedEx = ex;
            }
            catch (ArgumentException ex)
            {
                catchedEx = ex;
            }
            catch (InvalidOperationException ex)
            {
                catchedEx = ex;
            }

            if (catchedEx != null)
            {
                return OperationResult.CreateFailure(
                    new InvalidOperationException($"From: {emailMessage.From}\nTo: {emailMessage.To}\nSubject: {emailMessage}\nmessage: {emailMessage.Body}", catchedEx)
                    );
            }

            return OperationResult.CreateSuccess();
        }
        #endregion API - Public Methods

        #region Private Methods
        private static void AddAllComaOrSemicolonSeparatedToAdresses(this MailAddressCollection mailAddressCollection, string toAddresses)
        {
            if (mailAddressCollection == null ||
                string.IsNullOrWhiteSpace(toAddresses)
                )
            {
                return; // skip
            }

            foreach (var address in toAddresses.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                mailAddressCollection.Add(address);
            }
        }
        #endregion Private Methods
    }
}

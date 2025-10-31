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
    /// The SMTP Mail Message Data
    /// </summary>
    public class SmtpMailMessage : ISimpleValidatable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the email address "from" whom this mail is being sent.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets a coma or semicolon delimited list of recipient e-mail addresses "to" whome this mail is being sent.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets a coma or semicolon delimited list of recipient e-mail addresses to whome this mail is being sent as carbon copy (CC).
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// Gets or sets a coma or semicolon delimited list of recipient e-mail addresses to whome this mail is being sent as carbon copy (BCC).
        /// </summary>
        public string Bcc { get; set; }

        /// <summary>
        /// Gets or sets a coma or semicolon delimited list of reply-to e-mail addresses. <see cref="MailMessage.ReplyToList"/>.
        /// </summary>
        public string ReplyTo { get; set; }


        /// <summary>
        /// Specifies the priority of a <see cref="MailMessage"/>.
        /// </summary>
        public MailPriority Priority { get; set; } = MailPriority.Normal;

        /// <summary>
        /// Gets or sets the subject line of the e-mail message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the encoding used to encode the message subject. Default is: <see cref="Encoding.UTF8"/>.
        /// </summary>
        public Encoding SubjectEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Gets or sets a value indicating whether the mail message body is in Html (if true) or a plain text if false.
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// The body template, which can use custom variables e.g. like "{varName}" as a replacements, which will be replaced on runtime
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the encoding used to encode the message body. Default is: <see cref="Encoding.UTF8"/>.
        /// </summary>
        public Encoding BodyEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Optional. Can be null or empty. Collection of Attachment File Paths.
        /// </summary>
        public IEnumerable<string> AttachmentFilePaths { get; set; }

        /// <summary>
        /// Optional. Can be null or empty. Collection of <see cref="Attachment"/>-s.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(ISimpleValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// NOTE: This method is intended for pure validation of objects, i.e. there sould be NO side effects during the validation of object implementing this interface.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PreStructureValidation([ValidatedNotNull] ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIfNullOrWhiteSpace(this.From, nameof(this.From));

            validationResult.InvalidateIf(
                string.IsNullOrWhiteSpace(this.To) && string.IsNullOrWhiteSpace(this.CC) && string.IsNullOrWhiteSpace(this.Bcc),
                nameof(this.To),
                $"At least one of: {this.To}, or {this.CC}, or {this.Bcc} must be specified."
                );

            validationResult.InvalidateIf(
                string.IsNullOrWhiteSpace(this.Subject) && string.IsNullOrWhiteSpace(this.Body),
                nameof(this.Subject),
                $"At least one of: {this.Subject}, or {this.Body} must be specified."
                );

            validationResult.InvalidateIfNullOrWhiteSpace(this.From, nameof(this.From));
            validationResult.InvalidateIfNullOrWhiteSpace(this.From, nameof(this.From));
        }

        /// <summary>
        /// Replace variables in <see cref="Subject"/> and <see cref="Body"/> based on <paramref name="variablesDictionary"/>.
        /// This method does not throw. If <paramref name="variablesDictionary"/>, or <see cref="Subject"/> or <see cref="Body"/> is null or empty, nothing will be done.
        /// </summary>
        /// <param name="variablesDictionary">The variables to replace to.</param>
        /// <returns>self, to enable "fluent" usage.</returns>
        public SmtpMailMessage UpdateSubjectAndBody(IDictionary<string, string> variablesDictionary)
        {
            this.Subject = this.Subject.ReplaceStrings(variablesDictionary, false);
            this.Body = this.Body.ReplaceStrings(variablesDictionary, false);

            return this;
        }

        /// <summary>
        /// Clone method
        /// </summary>
        /// <returns></returns>
        public SmtpMailMessage Clone()
        {
            // make ShallowCopy first:
            SmtpMailMessage clone = (SmtpMailMessage)this.MemberwiseClone();
            return clone;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Mail From: {this.From}, ");

            if (!string.IsNullOrWhiteSpace(this.To))
            {
                sb.Append($"To: {this.To}, ");
            }
            if (!string.IsNullOrWhiteSpace(this.CC))
            {
                sb.Append($"CC: {this.CC}, ");
            }
            if (!string.IsNullOrWhiteSpace(this.Bcc))
            {
                sb.Append($"BCC: {this.Bcc}, ");
            }
            if (this.Priority != MailPriority.Normal)
            {
                sb.Append($"Priority: {this.Priority}, ");
            }

            sb.Append($"Attachments: {this.AttachmentFilePaths?.Count() ?? 0 + this.Attachments?.Count() ?? 0}, ");
            sb.Append($"Subject: {this.Subject}, ");
            sb.Append(Environment.NewLine);
            sb.Append(this.Body);

            return sb.ToString();
        }
        #endregion API - Public Methods
    }
}

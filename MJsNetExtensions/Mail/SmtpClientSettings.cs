using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectValidation;

namespace MJsNetExtensions.Mail
{
    /// <summary>
    /// The SMTP Client Settings for sending an email via: <see cref="SmtpMailSender.SendMail(SmtpClientSettings, SmtpMailMessage)"/>.
    /// </summary>
    public class SmtpClientSettings : ISimpleValidatable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the SMTP relay mail server to use to send the e-mail messages.
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// The port on which the SMTP server is listening. The default port is <c>25</c>.
        /// </summary>
        public int Port { get; set; } = 25;

        /// <summary>
        /// Enable or disable use of SSL when sending e-mail message. Default is false.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the credentials used to authenticate the sender. Default is: <see cref="CredentialCache.DefaultCredentials"/>.
        /// E.g.: for basic authentication create: new System.Net.NetworkCredential(username, password);
        /// or for NTLM use e.g. the current user defaut: <see cref="CredentialCache.DefaultNetworkCredentials"/>.
        /// </summary>
        public ICredentialsByHost Credentials { get; set; } = (ICredentialsByHost)CredentialCache.DefaultCredentials;

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
                .InvalidateIfNullOrWhiteSpace(this.SmtpHost, nameof(this.SmtpHost));

            validationResult.InvalidateIf(this.Port < 1, nameof(this.Port), "must be > 0, but is: {0}", this.Port);

            validationResult.InvalidateIfNull(this.Credentials, nameof(this.Credentials));
        }

        /// <summary>
        /// Clone method
        /// </summary>
        /// <returns></returns>
        public SmtpClientSettings Clone()
        {
            // make ShallowCopy first:
            SmtpClientSettings clone = (SmtpClientSettings)this.MemberwiseClone();
            return clone;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"SmtpHost: {this.SmtpHost}:{this.Port}");
            if (this.EnableSsl)
            {
                sb.Append($" SSL");
            }

            try
            {
                NetworkCredential netKredenc = this.Credentials?.GetCredential(this.SmtpHost, this.Port, "");
                if (netKredenc != null)
                {
                    sb.Append($", User: ");
                    if (!string.IsNullOrWhiteSpace(netKredenc.Domain))
                    {
                        sb.Append($"{netKredenc.Domain}\\");
                    }
                    sb.Append($"{netKredenc.UserName}");
                }
            }
            catch (Exception)
            {
                // we ignore any exception here, as this is only for ToString output
            }

            return sb.ToString();
        }
        #endregion API - Public Methods
    }
}

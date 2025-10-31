namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Validation Issue Event Args class for <see cref="XmlValidationIssue"/> event propagation.
    /// </summary>
    public class XmlValidationIssueEventArgs : EventArgs
    {
        // default constructor is prohibited
        private XmlValidationIssueEventArgs() { }

        /// <summary>
        /// Constructs a XmlValidationIssueEventArgs with context of a <see cref="XmlValidationIssue"/> which has to be propagated.
        /// </summary>
        /// <param name="issue"></param>
        public XmlValidationIssueEventArgs(XmlValidationIssue issue)
        {
            this.Issue = issue;
        }

        /// <summary>
        /// Gets the related <see cref="XmlValidationIssue"/>.
        /// </summary>
        public XmlValidationIssue Issue { get; private set; }
    }
}

using MJsNetExtensions.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeCustomer.Types.Schema
{
    public interface IRequestCommons : IXsiSchemaLocationInformation
    {
        int MaxSupportedInterfaceVersion { get; }

        int InterfaceVersion { get; }

        bool InterfaceVersionSpecified { get; }

        int BranchNo { get; }

        /// <summary>
        /// Client Number is also known as the Customer Number
        /// </summary>
        string ClientNumber { get; }

        /// <summary>
        /// This is the numerical equivalent to <see cref="ClientNumber"/>.
        /// It is &gt; 0 only if the <see cref="ClientNumber"/> could be parsed as an integer number.
        /// </summary>
        int CustomerNo { get; }

        /// <summary>
        /// Is language attribute provided in the request
        /// </summary>
        bool LanguageSpecified { get; }

        string ReferenceNumber { get; }

        int PositionsCount { get; }

        void PostXmlDeserializeUpdate(string requestString, int branchNo);
    }
}

#pragma warning disable S125
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MJsNetExtensions.Xml.Validation
{
    /// <summary>
    /// Summary description for ValidatingXmlReader
    /// Inspired mostly by: https://stackoverflow.com/a/56261938/3426897
    /// and I also just scrolled down the: https://link.springer.com/content/pdf/bbm%3A978-1-4302-0998-0%2F1.pdf
    /// Technology tested in KompletnyCMD and idea gained from XmlReWriter + the key point for this reader is the possibility 
    /// to wind a little back with: <see cref="XmlReader.MoveToElement()"/>. Interesting is also <see cref="XmlReader.MoveToFirstAttribute()"/>.
    /// </summary>
    internal sealed class ValidatingXmlReader : XmlReader
    {
        #region Fields

        private XmlReader coreReader;

        StringReader justTakenCareOfDisposeOfStringReader;

        private List<string> xPath = new();

        private bool isXmlValidatorReentrancyDecremented;

        #endregion Fields

        #region Construction / Destruction
        // prohibit user default construction
        private ValidatingXmlReader() { }

        public static ValidatingXmlReader Create(XmlValidator xmlValidator, XmlReader reader, StringReader justCareOfDisposeOfStringReaderTmp, string xmlFilePath)
        {
            Throw.IfNull(xmlValidator, nameof(xmlValidator));
            Throw.IfNull(reader, nameof(reader));

            // Implementing protected creation of IDisposable in a factory method:
            ValidatingXmlReader validatingXmlReaderTmp = null;

            try
            {
                validatingXmlReaderTmp = new ValidatingXmlReader();
                validatingXmlReaderTmp.XmlValidator = xmlValidator;
                validatingXmlReaderTmp.coreReader = reader;
                validatingXmlReaderTmp.justTakenCareOfDisposeOfStringReader = justCareOfDisposeOfStringReaderTmp;

                validatingXmlReaderTmp.ValidationResult.UpdateInfo(xmlValidator, xmlFilePath);
                xmlValidator.XmlValidationIssueHandler += validatingXmlReaderTmp.ValidationResult.HandleXmlValidationIssue;

                // Decouple IDisposable to return it and return it immediatelly now without the protecting dispose in finally:
                ValidatingXmlReader validatingXmlReader = validatingXmlReaderTmp;
                validatingXmlReaderTmp = null;

                return validatingXmlReader;
            }
            finally
            {
                validatingXmlReaderTmp?.Dispose();
            }
        }
        #endregion Construction / Destruction

        #region Properties

        public XmlValidator XmlValidator { get; private set; }

        /// <summary>
        /// The result of this XML validation.
        /// </summary>
        public XmlValidationResult ValidationResult { get; } = new();

        /// <summary>
        /// This flag is (automatically) set to true only if there was an serious error, and the further reading and validation does not make a sense.
        /// This is the case e.g. if the root element does not correspond to any defined root level element in any available XSD or DTD.
        /// </summary>
        public bool StopReadingInvalidXml { get; internal set; }

        /// <summary>
        /// Temporary storing validation issues, to be able to update their content. They get propagated to subscribers at the end of the validation.
        /// </summary>
        internal List<XmlValidationIssue> PendingXmlValidationErrors { get; } = new();

        #region Wrapper Boilerplate

        public override XmlNodeType NodeType => coreReader.NodeType;

        public override string LocalName => coreReader.LocalName;

        public override string NamespaceURI => coreReader.NamespaceURI;

        public override string Prefix => coreReader.Prefix;

        public override string Value => coreReader.Value;

        public override int Depth => coreReader.Depth;

        public override string BaseURI => coreReader.BaseURI;

        public override bool IsEmptyElement => coreReader.IsEmptyElement;

        public override int AttributeCount => coreReader.AttributeCount;

        public override bool EOF => this.StopReadingInvalidXml || coreReader.EOF;

        public override ReadState ReadState => coreReader.ReadState >= ReadState.Error || !this.StopReadingInvalidXml ? coreReader.ReadState : ReadState.Error;

        public override XmlNameTable NameTable => coreReader.NameTable;

        #endregion Wrapper Boilerplate

        #endregion Properties

        #region API - Public Methods

        public override bool Read()
        {
            if (this.StopReadingInvalidXml)
            {
                RaisePendingValidationErrors();
                return false;
            }

            bool readResult = ProtectingFunc(() => coreReader.Read());

            if (readResult && coreReader.NodeType == XmlNodeType.Element)
            {
                this.xPath.Add(coreReader.Name); // == (string.IsNullOrEmpty(coreReader.Prefix)) ? coreReader.LocalName : coreReader.Prefix + ":" + coreReader.LocalName;

                XElement root = 
                    coreReader.Depth == 0 && (this.XmlValidator?.HasRootElementHandlerRegistered ?? false) ? 
                    new XElement(((XNamespace)coreReader.NamespaceURI) + coreReader.LocalName) :
                    null;

                // handle pending errors:
                if (this.PendingXmlValidationErrors.Count > 0)
                {
                    string curPath = string.Join("/", this.xPath);

                    foreach (XmlValidationIssue validationErr in this.PendingXmlValidationErrors)
                    {
                        // just set the default...
                        validationErr.XPath = curPath;

                        if (!string.IsNullOrWhiteSpace(validationErr.AttributeLocalName)) // == if (string.IsNullOrWhiteSpace(validationErr.ElementLocalName))
                        {
                            // iterate attributes to try to override the xPath default if possible, while collecting optional root attributes:
                            while (coreReader.MoveToNextAttribute())
                            {
                                SetAttributeHelper(root, coreReader);

                                if (coreReader.LocalName == validationErr.AttributeLocalName)
                                {
                                    validationErr.XPath = $"{curPath}/@{coreReader.Name}";
                                    break;
                                }
                            }
                        }

                        OnRaiseXmlValidationIssueEvent(validationErr);
                    }

                    this.PendingXmlValidationErrors.Clear();
                }

                if (root != null)
                {
                    // read all or the rest of root's attributes:
                    while (coreReader.MoveToNextAttribute())
                    {
                        SetAttributeHelper(root, coreReader);
                    }

                    this.XmlValidator?.OnRaiseRootElementEvent(root);
                }

                //NOTE: both of following calls work, but I need here the Move-back-To-current-Element()
                //coreReader.MoveToFirstAttribute();
                coreReader.MoveToElement();

                // update XPath:
                if (coreReader.IsEmptyElement)
                {
                    this.xPath.RemoveAt(this.xPath.Count - 1);
                }
            }
            else
            {
                RaisePendingValidationErrors();

                if (coreReader.NodeType == XmlNodeType.EndElement)
                {
                    this.xPath.RemoveAt(this.xPath.Count - 1);
                }
            }

            return readResult;
        }

        public override void Close()
        {
            coreReader.Close();
            justTakenCareOfDisposeOfStringReader?.Close();
            base.Close();

            // check any pending or remaining messages and write them out
            RaisePendingValidationErrors();

            DecrementReentrancyProtection();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // check any pending or remaining messages and write them out
                RaisePendingValidationErrors();

                DecrementReentrancyProtection();

                if (this.XmlValidator != null)
                {
                    this.XmlValidator.XmlValidationIssueHandler -= this.ValidationResult.HandleXmlValidationIssue;
                }
            }

            //NOTE: base.Dispose(disposing) => calls Close()
            base.Dispose(disposing);

            coreReader.Dispose();
            justTakenCareOfDisposeOfStringReader?.Dispose();
        }

        #region Wrapper Boilerplate

        public override string GetAttribute(string name) => ProtectingFunc(() => coreReader.GetAttribute(name));

        public override string GetAttribute(string name, string namespaceURI) => ProtectingFunc(() => coreReader.GetAttribute(name, namespaceURI));

        public override string GetAttribute(int i) => ProtectingFunc(() => coreReader.GetAttribute(i));

        public override string LookupNamespace(string prefix) => ProtectingFunc(() => coreReader.LookupNamespace(prefix));

        public override bool MoveToAttribute(string name) => ProtectingFunc(() => coreReader.MoveToAttribute(name));

        public override bool MoveToAttribute(string name, string ns) => ProtectingFunc(() => coreReader.MoveToAttribute(name, ns));

        public override bool MoveToElement() => ProtectingFunc(() => coreReader.MoveToElement());

        public override bool MoveToFirstAttribute() => ProtectingFunc(() => coreReader.MoveToFirstAttribute());

        public override bool MoveToNextAttribute() => ProtectingFunc(() => coreReader.MoveToNextAttribute());

        public override bool ReadAttributeValue() => ProtectingFunc(() => coreReader.ReadAttributeValue());

        public override void ResolveEntity() => ProtectingAction(() => coreReader.ResolveEntity());

        #endregion Wrapper Boilerplate

        #endregion API - Public Methods

        #region Private Methods

        private void ProtectingAction(Action action)
        {
            ProtectingFunc(() =>
            {
                action?.Invoke();
                return true;
            });
        }

        private T ProtectingFunc<T>(Func<T> functionToProtect)
        {
            XmlException catchedEx = null;
            try
            {
                if (!this.StopReadingInvalidXml)
                {
                    return functionToProtect();
                }
            }
            catch (XmlException ex)
            {
                catchedEx = ex;
                this.StopReadingInvalidXml = true;
                this.ValidationResult.WasException = true;
            }

            RaisePendingValidationErrors();

            if (catchedEx != null)
            {
                // store it for handling outside this catch block
                this.PendingXmlValidationErrors.Add(
                    new XmlValidationIssue(catchedEx, this.ValidationResult.ValidatedXmlFile)
                    );

                RaisePendingValidationErrors();

                throw new XmlException($"Validating XML: {this.ValidationResult.ValidatedXmlFile}", catchedEx); //not necessary: , catchedEx.LineNumber, catchedEx.LinePosition
            }

            return default(T);
        }

        private void RaisePendingValidationErrors()
        {
            // check any pending or remaining messages and write them out
            if (this.PendingXmlValidationErrors.Count > 0)
            {
                string curPath = string.Join("/", this.xPath);

                foreach (XmlValidationIssue validationErr in this.PendingXmlValidationErrors)
                {
                    validationErr.XPath = curPath;
                    OnRaiseXmlValidationIssueEvent(validationErr);
                }

                this.PendingXmlValidationErrors.Clear();
            }
        }

        private void OnRaiseXmlValidationIssueEvent(XmlValidationIssue issue)
        {
            if (issue.Severity < ValidationSeverity.Warning)
            {
                // prepared for future, if there will be a need for ValidationSeverity.Info etc.
            }
            else if (issue.Severity == ValidationSeverity.Warning)
            {
                this.ValidationResult.WarningsCount++;
            }
            else
            {
                this.ValidationResult.ErrorsCount++;
            }

            // forward:
            this.XmlValidator?.OnRaiseXmlValidationIssueEvent(issue);
        }

        private static void SetAttributeHelper(XElement element, XmlReader reader)
        {
            if (element == null)
            {
                return;
            }

            XName attribName = null;
            if (reader.LocalName == "xmlns")
            {
                attribName = reader.LocalName;
            }
            else if (!string.IsNullOrWhiteSpace(reader.NamespaceURI))
            {
                XNamespace attribsNs = reader.NamespaceURI;
                attribName = attribsNs + reader.LocalName;
            }
            else
            {
                attribName = reader.LocalName;
            }

            element.SetAttributeValue(attribName, reader.Value);
        }

        private void DecrementReentrancyProtection()
        {
            if (!this.isXmlValidatorReentrancyDecremented)
            {
                this.isXmlValidatorReentrancyDecremented = true;
                this.XmlValidator?.DecrementReentrancyProtectionAndDeregisterXmlReader();
            }
        }


        #endregion Private Methods
    }
}

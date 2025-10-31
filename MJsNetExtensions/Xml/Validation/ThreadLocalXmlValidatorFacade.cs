namespace MJsNetExtensions.Xml.Validation
{
    using MJsNetExtensions;
    using MJsNetExtensions.Xml;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;


    /// <summary>
    /// A facade allowing transparent use of the (same) XML Validation (i.e. an XmlValidator with the same XSDs, or DTDs) no matter on which thread it is used.
    /// This wrapper facade automatically creates and caches a thread local XmlValidator instance in each thread it is beaing used on.
    /// Each thread local XmlValidator is created equal with the same XSDs, or DTDs provided to the factory method of this class, e.g. <see cref="ThreadLocalXmlValidatorFacade.Create(XmlValidatorSettings)"/>.
    /// The necessity for the existence of this class is the fact, that .Net does not provide a thread safe XSD valiadation!
    /// See e.g.: XmlSchemaSet Thread Safety
    /// https://blogs.msdn.microsoft.com/xmlteam/2009/04/27/xmlschemaset-thread-safety/
    /// Schema validation error / Thread safety of XmlSchemaSet?
    /// https://stackoverflow.com/questions/1486746/schema-validation-error-thread-safety-of-xmlschemaset
    /// </summary>
    public class ThreadLocalXmlValidatorFacade : IDisposable
    {
        #region Fields

        // XmlValidatorBase creation params - an own copy:
        private XmlValidatorSettings settings;

        #endregion Fields

        #region Construction / Destruction
        // prohibit user default construction
        private ThreadLocalXmlValidatorFacade()
        {
        }

        /// <summary>
        /// Construct <see cref="ThreadLocalXmlValidatorFacade"/> for XSD or DTD validation.
        /// NOTE: creating without settings. In this case no XSD or DTD validation will be done. Just XML well-formedness will be checked.
        /// </summary>
        public static ThreadLocalXmlValidatorFacade Create()
        {
            return Create(null);
        }

        /// <summary>
        /// Construct <see cref="ThreadLocalXmlValidatorFacade"/> for XSD or DTD validation.
        /// </summary>
        /// <param name="settings">Optional. Can be null. The <see cref="XmlValidatorSettings"/> object used to configure the new <see cref="XmlValidator"/>.
        /// NOTE: in a case of creating without settings or with empty settings, no XSD or DTD validation will be done. Just XML well-formedness will be checked.</param>
        public static ThreadLocalXmlValidatorFacade Create(XmlValidatorSettings settings)
        {
            if (settings == null)
            {
                // create default settings -> in this case no XSD or DTD validation will be done. Just XML well-formedness will be checked.
                settings = new XmlValidatorSettings();
            }


            // Implementing protected creation of IDisposable in a factory method:
            ThreadLocalXmlValidatorFacade threadLocalXmlValidatorFacadeTmp = null;

            try
            {
                threadLocalXmlValidatorFacadeTmp = new ThreadLocalXmlValidatorFacade();
                threadLocalXmlValidatorFacadeTmp.settings = settings.Clone();

                threadLocalXmlValidatorFacadeTmp.ThreadLocalXmlValidator = new ThreadLocal<XmlValidator>(threadLocalXmlValidatorFacadeTmp.CreateThreadLocalXmlXsdValidatorBase);

                // Decouple IDisposable to return it and return it immediatelly now without the protecting dispose in finally:
                ThreadLocalXmlValidatorFacade threadLocalXmlValidatorFacade = threadLocalXmlValidatorFacadeTmp;
                threadLocalXmlValidatorFacadeTmp = null;

                return threadLocalXmlValidatorFacade;
            }
            finally
            {
                threadLocalXmlValidatorFacadeTmp?.Dispose();
            }
        }

        /// <summary>
        /// Implementing IDisposable pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources.
                this.ThreadLocalXmlValidator?.Dispose();
            }

            this.ThreadLocalXmlValidator = null;

            // clean up unmanaged resources here.
        }

        #region Disposable default stuff
        /// <summary>
        /// Finalizer. Implementing IDisposable pattern. It is not virtual on purpose.
        /// </summary>
        ~ThreadLocalXmlValidatorFacade()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implementing IDisposable. 
        /// This method is NOT virtual ON PURPOSE! 
        /// A derived class should not be able to override this method. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // if disposed, take this object off the finalization queue 
            // and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }
        #endregion Disposable default stuff
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// NOTE: use property <see cref="XmlValidator"/> instead of this property. This is the internal "Thread Local XmlValiators" property. 
        /// It shall be only used if you want to do all the creation and creation exception checking yourselves!
        /// </summary>
        public ThreadLocal<XmlValidator> ThreadLocalXmlValidator { get; private set; }

        /// <summary>
        /// If there was a problem on this thread in the Thread Factory Method, it is stored initialization <see cref="Exception"/>.
        /// </summary>
        /// <exception cref="Exception">Propagating of any XmlValidator creation exceptions to the caller</exception>
#pragma warning disable CA1024
        public XmlValidator GetXmlValidator()
#pragma warning restore CA1024
        {
            if (this.ThreadLocalXmlValidator.Value == null) //NOTE: accessing the Value forces one time per thrread initialization!
            {
                throw this.InitializationException;
            }

            return this.ThreadLocalXmlValidator.Value;
        }

        /// <summary>
        /// If there was a problem in the Thread Factory Method, it is stored initialization <see cref="Exception"/>.
        /// </summary>
        public Exception InitializationException { get; private set; }

        /// <summary>
        /// Flag indicating if there was a problem in the Thread Factory Method. The problem is then stored in <see cref="InitializationException"/>.
        /// </summary>
        public bool WasInitializationException => this.InitializationException != null;

        #endregion Properties

        #region Private Methods

        private XmlValidator CreateThreadLocalXmlXsdValidatorBase()
        {
            //NOTE: does not matter on which thread the exception happened. If there was an Initialization Exception => do nothing!
            if (this.WasInitializationException)
            {
                return null;
            }

            XmlValidator xmlValidator = null;

            try
            {
                xmlValidator = XmlValidator.Create(this.settings);
            }
            catch (Exception ex)
            {
                //this.initializationException = $"ERROR: Could not create {this.settings.XmlValidationType} XML Validator: {ex}";
                this.InitializationException = ex;
                xmlValidator = null;
            }

            return xmlValidator;
        }

        #endregion Private Methods
    }
}

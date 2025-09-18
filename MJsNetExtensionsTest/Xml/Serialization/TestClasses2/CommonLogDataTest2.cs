namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses2
{
    using MJsNetExtensions;
    using TestClasses1;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using MJsNetExtensions.ObjectValidation;


    /// <summary>
    /// Summary description for CommonLogData
    /// </summary>
    [Serializable]
    public class CommonLogDataTest2 : IValidatableAndUpdatable
    {
        #region Statics and Constants

        internal static int CommonDataCounter = 0;

        #endregion Statics and Constants

        #region Dummy Factories
        internal static CommonLogDataTest2 GetNewCommonLogData()
        {
            return new CommonLogDataTest2
            {
                InsertStoredProcedureName = "ChCcLogInsert", // same as: "dbo.ChCcLogInsert", or "[dbo].[ChCcLogInsert]"

                Id = 123,

                StartProcessing = DateTime.Now,
                EndProcessing = DateTime.Now,

                BranchNo = 12,
                CustomerNo = 1086,
                UserName = "ch0001086",
                Request = $"reku! {++CommonDataCounter}",
                Response = "respi :)",

                ItemCountRequest = 1,
                ItemCountResponse = 7,

                ResultLevel = "Success",
                ResultMessage = "",

                FrontendServer = "Fronti",
                BackendServer = "Backi",

                ProcessId = 123,
                ThreadId = 456,
                Version = "1.2.3.4",

                Details = GetnewDetailsLogDataList(),
            };
        }

        internal static List<DetailsLogDataTest2> GetnewDetailsLogDataList()
        {
            return new List<DetailsLogDataTest2>
            {
                new DetailsLogDataTest2
                {
                    Id = 567,
                    LogId = 123,
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Info,
                    Component = "Trulo",
                    Message = $"Haha hihi: {CommonDataCounter}",
                },
                new DetailsLogDataTest2
                {
                    Id = 568,
                    LogId = 123,
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Warn,
                    Component = "Falslo",
                    Message = $"Blaaa blubber bliiiii!: {CommonDataCounter}",
                },
                new DetailsLogDataTest2
                {
                    Id = 569,
                    LogId = 123,
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Error,
                    Component = "MyMethodX",
                    Message = $"Yet another detail message - fuzz, buzz, lorem ipsum. {CommonDataCounter}",
                },
            };
        }

        #endregion Dummy Factories

        #region Properties

        /// <summary>
        /// E.g. "ChPaLog" for Product Avaliability. This is used to address the SQL DB Table.
        /// </summary>
        [XmlAttribute]
        public string InsertStoredProcedureName { get; set; } = null;

        /// <summary>
        /// NOTE: change setter to protected! 
        /// See: Reflection can't find private setter on property of abstract class
        /// https://stackoverflow.com/questions/20763766/reflection-cant-find-private-setter-on-property-of-abstract-class
        /// </summary>
        public long Id { get; set; } = 0;


        public DateTime StartProcessing { get; set; } = DateTime.MinValue;

        public DateTime EndProcessing { get; set; } = DateTime.MinValue;


        public short BranchNo { get; set; } = 0;

        public int CustomerNo { get; set; } = 0;

        public string UserName { get; set; } = null;

        public string Request { get; set; } = null;

        public string Response { get; set; } = null;


        public string ResultLevel { get; set; } = null;

        public string ResultMessage { get; set; } = null;


        public int ItemCountRequest { get; set; } = 0;

        public int ItemCountResponse { get; set; } = 0;


        public string FrontendServer { get; set; } = null;

        public string BackendServer { get; set; } = null;

        public int ProcessId { get; set; } = 0;

        public int ThreadId { get; set; } = 0;

        public string Version { get; set; } = null;

        public List<DetailsLogDataTest2> Details { get; set; } = new List<DetailsLogDataTest2>();

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public virtual void PreStructureValidationAndUpdate(ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIfNullOrWhiteSpace(this.InsertStoredProcedureName, nameof(this.InsertStoredProcedureName));

            validationResult.InvalidateIf(this.StartProcessing == DateTime.MinValue, "{0} not provided", nameof(this.StartProcessing));
            validationResult.InvalidateIf(this.EndProcessing == DateTime.MinValue, "{0} not provided", nameof(this.EndProcessing));

            //TODO: BranchNo: for CH there can be just 12 and 13! This value can be validated with the config data!
            validationResult.InvalidateIf(this.BranchNo < 1, "Invalid {0}: {1}", nameof(this.BranchNo), this.BranchNo);
            validationResult.InvalidateIf(this.CustomerNo < 1, "Invalid {0}: {1}", nameof(this.CustomerNo), this.CustomerNo);
            validationResult.InvalidateIfNullOrWhiteSpace(this.UserName, nameof(this.UserName));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Request, nameof(this.Request));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Response, nameof(this.Response));

            validationResult.InvalidateIf(this.ItemCountRequest < 0, "Invalid {0}: {1}", nameof(this.ItemCountRequest), this.ItemCountRequest);
            validationResult.InvalidateIf(this.ItemCountResponse < 0, "Invalid {0}: {1}", nameof(this.ItemCountResponse), this.ItemCountResponse);

            validationResult.InvalidateIfNullOrWhiteSpace(this.ResultLevel, nameof(this.ResultLevel));
            validationResult.InvalidateIfNull(this.ResultMessage, nameof(this.ResultMessage));

            validationResult.InvalidateIfNullOrWhiteSpace(this.FrontendServer, nameof(this.FrontendServer));
            validationResult.InvalidateIfNullOrWhiteSpace(this.BackendServer, nameof(this.BackendServer));
            validationResult.InvalidateIf(this.ProcessId < 1, "Invalid {0}: {1}", nameof(this.ProcessId), this.ProcessId);
            validationResult.InvalidateIf(this.ThreadId < 1, "Invalid {0}: {1}", nameof(this.ThreadId), this.ThreadId);
            validationResult.InvalidateIfNullOrWhiteSpace(this.Version, nameof(this.Version));

            //validationResult.ValidateSubcomponents(this.Details, false, nameof(this.Details));
        }

        /// <summary>
        /// The Post Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> after all subcomponents of the validatable object gets validated.
        /// In this phase the complex object subtree is already validated and one can react selectively, based on the intermediate state of Validation <seealso cref="ValidationResult.IsValid"/>
        /// and validate all the simple properties, which are closely related to complex object subtree of this validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public virtual void PostStructureValidationAndUpdate(ValidationResult validationResult)
        {
        }

        #region Object Equality Comparison
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as CommonLogDataTest2);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CommonLogDataTest2"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="CommonLogDataTest2"/> equals this instance</returns>
        public bool Equals(CommonLogDataTest2 that)
        {
            // If parameter is null, return false. 
            if (ReferenceEquals(that, null))
            {
                return false;
            }

            // Optimization for a common success case. 
            if (ReferenceEquals(this, that))
            {
                return true;
            }

            //// If run-time types are not exactly the same, return false. 
            //if (this.GetType() != that.GetType())
            //{
            //    return false;
            //}

            // Return true if the fields match. 
            // Note that the base class is not invoked because it is 
            // System.Object, which defines Equals as reference equality. 

            if (this.Id != that.Id)
            {
                return false;
            }

            if (this.StartProcessing != that.StartProcessing)
            {
                return false;
            }

            if (this.EndProcessing != that.EndProcessing)
            {
                return false;
            }

            if (this.BranchNo != that.BranchNo)
            {
                return false;
            }

            if (this.CustomerNo != that.CustomerNo)
            {
                return false;
            }

            if (string.Compare(this.UserName, that.UserName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(this.Request, that.Request, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(this.Response, that.Response, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(this.ResultLevel, that.ResultLevel, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(this.ResultMessage, that.ResultMessage, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (this.ItemCountRequest != that.ItemCountRequest)
            {
                return false;
            }

            if (this.ItemCountResponse != that.ItemCountResponse)
            {
                return false;
            }

            if (string.Compare(this.FrontendServer, that.FrontendServer, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(this.BackendServer, that.BackendServer, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (this.ProcessId != that.ProcessId)
            {
                return false;
            }

            if (this.ThreadId != that.ThreadId)
            {
                return false;
            }

            if (string.Compare(this.Version, that.Version, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (this.Details != that.Details)
            {
                if ((this.Details?.Count() ?? 0) != (that.Details?.Count() ?? 0))
                {
                    return false;
                }

                if (this.Details != null && that.Details != null)
                {
                    for (int ii = 0; ii < this.Details.Count(); ii++)
                    {
                        if (this.Details[ii] == that.Details[ii])
                        { }
                        else if (this.Details[ii] == null)
                        {
                            return false;
                        }
                        else if (!this.Details[ii].Equals(that.Details[ii]))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = this.Id.GetHashCode();
            hash ^= this.StartProcessing.GetHashCode();
            hash ^= this.EndProcessing.GetHashCode();
            hash ^= this.BranchNo.GetHashCode();
            hash ^= this.CustomerNo.GetHashCode();
            hash ^= (this.UserName ?? "").GetHashCode();
            hash ^= (this.Request ?? "").GetHashCode();
            hash ^= (this.Response ?? "").GetHashCode();
            hash ^= (this.ResultLevel ?? "").GetHashCode();
            hash ^= (this.ResultMessage ?? "").GetHashCode();
            hash ^= this.ItemCountRequest.GetHashCode();
            hash ^= this.ItemCountResponse.GetHashCode();
            hash ^= (this.FrontendServer ?? "").GetHashCode();
            hash ^= (this.BackendServer ?? "").GetHashCode();
            hash ^= this.ProcessId.GetHashCode();
            hash ^= this.ThreadId.GetHashCode();
            hash ^= (this.Version ?? "").GetHashCode();

            if ((this.Details?.Count() > 0))
            {
                foreach (var detail in this.Details.Where(it => it != null))
                {
                    hash ^= detail.GetHashCode();
                }
            }

            return hash;
        }
        #endregion Object Equality Comparison

        #endregion API - Public Methods
    }
}

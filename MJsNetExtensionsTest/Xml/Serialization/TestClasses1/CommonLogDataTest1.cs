namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses1
{
    using MJsNetExtensions;
    using MJsNetExtensions.ObjectValidation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for CommonLogData
    /// </summary>
    [Serializable]
    public class CommonLogDataTest1 : IValidatable
    {
        #region Statics and Constants

        internal static int CommonDataCounter;

        #endregion Statics and Constants

        #region Dummy Factories

        internal static CommonLogDataTest1 GetNewCommonLogData()
        {
            return new CommonLogDataTest1
            {
                DbTableName = "ChCcLog",
                //CountryCode = "CH",
                //ServiceAbbreviation = "DN",

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

                CreationDateTime = DateTime.Now,
                CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,

                Details = GetnewDetailsLogDataList(),
            };
        }

        internal static List<DetailsLogDataTest1> GetnewDetailsLogDataList()
        {
            return new List<DetailsLogDataTest1>
            {
                new()
                {
                    Id = 567,
                    LogId = 123,
                    DbTableName = "ChCcDetailsLog",
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Info,
                    Component = "Trulo",
                    Message = $"Haha hihi: {CommonDataCounter}",
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
                new()
                {
                    Id = 568,
                    LogId = 123,
                    DbTableName = "ChCcDetailsLog",
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Warn,
                    Component = "Falslo",
                    Message = $"Blaaa blubber bliiiii!: {CommonDataCounter}",
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
                new()
                {
                    Id = 569,
                    LogId = 123,
                    DbTableName = "ChCcDetailsLog",
                    DetailDateTime = DateTime.Now,
                    Level = DummyLevel.Error,
                    Component = "MyMethodX",
                    Message = $"Yet another detail message - fuzz, buzz, lorem ipsum. {CommonDataCounter}",
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
            };
        }


        #endregion Dummy Factories

        #region Properties

        /// <summary>
        /// E.g. "ChPaLog" for Product Avaliability. This is used to address the SQL DB Table.
        /// </summary>
        [XmlAttribute]
        public string DbTableName { get; set; }

        ///// <summary>
        ///// E.g. "CH" for Switzerland. This is used to address the SQL DB.
        ///// </summary>
        //[XmlAttribute]
        //public string CountryCode { get; set; }

        ///// <summary>
        ///// E.g. "PA" for Product Avaliability. This is used to address the SQL DB Table.
        ///// </summary>
        //[XmlAttribute]
        //public string ServiceAbbreviation { get; set; }

        /// <summary>
        /// NOTE: change setter to protected! 
        /// See: Reflection can't find private setter on property of abstract class
        /// https://stackoverflow.com/questions/20763766/reflection-cant-find-private-setter-on-property-of-abstract-class
        /// </summary>
        public long Id { get; set; }


        public DateTime StartProcessing { get; set; } = DateTime.MinValue;

        public DateTime EndProcessing { get; set; } = DateTime.MinValue;


        public short BranchNo { get; set; }

        public int CustomerNo { get; set; }

        public string UserName { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }


        public string ResultLevel { get; set; }

        public string ResultMessage { get; set; }


        public int ItemCountRequest { get; set; }

        public int ItemCountResponse { get; set; }


        public string FrontendServer { get; set; }

        public string BackendServer { get; set; }

        public int ProcessId { get; set; }

        public int ThreadId { get; set; }

        public string Version { get; set; }

        public DateTime CreationDateTime { get; set; }

        public int CreationDate { get; set; }

        public List<DetailsLogDataTest1> Details { get; set; } = new();

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public virtual void PreStructureValidation(ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIfNullOrWhiteSpace(this.DbTableName, nameof(this.DbTableName));
            //validationResult.InvalidateIfNullOrWhiteSpace(this.CountryCode, nameof(this.CountryCode));
            //validationResult.InvalidateIfNullOrWhiteSpace(this.ServiceAbbreviation, nameof(this.ServiceAbbreviation));

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
        public virtual void PostStructureValidation(ValidationResult validationResult)
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
            return this.Equals(obj as CommonLogDataTest1);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CommonLogDataTest1"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="CommonLogDataTest1"/> equals this instance</returns>
        public bool Equals(CommonLogDataTest1 that)
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

            if (!string.Equals(this.DbTableName, that.DbTableName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

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

            if (!string.Equals(this.UserName, that.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.Request, that.Request, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.Response, that.Response, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.ResultLevel, that.ResultLevel, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.ResultMessage, that.ResultMessage, StringComparison.OrdinalIgnoreCase))
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

            if (!string.Equals(this.FrontendServer, that.FrontendServer, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.BackendServer, that.BackendServer, StringComparison.OrdinalIgnoreCase))
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

            if (!string.Equals(this.Version, that.Version, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (this.CreationDateTime != that.CreationDateTime)
            {
                return false;
            }

            if (this.CreationDate != that.CreationDate)
            {
                return false;
            }

            if (this.Details != that.Details)
            {
                if ((this.Details?.Count ?? 0) != (that.Details?.Count ?? 0))
                {
                    return false;
                }

                if (this.Details != null && that.Details != null)
                {
                    for (int ii = 0; ii < this.Details.Count; ii++)
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
            int hash = (this.DbTableName ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.Id.GetHashCode();
            hash ^= this.StartProcessing.GetHashCode();
            hash ^= this.EndProcessing.GetHashCode();
            hash ^= this.BranchNo.GetHashCode();
            hash ^= this.CustomerNo.GetHashCode();
            hash ^= (this.UserName ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Request ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Response ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.ResultLevel ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.ResultMessage ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.ItemCountRequest.GetHashCode();
            hash ^= this.ItemCountResponse.GetHashCode();
            hash ^= (this.FrontendServer ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.BackendServer ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.ProcessId.GetHashCode();
            hash ^= this.ThreadId.GetHashCode();
            hash ^= (this.Version ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.CreationDateTime.GetHashCode();
            hash ^= this.CreationDate.GetHashCode();

            if ((this.Details?.Count > 0))
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

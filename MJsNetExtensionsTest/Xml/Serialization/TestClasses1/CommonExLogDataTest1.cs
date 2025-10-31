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


    /// <summary>
    /// Summary description for CommonExLogData
    /// </summary>
    public class CommonExLogDataTest1 : CommonLogDataTest1
    {
        #region Dummy Factories

        internal static CommonExLogDataTest1 GetNewCommonExLogData()
        {
            return new CommonExLogDataTest1
            {
                DbTableName = "ChOrLog",
                //CountryCode = "CH",
                //ServiceAbbreviation = "OR",

                Id = 123,

                StartProcessing = DateTime.Now,
                EndProcessing = DateTime.Now,

                BranchNo = 13,
                CustomerNo = 7778,
                UserName = "Ex ch0007778",
                Request = $"Ex Reku! {++CommonDataCounter}",
                Response = "Ex Respi :)",

                ItemCountRequest = 71,
                ItemCountResponse = 77,

                ResultLevel = "Ex Success",
                ResultMessage = "Yes!",

                FrontendServer = "Ex Fronti",
                BackendServer = "Ex Backi",

                ProcessId = 723,
                ThreadId = 756,
                Version = "7.2.3.4",

                CreationDateTime = DateTime.Now,
                CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,

                Reference = "Refik",

                Details = CommonLogDataTest1.GetnewDetailsLogDataList(),

                Orders = GetnewOrdersLogDataList(),
            };
        }

        internal static List<OrdersLogDataTest1> GetnewOrdersLogDataList()
        {
            return new List<OrdersLogDataTest1>
            {
                new()
                {
                    Id = 777,
                    LogId = 123,
                    DbTableName = "ChOrOrdersLog",
                    OrderNo = 457,
                    PosCountRequest = 7,
                    PosCountResponse = 8,
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
                new()
                {
                    Id = 778,
                    LogId = 123,
                    DbTableName = "ChOrOrdersLog",
                    OrderNo = 458,
                    PosCountRequest = 17,
                    PosCountResponse = 18,
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
                new()
                {
                    Id = 779,
                    LogId = 123,
                    DbTableName = "ChOrOrdersLog",
                    OrderNo = 459,
                    PosCountRequest = 27,
                    PosCountResponse = 28,
                    CreationDateTime = DateTime.Now,
                    CreationDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day,
                },
            };
        }

        #endregion Dummy Factories

        #region Properties

        public string Reference { get; set; }

        public List<OrdersLogDataTest1> Orders { get; set; } = new();

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public override void PreStructureValidation(ValidationResult validationResult)
        {
            base.PreStructureValidation(validationResult);

            validationResult?.InvalidateIfNullOrWhiteSpace(this.Reference, nameof(this.Reference));
        }

        /// <summary>
        /// The Post Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> after all subcomponents of the validatable object gets validated.
        /// In this phase the complex object subtree is already validated and one can react selectively, based on the intermediate state of Validation <seealso cref="ValidationResult.IsValid"/>
        /// and validate all the simple properties, which are closely related to complex object subtree of this validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public override void PostStructureValidation(ValidationResult validationResult)
        {
            base.PostStructureValidation(validationResult);
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
            return this.Equals(obj as CommonExLogDataTest1);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CommonExLogDataTest1"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="CommonExLogDataTest1"/> equals this instance</returns>
        public bool Equals(CommonExLogDataTest1 that)
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

            // If run-time types are not exactly the same, return false. 
            if (this.GetType() != that.GetType())
            {
                return false;
            }

            if (!base.Equals(that as CommonLogDataTest1))
            {
                return false;
            }

            // Return true if the fields match. 
            // Note that the base class is not invoked because it is 
            // System.Object, which defines Equals as reference equality. 

            if (!string.Equals(this.Reference, that.Reference, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (this.Orders != that.Orders)
            {
                if ((this.Orders?.Count ?? 0) != (that.Orders?.Count ?? 0))
                {
                    return false;
                }

                if (this.Orders != null && that.Orders != null)
                {
                    for (int ii = 0; ii < this.Orders.Count; ii++)
                    {
                        if (this.Orders[ii] == that.Orders[ii])
                        { }
                        else if (this.Orders[ii] == null)
                        {
                            return false;
                        }
                        else if (!this.Orders[ii].Equals(that.Orders[ii]))
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
            int hash = (this.Reference ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);

            if ((this.Orders?.Count > 0))
            {
                foreach (var order in this.Orders.Where(it => it != null))
                {
                    hash ^= order.GetHashCode();
                }
            }

            return hash;
        }
        #endregion Object Equality Comparison

        #endregion API - Public Methods
    }
}

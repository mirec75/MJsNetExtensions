namespace MJsNetExtensions.xUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Summary description for DummyPerson
    /// </summary>
    public class DummyPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"Person: {FirstName} {LastName}, Id: {Id}, Company: {CompanyName}";
        }
    }
}

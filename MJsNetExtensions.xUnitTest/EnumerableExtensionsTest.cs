using MJsNetExtensions;

namespace MJsNetExtensions.xUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// NOTE: to make this xUnit run also for .Net Framework 4.8:
    /// https://xunit.net/docs/getting-started/netfx/visual-studio
    /// </summary>
    public class EnumerableExtensionsTest
    {
        #region Test Data

        private static readonly List<string> _dummyNullsAndEmptyStrings = new()
        {
            null,
            "",
            " ",
            " \t \r \n ",
        };
        private static readonly List<string> _dummyNullsEmptyAndFilledStrings = new()
        {
            null,
            "",
            " ",
            " \t \r \n ",
            " haha ",
            null,
            "foo",
            null,
            "bar",
            null,
        };
        private static readonly List<DummyPerson> _dummyNullPersons = new()
        {
            null,
            null,
            null,
        };
        private static readonly List<DummyPerson> _dummyNullsAndPersons = new()
        {
            new DummyPerson
            {
                Id = 1,
                FirstName = "Max",
                LastName = "Mustermann",
                CompanyName = "Foo",
            },
            null,
            null,
            null,
            new DummyPerson
            {
                Id = 3,
                FirstName = "Jacqueline",
                LastName = "Fassbinder",
                CompanyName = "Bar",
            },
            null,
            null,
            new DummyPerson
            {
                Id = 7,
                FirstName = "Frerk",
                LastName = "Müller",
                CompanyName = "United Windmills",
            },
            null,
        };

        #endregion Test Data

        #region ToNonNullList for strings

        [Fact]
        public void StringsToNonNullListNullSource()
        {
            // Arrange
            IEnumerable<string> myCollection = null;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void StringsToNonNullListEmptySource()
        {
            // Arrange
            IEnumerable<string> myCollection = [];

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void StringsToNonNullListNullsAndEmptyStrings()
        {
            // Arrange
            IEnumerable<string> myCollection = _dummyNullsAndEmptyStrings;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void StringsToNonNullListNullsEmptyAndFilledStrings()
        {
            // Arrange
            IEnumerable<string> myCollection = _dummyNullsEmptyAndFilledStrings;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Collection(result,
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[4], item),
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[6], item),
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[8], item)
                );

            // Act
            result = result.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Collection(result,
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[4], item),
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[6], item),
                item => Assert.Equal(_dummyNullsEmptyAndFilledStrings[8], item)
            );
        }

        #endregion ToNonNullList for strings

        #region ToNonNullList for Generics

        [Fact]
        public void GenericsToNonNullListNullSource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = null;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void GenericsToNonNullListEmptySource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = [];

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void GenericsToNonNullListNullsAndEmptyStrings()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullPersons;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void GenericsToNonNullListNullsEmptyAndFilledStrings()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullsAndPersons;

            // Act
            var result = myCollection.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Collection(result,
                item => AssertDummyPerson(item, _dummyNullsAndPersons[0]),
                item => AssertDummyPerson(item, _dummyNullsAndPersons[4]),
                item => AssertDummyPerson(item, _dummyNullsAndPersons[7])
                );

            // Act
            result = result.ToNonNullList();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Collection(result,
                item => AssertDummyPerson(item, _dummyNullsAndPersons[0]),
                item => AssertDummyPerson(item, _dummyNullsAndPersons[4]),
                item => AssertDummyPerson(item, _dummyNullsAndPersons[7])
            );
        }

        private void AssertDummyPerson(DummyPerson resultDummyPerson, DummyPerson expectedDummyPerson)
        {
            Assert.Equal(resultDummyPerson.Id, expectedDummyPerson.Id);
            Assert.Equal(resultDummyPerson.FirstName, expectedDummyPerson.FirstName);
            Assert.Equal(resultDummyPerson.LastName, expectedDummyPerson.LastName);
            Assert.Equal(resultDummyPerson.CompanyName, expectedDummyPerson.CompanyName);
        }

        #endregion ToNonNullList for Generics

        #region JoinToString for strings

        [Fact]
        public void StringsJoinToStringNullSource()
        {
            // Arrange
            IEnumerable<string> myCollection = null;

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void StringsJoinToStringEmptySource()
        {
            // Arrange
            IEnumerable<string> myCollection = [];

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void StringsJoinToStringNullsAndEmptyStrings()
        {
            // Arrange
            IEnumerable<string> myCollection = _dummyNullsAndEmptyStrings;

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal(", ,  ,  \t \r \n ", result);

            // Act
            result = myCollection.ToNonNullList().JoinToString(", ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void StringsJoinToStringNullsEmptyAndFilledStrings()
        {
            // Arrange
            IEnumerable<string> myCollection = _dummyNullsEmptyAndFilledStrings;

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal(", ,  ,  \t \r \n ,  haha , , foo, , bar, ", result);

            // Act
            result = myCollection.ToNonNullList().JoinToString(", ");

            // Assert
            Assert.Equal(" haha , foo, bar", result);
        }

        [Fact]
        public void StringsJoinToStringNullsAndEmptyStringsNullSeparator()
        {
            // Arrange
            IEnumerable<string> myCollection = _dummyNullsAndEmptyStrings;

            // Act
            var result = myCollection.JoinToString(null);

            // Assert
            Assert.Equal("  \t \r \n ", result);

            // Act
            result = myCollection.ToNonNullList().JoinToString(null);

            // Assert
            Assert.Equal("", result);
        }

        #endregion JoinToString for strings

        #region JoinToString for Generics

        [Fact]
        public void GenericsJoinToStringNullSource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = null;

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringEmptySource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = [];

            // Act
            var result = myCollection.JoinToString(", ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringNullPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullPersons;

            // Act
            var result = myCollection.JoinToString("; ");

            // Assert
            Assert.Equal("; ; ", result);

            // Act
            result = myCollection.ToNonNullList().JoinToString("; ");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringNullsAndPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullsAndPersons;

            // Act
            var result = myCollection.JoinToString("; ");

            // Assert
            Assert.Equal(
                "Person: Max Mustermann, Id: 1, Company: Foo; ; ; ; Person: Jacqueline Fassbinder, Id: 3, Company: Bar; ; ; Person: Frerk Müller, Id: 7, Company: United Windmills; ",
                result);

            // Act
            result = myCollection.ToNonNullList().JoinToString("; ");

            // Assert
            Assert.Equal(
                "Person: Max Mustermann, Id: 1, Company: Foo; Person: Jacqueline Fassbinder, Id: 3, Company: Bar; Person: Frerk Müller, Id: 7, Company: United Windmills",
                result);
        }

        #endregion JoinToString for Generics


        #region JoinToString for Generics with null Select

        [Fact]
        public void GenericsJoinToStringWithNullSelectNullSource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = null;

            // Act
            var result = myCollection.JoinToString(", ", null);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithNullSelectEmptySource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = [];

            // Act
            var result = myCollection.JoinToString(", ", null);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithNullSelectNullPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullPersons;

            // Act
            var result = myCollection.JoinToString("; ", null);

            // Assert
            Assert.Equal("; ; ", result);

            // Act
            result = myCollection.ToNonNullList().JoinToString("; ", null);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithNullSelectNullsAndPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullsAndPersons;

            // Act
            var result = myCollection.JoinToString("; ", null);

            // Assert
            Assert.Equal(
                "Person: Max Mustermann, Id: 1, Company: Foo; ; ; ; Person: Jacqueline Fassbinder, Id: 3, Company: Bar; ; ; Person: Frerk Müller, Id: 7, Company: United Windmills; ",
                result);

            // Act
            result = myCollection.ToNonNullList().JoinToString("; ", null);

            // Assert
            Assert.Equal(
                "Person: Max Mustermann, Id: 1, Company: Foo; Person: Jacqueline Fassbinder, Id: 3, Company: Bar; Person: Frerk Müller, Id: 7, Company: United Windmills",
                result);
        }

        #endregion JoinToString for Generics null Select


        #region JoinToString for Generics with Select

        [Fact]
        public void GenericsJoinToStringWithSelectNullSource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = null;

            // Act
            var result = myCollection.JoinToString(", ", x => x.FirstName);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithSelectEmptySource()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = [];

            // Act
            var result = myCollection.JoinToString(", ", x => x.FirstName);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithSelectNullPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullPersons;

            // Act

            //NOTE: this just throws:
            Assert.Throws<NullReferenceException>(() => myCollection.JoinToString("; ", x => x.FirstName));

            // 1st method of avoiding NullReferenceException: 
            var result = myCollection.JoinToString("; ", x => x?.FirstName);

            // Assert
            Assert.Equal("; ; ", result);

            // Act
            // 2nd method of avoiding NullReferenceException: 
            result = myCollection.ToNonNullList().JoinToString("; ", x => x.FirstName);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenericsJoinToStringWithSelectNullsAndPersons()
        {
            // Arrange
            IEnumerable<DummyPerson> myCollection = _dummyNullsAndPersons;

            // Act

            //NOTE: this just throws:
            Assert.Throws<NullReferenceException>(() => myCollection.JoinToString("; ", x => x.FirstName));

            // 1st method of avoiding NullReferenceException: 
            var result = myCollection.JoinToString("; ", x => x?.FirstName);

            // Assert
            Assert.Equal(
                "Max; ; ; ; Jacqueline; ; ; Frerk; ",
                result);

            // Act
            // 2nd method of avoiding NullReferenceException: 
            result = myCollection.ToNonNullList().JoinToString("; ", x => x.FirstName);

            // Assert
            Assert.Equal(
                "Max; Jacqueline; Frerk",
                result);
        }

        #endregion JoinToString for Generics with Select
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using System.Collections;
using System.Collections.Generic;

namespace MJsNetExtensionsTest
{
    [TestClass]
    public class ThrowTest
    {
        #region Throw.If Tests
        [TestMethod]
        public void ThrowIfTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.If(false, "haha");

            // Assert:
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void ThrowIfTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha"));
        }

        [TestMethod]
        public void ThrowIfTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.If(false, null));
        }

        [TestMethod]
        public void ThrowIfTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.If(false, " "));
        }

        [TestMethod]
        public void ThrowIfTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.If(true, null));
        }

        [TestMethod]
        public void ThrowIfTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.If(true, " "));
        }


        [TestMethod]
        public void ThrowIfWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.If(false, "haha", "msg1");

            // Assert:
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void ThrowIfWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1"));
        }

        [TestMethod]
        public void ThrowIfWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1 {0}", null));
        }

        [TestMethod]
        public void ThrowIfWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1 {0} and {1}", null, null));
        }

        [TestMethod]
        public void ThrowIfWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ThrowIfWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ThrowIfWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.If(true, "haha", "msg1 {0}", param1));
        }
        #endregion Throw.If Tests

        #region Throw.IfNot Tests
        [TestMethod]
        public void ThrowIfNotTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.IfNot(true, "haha");

            // Assert:
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void ThrowIfNotTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha"));
        }

        [TestMethod]
        public void ThrowIfNotTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNot(true, null));
        }

        [TestMethod]
        public void ThrowIfNotTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNot(true, " "));
        }

        [TestMethod]
        public void ThrowIfNotTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNot(false, null));
        }

        [TestMethod]
        public void ThrowIfNotTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNot(false, " "));
        }


        [TestMethod]
        public void ThrowIfNotWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.IfNot(true, "haha", "msg1");

            // Assert:
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1"));
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1 {0}", null));
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1 {0} and {1}", null, null));
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ThrowIfNotWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Throw.IfNot(false, "haha", "msg1 {0}", param1));
        }
        #endregion Throw.IfNot Tests

        #region Throw.IfNullOrEmpty - IEnumerable Non-Generic Variant

        #region IEnumerable Subclasses for Test
        internal class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        /// <summary>
        /// Collection of Person objects. This class implements IEnumerable so that it can be used with ForEach syntax.
        /// Copied from: https://docs.microsoft.com/de-de/dotnet/api/system.collections.ienumerable?view=netframework-4.8
        /// </summary>
        internal class People : IEnumerable
        {
            public Person[] Folks { get; set; }

            // Implementation for the GetEnumerator method.
            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }

            internal PeopleEnum GetEnumerator()
            {
                return new PeopleEnum { Folks = this.Folks, };
            }
        }

        /// <summary>
        /// When you implement IEnumerable, you must also implement IEnumerator.
        /// Copied from: https://docs.microsoft.com/de-de/dotnet/api/system.collections.ienumerable?view=netframework-4.8
        /// </summary>
        internal class PeopleEnum : IEnumerator
        {
            public Person[] Folks { get; set; }

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public bool MoveNext()
            {
                position++;
                return (position < this.Folks.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current => Current;

            public Person Current
            {
                get
                {
                    try
                    {
                        return this.Folks[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
        #endregion IEnumerable Subclasses for Test

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest1_ExpectSuccess()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks =
                [
                    new Person {FirstName = "John", LastName = "Smith", },
                    new Person {FirstName = "Jim",  LastName = "Johnson", },
                    new Person {FirstName = "Sue",  LastName = "Rabon", },
                ],
            };
            IEnumerable param1 = peopleList;

            // Act:
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest2_ExpectException()
        {
            // Arrange:
            People peopleList = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(peopleList, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(param1, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest4_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = [],
            };
            IEnumerable param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(param1, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest5_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = [],
            };

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(peopleList, "haha"));
        }
        #endregion Throw.IfNullOrEmpty - IEnumerable Non-Generic Variant

        #region Throw.IfNullOrEmpty - IEnumerable<T>

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest1_ExpectSuccess()
        {
            // Arrange:
            List<Person> peopleList = new List<Person> 
            {
                new() {FirstName = "John", LastName = "Smith", },
                new() {FirstName = "Jim",  LastName = "Johnson", },
                new() {FirstName = "Sue",  LastName = "Rabon", },
            };
            IEnumerable<Person> param1 = peopleList;

            // Act:
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest2_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(peopleList, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable<Person> param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(param1, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest4_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable<Person> param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(param1, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest5_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(peopleList, "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest6_ExpectSuccess()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>
            {
                new() {FirstName = "John", LastName = "Smith", },
                new() {FirstName = "Jim",  LastName = "Johnson", },
                new() {FirstName = "Sue",  LastName = "Rabon", },
            };
            IEnumerable param1 = peopleList;

            // Act:
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest7_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(param1, "haha"));
        }
        #endregion Throw.IfNullOrEmpty - IEnumerable<T>

        #region Throw.IfNullOrEmpty - String

        [TestMethod]
        public void ThrowIfIfNullOrEmptyStringTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.IfNullOrEmpty("z", "haha");

            // Assert:
            Assert.AreEqual("z", ret);
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyStringTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty("", "haha"));
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyStringTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => Throw.IfNullOrEmpty(null, "haha"));
        }
        #endregion Throw.IfNullOrEmpty - String
    }
}

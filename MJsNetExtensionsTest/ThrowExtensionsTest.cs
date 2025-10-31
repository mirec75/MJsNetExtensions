using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using System.Collections;
using System.Collections.Generic;

namespace MJsNetExtensionsTest
{
    [TestClass]
    public class ThrowExtensionsTest
    {
        #region Extensions ThrowIf Tests
        [TestMethod]
        public void ExtensionsThrowIfTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = false.ThrowIf("haha");

            // Assert:
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void ExtensionsThrowIfTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => false.ThrowIf(null));
        }

        [TestMethod]
        public void ExtensionsThrowIfTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => false.ThrowIf(" "));
        }

        [TestMethod]
        public void ExtensionsThrowIfTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => true.ThrowIf(null));
        }

        [TestMethod]
        public void ExtensionsThrowIfTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => true.ThrowIf(" "));
        }


        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = false.ThrowIf("haha", "msg1");

            // Assert:
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1"));
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1 {0}", null));
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1 {0} and {1}", null, null));
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => true.ThrowIf("haha", "msg1 {0}", param1));
        }
        #endregion Extensions ThrowIf Tests

        #region Extensions ThrowIfNot Tests
        [TestMethod]
        public void ExtensionsThrowIfNotTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = true.ThrowIfNot("haha");

            // Assert:
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void ExtensionsThrowIfNotTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => true.ThrowIfNot(null));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => true.ThrowIfNot(" "));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => false.ThrowIfNot(null));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => false.ThrowIfNot(" "));
        }


        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = true.ThrowIfNot("haha", "msg1");

            // Assert:
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1"));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1 {0}", null));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1 {0} and {1}", null, null));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1 {0}", param1));
        }

        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => false.ThrowIfNot("haha", "msg1 {0}", param1));
        }
        #endregion Extensions ThrowIfNot Tests

        #region Extensions ThrowIfNullOrEmpty - IEnumerable Non-Generic Variant

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
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest1_ExpectSuccess()
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
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest2_ExpectException()
        {
            // Arrange:
            People peopleList = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => peopleList.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => param1.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest4_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = [],
            };
            IEnumerable param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => param1.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest5_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = [],
            };

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => peopleList.ThrowIfNullOrEmpty("haha"));
        }
        #endregion Extensions ThrowIfNullOrEmpty - IEnumerable Non-Generic Variant

        #region Extensions ThrowIfNullOrEmpty - IEnumerable<T>

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest1_ExpectSuccess()
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
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest2_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => peopleList.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable<Person> param1 = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => param1.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest4_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable<Person> param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => param1.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest5_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => peopleList.ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest6_ExpectSuccess()
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
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest7_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable param1 = peopleList;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => param1.ThrowIfNullOrEmpty("haha"));
        }
        #endregion Extensions ThrowIfNullOrEmpty - IEnumerable<T>

        #region Extensions ThrowIfNullOrEmpty - String

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyStringTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = "z".ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.AreEqual("z", ret);
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyStringTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => "".ThrowIfNullOrEmpty("haha"));
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyStringTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => ((string)null).ThrowIfNullOrEmpty("haha"));
        }
        #endregion Extensions ThrowIfNullOrEmpty - String
    }
}

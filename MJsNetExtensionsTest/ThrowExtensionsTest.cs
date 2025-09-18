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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsFalse(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfTest2_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfTest3_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIf(null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfTest4_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIf(" ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfTest5_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf(null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfTest6_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf(" ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }


        [TestMethod]
        public void ExtensionsThrowIfWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = false.ThrowIf("haha", "msg1");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsFalse(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf("haha", "msg1");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf("haha", "msg1 {0}", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIf("haha", "msg1 {0} and {1}", null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            true.ThrowIf("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            true.ThrowIf("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            true.ThrowIf("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsTrue(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotTest2_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfNotTest3_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIfNot(null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfNotTest4_ExpectException()
        {
            // Arrange:
            // Act:
            true.ThrowIfNot(" ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfNotTest5_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot(null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfNotTest6_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot(" ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }


        [TestMethod]
        public void ExtensionsThrowIfNotWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = true.ThrowIfNot("haha", "msg1");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsTrue(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot("haha", "msg1");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot("haha", "msg1 {0}", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            false.ThrowIfNot("haha", "msg1 {0} and {1}", null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            false.ThrowIfNot("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            false.ThrowIfNot("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExtensionsThrowIfNotWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            false.ThrowIfNot("haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Extensions ThrowIfNot Tests

        #region Extensions ThrowIfNullOrEmpty - IEnumerable Non-Generic Variant

        #region IEnumerable Subclasses for Test
        internal class Person
        {
            public string FirstName { get; set; } = null;
            public string LastName { get; set; } = null;
        }

        /// <summary>
        /// Collection of Person objects. This class implements IEnumerable so that it can be used with ForEach syntax.
        /// Copied from: https://docs.microsoft.com/de-de/dotnet/api/system.collections.ienumerable?view=netframework-4.8
        /// </summary>
        internal class People : IEnumerable
        {
            public Person[] Folks { get; set; } = null;

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
            public Person[] Folks { get; set; } = null;

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

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

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
                Folks = new Person[3]
                {
                    new Person {FirstName = "John", LastName = "Smith", },
                    new Person {FirstName = "Jim",  LastName = "Johnson", },
                    new Person {FirstName = "Sue",  LastName = "Rabon", },
                },
            };
            IEnumerable param1 = peopleList;

            // Act:
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest2_ExpectException()
        {
            // Arrange:
            People peopleList = null;

            // Act:
            peopleList.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable param1 = null;

            // Act:
            param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest4_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = new Person[0],
            };
            IEnumerable param1 = peopleList;

            // Act:
            param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableNonGenericTest5_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = new Person[0],
            };

            // Act:
            peopleList.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Extensions ThrowIfNullOrEmpty - IEnumerable Non-Generic Variant

        #region Extensions ThrowIfNullOrEmpty - IEnumerable<T>

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest1_ExpectSuccess()
        {
            // Arrange:
            List<Person> peopleList = new List<Person> 
            {
                new Person {FirstName = "John", LastName = "Smith", },
                new Person {FirstName = "Jim",  LastName = "Johnson", },
                new Person {FirstName = "Sue",  LastName = "Rabon", },
            };
            IEnumerable<Person> param1 = peopleList;

            // Act:
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest2_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = null;

            // Act:
            peopleList.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable<Person> param1 = null;

            // Act:
            param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest4_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable<Person> param1 = peopleList;

            // Act:
            param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest5_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();

            // Act:
            peopleList.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest6_ExpectSuccess()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>
            {
                new Person {FirstName = "John", LastName = "Smith", },
                new Person {FirstName = "Jim",  LastName = "Johnson", },
                new Person {FirstName = "Sue",  LastName = "Rabon", },
            };
            IEnumerable param1 = peopleList;

            // Act:
            var ret1 = peopleList.ThrowIfNullOrEmpty("haha");
            var ret2 = param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyIEnumerableGenericTest7_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable param1 = peopleList;

            // Act:
            param1.ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreEqual("z", ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyStringTest2_ExpectException()
        {
            // Arrange:
            // Act:
            "".ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionsThrowIfIfNullOrEmptyStringTest3_ExpectException()
        {
            // Arrange:
            // Act:
            ((string)null).ThrowIfNullOrEmpty("haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Extensions ThrowIfNullOrEmpty - String
    }
}

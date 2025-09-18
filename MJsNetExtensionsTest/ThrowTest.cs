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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsFalse(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(false, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(false, " ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, " ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }


        [TestMethod]
        public void ThrowIfWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.If(false, "haha", "msg1");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsFalse(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, "haha", "msg1");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, "haha", "msg1 {0}", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.If(true, "haha", "msg1 {0} and {1}", null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Throw.If(true, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Throw.If(true, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Throw.If(true, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsTrue(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNotTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(true, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNotTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(true, " ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNotTest5_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNotTest6_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, " ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }


        [TestMethod]
        public void ThrowIfNotWithMessageTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            var ret = Throw.IfNot(true, "haha", "msg1");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.IsTrue(ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, "haha", "msg1");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, "haha", "msg1 {0}", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest4_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNot(false, "haha", "msg1 {0} and {1}", null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest5_ExpectException()
        {
            // Arrange:
            string param1 = null;

            // Act:
            Throw.IfNot(false, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest6_ExpectException()
        {
            // Arrange:
            object param1 = null;

            // Act:
            Throw.IfNot(false, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowIfNotWithMessageTest7_ExpectException()
        {
            // Arrange:
            object param1 = "bar!";

            // Act:
            Throw.IfNot(false, "haha", "msg1 {0}", param1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Throw.IfNot Tests

        #region Throw.IfNullOrEmpty - IEnumerable Non-Generic Variant

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
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest1_ExpectSuccess()
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
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest2_ExpectException()
        {
            // Arrange:
            People peopleList = null;

            // Act:
            Throw.IfNullOrEmpty(peopleList, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable param1 = null;

            // Act:
            Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest4_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = new Person[0],
            };
            IEnumerable param1 = peopleList;

            // Act:
            Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableNonGenericTest5_ExpectException()
        {
            // Arrange:
            People peopleList = new People
            {
                Folks = new Person[0],
            };

            // Act:
            Throw.IfNullOrEmpty(peopleList, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Throw.IfNullOrEmpty - IEnumerable Non-Generic Variant

        #region Throw.IfNullOrEmpty - IEnumerable<T>

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest1_ExpectSuccess()
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
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest2_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = null;

            // Act:
            Throw.IfNullOrEmpty(peopleList, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest3_ExpectException()
        {
            // Arrange:
            IEnumerable<Person> param1 = null;

            // Act:
            Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest4_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable<Person> param1 = peopleList;

            // Act:
            Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest5_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();

            // Act:
            Throw.IfNullOrEmpty(peopleList, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest6_ExpectSuccess()
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
            var ret1 = Throw.IfNullOrEmpty(peopleList, "haha");
            var ret2 = Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreSame(peopleList, ret1);
            Assert.AreSame(param1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyIEnumerableGenericTest7_ExpectException()
        {
            // Arrange:
            List<Person> peopleList = new List<Person>();
            IEnumerable param1 = peopleList;

            // Act:
            Throw.IfNullOrEmpty(param1, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
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
            Assert.IsTrue(true); //NOTE: Exception not thrown
            Assert.AreEqual("z", ret);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyStringTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNullOrEmpty("", "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfIfNullOrEmptyStringTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Throw.IfNullOrEmpty(null, "haha");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }
        #endregion Throw.IfNullOrEmpty - String
    }
}

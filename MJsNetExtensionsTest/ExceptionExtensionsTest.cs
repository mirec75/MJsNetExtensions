using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;

namespace MJsNetExtensionsTest
{
    [TestClass]
    public class ExceptionExtensionsTest
    {
        [TestMethod]
        public void ExceptionFlattenTest1_ExpectSuccess()
        {
            // Arrange:
            Exception ex = null;

            // Act:
            var ret = ex.Flatten();

            // Assert:
            Assert.IsNotNull(ret);
            Assert.AreEqual(0, ret.Count());
        }

        [TestMethod]
        public void ExceptionGetMessagesTest1_ExpectSuccess()
        {
            // Arrange:
            Exception ex = null;

            // Act:
            var ret = ex.GetMessages();

            // Assert:
            Assert.IsNotNull(ret);
            Assert.AreEqual(0, ret.Count());
        }

        [TestMethod]
        public void ExceptionGetMessagesTest2_ExpectSuccess()
        {
            // Arrange:
            Exception ex = new AggregateException("I'm aggregated msg",
            [
                new WebException("Don't go to net now", new InvalidOperationException("So invalid", new DivideByZeroException("wanna reach infinity?!"))),
                new FileNotFoundException("It's simply good hidden", "foo.txt"),
                new Exception("Ga ga u lala!"),
            ]);

            // Act:
            var ret = ex.GetMessages();

            // Assert:
            Assert.IsNotNull(ret);
            Assert.AreEqual(6, ret.Count());
        }

        #region JoinMessages() Tests
        [TestMethod]
        public void ExceptionJoinMessagesTest1_ExpectSuccess()
        {
            // Arrange:
            Exception ex = null;

            // Act:
            var ret = ex.JoinMessages();

            // Assert:
            Assert.AreEqual(null, ret);
        }

        [TestMethod]
        public void ExceptionJoinMessagesTest2_ExpectSuccess()
        {
            // Arrange:
            Exception ex = new AggregateException("I'm aggregated msg",
            [
                new WebException("Don't go to net now", new InvalidOperationException("So invalid", new DivideByZeroException("wanna reach infinity?!"))),
                new FileNotFoundException("It's simply good hidden", "foo.txt"),
                new Exception("Ga ga u lala!"),
            ]);

            // Act:
            var ret = ex.JoinMessages();

            // Assert:
            Assert.AreEqual("I'm aggregated msg (Don't go to net now) (It's simply good hidden) (Ga ga u lala!) --> Don't go to net now --> So invalid --> wanna reach infinity?! --> It's simply good hidden --> Ga ga u lala!", ret);
        }

        [TestMethod]
        public void ExceptionJoinMessagesTest3_ExpectSuccess()
        {
            // Arrange:
            Exception ex = new ArgumentOutOfRangeException(
                "paramFefe",
                new AggregateException("I'm aggregated msg",
                [
                    new WebException("Don't go to net now", new InvalidOperationException("So invalid", new DivideByZeroException("wanna reach infinity?!"))),
                    new FileNotFoundException("It's simply good hidden", "foo.txt"),
                    new Exception("Ga ga u lala!"),
                ])
                );

            // Act:
            var ret = ex.JoinMessages();

            // Assert:
            Assert.AreEqual("paramFefe --> I'm aggregated msg (Don't go to net now) (It's simply good hidden) (Ga ga u lala!) --> Don't go to net now --> So invalid --> wanna reach infinity?! --> It's simply good hidden --> Ga ga u lala!", ret);
        }

        [TestMethod]
        public void ExceptionJoinMessagesTest4_ExpectSuccess()
        {
            // Arrange:
            var curCul = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;


            Exception catchedEx = null;
            try
            {
                try
                {
                    try
                    {
                        int foo = 0;
                        Console.WriteLine(5 / foo);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentOutOfRangeException("divisorOfX", ex);
                    }
                }
                catch (Exception ex)
                {
                    throw new AggregateException("aggreging it all", ex);
                }
            }
            catch (Exception ex)
            {
                catchedEx = ex;
            }


            // Act:
            var ret = catchedEx.JoinMessages();

            // Assert:
            //Assert.AreEqual("aggreging it all --> divisorOfX --> Es wurde versucht, durch 0 (null) zu teilen.", ret);
            Assert.AreEqual("aggreging it all (divisorOfX) --> divisorOfX --> Attempted to divide by zero.", ret);

            Thread.CurrentThread.CurrentCulture = curCul;
            Thread.CurrentThread.CurrentUICulture = curCul;
        }
        #endregion JoinMessages() Tests

        #region JoinMessagesWithTypes() Tests
        [TestMethod]
        public void ExceptionJoinMessagesWithTypesTest1_ExpectSuccess()
        {
            // Arrange:
            Exception ex = null;

            // Act:
            var ret = ex.JoinMessagesWithTypes();

            // Assert:
            Assert.AreEqual(null, ret);
        }

        [TestMethod]
        public void ExceptionJoinMessagesWithTypesTest2_ExpectSuccess()
        {
            // Arrange:
            Exception ex = new AggregateException("I'm aggregated msg",
            [
                new WebException("Don't go to net now", new InvalidOperationException("So invalid", new DivideByZeroException("wanna reach infinity?!"))),
                new FileNotFoundException("It's simply good hidden", "foo.txt"),
                new Exception("Ga ga u lala!"),
            ]);

            // Act:
            var ret = ex.JoinMessagesWithTypes();

            // Assert:
            Assert.AreEqual(
                "AggregateException: I'm aggregated msg (Don't go to net now) (It's simply good hidden) (Ga ga u lala!) --> WebException: Don't go to net now --> InvalidOperationException: So invalid --> DivideByZeroException: wanna reach infinity?! --> FileNotFoundException: It's simply good hidden --> Exception: Ga ga u lala!", 
                ret
                );
        }

        [TestMethod]
        public void ExceptionJoinMessagesWithTypesTest3_ExpectSuccess()
        {
            // Arrange:
            Exception ex = new ArgumentOutOfRangeException(
                "paramFefe",
                new AggregateException("I'm aggregated msg",
                [
                    new WebException("Don't go to net now", new InvalidOperationException("So invalid", new DivideByZeroException("wanna reach infinity?!"))),
                    new FileNotFoundException("It's simply good hidden", "foo.txt"),
                    new Exception("Ga ga u lala!"),
                ])
                );

            // Act:
            var ret = ex.JoinMessagesWithTypes();

            // Assert:
            Assert.AreEqual(
                "ArgumentOutOfRangeException: paramFefe --> AggregateException: I'm aggregated msg (Don't go to net now) (It's simply good hidden) (Ga ga u lala!) --> WebException: Don't go to net now --> InvalidOperationException: So invalid --> DivideByZeroException: wanna reach infinity?! --> FileNotFoundException: It's simply good hidden --> Exception: Ga ga u lala!",
                ret
                );
        }

        [TestMethod]
        public void ExceptionJoinMessagesWithTypesTest4_ExpectSuccess()
        {
            // Arrange:
            var curCul = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;


            Exception catchedEx = null;
            try
            {
                try
                {
                    try
                    {
                        int foo = 0;
                        Console.WriteLine(5 / foo);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentOutOfRangeException("divisorOfX", ex);
                    }
                }
                catch (Exception ex)
                {
                    throw new AggregateException("aggreging it all", ex);
                }
            }
            catch (Exception ex)
            {
                catchedEx = ex;
            }


            // Act:
            var ret = catchedEx.JoinMessagesWithTypes();

            //string zzz = catchedEx.ToString();
            //Assert.IsNotNull(zzz);

            ////var usCulture = new System.Globalization.CultureInfo("en-US");
            //zzz = string.Format(CultureInfo.InvariantCulture, "{0}", catchedEx);
            //Assert.IsNotNull(zzz);


            // Assert:
            bool equals =
                string.Equals("AggregateException: aggreging it all (divisorOfX) --> ArgumentOutOfRangeException: divisorOfX --> DivideByZeroException: Es wurde versucht, durch 0 (null) zu teilen.", ret, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("AggregateException: aggreging it all (divisorOfX) --> ArgumentOutOfRangeException: divisorOfX --> DivideByZeroException: Attempted to divide by zero.", ret, StringComparison.OrdinalIgnoreCase)
                ;
            Assert.IsTrue(equals);

            Thread.CurrentThread.CurrentCulture = curCul;
            Thread.CurrentThread.CurrentUICulture = curCul;
        }
        #endregion JoinMessagesWithTypes() Tests
    }
}

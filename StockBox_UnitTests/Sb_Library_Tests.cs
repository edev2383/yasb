using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Base.Types;

namespace StockBox_UnitTests
{


    [TestClass]
    public class Sb_Library_Tests
    {

        [TestMethod]
        public void Sb_Lib_01_Test()
        {
            var x = new OptionList<int>() { 1, 2, 3, 4, 5 };

            var found = x.Find(i => i > 3) as Some<int>;

            Assert.IsNotNull(found);
            Assert.IsFalse(found.IsNone);
            Assert.IsNotNull(found.Value);
            Assert.AreEqual(4, found.Value);
            Assert.IsInstanceOfType(found, typeof(Some<int>));

            var notfound = x.Find(i => i > 5);

            Assert.IsInstanceOfType(notfound, typeof(None));
            Assert.IsTrue(notfound.IsNone);
        }

        [TestMethod]
        public void Sb_Lib_02_Test()
        {
            var x = new OptionList<int>() { 1, 2, 3, 4, 5 };
            var indexExists = x[1];

            var indexDoesNotExists = x[5];

            Assert.IsInstanceOfType(indexExists, typeof(Some<int>));
            Assert.IsInstanceOfType(indexDoesNotExists, typeof(None));
        }

        [TestMethod]
        public void Sb_Lib_03_Test()
        {
            var x = new OptionList<int>() { 1, 2, 3, 4, 5, };
            x.SetAt(5, 6);
            x.SetAt(0, 45);

            Assert.AreEqual((x[5] as Some<int>).Value, 6);
            Assert.AreEqual((x[0] as Some<int>).Value, 45);
        }
    }
}

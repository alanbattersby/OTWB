using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Geometric_Chuck;
using System.Diagnostics;
using OTWB.CodeGeneration;
using OTWB.Settings;
using Windows.UI.Xaml.Data;
using OTWB;
using Windows.UI.Xaml.Controls;
using OTWB.Common;

namespace UnitTestLibrary1
{
    [TestClass]
    public class UnitTest1
    {
       

        [TestMethod]
        public void TestMethod1()
        {

            Rational r1 = new Rational(360,1);
            Rational r2 = new Rational(360, 7);
            Rational r3 = new Rational(360, 15);
            Rational r4 = new Rational(0.625, 3);
            Assert.AreEqual(new Rational(5, 8), r4);

            Rational rt = r2 * r3;
            Rational rd = r2 / r3;
            Rational rp = r2 + r3;
            Rational rm = r2 - r3;

            Debug.WriteLine("r1 as double is {0}", r1.ToDouble);
            Debug.WriteLine("r2 as double is {0}", r2.ToDouble);
            Debug.WriteLine("r2 * r3 is {0}", rt);
            Debug.WriteLine("r2 / r3 is {0}", rd);
            Debug.WriteLine("r2 + r3 is {0}", rp);
            Debug.WriteLine("r2 - r3 is {0}", rm);

            Rational r = Rational.LCM(r1, r2);
            Debug.WriteLine("LCM(r1,r2) is {0}",r);
            Rational rr = Rational.LCM(r, r3);
            Debug.WriteLine("LCM(r1,r2,r3) is {0}", rr);
            Debug.WriteLine(rr);
            Assert.AreEqual(r1, rr);
        }
    }

    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestQuadrant()
        {
            Assert.AreEqual(0, BasicLib.Quadrant(89.9));
            Assert.AreEqual(3, BasicLib.Quadrant(-89.9));

            Assert.AreEqual(1, BasicLib.Quadrant(179.9));
            Assert.AreEqual(2, BasicLib.Quadrant(-179.9));

            Assert.AreEqual(2, BasicLib.Quadrant(269.9));
            Assert.AreEqual(1, BasicLib.Quadrant(-269.9));

            Assert.AreEqual(3, BasicLib.Quadrant(719.9));
            Assert.AreEqual(0, BasicLib.Quadrant(-719.9));

            Assert.IsTrue(BasicLib.Quadrant3To0(719, 0));
            Assert.IsFalse(BasicLib.Quadrant0To3(719, 0));
        }
    }

    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void TestTemplateLookup()
        {
           
        }

        [TestMethod]
        public void TestBinding()
        {
          
        }
    }
}

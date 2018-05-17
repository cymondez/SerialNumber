using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerialNumber;
using SerialNumber.CommonNumerDisks;
using SerialNumber.CommonUtils;
using System.Linq;
using System.Text.RegularExpressions;

namespace SerialNumberUnitTest {
    [TestClass]
    public class DispenserExtensionUnitTest {

        private readonly string _testSerailNumberFormatString = @"TEST20180517-\c:'甲乙丙'\a\d\d";


        [TestMethod ,TestCategory(nameof(Dispenser))]
        public void DispenserToRegularTest () {
            Dispenser dispenser = FormatParser.Parse(_testSerailNumberFormatString);
            var expectedPattern = @"TEST20180517-[甲乙丙][A-Z]\d\d";
            var regex = dispenser.ToRegularExpression();
            var target = regex.ToString();
            
            Assert.AreEqual(expectedPattern , target);
            
        }

        [TestMethod ,TestCategory(nameof(Dispenser))]
        public void TestStartNumberSetting () {
            Dispenser dispenser = FormatParser.Parse(_testSerailNumberFormatString);
            var expectedSerialNumber = "TEST20180517-乙Y39";
            dispenser.SetStartNumber(expectedSerialNumber);

            var startSerialNumber = dispenser.First();
            Assert.AreEqual(expectedSerialNumber , startSerialNumber);

            var sencondSerialNumber = dispenser.Skip(1).First();
            Assert.AreEqual("TEST20180517-乙Y40" , sencondSerialNumber);
        }

        [TestMethod, TestCategory(nameof(Dispenser))]
        [ExpectedException( typeof( FormatException))]

        public void TestStartNumberSettingErrorFormat () {
            Dispenser dispenser = FormatParser.Parse(_testSerailNumberFormatString);
            var expectedSerialNumber = "TEST20180517-Y39";
            dispenser.SetStartNumber(expectedSerialNumber);
        }

    }
}

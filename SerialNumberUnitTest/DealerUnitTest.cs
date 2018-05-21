using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerialNumber;
using SerialNumber.CommonUtils;
namespace SerialNumberUnitTest {
    /// <summary>
    /// Dealer 的 單元測試類別 
    /// </summary>
    [TestClass]
    public class DealerUnitTest {

        [TestMethod ,TestCategory(nameof(Dealer))]
        public void DealerStartNumberTest () {
            var dealer = new Dealer(FormatParser.Parse(@"Test-\a\d"));
            dealer.Dispenser.SetStartNumber("Test-A9");

            Assert.AreEqual(null , dealer.Current());

            var firstNumber = dealer.Next();
            Assert.AreEqual("Test-A9" , firstNumber);
            Assert.AreEqual(1 , dealer.AlreadyDealCount);

            Assert.AreEqual(firstNumber , dealer.Current());


            var secondNumber = dealer.Next();
            Assert.AreEqual("Test-B0" , secondNumber);
            Assert.AreEqual(2 , dealer.AlreadyDealCount);

            Assert.AreEqual(secondNumber , dealer.Current());
        }

        [TestMethod, TestCategory(nameof(Dealer))]
        public void DealerResetTest () {
            var dealer = new Dealer(FormatParser.Parse(@"Test-\a\d"));
            dealer.Dispenser.SetStartNumber("Test-A9");



            var firstNumber = dealer.Next();
            var secondNumber = dealer.Next();

            Assert.AreEqual(2 , dealer.AlreadyDealCount);

            dealer.Reset();
            Assert.AreEqual(null , dealer.Current());
            Assert.AreEqual(0 , dealer.AlreadyDealCount);

        }
    }
}

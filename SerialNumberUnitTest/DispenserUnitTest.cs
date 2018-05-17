using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SerialNumber;
using SerialNumber.CommonNumerDisks;
namespace SerialNumberUnitTest {
    [TestClass]
    public class DispenserUnitTest {
        //Test-甲-A0 ~ Test-癸-Z9
        private Dispenser _dispenser = new Dispenser(
            new ConstantNumberDisk('T'),
            new ConstantNumberDisk('e'),
            new ConstantNumberDisk('s'),
            new ConstantNumberDisk('t'),
            new ConstantNumberDisk('-'),
            new CustomNumberDisk("Chinese Era", "甲乙丙丁戊己庚辛壬癸".ToCharArray()),
            new ConstantNumberDisk('-') ,
            new AlphaNumberDisk(),
            new DigitalNumberDisk()
            );

        [TestMethod ,TestCategory(nameof(Dispenser))]
        public void SerialNumberTotalCountTest () {
            var expectedCount = 10 * 26 * 10; // [Chinese Era] * [A-Z] * [0-9]
            var targetCount = _dispenser.TotalCount;
            Assert.AreEqual(expectedCount , targetCount);
         }

        [TestMethod, TestCategory(nameof(Dispenser))]
        public void SerialNumberCloneTest () {
            var cloneDispenser = _dispenser.Clone();

            var setableDisk =  cloneDispenser.SymbolDisks.OfType<SetableNumberDisk>().Last();
            setableDisk.Skip = 1;

            var expectedSerialNumber = cloneDispenser.First();
            Assert.AreEqual(expectedSerialNumber , "Test-甲-A1");

            Assert.AreNotEqual(cloneDispenser.Skip(2).First() , _dispenser.Skip(2).First());
        }
        [TestMethod, TestCategory(nameof(Dispenser))]
        public void SerialNumberRealCountTest () {
            var cloneDispenser = _dispenser.Clone();
            var setableDisks = cloneDispenser.SymbolDisks.OfType<SetableNumberDisk>().ToArray();
            setableDisks [0].Skip = 2;//start with '丙'
            setableDisks [1].Skip = 5;//start with 'F'
            setableDisks [2].Skip = 3;//start with '3'

            //start number is Test-丙-F3

            var totalCount = 10 * 26 * 10;
            var alreaySkipCount = 2 * 26 * 10 + 5 * 10 + 3;
            var expectedCount = totalCount - alreaySkipCount;
            var targetCount = cloneDispenser.Count;
            Assert.AreEqual(expectedCount , targetCount);
        }
    }
}

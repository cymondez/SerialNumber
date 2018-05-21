using System;
using System.Collections.Generic;
using System.Text;

namespace SerialNumber {

    /// <summary>
    /// 發牌機
    /// </summary>
    public class Dealer {

        private IEnumerator<string> _dispenserEnumerator = null;

        /// <summary>
        /// 已發出的數量
        /// </summary>
        public int AlreadyDealCount { get; private set; } = 0;

        /// <summary>
        /// 抽號機 <see cref="SerialNumber.Dealer">
        /// </summary>
        public Dispenser Dispenser { get; private set; }
        
        /// <summary>
        /// 抽號機的迭代器<see cref="Dispenser.Enumerator"/>
        /// </summary>
        private IEnumerator<string> DispenserEnumerator {
            get {
                if ( _dispenserEnumerator == null ) {
                    _dispenserEnumerator = Dispenser.GetEnumerator();
                }
                return _dispenserEnumerator;
            }
        }
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="dispenser">抽號機<see cref="SerialNumber.Dispenser"/><seealso cref="FormatParser"/></param>
        /// <param name="cloneSource">是否使用使用傳入的dispenser的副本:預設值為false</param>
        public Dealer(Dispenser dispenser ,bool cloneSource = false) {
            Dispenser src = ( dispenser ?? throw new ArgumentNullException(nameof(dispenser)) );
            this.Dispenser = cloneSource ? src.Clone() : src;
        }

        /// <summary>
        /// 重置發牌機
        /// </summary>
        public void Reset () {
            _dispenserEnumerator = null;
            AlreadyDealCount = 0;
        }

        /// <summary>
        /// 下一個號碼
        /// </summary>
        /// <returns></returns>
        public string Next () {
            var canMoveToNext =  DispenserEnumerator.MoveNext();
            if ( canMoveToNext ) {
                AlreadyDealCount++;
                return DispenserEnumerator.Current;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 當前發出的號碼
        /// </summary>
        /// <returns></returns>
        public string Current () {
            return DispenserEnumerator.Current;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialNumber.CommonNumerDisks {
    /// <summary>
    /// 常值符號
    /// </summary>
    public class ConstantNumberDisk : NumberDisk {

        private char [] _char = new char [1];

        public override int Length {
            get { return _char.Length; }
        }

        public override string Name {
            get { return string.Format("Constant {0}", _char [0]); }
        }

        public ConstantNumberDisk ( char constSymbol ) {
            _char [0] = constSymbol;
        }

        public override IEnumerator GetEnumerator () {
            return _char.GetEnumerator();
        }

        /// <summary>
        /// 將 可跳號符號 轉成 常值符號 (受Skip值影響)
        /// </summary>
        /// <param name="canSkipSymbol"></param>
        /// <returns></returns>
        public static explicit operator ConstantNumberDisk ( SetableNumberDisk canSkipSymbol ) {
            var itr = canSkipSymbol.GetEnumerator();
            itr.MoveNext();
            var c = itr.Current.ToString().ToArray() [0];
            return new ConstantNumberDisk(c);
        }

        public override string ToString () {
            return _char [0].ToString();
        }

        public override NumberDisk Clone () {
            return new ConstantNumberDisk(_char [0]);
        }
    }
}

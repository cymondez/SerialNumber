using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SerialNumber.CommonUtils;

namespace SerialNumber.CommonNumerDisks {
    /// <summary>
    /// 大寫英文字母的數字盤
    /// </summary>
    public class AlphaNumberDisk : SetableNumberDisk {

        private readonly static char [] _alpha = Enumerable.Range(65, 26).Select(i => (Char) i).ToArray();

        protected override ICollection<char> Collection {
            get { return new ReadOnlyCollectionBuilder<char>(_alpha.AsEnumerable()); }
        }

        /// <summary>
        /// <see cref="NumberDisk.Name"/>
        /// </summary>
		public override string Name {
            get { return "Alpah"; }
        }

        public override IEnumerator GetEnumerator () {
            return _alpha.GetEnumerator().Skip(Skip, true);
        }

        public override NumberDisk Clone () {
            return new AlphaNumberDisk() { Skip = this.Skip };
        }
    }
}

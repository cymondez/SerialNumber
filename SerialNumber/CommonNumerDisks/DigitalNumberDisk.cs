using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SerialNumber.CommonUtils;

namespace SerialNumber.CommonNumerDisks {
    /// <summary>
    /// 十進位流水號
    /// </summary>
    public class DigitalNumberDisk : SetableNumberDisk {
        private readonly static char [] _digital = Enumerable.Range(48, 10).Select(i=>(char)i).ToArray();

        protected override ICollection<char> Collection {
            get { return new ReadOnlyCollectionBuilder<char>(_digital.AsEnumerable()); }
        }

        public override string Name {
            get { return "Digital"; }
        }

        public override IEnumerator GetEnumerator () {
            return _digital.GetEnumerator().Skip(Skip);
        }

        public override NumberDisk Clone () {
            return new DigitalNumberDisk() { Skip = this.Skip };
        }
    }
}

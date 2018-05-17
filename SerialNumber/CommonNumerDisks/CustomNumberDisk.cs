using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SerialNumber.CommonUtils;

namespace SerialNumber.CommonNumerDisks {
	/// <summary>
	/// 自訂類型的流水號
	/// </summary>
	public class CustomNumberDisk : SetableNumberDisk {

		private string _name;
		private List<char> _list;

		protected override ICollection<char> Collection {
			get { return new ReadOnlyCollectionBuilder<char>(_list); }
		}

		public override int Length {
			get { return _list.Count; }
		}

		public override string Name {
			get { return _name; }
		}
		public CustomNumberDisk( string name ,IEnumerable<char> enumrable ) {
			_name = name;
			_list = enumrable.ToList();
		}



		public override IEnumerator GetEnumerator() {
			return _list.GetEnumerator().Skip(Skip);
		}

        public override NumberDisk Clone () {
            return new CustomNumberDisk(this._name , this._list.ToList()) { Skip = this.Skip };
        }
    }
}

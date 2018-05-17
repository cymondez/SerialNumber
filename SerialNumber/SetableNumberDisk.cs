using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialNumber {
	/// <summary>
	/// 可選擇起始值的流水號(抽象類別)
	/// </summary>
	public abstract class SetableNumberDisk : NumberDisk {

		/// <summary>
		/// 取得 符號類型的集合 ex. : { 1 ,2 ,3 ,...... }
		/// </summary>
		protected abstract ICollection<char> Collection { get; }


		private int _skip = 0;
		/// <summary>
		/// 跳過符號的數量
		/// </summary>
		public virtual int Skip {
			get { return _skip; }
			set {
				if ( _skip != value ) {
					CheckSkipInRange(value);
					_skip = value;
				}
			}
		}
        
		public override int Length {
			get { return Collection.Count; }
		}

		/// <summary>
		/// 檢查 Skip的值 在合理範圍
		/// </summary>
		/// <param name="value"></param>
		protected virtual void CheckSkipInRange( int value ) {
            if (value < 0 || value >= Length)
            {
                throw new InvalidOperationException(string.Format("skip must be 0 ~ {0} !", Length - 1));
            }
		}

        internal IEnumerable<char> GetEnumerableSymbols () {
            return this.Collection;
        }

		public override string ToString() {
			return this.Collection.Cast<object>().ToList()[Skip].ToString();
		}
	}

}

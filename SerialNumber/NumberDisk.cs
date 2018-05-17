using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialNumber {
    /// <summary>
    /// 數字盤
    /// </summary>
    public abstract class NumberDisk : IEnumerable ,ICloneable{

        /// <summary>
        /// 數字盤上的符號總數
        /// </summary>
		public abstract int Length { get; }

        /// <summary>
        /// 數字盤的名稱
        /// </summary>
		public abstract string Name { get; }

        protected NumberDisk () {
        }

        #region IEnumerable 成員
        public abstract System.Collections.IEnumerator GetEnumerator ();
        public abstract NumberDisk Clone ();
        object ICloneable.Clone () {
            return this.Clone();
        }
        #endregion
    }
}

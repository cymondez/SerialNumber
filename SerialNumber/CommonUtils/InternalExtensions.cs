using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace SerialNumber.CommonUtils {
    internal static class InternalExtensions {
        public static IEnumerator Skip ( this IEnumerator itr, int skip, bool cycle = false ) {
            if ( skip <= 0 ) {
                return itr;
            }

            for ( var i = 0; i < skip; i++ ) {
                if ( !itr.MoveNext() ) {
                    if ( !cycle ) {
                        throw new IndexOutOfRangeException();
                    }
                    itr.Reset();
                    itr.MoveNext();
                }
            }//end for

            return itr;
        }
        /// <summary>
		/// 將 List 中的兩個元素交換位置
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="firstIndex"></param>
		/// <param name="secondIndex"></param>
		public static void Swap<T> ( this IList<T> list, int firstIndex, int secondIndex ) {
            Contract.Requires(list != null);
            Contract.Requires(firstIndex >= 0 && firstIndex < list.Count);
            Contract.Requires(secondIndex >= 0 && secondIndex < list.Count);
            if ( firstIndex == secondIndex ) {
                return;
            }
            T temp = list [firstIndex];
            list [firstIndex] = list [secondIndex];
            list [secondIndex] = temp;
        }
    }
}

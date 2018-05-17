using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

using SerialNumber.CommonUtils;

namespace SerialNumber {
    /// <summary>
    /// 抽號機
    /// </summary>
    public class Dispenser : IEnumerable<string> ,ICloneable {

        private List<NumberDisk> _symbolDiskList = new List<NumberDisk>();
        public NumberDisk [] SymbolDisks {
            get { return _symbolDiskList.ToArray(); }
        }

        /// <summary>
        /// 此格式可以產生的流水號總數
        /// </summary>
        public long TotalCount {
            get { return _symbolDiskList.Select(s => (long) s.Length).Aggregate(( c1, c2 ) => c1 * c2); }
        }

        /// <summary>
        /// 流水號產生的數量 (受到 CanSkipSymbol 的 Skip值 影響)
        /// </summary>
        public long Count {
            get { return TotalCount - GetAlreaySkipCount(); }
        }

        public Dispenser () {

        }
        public Dispenser ( params NumberDisk [] disks ) {
            if ( disks == null || disks.Length == 0 ) {
                throw new ArgumentException($"{nameof(disks)} cannot be null or empty");
            }
            _symbolDiskList.AddRange(disks.Cast<NumberDisk>());
        }

        /// <summary>
        /// 加入符號至尾端
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Dispenser Append ( NumberDisk symbol ) {
            _symbolDiskList.Add(symbol);
            return this;
        }

        public Dispenser Insert ( int index, NumberDisk symbol ) {
            _symbolDiskList.Insert(index, symbol);
            return this;
        }

        public int IndexOf ( NumberDisk symbol ) {
            return _symbolDiskList.IndexOf(symbol);
        }

        /// <summary>
        /// 刪除 符號
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveAt ( int idx ) {
            _symbolDiskList.RemoveAt(idx);
        }

        /// <summary>
        /// 刪除 符號
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool Remove ( NumberDisk symbol ) {
            return _symbolDiskList.Remove(symbol);
        }

        /// <summary>
        /// 清空符號
        /// </summary>
        public void Clear () {
            _symbolDiskList.Clear();
        }

        /// <summary>
        /// 交換兩個符號的位置 (可與集合外的符號做交換)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public bool Swap ( NumberDisk src, NumberDisk dest ) {
            int srcIdx = IndexOf(src);
            if ( srcIdx < 0 ) {
                return false;
            }
            int destIdx = IndexOf(dest);
            if ( destIdx < 0 ) {
                _symbolDiskList [srcIdx] = dest;
            }
            else {
                _symbolDiskList.Swap(srcIdx, destIdx);
            }
            return true;
        }

        /// <summary>
        /// 取得 進位單位值
        /// </summary>
        /// <param name="lastIdx"></param>
        /// <returns></returns>
        private long GetUnit ( int lastIdx ) {
            if ( lastIdx == 0 ) {
                return 1;
            }
            else {
                var reverse = _symbolDiskList.Reverse<NumberDisk>().ToArray();
                return GetUnit(lastIdx - 1) * reverse [lastIdx - 1].Length;
            }
        }

        /// <summary>
        /// 取得已經跳過的流水號總數
        /// </summary>
        /// <returns></returns>
        private long GetAlreaySkipCount () {
            long skipNum = 0;
            var reverse = _symbolDiskList.Reverse<NumberDisk>().ToArray();
            for ( int i = 0; i < _symbolDiskList.Count; i++ ) {
                var symbol = reverse [i] as SetableNumberDisk;
                if ( symbol != null ) {
                    skipNum += GetUnit(i) * symbol.Skip;
                }
            }
            return skipNum;
        }

        #region IEnumerable 成員

        public IEnumerator<string> GetEnumerator () {
            return new Enumerator(SymbolDisks.Select(sym => sym.GetEnumerator()).ToArray());
        }

        #endregion
        struct Enumerator : IEnumerator<string> {
            IEnumerator [] _iters;
            object _current;
            public Enumerator ( IEnumerator [] iters ) {
                _iters = new IEnumerator [iters.Length];
                Array.Copy(iters, _iters, iters.Length);
                _current = null;
                //Reset();
            }

            #region IEnumerator 成員
            /*注意！ .Net 的迭代器 大多是用 struct 實作 ，請注意值類操作與複製問題*/
            private bool MoveNext ( int idx ) {
                var itr = _iters [idx];//struct copy
                bool res = itr.MoveNext();
                _iters [idx] = itr;//set new struct value back
                return res;
            }
            private void Reset ( int idx ) {
                var itr = _iters [idx];
                itr.Reset();
                _iters [idx] = itr;
            }
            private bool Restart ( int idx ) {
                Reset(idx);
                return MoveNext(idx);
            }

            object IEnumerator.Current {
                get { return this.Current; }
            }

            public bool MoveNext () {

                if ( _current == null ) {
                    bool res = false;
                    for ( int i = 0; i < _iters.Length; i++ ) {

                        res |= MoveNext(i);
                    }
                    _current = _iters;
                    return res;
                }

                for ( int i = _iters.Length - 1; i > -1; i-- ) {

                    var canNext = MoveNext(i);
                    if ( canNext ) {
                        if ( i < _iters.Length - 1 ) {
                            for ( int j = i + 1; j < _iters.Length; j++ ) {
                                Restart(j);
                            }
                        }
                        _current = _iters;
                        return canNext;
                    }
                }
                return false;
            }

            public void Reset () {
                for ( int i = 0; i < _iters.Length; i++ ) {
                    Reset(i);
                }
            }

            #endregion

            #region IEnumerator<string> 成員

            public string Current {
                get { return GetItrsCurrentStringValue((IEnumerator []) _current); }
            }

            #endregion

            #region IDisposable 成員

            public void Dispose () {
            }

            #endregion

            string GetItrsCurrentStringValue ( IEnumerator [] itrs ) {
                StringBuilder sb = new StringBuilder();
                Array.ForEach(itrs, itr => {
                    if ( itr != null && itr.Current != null ) {
                        sb.Append(itr.Current);
                    }
                });
                return sb.ToString();
            }
        }



        #region IEnumerable 成員

        IEnumerator IEnumerable.GetEnumerator () {
            return this.GetEnumerator();
        }

        object ICloneable.Clone () {
            return this.Clone();
        }

        public Dispenser Clone() {
            var disks = SymbolDisks.Select(disk => disk.Clone()).ToArray();
            return new Dispenser(disks);
        }

        #endregion




    }

}

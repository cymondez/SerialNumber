using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SerialNumber.CommonUtils {
    using CommonNumerDisks;
    public static class DispenserExtensions {
        /// <summary>
        /// 取得符合抽號機的正規表示式
        /// </summary>
        /// <param name="dispenser"></param>
        /// <returns></returns>
        public static Regex ToRegularExpression (this Dispenser dispenser ) {
            if( dispenser is null ) {
                throw new ArgumentNullException(nameof(dispenser) , $"{nameof(dispenser)} cannott be null");
            }
            var pattern = string.Join("" , dispenser.SymbolDisks.Select(disk => disk.ToRegularFragmentString()));
            return new Regex(pattern);
        }

        private static string ToRegularFragmentString (this NumberDisk numberDisk){
            switch ( numberDisk ) {
                case ConstantNumberDisk constNumberDisk :
                    return constNumberDisk.ToString();

                case DigitalNumberDisk digiNumberDisk :
                    return @"\d";

                case AlphaNumberDisk alphaNumberDisk :
                    return @"[A-Z]";

                case CustomNumberDisk customNumberDisk: {
                    var symbolCharAry = customNumberDisk.GetEnumerableSymbols().Cast<char>().ToArray();
                    return $"[{new string(symbolCharAry)}]";
                }

                case SetableNumberDisk setableDisk: {
                    var symbolCharAry = setableDisk.GetEnumerableSymbols().Cast<char>().ToArray();
                    return $"[{new string(symbolCharAry)}]";
                }
                case var other:
                    return $"[{new string(other.Cast<object>().Select(o=>o.ToString()[0]).ToArray())}]";
            }
        }

        /// <summary>
        /// 設定抽號機的幾使號碼
        /// </summary>
        /// <param name="dispenser">抽號機</param>
        /// <param name="startSerialNumber">起始號碼</param>
        public static void SetStartNumber(this Dispenser dispenser ,string startSerialNumber ) {
            var regex = dispenser.ToRegularExpression();
            if ( !regex.IsMatch(startSerialNumber) ) {
                throw new FormatException($"{startSerialNumber} is not match this dispenser pattern !");
            }

            var setableDiskIndexes = dispenser.SymbolDisks
                                              .Select(( disk , index ) => (disk:disk as SetableNumberDisk, index))
                                              .Where(t => t.disk != null)
                                              .ToArray();

            foreach(var setableDisk in setableDiskIndexes ) {
                (SetableNumberDisk disk, var index) = setableDisk;
                var collection = disk.GetEnumerableSymbols().ToArray();
                var startIndex = Array.IndexOf(collection , startSerialNumber [index]);
                disk.Skip = startIndex;
            }
        }


        
    }
}

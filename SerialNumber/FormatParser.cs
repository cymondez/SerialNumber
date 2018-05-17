using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialNumber {
    using System.Data;
    using CommonNumerDisks;

    /// <summary>
    /// 將格式化字串轉成 抽號機
    ///		/a -> 大寫字母
    ///		/d -> 十進位
    ///		/c:'[char array]' -> 自訂的序列
    ///	<example>
    ///		var builder = SerialSymbolFormatParser.Parse("OZ-20160406-\c:'甲乙丙丁'\a\d")
    ///		( range :OZ-20160406-甲A0 ~ OZ-20160406-丁Z9 )
    /// </example>
    /// </summary>
    public class FormatParser {



        Lazy<Dispenser> _lazyDispenser = new Lazy<Dispenser>(true);

        // 受跳脫字元影響的符號        
        private readonly static char [] _slashSymbol = "adc\\".ToCharArray();


        private readonly Dictionary<char, Action<CharEnumerator>> _dictSlashSymbol = new Dictionary<char, Action<CharEnumerator>>();
        Dispenser Dispenser {
            get { return _lazyDispenser.Value; }
        }

        private string _context;

        public FormatParser ( string context ) {
            _context = context;
            _dictSlashSymbol ['\\'] = ( seq ) => Dispenser.Append(new ConstantNumberDisk('\\'));
            _dictSlashSymbol ['a'] = ( seq ) => CheckSkipAndAdd(seq, new AlphaNumberDisk());
            _dictSlashSymbol ['d'] = ( seq ) => CheckSkipAndAdd(seq, new DigitalNumberDisk());
            _dictSlashSymbol ['c'] = ( seq ) => {
                if ( !seq.MoveNext() || seq.Current != ':' ) {
                    throw new InvalidExpressionException("format is \\c:'[char array]'");
                }
                ParseColonSymbol(seq);
            };
        }

        /// <summary>
        /// 將格式化字串轉成 抽號機
        ///		/a -> 大寫字母
        ///		/d -> 十進位
        ///		/c:'[char array]' -> 自訂的序列
        ///	<example>
        ///		var builder = SerialSymbolFormatParser.Parse("OZ-20160406-\c:'甲乙丙丁'\a\d")
        ///		( range :OZ-20160406-甲A0 ~ OZ-20160406-丁Z9 )
        /// </example>
        /// </summary>
        public Dispenser Parse () {
            var charSequence = _context.GetEnumerator();

            ParseCharSymbol(charSequence);

            return Dispenser;
        }


        private void ParseCharSymbol ( CharEnumerator charSequence ) {
            while ( charSequence.MoveNext() ) {
                var charSymbol = charSequence.Current;

                switch ( charSymbol ) {
                    case '\\':
                        ParseDefinedSlashSymbol(charSequence);
                        break;
                    default:
                        Dispenser.Append(new ConstantNumberDisk(charSymbol));
                        break;
                }
            }

        }


        private void ParseDefinedSlashSymbol ( CharEnumerator charSequence ) {

            charSequence.MoveNext();
            var charSymbol = charSequence.Current;

            if ( !_dictSlashSymbol.Keys.Contains(charSymbol) ) {
                throw new InvalidExpressionException(
                    @"after symbol '\' must be {" + string.Join("、", _dictSlashSymbol.Keys) + "}");
            }

            _dictSlashSymbol [charSymbol](charSequence);

        }
        //針對 冒號(Colon)的處理
        private void ParseColonSymbol ( CharEnumerator charSequence ) {

            charSequence.MoveNext();
            var symbol = charSequence.Current;
            if ( symbol == '\'' ) {
                ParseApostropheSymbol(charSequence);
            }
        }

        private void ParseApostropheSymbol ( CharEnumerator charSequence ) {
            var list = new List<char>(100);
            char crnt = '\0';
            while ( charSequence.MoveNext() && ( crnt = charSequence.Current ) != '\'' ) {
                list.Add(charSequence.Current);
            }
            if ( crnt != '\'' ) {
                throw new InvalidExpressionException (
                       @"symbol '(Apostrophe) must be pair like '[char array]' ");
            }
            CheckSkipAndAdd(charSequence, new CustomNumberDisk("custom", list));
        }

        private char? PeekNextChar ( CharEnumerator charSequence ) {
            var clone = (CharEnumerator) charSequence.Clone();
            var canMove = clone.MoveNext();
            return canMove ? (char?) clone.Current : null;
        }

        private void ParseBigParanthesesPair ( CharEnumerator charSequence, SetableNumberDisk symbol ) {
            var c = PeekNextChar(charSequence);
            if ( c == null || c != '{' ) {
                return;
            }
            var list = new List<char>();
            charSequence.MoveNext();
            while ( charSequence.MoveNext() && charSequence.Current != '}' ) {
                list.Add(charSequence.Current);
            }
            int number = -1;
            try {
                number = int.Parse(new string(list.ToArray()));
            }
            catch {
                throw new InvalidExpressionException(
                       @"in  Big Parantheses block must be a number for skip , ex :\d{3}");
            }

            symbol.Skip = number;

        }
        private void CheckSkipAndAdd ( CharEnumerator charSequence, SetableNumberDisk symbol ) {
            ParseBigParanthesesPair(charSequence, symbol);
            Dispenser.Append(symbol);
        }
        /// <summary>
        /// 將格式化字串轉成 抽號機
        ///		/a -> 大寫字母
        ///		/d -> 十進位
        ///		/c:'[char array]' -> 自訂的序列
        ///	<example>
        ///		var builder = SerialSymbolFormatParser.Parse("OZ-20160406-\c:'甲乙丙丁'\a\d")
        ///		( range :OZ-20160406-甲A0 ~ OZ-20160406-丁Z9 )
        /// </example>
        /// </summary>
        public static Dispenser Parse ( string context ) {
            return new FormatParser(context).Parse();
        }
    }
}

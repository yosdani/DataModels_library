using CommonTypes.Util.RegExp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonTypes.Util.Password;
using CommonTypes.Util.Culture;
using Datamodels.Utils;
namespace Datamodels.Extentions
{
    public static class Extensions
    {
        private static readonly Lazy<RandomSecureVersion> RandomSecure = new Lazy<RandomSecureVersion>(() => new RandomSecureVersion());

        public static bool _IsValid(this double value) => !double.IsNaN(value) && !double.IsInfinity(value);

        public static string _RemoveDiacritics(this string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder(capacity: normalizedString.Length);
            UnicodeCategory unicodeCategory;
            char c;
            for (int i = 0; i < normalizedString.Length; i++)
            {
                unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c = normalizedString[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark && unicodeCategory != UnicodeCategory.SpacingCombiningMark && unicodeCategory != UnicodeCategory.EnclosingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string _Reverse(this string str)
        {
            char[] str_arr = str.ToCharArray();
            Array.Reverse(str_arr);
            return new string(str_arr);
        }

        public static IEnumerable<T> _ShuffleSecure<T>(this IEnumerable<T> source)
        {
            T[] sourceArray = source.ToArray();
            int randomIndex;
            for (int counter = 0; counter < sourceArray.Length; counter++)
            {
                randomIndex = RandomSecure.Value.Next(counter, sourceArray.Length);
                yield return sourceArray[randomIndex];
                sourceArray[randomIndex] = sourceArray[counter];
            }
        }

        public static int _MaxOrOne(this IEnumerable<int> source) => source.Any() ? source.Max() : 1;

        public static string _ShuffleTextSecure(this string source)
        {
            char[] shuffeldChars = source._ShuffleSecure().ToArray();
            return new string(shuffeldChars);
        }

        public static IQueryable<TResult> _LeftJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IQueryable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            Func<TOuter, TInner, TResult> method = resultSelector.Compile();
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (outerObj, inners) => new
            {
                outerObj,
                inners = inners.DefaultIfEmpty()
            }).SelectMany(a => a.inners.Select(innerObj => method(a.outerObj, innerObj)));
        }

        public static IEnumerable<TResult> _LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (outerObj, inners) => new
        {
            outerObj,
            inners = inners.DefaultIfEmpty()
        }).SelectMany(a => a.inners.Select(innerObj => resultSelector(a.outerObj, innerObj)));

        public static string _Trim(this Regex regex) => regex.ToString().TrimStart(RegexSupport.regexStart).TrimEnd(RegexSupport.regexEnd);

        public static Expression<Func<T, bool>> _AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            ParameterReplaceVisitor visitor = new ParameterReplaceVisitor()
            {
                Target = right.Parameters[0],
                Replacement = left.Parameters[0],
            };
            Expression rewrittenRight = visitor.Visit(right.Body);
            BinaryExpression andExpression = Expression.AndAlso(left.Body, rewrittenRight);
            return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
        }

        public static Expression<Func<T, bool>> _OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            ParameterReplaceVisitor visitor = new ParameterReplaceVisitor()
            {
                Target = right.Parameters[0],
                Replacement = left.Parameters[0],
            };
            Expression rewrittenRight = visitor.Visit(right.Body);
            BinaryExpression andExpression = Expression.OrElse(left.Body, rewrittenRight);
            return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
        }

        public static decimal? _NullSumOrDefault<T>(this IQueryable<T> source, Expression<Func<T, decimal?>> selector)
        {
            decimal? result = null;
            foreach (decimal? item in source.Select(selector))
            {
                if (item == null)
                    return null;
                if (result == null)
                    result = item;
                else
                    result += item.Value;
            }
            return result;
        }

        public static int? _MaxOrDefault<T>(this IQueryable<T> source, Expression<Func<T, int?>> selector)
        {
            int? result = null;
            foreach (int? item in source.Select(selector))
            {
                if (item == null)
                    return null;
                if (result == null || item > result)
                    result = item;
            }
            return result;
        }

        public static IEnumerable<IEnumerable<object>> _ToIEnumerable(this DataTable dt) => dt.Rows.OfType<DataRow>().Select(dr => dr.ItemArray);

        public static string _TextJoin<T>(this IEnumerable<T> ienum, string separator = ", ") => ienum == null ? string.Empty : string.Join(separator, ienum);

        public static string _CapitalizeFirstChar(this string text) => string.IsNullOrWhiteSpace(text) ? text : string.Concat(text[0].ToString().ToUpper(), text.AsSpan(1));

        public static string _Replace(this string text, string key, string replacement, RegexOptions options = RegexOptions.IgnoreCase, int? count = null)
        {
            Regex regex = new Regex(Regex.Escape(key), options);
            return count == null ? regex.Replace(text, replacement) : regex.Replace(text, replacement, count.Value);
        }

        public static double _Base64SizeBytes(this string textBase64) => textBase64 == null ? 0 : Math.Ceiling((double)textBase64.Length / 4) * 3;

        public static bool _Base64ToByteArray(this string textBase64, out byte[] arr)
        {
            if (!string.IsNullOrWhiteSpace(textBase64))
            {
                Span<byte> bytes = new byte[textBase64.Length];
                if (Convert.TryFromBase64String(textBase64, bytes, out int bytesWritten) && bytesWritten > 0)
                {
                    arr = bytes.Slice(0, bytesWritten).ToArray();
                    return true;
                }
            }
            arr = null;
            return false;
        }

        public static DateTime _GetWithoutTimeZone(this DateTime dt) => new DateTimeOffset(dt).DateTime;

        public static DateOnly _ToDateOnly(this DateTime dt) => DateOnly.FromDateTime(dt);

        public static DateOnly? _ToDateOnly(this DateTime? dt) => dt == null ? null : dt.Value._ToDateOnly();

        public static bool _IsNumeric(this object obj)
        {
            if (obj == null)
                return false;
            switch (obj)
            {
                case sbyte:
                case byte:
                case short:
                case ushort:
                case int:
                case uint:
                case long:
                case ulong:
                case float:
                case double:
                case decimal:
                    return true;
            }
            return double.TryParse(Convert.ToString(obj, CultureInfo.InvariantCulture), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double _);
        }

        public static bool _TryToInt(this byte[] barr, out int value)
        {
            try
            {
                value = BitConverter.ToInt32(barr);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public static bool _TryToDecimal(this object obj, out decimal value)
        {
            try
            {
                value = Convert.ToDecimal(obj, CultureSupport.en);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public static ushort[] _ToUShortArr(this byte[] source)
        {
            ushort[] result = new ushort[source.Length / 2];
            Buffer.BlockCopy(source, 0, result, 0, source.Length);
            return result;
        }

        public static int[] _ToInt32Arr(this byte[] source)
        {
            int[] result = new int[source.Length / 4];
            Buffer.BlockCopy(source, 0, result, 0, source.Length);
            return result;
        }

        public static byte[] _ToByteArr(this ushort[] source)
        {
            byte[] result = new byte[source.Length * 2];
            Buffer.BlockCopy(source, 0, result, 0, result.Length);
            return result;
        }

        public static byte[] _ToByteArr(this int[] source)
        {
            byte[] bytes = new byte[source.Length * 4];
            Buffer.BlockCopy(source, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static IEnumerable<IEnumerable<T>> _LazyBatch<T>(this IEnumerable<T> source, BigInteger size)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size));
            using (IEnumerator<T> enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, size);
        }

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, BigInteger size)
        {
            BigInteger count = 0;
            do
                yield return source.Current;
            while (++count < size && source.MoveNext());
        }
    }
}
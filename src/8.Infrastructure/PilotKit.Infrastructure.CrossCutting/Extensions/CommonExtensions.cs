// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonExtensions.cs" company="Company Capital LLC">
//   All Rights Reserved.
// </copyright>
// <summary>
//   Class contains generic extension methods for checking null or empty ,check whitespace and convert to CSV
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PilotKit.Infrastructure.CrossCutting.Extensions
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    #endregion

    /// <summary>
    ///     Class contains generic extension methods for checking null or empty ,check whitespace and convert to CSV
    /// </summary>
    public static class CommonExtensions
    {
        #region Public Methods and Operators

        private static int DefaultBufferSize = 80 * 1024;

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The
        ///     <see>
        ///         <cref>IList</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            items.ForEach(list.Add);
            return list;
        }

        /// <summary>
        /// The convert all.
        /// </summary>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        /// <param name="func">
        /// The func.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TOut">
        /// </typeparam>
        /// <returns>
        /// The
        ///     <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        /// </returns>
        public static IEnumerable<TOut> ConvertAll<T, TOut>(this IEnumerable<T> enumerable, Func<T, TOut> func)
        {
            foreach (var item in enumerable)
            {
                yield return func.Invoke(item);
            }
        }

        /// <summary>
        /// The exists.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Exists<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return new List<T>(source).Exists(predicate);
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Find<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return new List<T>(source).Find(predicate);
        }

        /// <summary>
        /// The find all.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// Returns list given object type
        /// </returns>
        public static IList<T> FindAll<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return new List<T>(source).FindAll(predicate);
        }

        /// <summary>
        /// The find last.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindLast<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return new List<T>(source).FindLast(predicate);
        }

        /// <summary>
        /// The for each.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (null != source)
            {
                foreach (T t in source)
                {
                    action(t);
                }
            }
        }

        /// <summary>
        /// Static method for check is null or empty
        /// </summary>
        /// <param name="value">
        /// pass string value
        /// </param>
        /// <returns>
        /// Boolean
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// The is null or has no.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrHasNo<T>(this List<T> values)
        {
            return values == null || !values.Any();
        }

        /// <summary>
        /// The is null or has no.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrHasNo<T>(this IList<T> values)
        {
            return values == null || !values.Any();
        }

        /// <summary>
        /// Static method for check is null or whitespace
        /// </summary>
        /// <param name="value">
        /// pass string value
        /// </param>
        /// <returns>
        /// Boolean
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// The is null or zero.
        /// </summary>
        /// <param name="val">
        /// The val.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrZero(this decimal? val)
        {
            return (val ?? 0) == 0;
        }

        /// <summary>
        /// The not null and has any.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NotNullAndHasAny<T>(this List<T> values)
        {
            return values != null && values.Any();
        }

        /// <summary>
        /// The not null and has any.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NotNullAndHasAny<T>(this IList<T> values)
        {
            return values != null && values.Any();
        }

        /// <summary>
        /// The round to multiple ofn.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="multiplesOf">
        /// The multiples of.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        public static double RoundToMultipleOfn(this double value, double multiplesOf)
        {
            return (Math.Round(value, 2) < 1) ? value : ((5 - value % 5) + value);
        }

        /// <summary>
        /// The sort.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void Sort<T>(this IEnumerable<T> source)
        {
            source.ToList().Sort();
        }

        /// <summary>
        /// Converts a string to a boolean
        /// </summary>
        /// <param name="value">
        /// The value to be converted
        /// </param>
        /// <returns>
        /// The converted <see cref="bool"/> value.
        /// </returns>
        public static bool ToBoolean(this string value)
        {
            bool parsedValue;
            return bool.TryParse(value, out parsedValue) ? parsedValue : false;
        }

        /// <summary>
        /// Static method to convert to CSV format
        /// </summary>
        /// <param name="values">
        /// list of string value
        /// </param>
        /// <returns>
        /// string
        /// </returns>
        public static string ToCsv(this IList<int> values)
        {
            return string.Join(",", values);
        }

        /// <summary>
        /// converts a culture dependent string representation into a date representation
        /// </summary>
        /// <param name="value">
        /// The culture dependent string representation of the date
        /// </param>
        /// <returns>
        /// The parsed date
        /// </returns>
        public static DateTime? ToDateTime(this string value)
        {
            DateTime parsedValue;
            return DateTime.TryParse(value, out parsedValue) ? parsedValue : (DateTime?)null;
        }

        /// <summary>
        /// Returns a delimited string of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">the collection of values to be concatenated</param>
        /// <param name="delimiter">The delimiter to be used for concatenation</param>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        public static string ToDelimiterSeparated<T>(this IList<T> values, string delimiter)
        {
            return string.Join(delimiter, values);
        }

        /// <summary>
        /// The to key value pair.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IList</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IList<KeyValuePair<string, string>> ToKeyValuePair(this IEnumerable<string> source)
        {
            return source.ConvertAll(s => new KeyValuePair<string, string>(s, s)).ToList();
        }

        /// <summary>
        /// The to key value pair.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IList</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IList<KeyValuePair<int, int>> ToKeyValuePair(this IEnumerable<int> source)
        {
            return source.ConvertAll(s => new KeyValuePair<int, int>(s, s)).ToList();
        }

        /// <summary>
        /// Builds a key value pair collection from a delimiter separated string
        /// </summary>
        /// <param name="source">
        /// The delimiter separated string
        /// </param>
        /// <param name="separator">
        /// The delimiter
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IList</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IList<KeyValuePair<string, string>> ToKeyValuePair(this string source, char separator)
        {
            return source.Split(separator).Where(i => (i.Length > 0)).Select(i => new KeyValuePair<string, string>(i, i)).ToList();
        }

        /// <summary>
        /// The to short date string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToShortDateString(this DateTime? value)
        {
            return (value == null) ? string.Empty : ((DateTime)value).ToShortDateString();
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToString(this DateTime? value, string format)
        {
            return (value == null) ? string.Empty : ((DateTime)value).ToString(format);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToString(this string value, string format)
        {
            return (value == null) ? string.Empty : string.Format("{0:C3}", value);
        }

        /// <summary>
        /// The to valid identifier.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToValidIdentifier(this string value)
        {
            return Regex.Replace(value, @"^[^a-zA-Z_]+|\W+", string.Empty);
        }

        /// <summary>
        /// Validates the short value and return null or actual value.
        /// </summary>
        /// <param name="value">
        /// pass short value.
        /// </param>
        /// <returns>
        /// short value
        /// </returns>
        public static short? ValidateShortIntValue(this short value)
        {
            if (value <= 0)
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// Validates the string value.
        /// </summary>
        /// <param name="value">
        /// pass string value.
        /// </param>
        /// <returns>
        /// string value
        /// </returns>
        public static string ValidateStringValue(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        /// <summary>
        /// Validates the string value and return boolean.
        /// </summary>
        /// <param name="value">
        /// pass string value.
        /// </param>
        /// <returns>
        /// Success or failure
        /// </returns>
        public static bool ValidateStringValueReturnBool(this string value)
        {
            return string.IsNullOrEmpty(value) ? false : Convert.ToBoolean(value);
        }

        /// <summary>
        /// Validates the string value and return guid.
        /// </summary>
        /// <param name="value">
        /// pass string value.
        /// </param>
        /// <returns>
        /// null or guid value
        /// </returns>
        public static Guid? ValidateStringValueReturnGuid(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Guid.Parse(value);
        }

        /// <summary>
        /// Validates the string value and return nullValue or date time.
        /// </summary>
        /// <param name="value">
        /// pass string value.
        /// </param>
        /// <returns>
        /// Date value or current date
        /// </returns>
        public static DateTime? ValidateStringValueReturnNullOrDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }


        public static string GetClaim(this ClaimsPrincipal principal, string claimType)
        {
            if (principal.HasClaim(c => c.Type == claimType))
            {
                Claim claim = principal.Claims.FirstOrDefault(c => c.Type == claimType);
                return claim.Value;
            }

            return string.Empty;
        }

        public static IList<string> GetClaims(this ClaimsPrincipal principal, string claimType)
        {
            if (principal.HasClaim(c => c.Type == claimType))
            {
                var claims = principal.Claims.Where(c => c.Type == claimType);
                return claims.Select(claim => claim.Value).ToList();
            }

            return new List<string>();
        }

        public static decimal? ToPercent(this decimal? value)
        {
            //return (value ?? 0m) * 100m;
            return value.HasValue ? (value * 100m) : null;
        }

        public static decimal? FromPercent(this decimal? value)
        {
            //return (value ?? 0m) / 100m;
            return value.HasValue ? (value / 100m) : null;
        }

        /// <summary>
        /// Asynchronously saves the contents of an uploaded file.
        /// </summary>
        /// <param name="formFile">The <see cref="IFormFile"/>.</param>
        /// <param name="filename">The name of the file to create.</param>
        public async static Task SaveAsAsync(this IFormFile formFile, string filename, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                var inputStream = formFile.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);
            }
        }
        #endregion
    }
}
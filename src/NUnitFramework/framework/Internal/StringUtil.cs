﻿using System;
using System.Globalization;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// Provides methods to support legacy string comparison methods.
    /// </summary>
    public class StringUtil
    {
        /// <summary>
        /// Compares two strings for equality, ignoring case if requested.
        /// </summary>
        /// <param name="strA">The first string.</param>
        /// <param name="strB">The second string..</param>
        /// <param name="ignoreCase">if set to <c>true</c>, the case of the letters in the strings is ignored.</param>
        /// <returns>Zero if the strings are equivalent, a negative number if strA is sorted first, a positive number if
        /// strB is sorted first</returns>
        public static int Compare(string strA, string strB, bool ignoreCase)
        {
            CompareOptions options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
#if NETCF
            return string.Compare(strA, strB, ignoreCase);
#elif CORECLR
            return string.Compare(strA, strB, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
#else
            return string.Compare(strA, strB, CultureInfo.CurrentCulture, options);
#endif
        }

        /// <summary>
        /// Compares two strings for equality, ignoring case if requested.
        /// </summary>
        /// <param name="strA">The first string.</param>
        /// <param name="strB">The second string..</param>
        /// <param name="ignoreCase">if set to <c>true</c>, the case of the letters in the strings is ignored.</param>
        /// <returns>True if the strings are equivalent, false if not.</returns>
        public static bool StringsEqual(string strA, string strB, bool ignoreCase)
        {
            return Compare(strA, strB, ignoreCase) == 0;
        }
    }
}
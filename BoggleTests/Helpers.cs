///////////////////////////////////////////////////////////////////////////////
// File: Helpers.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/12/2016
// Description: Helper utilities for testing
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoggleTests
{
    public static class Helpers
    {
        public static string ToEnumeratedString(this IEnumerable<string> Enumerable)
        {
            return Enumerable.Aggregate("[",(curr, next) => curr + "\"" + next + "\", ") + "]";
        }
        
        public static void AssertEnumeratorsAreEqual(IEnumerable<string> expected, IEnumerable<string> actual, string message)
        {
            var actualenum = actual.GetEnumerator();
            actualenum.MoveNext();
            foreach(string s in expected)
            {
                System.Diagnostics.Debug.Assert(String.Equals(s, actualenum.Current, StringComparison.InvariantCulture));
                actualenum.MoveNext();
            }
            if (!expected.SequenceEqual(actual))
            {
                Assert.Fail(String.Format("{0}\nExpected: {1}\nActual: {2}\n", message, expected.ToEnumeratedString(), actual.ToEnumeratedString()));
            }
        }
    }
}

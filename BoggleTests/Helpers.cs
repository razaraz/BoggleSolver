///////////////////////////////////////////////////////////////////////////////
// File: Helpers.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/12/2016
// Description: Helper utilities for testing
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (!expected.SequenceEqual(actual))
            {
                Assert.Fail(String.Format("{0}\nExpected: {1}\nActual: {2}\n", message, expected.ToEnumeratedString(), actual.ToEnumeratedString()));
            }
        }
    }
}

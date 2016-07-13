///////////////////////////////////////////////////////////////////////////////
// File: PerformanceTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/12/2016
// Description: Tests that measure performance of the boggle solver.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    [TestClass]
    public class PerformanceTests
    {

        const string largeDictionary = "Dictionaries\\EnglishDictionary1.txt";
        static readonly Dictionary<char, uint> testTileInfoLargeSet
            = new Dictionary<char, uint>
            {
                ['a'] = 2,
                ['b'] = 2,
                ['c'] = 2,
                ['d'] = 2,
                ['e'] = 2,
                ['f'] = 2,
                ['g'] = 2,
                ['h'] = 2,
                ['i'] = 2,
                ['j'] = 2,
                ['k'] = 2,
                ['l'] = 2,
                ['m'] = 2,
                ['n'] = 2,
                ['o'] = 2,
                ['p'] = 2,
                ['q'] = 2,
                ['r'] = 2,
                ['s'] = 2,
                ['t'] = 2,
                ['u'] = 2,
                ['v'] = 2,
                ['w'] = 2,
                ['x'] = 2,
                ['y'] = 2,
                ['z'] = 2
            };

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(largeDictionary, "Dictionaries")]
        public void LoadLargeDictionary()
        {
            DictionaryTree d = new DictionaryTree(largeDictionary, testTileInfoLargeSet, 1, false);

            foreach (DictNode n in d.RootNodes)
                Assert.IsNotNull(n);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void SolveLargeBoard()
        {
            // Maybe run the tree first, then save it in the testcontext, and then solve a large board?
            throw new NotImplementedException();
        }
    }
}

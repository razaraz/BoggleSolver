///////////////////////////////////////////////////////////////////////////////
// File: PerformanceTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/12/2016
// Description: Tests that measure performance of the boggle solver.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
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
        const uint largeDictionaryWordCount = 273193U;

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(largeDictionary, "Dictionaries")]
        [Timeout(10 * 1000)]
        public void LoadLargeDictionary()
        {
            DictionaryTree d = new DictionaryTree(largeDictionary, testTileInfoLargeSet, 1, false);

            Assert.AreEqual(d.WordCount, largeDictionaryWordCount, "The dictionary did not read the expected ammount of words");
        }

        const string minimalDict = "Dictionaries\\MinimalDict.txt";

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(minimalDict,"Dictionaries")]
        [Timeout(120 * 1000)]
        public void SolveMaxSizeBoardWithSmallDictionary()
        {
            char[] board = {
                't', 'M', 'm', 'm', 'M', 't', 't', 't',
                't', 'm', 'r', 't', 'a', 'r', 'r', 'm',
                'm', 'M', 'a', 't', 'a', 't', 'M', 't',
                'M', 'r', 'r', 'm', 'r', 'a', 't', 'r',
                'M', 'r', 't', 'm', 'M', 'M', 'M', 'm',
                'a', 'a', 't', 'm', 'M', 't', 't', 'm',
                'M', 'm', 'm', 'r', 'M', 't', 'r', 't',
                't', 'm', 'a', 'm', 'M', 'm', 'M', 'm'
            };

            string[] solutions = {
                "Mat", "am", "at", "arm", "art", "mart", "mt", "ra", "ram", "raam", "rat", "tar", "tram"};

            // Maybe run the tree first, then save it in the testcontext, and then solve a large board?
            Boggle b = new Boggle(8, 8, board);
            var testSolutions = from solution in b.Solve(minimalDict, 1, true) orderby solution ascending select solution;

            Assert.IsTrue(testSolutions.SequenceEqual(solutions));
        }
    }
}

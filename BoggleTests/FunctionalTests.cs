///////////////////////////////////////////////////////////////////////////////
// File: FunctionalTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Tests that verify the Boggle program gives the expected results
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    /// <summary>
    /// Summary description for FunctionalTests
    /// </summary>
    [TestClass]
    public class FunctionalTests
    {
        public FunctionalTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        const string minimalDict = "Dictionaries\\MinimalDict.txt";
        const string unicodeDict = "Dictionaries\\UnicodeDict.txt";
        const string englishDictionary2 = "Dictionaries\\EnglishDictionary2.txt";

        [TestMethod]
        [DeploymentItem(englishDictionary2, "Dictionaries")]
        public void ThreeByThreeBoard()
        {
            char[] board = {
                'y', 'o', 'x',
                'r', 'b', 'a',
                'v', 'e', 'd' };

            const uint boardWidth = 3;
            const uint boardHeight = 3;
            string[] solutions = {
                "bred", "yore", "byre", "abed", "oread",
                "bore", "orby", "robed", "broad", "byroad",
                "robe", "bored", "derby", "bade", "aero",
                "read", "orbed", "verb", "aery", "bead",
                "bread", "very", "road",
                // Extra words not found in the original solutions
                "abox", "boread", "daer", "rebox", "verby"};

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(englishDictionary2, 4) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions.OrderBy(s => s), testSolutions, "Solver failed to solve a board with complexity above minimum.");
        }

        
        [TestMethod]
        [DeploymentItem(unicodeDict,"Dictionaries")]
        public void UnicodeBoard()
        {
            char[] board = {
                '日', '今',
                'a', 'は' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "今日は" };

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(unicodeDict, 3) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "Solver failed to solve a board with unicode characters.");
        }

        [TestMethod]
        [DeploymentItem(minimalDict,"Dictionaries")]
        public void Duplicates()
        {
            char[] board = {
                'm', 'a',
                'a', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "arm", "raam", "ram",};

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(minimalDict, 3) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "Solver failed to properly dispose of duplicates.");
        }

        [TestMethod]
        public void NeighborsCalculatedProperly()
        {
            char[] board = {
                'y', 'o', 'x',
                'r', 'b', 'a',
                'v', 'e', 'd' };

            UInt64[] solutions = {
                0x1A,   0x3D,   0x32,
                0xD3,   0x1EF,  0x196,
                0x98,   0x178,  0xB0
                };

            Boggle b = new Boggle(3, 3, board);

            Helpers.AssertEnumeratorsAreEqual(solutions.Select(s => s.ToString("X")), b.NeighborArray.Select(s => s.ToString("X")), "Solver did not calculate the arrays for finding the neigbors properly.");
        }

        [TestMethod]
        [DeploymentItem(minimalDict,"Dictionaries")]
        public void MinimalBoard()
        {
            char[] board = {
                'm', 'a',
                't', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "arm", "art", "mart", "ram", "rat", "tar", "tram" };

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(minimalDict, 3) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "Solver did not find the expected solutions for the simplest case board.");
        }

        [TestMethod]
        public void TestTreeVisitor()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TraverseAllPossibleLetterCandidates()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [DeploymentItem(minimalDict,"Dictionaries")]
        public void CaseInsensitiveSensitiveMinimalBoard()
        {
            char[] board = {
                'M', 'a',
                't', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "arm", "art", "mart", "mat", "ram", "rat", "tar", "tram" };

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(minimalDict, 3, false) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "Board was not solved properly for case insensitivity.");
        }

        [TestMethod]
        [DeploymentItem(minimalDict,"Dictionaries")]
        public void CaseSensitiveMinimalBoard()
        {
            char[] board = {
                'M', 'a',
                't', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "art", "Mat", "rat", "tar"};

            Boggle b = new Boggle(boardWidth, boardHeight, board);

            var testSolutions = from solution in b.Solve(minimalDict, 3, true) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "Board was not solved properly for case sensitivity.");
        }
    }
}

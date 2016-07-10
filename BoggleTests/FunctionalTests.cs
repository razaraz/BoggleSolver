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

        [TestMethod]
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
                "bread", "very", "road"};

            Boggle b = new Boggle(boardWidth, boardHeight, board, 3);

            var testSolutions = from solution in b.Solve(minimalDict) orderby solution ascending select solution;

            Assert.IsTrue(testSolutions.SequenceEqual(solutions));
        }

        private static string minimalDict = "MinimalDict.txt";
        
        [TestMethod]
        public void UnicodeBoard()
        {
            char[] board = {
                '日', '今',
                'a', 'は' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "今日は" };

            Boggle b = new Boggle(boardWidth, boardHeight, board, 3);

            var testSolutions = from solution in b.Solve(minimalDict) orderby solution ascending select solution;

            Assert.IsTrue(testSolutions.SequenceEqual(solutions));
        }

        [TestMethod]
        public void MinimalBoard()
        {
            char[] board = {
                'm', 'a',
                't', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "arm", "art", "mart", "ram", "rat", "tar", "tram" };

            Boggle b = new Boggle(boardWidth, boardHeight, board, 3);

            var testSolutions = from solution in b.Solve(minimalDict) orderby solution ascending select solution;

            Assert.IsTrue(testSolutions.SequenceEqual(solutions));
        }

        [TestMethod]
        public void CaseSensitiveMinimalBoard()
        {
            char[] board = {
                'M', 'a',
                't', 'r' };
            const uint boardWidth = 2;
            const uint boardHeight = 2;
            string[] solutions = {
                "art", "Mat", "rat", "tar"};

            Boggle b = new Boggle(boardWidth, boardHeight, board, 3);

            var testSolutions = from solution in b.Solve(minimalDict) orderby solution ascending select solution;

            Assert.IsTrue(testSolutions.SequenceEqual(solutions));
        }
    }
}

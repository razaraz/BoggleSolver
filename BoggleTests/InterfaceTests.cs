///////////////////////////////////////////////////////////////////////////////
// File: InterfaceTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Tests that verify the interface works as expected.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    [TestClass]
    public class InterfaceTests
    {
        private static char[] board = {
            'y', 'o', 'x',
            'r', 'b', 'a',
            'v', 'e', 'd' };

        private static uint width = 3;
        private static uint height = 3;
        private static string[] solutions = {
            "bred", "yore", "byre", "abed", "oread",
            "bore", "orby", "robed", "broad", "byroad",
            "robe", "bored", "derby", "bade", "aero",
            "read", "orbed", "verb", "aery", "bead",
            "bread", "very", "road"};
        
        private static string invalidDict = "InvalidDict.txt";

        [TestMethod]
        public void EmptyBoard()
        {
            Boggle b = new Boggle(0, 0, new char[]{ }, 0);
            IEnumerable<string> solutions = b.Solve(minimalDict);

            Assert.AreEqual(solutions.Count(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Width and Height do not match the board size.")]
        public void WrongBoardDimentions()
        {
            Boggle b = new Boggle(width + 1, height, board);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum word length is greater than the total size of the board.")]
        public void MinWordLengthTooLong()
        {
            Boggle b = new Boggle(width, height, board, (uint)board.Length + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullBoard()
        {
            Boggle b = new Boggle(0, 0, null);
        }

        private static char[] minimalBoard = {
            'm', 'a',
            't', 'r' };
        private static uint minimalBoardWidth = 2;
        private static uint minimalBoardHeight = 2;
        private static string[] minimalSolutions = {
            "mat", "ram", "rat", "tram" };
        private static string minimalDict = "MinimalDict.txt";

        [TestMethod]
        public void MinimalBoard()
        {
            Boggle b = new Boggle(minimalBoardWidth, minimalBoardHeight, minimalBoard, 3);

            var solutions = from solution in b.Solve(minimalDict) orderby solution ascending select solution;

            Assert.IsTrue(solutions.SequenceEqual(minimalSolutions));
        }

        private static string nonexistentDictionary = "NonexistentDict.txt";

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void DictionaryDoesNotExist()
        {
            Boggle b = new Boggle(minimalBoardWidth, minimalBoardHeight, minimalBoard);

            b.Solve(nonexistentDictionary);
        }
    }
}

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
        private const string minimalDict = "Dictionaries\\MinimalDict.txt";

        [TestMethod]
        [TestCategory("Interface")]
        [DeploymentItem(minimalDict, "Dictionaries")]
        public void EmptyBoard()
        {
            Boggle b = new Boggle(0, 0, new char[]{ });
            IEnumerable<string> solutions = b.Solve(minimalDict, 0);

            Assert.IsNotNull(solutions);
            Assert.AreEqual(solutions.Count(), 0);
        }

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Width and Height do not match the board size.")]
        public void WrongBoardDimentions()
        {
            Boggle b = new Boggle(1, 1, new char[] { });
        }

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Boards larger than 64 spaces are not supported.")]
        public void BoardTooBig()
        {
            Boggle b = new Boggle(1, 65, new char[65]);
        }

        [TestMethod]
        [TestCategory("Interface")]
        [DeploymentItem(minimalDict, "Dictionaries")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum word length is greater than the total size of the board.")]
        public void MinWordLengthTooLong()
        {
            Boggle b = new Boggle(0, 0, new char[] { });

            b.Solve(minimalDict, 1);
        }

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullBoard()
        {
            Boggle b = new Boggle(0, 0, null);
        }


        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(System.IO.FileNotFoundException), "Could not open the dictionary file.")]
        public void DictionaryDoesNotExist()
        {
            string nonexistentDictionary = "NonexistentDict.txt";

            Boggle b = new Boggle(0, 0, new char[] { });

            b.Solve(nonexistentDictionary, 0);
        }
    }
}

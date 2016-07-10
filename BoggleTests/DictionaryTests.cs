﻿///////////////////////////////////////////////////////////////////////////////
// File: DictionaryTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Tests that verify the functionality of the Boggle Dictionary
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    /// <summary>
    /// Summary description for DictionaryTests
    /// </summary>
    [TestClass]
    public class DictionaryTests
    {
        public DictionaryTests()
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

        static readonly Boggle.TileInfo[] testTileInfo = {
            new Boggle.TileInfo( 'a', 2 ),
            new Boggle.TileInfo( 'a', 2 ),
            new Boggle.TileInfo( 'a', 2 ),
            new Boggle.TileInfo( 'a', 2 ),
            new Boggle.TileInfo( 'a', 2 ),
        };

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException), "Could not open the dictionary file.")]
        public void FileDoesNotExist()
        {
            string nonexistentDictionary = "NonexistentDict.txt";

            Dictionary d = new Dictionary(nonexistentDictionary, null);
        }
    }
}

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

        const string basicDict = "BasicDict.txt";

        #region Lower case
        static readonly Boggle.TileInfo[] testTileInfoLowerCase = {
            new Boggle.TileInfo( 'a', 2 ),
            new Boggle.TileInfo( 'b', 1 ),
            new Boggle.TileInfo( 'c', 1 ),
        };

        static readonly DictNode testDictTreeLowerCase
            = new DictNode
            (
                null,
                false,
                new DictNode[]
                {
                    new DictNode
                    (
                        'a',
                        true,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                true,
                                null
                            ),
                            new DictNode
                            (
                                'b',
                                true,
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'a',
                                        true,
                                        null
                                    )
                                }
                            )
                        }
                    ),
                    new DictNode
                    (
                        'b',
                        false,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                true,
                                null
                            )
                        }
                    )
                } 
            );

        #endregion

        #region Mixed Case
        static readonly Boggle.TileInfo[] testTileInfoMixedCase = {
            new Boggle.TileInfo( 'A', 1 ),
            new Boggle.TileInfo( 'a', 1 ),
            new Boggle.TileInfo( 'b', 1 ),
            new Boggle.TileInfo( 'c', 1 ),
        };

        static readonly DictNode testDictTreeMixedCase
            = new DictNode
            (
                (char)0,
                false,
                new DictNode[]
                {
                    new DictNode
                    (
                        'A',
                        true,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                true,
                                null
                            ),
                            new DictNode
                            (
                                'b',
                                false,
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'a',
                                        true,
                                        null
                                    )
                                }
                            )
                        }
                    ),
                    new DictNode
                    (
                        'a',
                        true,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'b',
                                true,
                                null
                            )
                        }
                    ),
                    new DictNode
                    (
                        'b',
                        false,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                true,
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'A',
                                        true,
                                        null
                                    )
                                }
                            )
                        }
                    )
                } 
            );
        #endregion

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException), "Could not open the dictionary file.")]
        public void DictionaryFileDoesNotExist()
        {
            string nonexistentDictionary = "NonexistentDict.txt";

            Dictionary d = new Dictionary(nonexistentDictionary, testTileInfoLowerCase);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.InvalidDataException), "The dictionary is not formatted in a proper format.")]
        public void DictionaryFileMalformed()
        {
            string malformedDictionary = "MalformedDict.txt";

            Dictionary d = new Dictionary(malformedDictionary, testTileInfoLowerCase);
        }

        static readonly string[] testBasicDictionaryWords;

        [TestMethod]
        public void DictionaryReadBasicDictionary()
        {
            Dictionary d = new Dictionary(basicDict, testTileInfoMixedCase, false);

            // Check to see if dictionaries are equal
            Assert.AreEqual(d.Root, testDictTreeMixedCase);
        }

        [TestMethod]
        public void DictionaryCaseSensitive()
        {
            Dictionary d = new Dictionary(basicDict, testTileInfoLowerCase, true);

            // Check to see if dictionaries are equal
            Assert.AreEqual(d.Root, testDictTreeLowerCase);
        }
    }
}
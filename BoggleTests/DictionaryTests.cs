﻿///////////////////////////////////////////////////////////////////////////////
// File: DictionaryTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Tests that verify the functionality of the Boggle DictionaryTree
///////////////////////////////////////////////////////////////////////////////
using System;
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

        const string basicDict = "Dictionaries\\BasicDict.txt";

        #region Lower case
        static readonly Dictionary<char, uint> testTileInfoLowerCase
            = new Dictionary<char, uint>
            {
                ['a'] = 2,
                ['b'] = 1,
                ['c'] = 1
            };

        static readonly DictionaryTree testDictTreeLowerCase
            = new DictionaryTree( new DictNode[]
                {
                    new DictNode
                    (
                        'a',
                        "a",
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                "aa",
                                null
                            ),
                            new DictNode
                            (
                                'b',
                                "ab",
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'a',
                                        "aba",
                                        null
                                    )
                                }
                            )
                        }
                    ),
                    new DictNode
                    (
                        'b',
                        null,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                "ba",
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'a',
                                        "baa",
                                        null
                                    )
                                }
                            )
                        }
                    )
                });

        #endregion

        #region Mixed Case
        static readonly Dictionary<char, uint> testTileInfoMixedCase
            = new Dictionary<char, uint>
            {
                ['A'] = 1,
                ['a'] = 1,
                ['b'] = 1,
                ['c'] = 1
        };

        static readonly DictionaryTree testDictTreeMixedCase
            = new DictionaryTree( new DictNode[]
                {
                    new DictNode
                    (
                        'A',
                        "A",
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                "Aa",
                                null
                            ),
                            new DictNode
                            (
                                'b',
                                null,
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'a',
                                        "Aba",
                                        null
                                    )
                                }
                            )
                        }
                    ),
                    new DictNode
                    (
                        'a',
                        "a",
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'b',
                                "ab",
                                null
                            )
                        }
                    ),
                    new DictNode
                    (
                        'b',
                        null,
                        new DictNode[]
                        {
                            new DictNode
                            (
                                'a',
                                "ba",
                                new DictNode[]
                                {
                                    new DictNode
                                    (
                                        'A',
                                        "baA",
                                        null
                                    )
                                }
                            )
                        }
                    )
                });
        #endregion

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(System.IO.FileNotFoundException), "Could not open the dictionary file.")]
        public void DictionaryFileDoesNotExist()
        {
            string nonexistentDictionary = "NonexistentDict.txt";

            DictionaryTree d = new DictionaryTree(nonexistentDictionary, testTileInfoLowerCase);
        }

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(System.IO.InvalidDataException), "The dictionary is not formatted in a proper format.")]
        public void DictionaryFileMalformed()
        {
            string malformedDictionary = "BoggleTests.dll";

            DictionaryTree d = new DictionaryTree(malformedDictionary, testTileInfoLowerCase);
        }

        [TestMethod]
        [DeploymentItem(basicDict, "Dictionaries")]
        public void DictionaryReadBasicDictionary()
        {
            DictionaryTree d = new DictionaryTree(basicDict, testTileInfoMixedCase, 1, true);

            // Check to see if dictionaries are equal
            Assert.AreEqual(testDictTreeMixedCase, d);
        }

        [TestMethod]
        [DeploymentItem(basicDict, "Dictionaries")]
        public void DictionaryCaseInsensitive()
        {
            DictionaryTree d = new DictionaryTree(basicDict, testTileInfoLowerCase, 1, false);

            // Check to see if dictionaries are equal
            Assert.AreEqual(testDictTreeLowerCase, d);
        }

        [TestMethod]
        [TestCategory("Interface")]
        [ExpectedException(typeof(NotSupportedException), "Only one DictNode Enumerator can be supported at this time")]
        [DeploymentItem(basicDict, "Dictionaries")]
        public void DictionaryCanOnlyHaveOneEnumarator()
        {
            DictionaryTree d = new DictionaryTree(basicDict, testTileInfoLowerCase, 1, false);

            var e0 = d.RootNodes.GetEnumerator();
            e0.MoveNext();
            var e1 = e0.Current.GetEnumerator();
            var e2 = e0.Current.GetEnumerator();
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
// File: DictionaryTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Tests that verify the functionality of the Boggle DictionaryTree
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

        const string basicDict = "Dictionaries\\BasicDict.txt";

        #region Lower case
        static readonly Dictionary<char, uint> testTileInfoLowerCase
            = new Dictionary<char, uint>
            {
                ['a'] = 2,
                ['b'] = 1,
                ['c'] = 1
            };

        static readonly DictNode[] testDictTreeLowerCase
            = new DictNode[]
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
                                null
                            )
                        }
                    )
                };

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

        static readonly DictNode[] testDictTreeMixedCase
            = new DictNode[]
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
                };
        #endregion

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException), "Could not open the dictionary file.")]
        public void DictionaryFileDoesNotExist()
        {
            string nonexistentDictionary = "NonexistentDict.txt";

            DictionaryTree d = new DictionaryTree(nonexistentDictionary, testTileInfoLowerCase);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.InvalidDataException), "The dictionary is not formatted in a proper format.")]
        public void DictionaryFileMalformed()
        {
            string malformedDictionary = "Dictionaries\\MalformedDict.txt";

            DictionaryTree d = new DictionaryTree(malformedDictionary, testTileInfoLowerCase);
        }

        static readonly string[] testBasicDictionaryWords;

        [TestMethod]
        public void DictionaryReadBasicDictionary()
        {
            DictionaryTree d = new DictionaryTree(basicDict, testTileInfoMixedCase, true);

            // Check to see if dictionaries are equal
            throw new NotImplementedException();
            //Assert.AreEqual(d.Root, testDictTreeMixedCase);
        }

        [TestMethod]
        public void DictionaryCaseInsensitive()
        {
            throw new NotImplementedException();
            DictionaryTree d = new DictionaryTree(basicDict, testTileInfoLowerCase, true);

            // Check to see if dictionaries are equal
            throw new NotImplementedException();
            //Assert.AreEqual(d.Root, testDictTreeLowerCase);
        }
    }
}

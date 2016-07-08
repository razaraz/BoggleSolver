using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    [TestClass]
    public class BasicFunctional
    {
        private static Char[] board = {
            'y', 'o', 'x',
            'r', 'b', 'a',
            'v', 'e', 'd' };

        private static uint width = 3;
        private static uint height = 3;
        private static String[] solutions = {
            "bred", "yore", "byre", "abed", "oread",
            "bore", "orby", "robed", "broad", "byroad",
            "robe", "bored", "derby", "bade", "aero",
            "read", "orbed", "verb", "aery", "bead",
            "bread", "very", "road"};

        [TestMethod]
        public void EmptyBoard()
        {
            Boggle b = new Boggle(0, 0, new Char[]{ });
            IEnumerable<String> solutions = b.Solve();

            Assert.AreEqual(solutions.Count<String>(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WrongDimentions()
        {
            Boggle b = new Boggle(width + 1, height, board);

            b.Solve();
        }
    }
}

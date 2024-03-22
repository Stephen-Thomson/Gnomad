using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelCompanionAPI.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI.Extras.Tests
{
    [TestClass()]
    public class UtilitiesTests //TODO: Not in untilities anymore. Rename?
    {
        [TestMethod()]
        public void parseFirstNameTest()
        {
            var full_name = "Bryce Schultz";
            var expected = "Bryce";

            var result = User.parseFirstName(full_name);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void parseLastNameTest()
        {
            var full_name = "Bryce Schultz";
            var expected = "Schultz";

            var result = User.parseLastName(full_name);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void parseFirstNameNoLastNameTest()
        {
            var full_name = "Bryce";
            var expected = "Bryce";

            var result = User.parseFirstName(full_name);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void parseLastNameNoLastNameTest()
        {
            var full_name = "Bryce";
            var expected = "";

            var result = User.parseLastName(full_name);

            Assert.AreEqual(expected, result);
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PeanutButter.RandomGenerators.Tests
{
    [TestFixture]
    public class TestEnumerableExtensions: TestBase
    {
        [Test]
        public void Randomize_OperatingOnEnumerableOfItems_ShouldGiveBackJumbledCollectionOfSameItems()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = new List<decimal>();
            for (var i = 0; i < RANDOM_TEST_CYCLES; i++)
            {
                var input = RandomValueGen.GetRandomCollection(() => RandomValueGen.GetRandomString(2, 10), 5, 10).ToArray();
                var jumbled = input.Randomize().ToArray();
                Assert.AreEqual(input.Length, jumbled.Length);
                var outOfPlace = 0;
                for (var j = 0; j < input.Length; j++)
                {
                    if (input[j] != jumbled[j])
                        outOfPlace++;
                }
                results.Add((decimal)outOfPlace / (decimal)input.Length);
            }

            //---------------Test Result -----------------------
            var average = results.Average();
            Assert.That(average, Is.GreaterThan(0.5));
        }

        [Test]
        public void Randomize_OperatingOnNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            List<int> ints = null;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = ints.Randomize();

            //---------------Test Result -----------------------
            Assert.IsNull(result);
        }

        [Test]
        public void Randomize_OperatingOnEmptyCollection_ShouldReturnEmptyCollection()
        {
            //---------------Set up test pack-------------------
            var ints = new List<int>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = ints.Randomize();

            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result);
        }


    }
}
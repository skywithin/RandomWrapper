using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RandomWrapper.UnitTests
{
    [TestClass]
    public class RandomWrapperTests
    {
        [TestMethod]
        public void ChooseRandomWeighted_ReturnsRandomObjectBasedOnWeight()
        {
            var listOfObjects = new List<WeightedTestObject>
            {
                new WeightedTestObject {Name = TestConst.Heavy, Weight = TestConst.WeightHeavy},
                new WeightedTestObject {Name = TestConst.Medium, Weight = TestConst.WeightMedium},
                new WeightedTestObject {Name = TestConst.Light, Weight = TestConst.WeightLight},
                new WeightedTestObject {Name = TestConst.Empty, Weight = TestConst.WeightNone},
            };

            int testRuns = 100;
            var stats = new Dictionary<string, int>();
            var rand = new RandomWrapper();

            for (int i = 0; i < testRuns; i++)
            {
                var selection = rand.ChooseRandomWeighted(listOfObjects);

                Assert.IsNotNull(selection);

                Debug.Print("Random select: {0}", selection.Name);

                if (stats.ContainsKey(selection.Name))
                {
                    stats[selection.Name] += 1;
                }
                else
                {
                    stats.Add(selection.Name, 1);
                }
            }

            foreach (var item in stats)
            {
                Debug.Print("{0}: {1}", item.Key, item.Value);
            }

            Assert.IsTrue(stats.ContainsKey(TestConst.Heavy));
            Assert.IsTrue(stats.ContainsKey(TestConst.Medium));
            Assert.IsTrue(stats.ContainsKey(TestConst.Light));
            Assert.IsTrue(stats.ContainsKey(TestConst.Empty) == false);

            Assert.IsTrue(stats[TestConst.Heavy] > stats[TestConst.Medium]);
            Assert.IsTrue(stats[TestConst.Medium] > stats[TestConst.Light]);
        }

        [TestMethod]
        public void ChooseRandomWeighted_WithZeroWeightedItemsAllowed_ReturnsAnObject()
        {
            var listOfObjects = new List<WeightedTestObject>
            {
                new WeightedTestObject {Name = TestConst.Empty, Weight = TestConst.WeightNone}
            };

            var selection = new RandomWrapper().ChooseRandomWeighted(listOfObjects, itemsWithZeroWeightCanBeSelected: true);

            Assert.IsNotNull(selection);
        }

        [TestMethod]
        public void ChooseRandomWeighted_WithZeroWeightedItemsNotAllowed_ReturnsNull()
        {
            var listOfObjects = new List<WeightedTestObject>
            {
                new WeightedTestObject {Name = TestConst.Empty, Weight = TestConst.WeightNone}
            };

            var selection = new RandomWrapper().ChooseRandomWeighted(listOfObjects, itemsWithZeroWeightCanBeSelected: false);

            Assert.IsNull(selection);
        }
    }

    class TestConst
    {
        public const string Heavy = "Heavy";
        public const string Medium = "Medium";
        public const string Light = "Light";
        public const string Empty = "Empty";

        public const int WeightHeavy = 50;
        public const int WeightMedium = 30;
        public const int WeightLight = 10;
        public const int WeightNone = 0;
    }

    class WeightedTestObject : IWeighted
    {
        public string Name { get; set; }

        public int Weight { get; set; }
    }
}

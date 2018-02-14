using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RandomWrapper
{
    public class RandomWrapper : IRandomWrapper
    {
        /// <summary>
        /// Gets a random number within specified range.
        /// </summary>
        /// <param name="minValue">Min value possibly returned.</param>
        /// <param name="maxValue">Value less than this will be returned.</param>
        /// <returns>Random number.</returns>
        public int GetRandomNumber(int minValue, int maxValue)
        {
            var seed = GetRandomSeed();
            return GetRandomNumber(seed, minValue, maxValue);
        }

        /// <summary>
        /// Gets a random number within specified range.
        /// </summary>
        /// <param name="seed">Seed to initialize random instance with.</param>
        /// <param name="minValue">Min value possibly returned.</param>
        /// <param name="maxValue">Value less than this will be returned.</param>
        /// <returns>Random number.</returns>
        public int GetRandomNumber(int seed, int minValue, int maxValue)
        {
            return new Random(seed).Next(minValue, maxValue);
        }

        public T ChooseRandomWeighted<T>(IEnumerable<T> list) where T : IWeighted
        {
            if (list == null || list.Any() == false)
            {
                return default(T);
            }

            var seed = GetRandomSeed();
            var rand = new Random(seed);
            int totalweight = list.Sum(c => c.Weight);
            int choice = rand.Next(totalweight);
            int sum = 0;

            foreach (var obj in list)
            {
                for (int i = sum; i < obj.Weight + sum; i++)
                {
                    if (i >= choice)
                    {
                        return obj;
                    }
                }
                sum += obj.Weight;
            }

            return list.First();
        }

        private int GetRandomSeed()
        {
            var regex = new Regex("[^0-9]");
            var randomStr = regex.Replace(Guid.NewGuid().ToString(""), "0"); // Replace non-numbers with zeros.
            var randomNum = long.Parse(randomStr.Substring(0, 17)); // Safeguard against overflow.
            return (int)randomNum;
        }
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomWrapper
{
    public class RandomWrapper : IRandomWrapper
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        /// <summary>
        /// Gets a random number within specified range.
        /// </summary>
        /// <param name="minValue">Min value possibly returned.</param>
        /// <param name="maxValue">Value less than this will be returned.</param>
        /// <returns>Random number.</returns>
        public int GetRandomNumber(int minValue, int maxValue)
        {
            lock (syncLock) // synchronize
            {
                return random.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// Gets a random number within specified range.
        /// </summary>
        /// <param name="maxValue">Value between 0 (incl) and less than this will be returned.</param>
        /// <returns>Random number.</returns>
        public int GetRandomNumber(int maxValue)
        {
            return GetRandomNumber(minValue: 0, maxValue: maxValue);
        }

        /// <summary>
        /// Randomly choose an item from a list taking into account item's weight. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">List of weighted object to choose from.</param>
        /// <param name="itemsWithZeroWeightCanBeSelected">If false, item with 0 weight will NEVER be selected.</param>
        /// <returns>Random item from a list taking into account its weight.</returns>
        public T ChooseRandomWeighted<T>(
            IEnumerable<T> items, 
            bool itemsWithZeroWeightCanBeSelected = false) where T : IWeighted
        {
            if (items == null || items.Any() == false)
            {
                // List is null or empty
                return default(T);
            }

            if (itemsWithZeroWeightCanBeSelected == false)
            {
                items = items.Where(p => p.Weight > 0);

                if (items.Any(p => p.Weight > 0) == false)
                {
                    return default(T);
                }
            }

            int totalWeight = items.Sum(c => c.Weight);
            int choice = GetRandomNumber(totalWeight);
            int sum = 0;

            foreach (var item in items)
            {
                for (int i = sum; i < item.Weight + sum; i++)
                {
                    if (i >= choice)
                    {
                        return item;
                    }
                }
                sum += item.Weight;
            }

            return items.OrderByDescending(p => p.Weight).First();
        }
    }
}

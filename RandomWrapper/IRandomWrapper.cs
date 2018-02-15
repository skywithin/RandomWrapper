using System.Collections.Generic;

namespace RandomWrapper
{
    public interface IRandomWrapper
    {
        int GetRandomNumber(int maxValue);

        int GetRandomNumber(int minValue, int maxValue);

        T ChooseRandomWeighted<T>(
            IEnumerable<T> items,
            bool itemsWithZeroWeightCanBeSelected = false) where T : IWeighted;
    }
}

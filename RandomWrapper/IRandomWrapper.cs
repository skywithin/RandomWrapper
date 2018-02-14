using System.Collections.Generic;

namespace RandomWrapper
{
    public interface IRandomWrapper
    {
        int GetRandomNumber(int minValue, int maxValue);

        int GetRandomNumber(int seed, int minValue, int maxValue);

        T ChooseRandomWeighted<T>(IEnumerable<T> list) where T : IWeighted;
    }
}

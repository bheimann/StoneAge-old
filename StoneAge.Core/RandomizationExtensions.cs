using System;
using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core
{
    public static class RandomizationExtensions
    {
        private static readonly Random _random = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                var randomPosition = _random.Next(list.Count());
                var temp = list[randomPosition];
                list[randomPosition] = list[i];
                list[i] = temp;
            }

            return list;
        }

        public static T ChooseAtRandom<T>(this IList<T> list)
        {
            var randomPosition = _random.Next(list.Count());

            return list[randomPosition];
        }

        public static T ChooseAtRandom<T>(this IEnumerable<T> list)
        {
            var randomPosition = _random.Next(list.Count());

            return list.ElementAt(randomPosition);
        }
    }
}

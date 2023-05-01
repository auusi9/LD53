using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class RandomNumberGenerator<T>
    {
        private readonly List<RandomItem<T>> _items = new List<RandomItem<T>>();
        private readonly System.Random _random = new System.Random();
            
        public void Add(double possibility, T item)
        {
            _items.Add(new RandomItem<T>(item, possibility));
        }
            
        public T NextItem()
        {
            var rand = _random.NextDouble() * _items.Sum(x => x.Possibility);
            double value = 0;
            foreach (var item in _items)
            {
                value += item.Possibility;
                if (rand <= value)
                {
                    return item.Item;
                }
            }
            return _items.Last().Item; // Should never happen
        }
    }
    
    [Serializable]
    public class RandomItem<T>
    {
        public T Item;
        public double Possibility;

        public RandomItem(T item, double possibility)
        {
            Item = item;
            Possibility = possibility;
        }
    }
}
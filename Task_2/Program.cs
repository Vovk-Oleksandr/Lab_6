using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericRepositoryDemo
{
    // Дженеричний клас Repository
    public class Repository<T>
    {
        private readonly List<T> _items;

        public Repository()
        {
            _items = new List<T>();
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public delegate bool Criteria<T>(T item);

        public IEnumerable<T> Find(Criteria<T> criteria)
        {
            return _items.Where(item => criteria(item));
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Repository<int> intRepository = new Repository<int>();
            intRepository.Add(10);
            intRepository.Add(20);
            intRepository.Add(30);
            intRepository.Add(40);

            // Критерій для пошуку чисел, більших за 25
            Repository<int>.Criteria<int> greaterThan25 = item => item > 25;

            Console.WriteLine("Елементи, більші за 25:");
            foreach (var item in intRepository.Find(greaterThan25))
            {
                Console.WriteLine(item);
            }

            Repository<string> stringRepository = new Repository<string>();
            stringRepository.Add("Pen");
            stringRepository.Add("Banana");
            stringRepository.Add("C");
            stringRepository.Add("Date");

            // Критерій для пошуку рядків, що починаються на 'B'
            Repository<string>.Criteria<string> startsWithB = item => item.StartsWith("B");

            Console.WriteLine("\nРядки, що починаються на 'B':");
            foreach (var item in stringRepository.Find(startsWithB))
            {
                Console.WriteLine(item);
            }
        }
    }
}

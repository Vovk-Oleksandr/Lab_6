using System;
using System.Collections.Generic;

namespace GenericFunctionCacheDemo
{
    // Дженеричний клас FunctionCache
    public class FunctionCache<TKey, TResult>
    {
        // Структура для збереження результату та часу його створення
        private class CacheEntry
        {
            public TResult Result { get; set; }
            public DateTime ExpiryTime { get; set; }
        }

        private readonly Dictionary<TKey, CacheEntry> _cache = new Dictionary<TKey, CacheEntry>();
        private readonly TimeSpan _cacheDuration;

        // Конструктор для ініціалізації терміну дії кешу
        public FunctionCache(TimeSpan cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        // Делегат для функцій
        public delegate TResult Func<TKey, TResult>(TKey key);

        // Метод для отримання результату з кешу або обчислення
        public TResult GetOrCalculate(TKey key, Func<TKey, TResult> function)
        {
            if (_cache.TryGetValue(key, out CacheEntry entry))
            {
                // Перевірка терміну дії
                if (DateTime.Now <= entry.ExpiryTime)
                {
                    return entry.Result;
                }
                _cache.Remove(key);
            }

            // Виконаннята збереження результату
            TResult result = function(key);
            _cache[key] = new CacheEntry
            {
                Result = result,
                ExpiryTime = DateTime.Now.Add(_cacheDuration)
            };

            return result;
        }

        // Метод для очищення
        public void Clear()
        {
            _cache.Clear();
        }
    }

    // Демонстрація роботи
    class Program
    {
        static void Main(string[] args)
        {
            // Ініціалізація кешу з терміном дії 10 секунд
            var cache = new FunctionCache<int, int>(TimeSpan.FromSeconds(10));

            FunctionCache<int, int>.Func<int, int> squareFunction = x =>
            {
                Console.WriteLine($"Обчислюю квадрат для {x}...");
                return x * x;
            };

            Console.WriteLine($"Результат: {cache.GetOrCalculate(5, squareFunction)}"); // Обчислення
            Console.WriteLine($"Результат: {cache.GetOrCalculate(5, squareFunction)}"); // З кешу

            // Очікування завершення терміну дії кешу
            Console.WriteLine("Очікую завершення терміну дії кешу...");
            System.Threading.Thread.Sleep(11000);

            Console.WriteLine($"Результат: {cache.GetOrCalculate(5, squareFunction)}"); // Повторне обчислення
        }
    }
}

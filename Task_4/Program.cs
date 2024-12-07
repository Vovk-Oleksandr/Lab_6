using System;
using System.Collections.Generic;

namespace GenericTaskSchedulerDemo
{
    // Дженеричний клас TaskScheduler
    public class TaskScheduler<TTask, TPriority> where TPriority : IComparable<TPriority>
    {
        // Черга завдань з пріоритетом
        private readonly SortedDictionary<TPriority, Queue<TTask>> _taskQueue = new();

        // Делегат для виконання завдання
        public delegate void TaskExecution<TTask>(TTask task);

        // Додавання завдання до черги з вказаним пріоритетом
        public void AddTask(TTask task, TPriority priority)
        {
            if (!_taskQueue.ContainsKey(priority))
            {
                _taskQueue[priority] = new Queue<TTask>();
            }
            _taskQueue[priority].Enqueue(task);
        }

        // Виконання наступного завдання з найвищим пріоритетом
        public void ExecuteNext(TaskExecution<TTask> executor)
        {
            if (_taskQueue.Count == 0)
            {
                Console.WriteLine("Черга завдань порожня.");
                return;
            }

            // Знаходимо пріоритет із найбільшим значенням
            var highestPriority = GetHighestPriority();
            var task = _taskQueue[highestPriority].Dequeue();

            // Видаляємо чергу, якщо вона порожня
            if (_taskQueue[highestPriority].Count == 0)
            {
                _taskQueue.Remove(highestPriority);
            }

            executor(task);
        }

        private TPriority GetHighestPriority()
        {
            foreach (var key in _taskQueue.Keys)
            {
                return key;
            }
            throw new InvalidOperationException("Черга завдань порожня.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо планувальник завдань
            var scheduler = new TaskScheduler<string, int>();

            // Делегат длявиконання завдань
            TaskScheduler<string, int>.TaskExecution<string> executor = task =>
            {
                Console.WriteLine($"Виконання завдання: {task}");
            };

            Console.WriteLine("Додавання завдань до планувальника.");
            while (true)
            {
                Console.WriteLine("Введіть завдання (або 'exit' для завершення):");
                var task = Console.ReadLine();
                if (task == "exit") break;

                Console.WriteLine("Введіть пріоритет завдання (ціле число):");
                if (int.TryParse(Console.ReadLine(), out var priority))
                {
                    scheduler.AddTask(task, priority);
                }
                else
                {
                    Console.WriteLine("Невірний формат пріоритету.");
                }
            }

            Console.WriteLine("\nВиконання завдань у порядку пріоритету:");
            while (true)
            {
                Console.WriteLine("Натисніть Enter для виконання наступного завдання або введіть 'exit' для завершення.");
                var command = Console.ReadLine();
                if (command == "exit") break;

                scheduler.ExecuteNext(executor);
            }
        }
    }
}
// Кожне завдання викликається натисканням Enter.
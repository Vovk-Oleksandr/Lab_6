using System;

namespace GenericCalculatorDemo
{
    // Дженеричний клас Calculator
    public class Calculator<T> where T : struct
    {
        // Делегати для основних арифметичних операцій
        public delegate T Operation(T a, T b);

        // Методи для виконання арифметичних операцій через делегати
        public T Add(T a, T b, Operation operation) => operation(a, b);
        public T Subtract(T a, T b, Operation operation) => operation(a, b);
        public T Multiply(T a, T b, Operation operation) => operation(a, b);
        public T Divide(T a, T b, Operation operation)
        {
            if (EqualityComparer<T>.Default.Equals(b, default(T)))
                throw new DivideByZeroException("Ділення на нуль неможливе.");
            return operation(a, b);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Інстанціювання калькулятора для типу int
            Calculator<int> intCalculator = new Calculator<int>();
            Calculator<int>.Operation intAdd = (a, b) => a + b;
            Calculator<int>.Operation intSubtract = (a, b) => a - b;
            Calculator<int>.Operation intMultiply = (a, b) => a * b;
            Calculator<int>.Operation intDivide = (a, b) => a / b;

            Console.WriteLine("Результати для типу int:");
            Console.WriteLine($"5 + 3 = {intCalculator.Add(5, 3, intAdd)}");
            Console.WriteLine($"5 - 3 = {intCalculator.Subtract(5, 3, intSubtract)}");
            Console.WriteLine($"5 * 3 = {intCalculator.Multiply(5, 3, intMultiply)}");
            Console.WriteLine($"5 / 3 = {intCalculator.Divide(5, 3, intDivide)}");

            // Інстанціювання калькулятора для типу double
            Calculator<double> doubleCalculator = new Calculator<double>();
            Calculator<double>.Operation doubleAdd = (a, b) => a + b;
            Calculator<double>.Operation doubleSubtract = (a, b) => a - b;
            Calculator<double>.Operation doubleMultiply = (a, b) => a * b;
            Calculator<double>.Operation doubleDivide = (a, b) => a / b;

            Console.WriteLine("\nРезультати для типу double:");
            Console.WriteLine($"5.5 + 3.2 = {doubleCalculator.Add(5.5, 3.2, doubleAdd)}");
            Console.WriteLine($"5.5 - 3.2 = {doubleCalculator.Subtract(5.5, 3.2, doubleSubtract)}");
            Console.WriteLine($"5.5 * 3.2 = {doubleCalculator.Multiply(5.5, 3.2, doubleMultiply)}");
            Console.WriteLine($"5.5 / 3.2 = {doubleCalculator.Divide(5.5, 3.2, doubleDivide)}");
        }
    }
}


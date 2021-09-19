using System;
using static System.Console;
using static System.Math;
using static System.Random;
using System.Collections.Generic;


// Консольное приложение, реализующее игру "Быки и Коровы". Приложение стремится поддерживать паттерн инкапсуляции.
namespace BullsAndCows
{
    class Program
    {

        // Метод, отвечающий за ввод количества разрядов в загаданном числе
        static void InputQuantity(out int number)
        {
            do
            {
                Console.Clear();
                Console.Write("Введите количество разрядов, которое будет в числе, которое вы хотите угадать. Оно может быть от 1 до 10: ");
            } while (!int.TryParse(ReadLine(), out number) || number < 1 || number > 10);
            Console.Clear();
        }




        // Метод, генерирующий случайное число
        static long InitializeNumber(int count)
        {
            const int maxSize = 10;

            List<int> generatedList = new List<int>();
            for (int i = 0; i < maxSize; ++i)
            {
                generatedList.Add(maxSize - i - 1);
            }
            shuffleList(ref generatedList, count);
            long value = 0;
            for (int i = 0; i < count; ++i)
            {
                value *= 10;
                value += generatedList[i];
            }
            return value;
        }
        // Случайное число генерируется с помощью заранее созданного списка, в котором каждый элемент (от 0 до 9 включительно) встречается 1 раз
        static void shuffleList(ref List<int> list, int count)
        {
            Random random = new Random();
            for (int i = 0; i < list.Count * list.Count * list.Count * list.Count; ++i)
            {
                int k = random.Next(list.Count);
                int p = random.Next(list.Count);
                (list[k], list[p]) = (list[p], list[k]);
            }
            if (count > 1 && list[0] == 0)
                (list[0], list[1]) = (list[1], list[0]);

        }




        // Метод, отвечающий за корректный ввод числа от пользователя на той стадии, когда тот уже пытается отгадать зашифрованное число
        static void InputUserNumber(ref long inputNumber, int count)
        {
            bool flag = true;
            while (flag)
            {
                Console.Write("Введите число, чтобы попытаться угадать количество коров и быков.\nУчтите, что число должно иметь такое же количество разрядов, которое вы ввели изначально,\n" +
                    "все цифры введённого числа должны быть различны и оно не может начинаться с нуля: ");
                string s = ReadLine();
                // число проверяется на соответствие количеству разрядов от введённого
                if (long.TryParse(s, out inputNumber) && inputNumber.ToString().Length == count && s.Length == count)
                {
                    const int size = 10;
                    // идёт проверка на одинаковые цифры в числе
                    int[] a = new int[size];
                    long copy = inputNumber; 
                    while (copy > 0)
                    {
                        a[copy % 10]++;
                        copy /= 10;
                    }
                    flag = false;
                    for (int i = 0; i < size; ++i)
                    {
                        if (a[i] > 1)
                            flag = true;
                    }
                }
                Console.Clear();
            }
            Console.Clear();
        }

        // Метод, сравнивающий число, введённое пользователем, и зашифрованное число
        static void CheckTheDifference(long inputNumber, long generatedNumber, ref int cows, ref int bulls)
        {
            cows = 0;
            bulls = 0;
            const int size = 10;
            int[] a = new int[size];
            for (int i = 1; inputNumber > 0; i++, inputNumber /= 10)
            {
                a[inputNumber % 10] = i;
            }
            for (int i = 1; generatedNumber > 0; i++, generatedNumber /= 10)
            {
                if (a[generatedNumber % 10] == i)
                    bulls++;
                else if (a[generatedNumber % 10] != 0)
                    cows++;
            }

        }

        // Основной метод, отвечающий за взаимодействие с пользователем, когда тот уже ввёл количество необходимых разрядов
        static void PlayGame(long generatedNumber, int count)
        {
            long inputNumber = -1;
            bool flag = true;
            while (flag)
            {
                int cows = -1, bulls = -1;
                InputUserNumber(ref inputNumber, count);
                CheckTheDifference(inputNumber, generatedNumber, ref cows, ref bulls);
                if (bulls == count)
                    flag = false;
                else
                {
                    Console.WriteLine($"Коровы: {cows}\nБыки: {bulls}");
                }
            }
            Console.Clear();
            Console.WriteLine("Поздравляем, вы угадали число!");
        }


        // Точка входа в программу, отсюда вызываются все ключевые методы
        static void Main(string[] args)
        {
            Console.Title = "Быки и Коровы";
            do
            {
                Console.Clear();
                int count;
                InputQuantity(out count);
                long number = InitializeNumber(count);
                PlayGame(number, count);
                Console.WriteLine("Если вы не хотите больше играть, нажмите ESC, в противном случае, нажмите любую другую клавишу");
            } while (ReadKey().Key != ConsoleKey.Escape);
        }
    }
}


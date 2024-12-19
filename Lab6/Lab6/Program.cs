using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Виберіть завдання для виконання:");
            Console.WriteLine("1. Робота з двовимірним масивом");
            Console.WriteLine("2. Робота з колекцією студентів");
            Console.WriteLine("3. Робота з різними типами колекцій");
            Console.WriteLine("4. Видалення слів з парних рядків");
            Console.WriteLine("0. Вихід");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Некоректний вибір. Натисніть будь-яку клавішу для повтору");
                Console.ReadKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Task1();
                    break;
                case 2:
                    Task2();
                    break;
                case 3:
                    Task3();
                    break;
                case 4:
                    Task4();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір. Натисніть будь-яку клавішу для повтору");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void Task1()
    {
        Console.Clear();
        Console.Write("Введіть розмірність матриці n: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Некоректне значення");
            Console.ReadKey();
            return;
        }

        int[,] matrix = new int[n, n];

        Console.WriteLine("1. Заповнити випадковими значеннями");
        Console.WriteLine("2. Заповнити одиницями");
        Console.WriteLine("3. Вписати ромб з нулів");
        Console.Write("Виберіть дію: ");

        if (!int.TryParse(Console.ReadLine(), out int option))
        {
            Console.WriteLine("Некоректний вибір");
            Console.ReadKey();
            return;
        }

        switch (option)
        {
            case 1:
                FillMatrixRandom(matrix);
                break;
            case 2:
                FillMatrixOnes(matrix);
                break;
            case 3:
                FillMatrixWithDiamond(matrix);
                break;
            default:
                Console.WriteLine("Некоректний вибір");
                Console.ReadKey();
                return;
        }

        PrintMatrix(matrix);
        Console.ReadKey();
    }

    static void FillMatrixRandom(int[,] matrix)
    {
        Random rand = new Random();
        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = rand.Next(1, 10);
    }

    static void FillMatrixOnes(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = 1;
    }

    static void FillMatrixWithDiamond(int[,] matrix)
    {
        int center = matrix.GetLength(0) / 2;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int distance = Math.Abs(center - i) + Math.Abs(center - j);
                matrix[i, j] = distance <= center ? 0 : 1;
            }
        }
    }

    static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write(matrix[i, j] + " ");
            Console.WriteLine();
        }
    }

    static void Task2()
    {
        Console.Clear();
        List<string> students = new List<string>();

        while (true)
        {
            Console.WriteLine("1. Додати студента");
            Console.WriteLine("2. Видалити студента");
            Console.WriteLine("3. Пошук студента");
            Console.WriteLine("4. Сортувати за зростанням");
            Console.WriteLine("5. Сортувати за спаданням");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть дію: ");

            if (!int.TryParse(Console.ReadLine(), out int option)) break;

            switch (option)
            {
                case 1:
                    Console.Write("Введіть ім'я студента: ");
                    students.Add(Console.ReadLine());
                    break;
                case 2:
                    Console.Write("Введіть ім'я студента для видалення: ");
                    students.Remove(Console.ReadLine());
                    break;
                case 3:
                    Console.Write("Введіть ім'я для пошуку: ");
                    string name = Console.ReadLine();
                    Console.WriteLine(students.Contains(name) ? "Знайдено" : "Не знайдено");
                    break;
                case 4:
                    students.Sort();
                    break;
                case 5:
                    students.Sort();
                    students.Reverse();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір");
                    break;
            }
            Console.WriteLine("Колекція: " + string.Join(", ", students));
        }
    }

    static void Task3()
    {
        Console.Clear();
        Console.WriteLine("Виберіть тип колекції:");
        Console.WriteLine("1. ArrayList");
        Console.WriteLine("2. SortedList");
        Console.WriteLine("3. Stack");
        Console.WriteLine("4. Dictionary");
        Console.WriteLine("0. Назад");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > 4)
        {
            Console.WriteLine("Некоректний вибір");
            Console.ReadKey();
            return;
        }

        switch (choice)
        {
            case 1:
                ArrayList arrayList = new ArrayList();
                ManageArrayList(arrayList);
                break;
            case 2:
                SortedList sortedList = new SortedList();
                ManageSortedList(sortedList);
                break;
            case 3:
                Stack stack = new Stack();
                ManageStack(stack);
                break;
            case 4:
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                ManageDictionary(dictionary);
                break;
            case 0:
                return;
        }
    }

    static void ManageArrayList(ArrayList arrayList)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Додати елемент");
            Console.WriteLine("2. Видалити елемент");
            Console.WriteLine("3. Показати елементи");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть дію: ");

            if (!int.TryParse(Console.ReadLine(), out int option)) return;

            switch (option)
            {
                case 1:
                    Console.Write("Введіть значення: ");
                    arrayList.Add(Console.ReadLine());
                    break;
                case 2:
                    Console.Write("Введіть значення для видалення: ");
                    arrayList.Remove(Console.ReadLine());
                    break;
                case 3:
                    foreach (var item in arrayList)
                    {
                        Console.WriteLine(item);
                    }
                    Console.ReadKey();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір");
                    break;
            }
        }
    }

    static void ManageSortedList(SortedList sortedList)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Додати елемент (ключ-значення)");
            Console.WriteLine("2. Видалити елемент");
            Console.WriteLine("3. Показати елементи");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть дію: ");

            if (!int.TryParse(Console.ReadLine(), out int option)) return;

            switch (option)
            {
                case 1:
                    Console.Write("Введіть ключ: ");
                    string key = Console.ReadLine();
                    Console.Write("Введіть значення: ");
                    string value = Console.ReadLine();
                    sortedList[key] = value;
                    break;
                case 2:
                    Console.Write("Введіть ключ для видалення: ");
                    sortedList.Remove(Console.ReadLine());
                    break;
                case 3:
                    foreach (DictionaryEntry item in sortedList)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value}");
                    }
                    Console.ReadKey();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір");
                    break;
            }
        }
    }

    static void ManageStack(Stack stack)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Додати елемент");
            Console.WriteLine("2. Видалити елемент (поп)");
            Console.WriteLine("3. Показати елементи");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть дію: ");

            if (!int.TryParse(Console.ReadLine(), out int option)) return;

            switch (option)
            {
                case 1:
                    Console.Write("Введіть значення: ");
                    stack.Push(Console.ReadLine());
                    break;
                case 2:
                    if (stack.Count > 0)
                        stack.Pop();
                    else
                        Console.WriteLine("Стек порожній");
                    break;
                case 3:
                    foreach (var item in stack)
                    {
                        Console.WriteLine(item);
                    }
                    Console.ReadKey();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір");
                    break;
            }
        }
    }

    static void ManageDictionary(Dictionary<string, string> dictionary)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Додати елемент (ключ-значення)");
            Console.WriteLine("2. Видалити елемент");
            Console.WriteLine("3. Показати елементи");
            Console.WriteLine("0. Назад");
            Console.Write("Виберіть дію: ");

            if (!int.TryParse(Console.ReadLine(), out int option)) return;

            switch (option)
            {
                case 1:
                    Console.Write("Введіть ключ: ");
                    string key = Console.ReadLine();
                    Console.Write("Введіть значення: ");
                    string value = Console.ReadLine();
                    dictionary[key] = value;
                    break;
                case 2:
                    Console.Write("Введіть ключ для видалення: ");
                    dictionary.Remove(Console.ReadLine());
                    break;
                case 3:
                    foreach (var item in dictionary)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value}");
                    }
                    Console.ReadKey();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Некоректний вибір");
                    break;
            }
        }
    }

    static void Task4()
    {
        Console.Clear();
        Console.WriteLine("Введіть парну кількість рядків (число):");
        int lineCount;
        if (!int.TryParse(Console.ReadLine(), out lineCount) || lineCount % 2 != 0 || lineCount <= 0)
        {
            Console.WriteLine("Некоректна кількість рядків");
            Console.ReadKey();
            return;
        }
        string[] lines = new string[lineCount];
        for (int i = 0; i < lineCount; i++)
        {
            Console.Write($"Введіть рядок {i + 1}: ");
            lines[i] = Console.ReadLine();
        }
        Console.WriteLine("Результат:");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        Console.ReadKey();
    }
}

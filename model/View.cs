using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork_6_ReadFileParsing.model
{
    public class View
    {
        public static void ShowMenu()
        {
            Console.WriteLine("                   Главное меню                 ");
            Console.WriteLine("************************************************");
            Console.WriteLine("1. Поиск запчасти по номеру");
            Console.WriteLine("2. Поиск запчасти по бренду");
            Console.WriteLine("3. Поиск запчасти по совпадению в наименовании");
            Console.WriteLine("4. Поиск запчасти по параметру «KEYZAK»");
            Console.WriteLine("5. Отсортировать запчасти по цене");
            Console.WriteLine("6. Вывести всё");
            Console.WriteLine("7. Выход из программы");
            Console.WriteLine();
            Console.WriteLine("Ваш выбор: ");
        }

        public static void Clear()
        {
            Console.ReadKey();
            Console.Clear();
        }


    }
}

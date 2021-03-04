using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_6_ReadFileParsing.model
{
    class Program
    {
        /*1.Распарсить файл приложенный к домашней работе, поместив его либо в массив типа object[] либо в List типа вашКласс – List< вашКласс >. 
                 * При этом каждый товар должен быть обернут в класс (Свойства класса можно получить из файла)

        2.	Создать метод, который приложит на выбор варианты поиска: (Варианты поиска можно организовать в виде enum)
        a.	По номеру запчасти
        b.	По бренду
        c.	По совпадению в наименовании
        d.	По параметру «KEYZAK»
        e.	Предложить пользователю отсортировать массив, по Цене
        f.	Вывести все

        В данных методах нет необходимости выводить все варианты, просто вывести что найдено из скольких. После вывести только первые 10 записей.
        3.	При выводе необходимо вывести таблицу в следующем порядке:
        a.	Бренд (BRAND)
        b.	Артикул (ARTID + PIN)
        c.	Наименование (NAME)
        d.	Цена (PRICE)
        e.	Наличие (RVALUE)
        */
        enum VARIANTS { NUMBER = 1, BRAND, NAME, KEYZAK, PRICE, PRINT, EXIT }
        static void Main(string[] args)
        {
            FileStream file = new FileStream("C:\\Users\\irish\\My documents\\C#\\HomeWork_6_ReadFileParsing\\Articles.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            string data = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Не удалось считать базу данных");
            reader.Close();
            string[] details = SplitObjects(data);
            List<SparePart> spareDetails = new List<SparePart>();
            ParseContent(details, spareDetails);

            while (true)
            {
                View.ShowMenu();
                int userChoice;
                Int32.TryParse(Console.ReadLine(), out userChoice);
                switch (userChoice)
                {
                    case (int)VARIANTS.NUMBER:
                        int artid;
                        Console.WriteLine("Для поиска укажите номер запчасти: ");
                        Int32.TryParse(Console.ReadLine(), out artid);
                        FindByARTID(spareDetails, artid);
                        View.Clear();
                        break;
                    case (int)VARIANTS.BRAND:
                        Console.WriteLine("Для поиска введите название бренда: ");
                        string brand = Console.ReadLine();
                        FindByBRAND(spareDetails, brand);
                        View.Clear();
                        break;
                    case (int)VARIANTS.NAME:
                        Console.WriteLine("Для поиска введите наименование запчасти: ");
                        string name = Console.ReadLine();
                        FindByNAME(spareDetails, name);
                        View.Clear();
                        break;
                    case (int)VARIANTS.KEYZAK:
                        Console.WriteLine("Для поиска введите параметр KEYZAK: ");
                        string keyzak = Console.ReadLine();
                        FindByKEYZAK(spareDetails, keyzak);
                        View.Clear();
                        break;
                    case (int)VARIANTS.PRICE:
                        Sort(spareDetails);
                        Console.WriteLine("Запчасти отсортированы по цене");
                        View.Clear();
                        break;
                    case (int)VARIANTS.PRINT:
                        PrintAll(spareDetails);
                        View.Clear();
                        break;
                    case (int)VARIANTS.EXIT:
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню. Повторите ввод");
                        break;
                }
            }
        }

        private static string[] SplitObjects(string data)
        {
            string[] details = data
                .Replace("},{", "***")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("=", "")
                .Replace(">", "")
                .Replace("\"\"", "null")
                .Replace(", ", ".")
                .Replace("\"", "")
                .Split(new string[] { "***" }, StringSplitOptions.None);
            return details;
        }
        private static void ParseContent(string[] details, List<SparePart> list)
        {
            for (int i = 0; i < details.Length; i++)
            {
                SparePart obj = new SparePart();
                var product = details[i].Split(',');
                foreach (var poz in product)
                {
                    var parameter = poz.Split(':');
                    if (parameter[0].Equals("PIN"))
                        obj.PIN = parameter[1];
                    else if (parameter[0].Equals("BRAND"))
                        obj.BRAND = parameter[1];
                    else if (parameter[0].Equals("NAME"))
                        obj.NAME = parameter[1];
                    else if (parameter[0].Equals("ARTID"))
                        obj.ARTID = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("PARNR"))
                        obj.PARNR = parameter[1];
                    else if (parameter[0].Equals("KEYZAK"))
                        obj.KEYZAK = parameter[1];
                    else if (parameter[0].Equals("RVALUE"))
                    {
                        if (parameter[1].Contains('.'))
                            parameter[1] = parameter[1].Remove(parameter[1].IndexOf('.'));
                        obj.RVALUE = Int32.Parse(parameter[1]);
                    }
                    else if (parameter[0].Equals("RDPRF"))
                        obj.RDPRF = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("MINBM"))
                    {
                        parameter[1] = parameter[1].Replace('.', ','); // по настройкам у меня дабл воспринимает через запятую, а не через точку
                        obj.MINBM = Double.Parse(parameter[1]);
                    }

                    else if (parameter[0].Equals("VENSL"))
                    {
                        parameter[1] = parameter[1].Replace('.', ',');
                        obj.VENSL = Double.Parse(parameter[1]);
                    }
                    else if (parameter[0].Equals("PRICE"))
                    {
                        parameter[1] = parameter[1].Replace('.', ',');
                        obj.PRICE = Double.Parse(parameter[1]);
                    }
                    else if (parameter[0].Equals("WAERS"))
                        obj.WAERS = parameter[1];
                    else if (parameter[0].Equals("DLVDT"))
                        obj.DLVDT = parameter[1];
                    else if (parameter[0].Equals("ANALOG"))
                        obj.ANALOG = parameter[1];
                    else if (parameter[0].Equals("MSG"))
                        obj.MSG = parameter[1];
                    else if (parameter[0].Equals("PriceTenge"))
                        obj.PriceTenge = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("PriceRub"))
                    {
                        parameter[1] = parameter[1].Replace('.', ',');
                        obj.PriceRub = Double.Parse(parameter[1]);
                    }
                    else if (parameter[0].Equals("ArtificialPrice"))
                        obj.ArtificialPrice = parameter[1];
                    else if (parameter[0].Equals("ShowArtificialByDefault"))
                        obj.ShowArtificialByDefault = parameter[1];
                    else if (parameter[0].Equals("SupplierId"))
                        obj.SupplierId = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("Delivery"))
                        obj.Delivery = parameter[1];
                    else if (parameter[0].Equals("DeliveryEx"))
                        obj.DeliveryEx = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("DeliveryGu"))
                        obj.DeliveryGu = Int32.Parse(parameter[1]);
                    else if (parameter[0].Equals("Vkorg"))
                        obj.Vkorg = parameter[1];
                    else if (parameter[0].Equals("Kunnr"))
                        obj.Kunnr = parameter[1];
                }
                list.Add(obj);
            }
        }
        private static void Print(SparePart obj)
        {
            Console.WriteLine("Бренд: " + obj.BRAND);
            Console.WriteLine("Артикул: " + obj.ARTID + ' ' + obj.PIN);
            Console.WriteLine("Наименование: " + obj.NAME);
            Console.WriteLine("Цена: " + obj.PRICE);
            Console.WriteLine("Наличие: " + obj.RVALUE);
            Console.WriteLine();
        }
        public static void FindByARTID(List<SparePart> list, int artid)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.ARTID == artid)
                {
                    Print(item);
                    count++;
                }
            }
            if (count == 0)
                Console.WriteLine("В базе нет запчасти с таким номером");
        }
        private static void FindByBRAND(List<SparePart> list, string brand)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.BRAND == brand)
                {
                    Print(item);
                    count++;
                }
            }
            if (count == 0)
                Console.WriteLine("В базе нет запчасти такого бренда");
        }
        private static void FindByNAME(List<SparePart> list, string name)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.NAME.Contains(name))
                {
                    Print(item);
                    count++;
                }
            }
            if (count == 0)
                Console.WriteLine("В базе нет запчасти с таким наименованием");
        }
        private static void FindByKEYZAK(List<SparePart> list, string keyzak)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.KEYZAK == keyzak)
                {
                    Print(item);
                    count++;
                }
            }
            if (count == 0)
                Console.WriteLine("В базе нет запчасти с таким параметром KEYZAK");
        }
        private static void Sort(List<SparePart> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - 1; j++)
                {
                    if (list[j].PRICE > list[j + 1].PRICE)
                    {
                        SparePart temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
        private static void PrintAll(List<SparePart> list)
        {
            foreach (var item in list)
            {
                Print(item);
            }
        }
    }
}



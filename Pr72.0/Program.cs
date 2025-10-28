using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr72._0
{
    class Program
    {
        static List<Parts> part;
        static int id;
        static int balance;
        static int markup;
        static decimal num;
        static int count = 0;
        static bool playerBuying = false;
        static bool clientServiced = false;

        static Dictionary<string, int> partsToBuy = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            markup = 1000;
            Random rand = new Random();
            balance = Convert.ToInt32(rand.Next(2000, 40000));
            Console.WriteLine($"Игра запустилась, ваш баланс {balance}");
            Console.WriteLine("Ваш склад: ");
            part = Core.Context.Parts.ToList();

            foreach (Parts part2 in part)
            {
                Console.WriteLine($"{part2.Name}; {part2.Price}; {part2.Count}");
            }
            Console.WriteLine("-------------------------------------------------");


            Console.WriteLine("К вам пришел новый клиент, у него сломалась деталь");
            int inkrement = 1;
            while (true)
            {

                if (playerBuying)
                {
                    if (count == 2)
                    {
                        count = 0;
                        playerBuying = false;
                        foreach (var partToBuy in partsToBuy)
                        {
                            Core.Context.Parts.First(p => p.Name == partToBuy.Key).Count += partToBuy.Value;
                        }
                        partsToBuy.Clear();
                    }
                    else
                    {
                        count++;
                    }
                }

                clientServiced = false;
                Random randomID = new Random();
                id = randomID.Next(1, part.Count);

                while (!clientServiced)
                {
                    Console.WriteLine($"Клиент {inkrement++}");
                    Console.WriteLine($"Поломка: {part[id].Name}");
                    Console.WriteLine($"Стоимость ремонта {part[id].Price + markup}");

                    Console.WriteLine("-------------------------------------------------");

                    Console.WriteLine("Выберите действие: ");
                    Console.WriteLine("1. Вывести весь список");
                    Console.WriteLine("2. Согласится на ремонт");
                    Console.WriteLine("3. Не соглашатся на ремонт");
                    Console.WriteLine("4. Докупить детали говна");

                    Console.WriteLine("-------------------------------------------------");
                    while (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                OutputDB();
                                break;
                            case 2:
                                Agree();
                                clientServiced = true;
                                break;
                            case 3:
                                Disagree();
                                clientServiced = true;
                                break;
                            case 4:
                                BuyParts();
                                break;
                            default:
                                Console.WriteLine("Вы ввели несуществующее действие");
                                break;
                        }
                    }
                }
            }
        }

        static void OutputDB()
        {
            part = Core.Context.Parts.ToList();

            Console.WriteLine("Ваш склад");

            foreach (Parts part2 in part)
            {
                Console.WriteLine($"{part2.Name}; {part2.Price}; {part2.Count}");}

            Console.WriteLine("-------------------------------------------------");
        }

        static void Agree()
        {
            Console.WriteLine("Вы согласились на ремонт");
            if (part[id].Count == 0)
            {
                Console.WriteLine("Ты мудак!");
                Console.WriteLine($"С вашего баланса списан штраф, теперь баланс составляет {balance - part[id].Price}");
                Console.WriteLine("-------------------------------------------------");
            }
            else
            {
                Core.Context.Parts.Find(part[id].Count--);
                Core.Context.SaveChanges();
                num = balance+part[id].Price + markup;
                Console.WriteLine($"ремонт прошел успешно, ваш баланс составляет {num}");
                Console.WriteLine("-------------------------------------------------");
            }
        }

        static void Disagree()
        {
            Console.WriteLine("Вы не согласились на ремонт");
            Console.WriteLine($"С вашего баланса будет списан штраф {part[id].Price}");
            Console.WriteLine($"Теперь ваш баланс составляет {balance - part[id].Price}");
            Console.WriteLine("-------------------------------------------------");
        }

        static void BuyParts()
        {
            while (true) 
            {
                foreach (Parts part in part)
                {
                    Console.WriteLine(part.Name);
                }
                Console.WriteLine("Введите детали для покупки");
                string namePart = Console.ReadLine();
                Console.WriteLine("Количество");
                if (int.TryParse(Console.ReadLine(), out int partsCount))
                {
                    partsToBuy.Add(namePart, partsCount);
                }
                balance -= (int)(Core.Context.Parts.First(p => p.Name == namePart).Price * partsCount);
                playerBuying = true;
                Console.WriteLine("Хотите ещё купить?");
                if(Console.ReadLine() == "нет")
                {
                    break;
                }
            }

        }
    }
}
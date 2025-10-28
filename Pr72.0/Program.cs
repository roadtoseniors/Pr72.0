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
        static decimal balance;
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
                            var partItem = Core.Context.Parts.First(p => p.Name == partToBuy.Key);
                            partItem.Count += partToBuy.Value;
                        }
                        Core.Context.SaveChanges();
                        partsToBuy.Clear();
                    }
                    else
                    {
                        count++;
                    }
                }

                clientServiced = false;
                Random randomID = new Random();
                id = randomID.Next(0, part.Count); 

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
                    Console.WriteLine("4. Докупить детали");

                    Console.WriteLine("-------------------------------------------------");

                   
                    if (int.TryParse(Console.ReadLine(), out int choice))
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
                    else
                    {
                        Console.WriteLine("Некорректный ввод. Введите число от 1 до 4.");
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
                Console.WriteLine($"{part2.Name}; {part2.Price}; {part2.Count}");
            }

            Console.WriteLine("-------------------------------------------------");
        }

        static void Agree()
        {
            Console.WriteLine("Вы согласились на ремонт");

            
            var currentPart = Core.Context.Parts.Find(part[id].ID); 
            if (currentPart == null)
            {
                Console.WriteLine("Деталь не найдена!");
                return;
            }

            if (currentPart.Count == 0)
            {
                Console.WriteLine("Нет деали!");
                balance -= currentPart.Price;
                Console.WriteLine($"С вашего баланса списан штраф, теперь баланс составляет {balance}");
                Console.WriteLine("-------------------------------------------------");
            }
            else
            {
                currentPart.Count--; 
                Core.Context.SaveChanges();
                balance += currentPart.Price + markup; 
                Console.WriteLine($"Ремонт прошел успешно, ваш баланс составляет {balance}");
                Console.WriteLine("-------------------------------------------------");
            }
        }

        static void Disagree()
        {
            Console.WriteLine("Вы не согласились на ремонт");
            var currentPart = Core.Context.Parts.Find(part[id].ID); 
            if (currentPart != null)
            {
                balance -= currentPart.Price; 
                Console.WriteLine($"С вашего баланса будет списан штраф {currentPart.Price}");
                Console.WriteLine($"Теперь ваш баланс составляет {balance}");
            }
            Console.WriteLine("-------------------------------------------------");
        }

        static void BuyParts()
        {
            while (true)
            {
                Console.WriteLine("Доступные детали:");
                foreach (Parts partItem in part) 
                {
                    Console.WriteLine($"{partItem.Name} - Цена: {partItem.Price}");
                }

                Console.WriteLine("Введите название детали для покупки:");
                string namePart = Console.ReadLine();

               
                var partToBuy = Core.Context.Parts.FirstOrDefault(p => p.Name == namePart);
                if (partToBuy == null)
                {
                    Console.WriteLine("Деталь с таким названием не найдена!");
                    continue;
                }

                Console.WriteLine("Количество:");
                if (int.TryParse(Console.ReadLine(), out int partsCount) && partsCount > 0)
                {
                    decimal totalCost = partToBuy.Price * partsCount;

                   
                    if (totalCost > balance)
                    {
                        Console.WriteLine($"Недостаточно средств! Нужно: {totalCost}, доступно: {balance}");
                        continue;
                    }

                   
                    if (partsToBuy.ContainsKey(namePart))
                    {
                        partsToBuy[namePart] += partsCount;
                    }
                    else
                    {
                        partsToBuy.Add(namePart, partsCount);
                    }

                    balance -= totalCost; 
                    playerBuying = true;

                    Console.WriteLine($"Добавлено в корзину: {namePart} x{partsCount}");
                    Console.WriteLine($"Текущий баланс: {balance}");
                }
                else
                {
                    Console.WriteLine("Некорректное количество!");
                }

                Console.WriteLine("Хотите ещё купить? (да/нет)");
                if (Console.ReadLine().ToLower() == "нет")
                {
                    break;
                }
            }
        }
    }
}
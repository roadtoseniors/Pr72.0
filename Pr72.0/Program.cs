using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr72._0
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }

    class CoreFile
    {
        public static DatabaseContext Context { get; set; }
    }

    class DatabaseContext
    {
        public List<Part> Part { get; set; }

        public void SaveChanges()
        {
        }

        public Part Find(int id)
        {
            return null;
        }
    }

    class Client
    {
        public int Id { get; set; }
        public Part BrokenPart { get; set; }
        public bool IsServiced { get; set; }

        public void GenerateBrokenPart()
        {
        }

        public int GetRepairCost(int markup)
        {
            return 0;
        }
    }

    class GameManager
    {
        public int Balance { get; set; }
        public int Markup { get; set; }
        public List<Client> Clients { get; set; }
        public Warehouse Warehouse { get; set; }
        public Random Random { get; set; }

        public void InitializeGame()
        {
        }

        public void ProcessClient()
        {
        }

        public void UpdateBalance(int amount)
        {
        }

        public bool CheckGameOver()
        {
            return false;
        }
    }

    class Warehouse
    {
        public List<Part> Parts { get; set; }
        public Dictionary<string, int> PartsToBuy { get; set; }

        public Part GetPart(int id)
        {
            return null;
        }

        public void AddPart(string partName, int quantity)
        {
        }

        public void RemovePart(int partId)
        {
        }

        public bool CheckAvailability(int partId, int quantity = 1)
        {
            return false;
        }
    }

}

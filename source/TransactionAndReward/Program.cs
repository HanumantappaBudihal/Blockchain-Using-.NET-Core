using Newtonsoft.Json;
using System;

namespace TransactionAndReward
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Blockchain simpleCoin = new Blockchain();
            simpleCoin.CreateTransaction(new Transaction("Henry", "MaHesh", 10));
            simpleCoin.ProcessPendingTransactions("Bill");
            Console.WriteLine(JsonConvert.SerializeObject(simpleCoin, Formatting.Indented));

            simpleCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            simpleCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            simpleCoin.ProcessPendingTransactions("Bill");

            var endTime = DateTime.Now;

            Console.WriteLine($"Duration: {endTime - startTime}");

            Console.WriteLine("=========================");
            Console.WriteLine($"Henry' balance: {simpleCoin.GetBalance("Henry")}");
            Console.WriteLine($"MaHesh' balance: {simpleCoin.GetBalance("MaHesh")}");
            Console.WriteLine($"Bill' balance: {simpleCoin.GetBalance("Bill")}");

            Console.WriteLine("=========================");
            Console.WriteLine($"phillyCoin");
            Console.WriteLine(JsonConvert.SerializeObject(simpleCoin, Formatting.Indented));

            Console.ReadKey();
        }
    }
}

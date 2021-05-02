using System;

namespace TransactionAndReward
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain simpleCoin = new Blockchain();
            simpleCoin.CreateTransaction(new Transaction("Henry", "MaHesh", 10));
        }
    }
}

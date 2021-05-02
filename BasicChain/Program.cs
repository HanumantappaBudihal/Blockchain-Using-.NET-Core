using Newtonsoft.Json;
using System;

namespace BasicChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain simpleCoin = new Blockchain();
            simpleCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            simpleCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            simpleCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));

            Console.WriteLine(JsonConvert.SerializeObject(simpleCoin, Formatting.Indented));

            Console.WriteLine($"Is Chain Valid: {simpleCoin.IsValid()}");

            Console.WriteLine($"Update amount to 1000");
            simpleCoin.Chain[1].Data = "{sender:Henry,receiver:MaHesh,amount:1000}";

            Console.WriteLine($"Is Chain Valid: {simpleCoin.IsValid()}");

            Console.WriteLine($"Update hash");
            simpleCoin.Chain[1].Hash = simpleCoin.Chain[1].GenerateHash();

            Console.WriteLine($"Is Chain Valid: {simpleCoin.IsValid()}");

            Console.WriteLine($"Update the entire chain");
            simpleCoin.Chain[2].PreviousHash = simpleCoin.Chain[1].Hash;
            simpleCoin.Chain[2].Hash = simpleCoin.Chain[2].GenerateHash();
            simpleCoin.Chain[3].PreviousHash = simpleCoin.Chain[2].Hash;
            simpleCoin.Chain[3].Hash = simpleCoin.Chain[3].GenerateHash();

            Console.WriteLine($"Is Chain Valid: {simpleCoin.IsValid()}");

            Console.ReadKey();
        }
    }
}

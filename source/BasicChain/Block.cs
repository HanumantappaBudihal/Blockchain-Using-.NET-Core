using System;
using System.Security.Cryptography;
using System.Text;

namespace BasicChain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }

        public Block(DateTime timeStamp, string previoiusHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previoiusHash;
            Hash = GenerateHash();
        }

        public string GenerateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}


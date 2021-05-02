using System;
using System.Security.Cryptography;
using System.Text;

namespace ProofOfWork
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        public int Nouce { get; set; }

        public Block(DateTime timeStamp, string previousHash, string data)
        {
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = GenerateHash();
        }

        public string GenerateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{Nouce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZero = new string('0', difficulty);

            while(this.Hash==null || this.Hash.Substring(0,difficulty)!=leadingZero)
            {
                this.Nouce++;
                this.Hash = this.GenerateHash();
            }
        }
    }
}

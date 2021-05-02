using System;
using System.Collections.Generic;

namespace ProofOfWork
{
    /// <summary>
    /// Simple block chain ( hashing - SHA256)
    /// </summary>
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }
        public int Difficulty { set; get; } = 3;

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }

        private void InitializeChain()
        {
            Chain = new List<Block>();
        }

        private void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        private Block CreateGenesisBlock()
        {
            return new Block(DateTime.UtcNow, null, "{This genesis block}");
        }

        /// <summary>
        /// Get the latest block in chain
        /// </summary>
        /// <returns></returns>
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        /// <summary>
        /// Used to add the new block to chain
        /// </summary>
        /// <param name="block"></param>
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(this.Difficulty); // Generate the hash (work of proof)

            Chain.Add(block);
        }

        /// <summary>
        /// This method validate the whether chain is valid or not
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            for (int iterator = 1; iterator < Chain.Count; iterator++)
            {
                Block currentBlock = Chain[iterator];
                Block previousBlock = Chain[iterator - 1];

                if (currentBlock.Hash != currentBlock.GenerateHash())
                    return false;

                if (currentBlock.PreviousHash != previousBlock.Hash)
                    return false;
            }

            return true;
        }
    }
}

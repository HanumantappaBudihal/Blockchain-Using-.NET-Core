using System;
using System.Collections.Generic;

namespace TransactionAndReward
{
    /// <summary>
    /// Simple block chain ( hashing - SHA256)
    /// </summary>
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }
        public int Difficulty { set; get; } = 3;
        public int Reward { get; set; } = 1;
        public IList<Transaction> PendingTransactions = new List<Transaction>();

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
            return new Block(DateTime.UtcNow, null, null);
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

        /// <summary>
        /// Create transaction 
        /// </summary>
        /// <param name="transaction"></param>
        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.UtcNow, GetLatestBlock().Hash, PendingTransactions);

            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        //TODO : Need to improve this method
        /// <summary>
        /// Get the balance by address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetBalance(string address)
        {
            int balance = 0;

            for (int iterator = 0; iterator < Chain.Count; iterator++)
            {
                if (Chain[iterator].Transactions != null && Chain[iterator].Transactions.Count > 0)
                    for (int innerIterator = 0; innerIterator < Chain[iterator].Transactions.Count; innerIterator++)
                    {
                        var transaction = Chain[iterator].Transactions[innerIterator];

                        if (transaction.FromAddress == address)
                        {
                            balance -= transaction.Amount;
                        }

                        if (transaction.ToAddress == address)
                        {
                            balance += transaction.Amount;
                        }
                    }
            }

            return balance;
        }
    }
}

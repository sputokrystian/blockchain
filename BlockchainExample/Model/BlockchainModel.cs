using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace BlockchainExample.Model
{
    [Serializable]
    public partial class Blockchain
    {
        public Blockchain()
        {
            TimeStamp = 0;
            this.Data = string.Empty;
            this.Hash = string.Empty;
            this.PrevHash = string.Empty;
            this.Nonce = 0;
        }

        public Blockchain(int Time, string Data, string Hash, string PrevHash, int Nonce)
        {
            TimeStamp = Time;
            this.Data = Data;
            this.Hash = Hash;
            this.PrevHash = PrevHash;
            this.Nonce = Nonce;
        }

        public int? AddBlock()
        {
            using (var db = new BlockchainExampleEntities())
            {
                using (
                    var dbTran = db.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (ValidateBlock())
                        {
                            db.Blockchain.Add(this);
                            db.SaveChanges();
                            dbTran.Commit();
                            return this.Index;
                        }
                        dbTran.Rollback();
                        return null;
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                        return null;
                    }
                }
            }
        }

        private bool ValidateBlock()
        {
            return GetLastBlock().Hash == this.PrevHash;
        }

        public Blockchain GetLastBlock()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    return db.Blockchain.OrderByDescending(b => b.Index).First();
                }
                catch (Exception)
                {
                    return new Blockchain(0, string.Empty, string.Empty, string.Empty, 0);
                }
            }
        }

        public static bool ValidateBlockchain()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var Blockchain = db.Blockchain.AsNoTracking().OrderBy(b => b.Index).ToArray();
                    for (int i = 1; i < Blockchain.Count(); i++)
                    {
                        if (!(Blockchain[i].PrevHash == Blockchain[i - 1].Hash))
                        {
                            Console.WriteLine("Block no " + i + " is invalid");
                            return false;
                        }
                    }
                    stopwatch.Stop();
                    Console.WriteLine("Blockchain is valid, checked in " + stopwatch.ElapsedMilliseconds + "ms");
                }
                catch (Exception)
                {
                    return false;
                }

                
                Console.ReadKey();
                Environment.Exit(0);
                return true;
            }
        }
    }
}
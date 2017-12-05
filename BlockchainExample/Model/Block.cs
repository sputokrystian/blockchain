using System;
using System.Data;

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
                using (var dbTran = db.Database.BeginTransaction(IsolationLevel.Serializable))
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
            return Helper.Block.GetLastBlock().Hash == this.PrevHash;
        }
    }
}
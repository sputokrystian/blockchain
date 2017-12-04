using System;

namespace BlockchainExample.Model
{
    [Serializable]
    public partial class Blockchain
    {
        public Blockchain()
        {
        }

        public Blockchain(int Time, string Data, string Hash, string PrevHash)
        {
            TimeStamp = Time;
            this.Data = Data;
            this.Hash = Hash;
            this.PrevHash = PrevHash;
        }

        public int? AddBlock()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    db.Blockchain.Add(this);
                    db.SaveChanges();
                    db.Dispose();
                    return this.Index;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
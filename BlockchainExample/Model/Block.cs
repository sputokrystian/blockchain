using System;

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
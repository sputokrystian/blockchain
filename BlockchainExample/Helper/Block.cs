using BlockchainExample.Model;
using System;
using System.Linq;

namespace BlockchainExample.Helper
{
    public class Block
    {
        public static Blockchain GetLastBlock()
        {
            using (var db = new BlockchainExampleEntities())
            {
                try
                {
                    return db.Blockchain.OrderByDescending(b => b.Index).First();
                }
                catch (Exception)
                {
                    return new Blockchain(0, string.Empty, string.Empty, string.Empty,0);
                }
            }
        }
    }
}
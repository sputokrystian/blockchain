using BlockchainExample.Helper;
using BlockchainExample.Model;
using System;
using System.Threading;

namespace BlockchainExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                GenerateBlock();
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }

        private static void GenerateBlock()
        {
            Users user = new Users(RandomStrings.RandomString(5), RandomStrings.RandomString(15));
            user.SaveUserInDatabase();
            Console.WriteLine("Dodano użytkownika, ID: " + user.ID);
            var UserHash = Hash.SHA512HashUser(user);

            var prevBlockHash = Helper.Block.GetLastBlock().Hash;
            Blockchain Block = new Blockchain(Timestamp.Get(), UserHash, string.Empty, prevBlockHash);
            var BlockHash = Hash.SHA512HashBlock(Block);
            Block.Hash = BlockHash;
            Block.AddBlock();

            Console.WriteLine("Dodano blok do łańcucha: " + Block.Index);
            Console.WriteLine("Hash bloku: " + BlockHash);
        }
    }
}
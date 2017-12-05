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
            Console.WriteLine("Enter proof of work difficulty (0-10): ");
            var difficultyString = Console.ReadLine();
            int.TryParse(difficultyString, out int difficulty);

            for (int i = 0; i < 100; i++)
            {
                GenerateBlock(difficulty);
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }

        private static void GenerateBlock(int difficulty)
        {
            string UserHash = AddUser();
            GenerateBlock(UserHash);
        }

        private static string AddUser()
        {
            //Random strings, to randomize data
            Users user = new Users(RandomStrings.RandomString(5), RandomStrings.RandomString(15));
            user.SaveUserInDatabase();
            Console.WriteLine("Added user, ID: " + user.ID);
            return Hash.SHA512HashUser(user);
        }

        private static void GenerateBlock(string Data)
        {
            var prevBlockHash = Helper.Block.GetLastBlock().Hash;
            Blockchain Block = new Blockchain(Timestamp.Get(), Data, string.Empty, prevBlockHash);
            var BlockHash = Hash.SHA512HashBlock(Block);
            Block.Hash = BlockHash;
            Block.AddBlock();

            Console.WriteLine("Added block to blockchain: " + Block.Index);
            Console.WriteLine("Block hash: " + BlockHash);
        }
    }
}
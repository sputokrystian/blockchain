using BlockchainExample.Helper;
using BlockchainExample.Model;
using System;
using System.Diagnostics;

namespace BlockchainExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter proof of work difficulty (0-10): ");
            var difficultyString = Console.ReadLine();
            int.TryParse(difficultyString, out int Difficulty);

            Console.WriteLine("Enter iteration: ");
            var iterationString = Console.ReadLine();
            int.TryParse(iterationString, out int iteration);

            for (int i = 0; i < iteration; i++)
            {
                try
                {
                    GenerateBlock(Difficulty);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }

        private static void GenerateBlock(int Difficulty)
        {
            string UserHash = AddUser();
            Stopwatch stopwatch = Stopwatch.StartNew();
            GenerateBlock(UserHash, Difficulty);
            stopwatch.Stop();
            Console.WriteLine("Block get in " + stopwatch.ElapsedMilliseconds + "ms");
        }

        private static string AddUser()
        {
            //Random strings, to randomize data
            Users user = new Users(RandomStrings.RandomString(5), RandomStrings.RandomString(15));
            user.SaveUserInDatabase();
            Console.WriteLine("Added user, ID: " + user.ID);
            return Hash.SHA512HashUser(user);
        }

        private static void GenerateBlock(string Data, int Difficulty)
        {
            var prevBlockHash = Helper.Block.GetLastBlock().Hash;
            Blockchain Block = new Blockchain(Timestamp.Get(), Data, string.Empty, prevBlockHash, 0);
            var BlockHash = ValidHash(ref Block, Difficulty);
            Block.Hash = BlockHash;
            Block.AddBlock();

            Console.WriteLine("Added block to blockchain: " + Block.Index);
            Console.WriteLine("Block hash: " + BlockHash);
        }

        //1 level of difficutly - 1 zero before rest of the hash
        private static string ValidHash(ref Blockchain block, int Difficulty)
        {
            string ProofOfWorkString = new string('0', Difficulty);
            string BlockHash = string.Empty;
            while (true)
            {
                BlockHash = Hash.SHA512HashBlock(ref block);
                if (!BlockHash.StartsWith(ProofOfWorkString))
                    block.Nonce++;
                else
                    break;
            }

            return BlockHash;
        }
    }
}
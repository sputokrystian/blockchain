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
            int Difficulty = 0;

            do
            {
                Console.WriteLine("Enter proof of work difficulty (1-10): ");
                var difficultyString = Console.ReadLine();
                int.TryParse(difficultyString, out Difficulty);
            } while (!(Difficulty > 0 && Difficulty < 10));

            Console.WriteLine("Enter iteration: ");
            var iterationString = Console.ReadLine();
            int.TryParse(iterationString, out int iteration);

            for (int i = 0; i < iteration; i++)
            {
                try
                {
                    string UserHash = AddUser(); // Generate some data for block

                    //Simulate concurate miners
                    Blockchain block = null;
                    Thread thread1 = new Thread(u => block = GenerateBlock(Difficulty, UserHash));
                    Thread thread2 = new Thread(u => block = GenerateBlock(Difficulty, UserHash));
                    Thread thread3 = new Thread(u => block = GenerateBlock(Difficulty, UserHash));
                    Thread thread4 = new Thread(u => block = GenerateBlock(Difficulty, UserHash));

                    thread1.Start();
                    thread2.Start();
                    thread3.Start();
                    thread4.Start();

                    //Wait for solution
                    while (block == null)
                    {
                    }

                    //Stop mining, we've got it
                    thread1.Abort();
                    thread2.Abort();
                    thread3.Abort();
                    thread4.Abort();

                    //Add it to DB
                    block.AddBlock();
                    Console.WriteLine();
                    Console.WriteLine("Found block! ID: " + block.Index + " Hash: " + block.Hash);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Enter to close");
            Console.ReadKey();
        }

        private static string AddUser()
        {
            //Random strings, to randomize data
            Users user = new Users(RandomStrings.RandomString(5), RandomStrings.RandomString(15));
            user.SaveUserInDatabase();
            Console.WriteLine("Added user, ID: " + user.ID);
            return Hash.SHA512HashUser(user);
        }

        private static Blockchain GenerateBlock(int Difficulty, string Data)
        {
            var prevBlockHash = Helper.Block.GetLastBlock().Hash;
            Blockchain Block = new Blockchain(Timestamp.Get(), Data, string.Empty /*We don't have hash yet, only data*/, prevBlockHash, 0);
            var BlockHash = ValidHash(ref Block, Difficulty);
            Block.Hash = BlockHash; //Here we have valid hash
            return Block; //valid block, but we don't know it is first solution
        }

        //1 level of difficutly - 1 zero before rest of the hash
        private static string ValidHash(ref Blockchain block, int Difficulty)
        {
            string ProofOfWorkString = new string('0', Difficulty);
            string BlockHash = string.Empty;
            //Try unless is valid hash for actual difficulty
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
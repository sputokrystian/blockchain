using BlockchainExample.Helper;
using BlockchainExample.Model;
using System;
using System.Diagnostics;
using System.Threading;

namespace BlockchainExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int Difficulty = 0;
            int Iteration = 0;

            do
            {
                Console.WriteLine("Enter proof of work difficulty (1-10) or Validate Blockchain (V): ");
                var difficultyString = Console.ReadLine();
                if (difficultyString == "V")
                {
                    Blockchain.ValidateBlockchain();
                }
                int.TryParse(difficultyString, out Difficulty);
            } while (!(Difficulty > 0 && Difficulty < 10));

            do
            {
                Console.WriteLine("Enter iteration: ");
                var iterationString = Console.ReadLine();
                int.TryParse(iterationString, out Iteration);
            } while (Iteration == 0);

            for (int i = 0; i < Iteration; i++)
            {
                try
                {
                    string DataHash = AddData(); // Generate some data for block

                    //Simulate concurate miners
                    Blockchain block = null;
                    Thread thread1 = new Thread(u => block = GenerateBlock(Difficulty, DataHash));
                    Thread thread2 = new Thread(u => block = GenerateBlock(Difficulty, DataHash));
                    Thread thread3 = new Thread(u => block = GenerateBlock(Difficulty, DataHash));
                    Thread thread4 = new Thread(u => block = GenerateBlock(Difficulty, DataHash));

                    thread1.Start();
                    thread2.Start();
                    thread3.Start();
                    thread4.Start();

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    //Wait for solution
                    while (block == null)
                    {

                    }
                    stopwatch.Stop();
                   

                    //Stop mining, we've got it
                    thread1.Abort();
                    thread2.Abort();
                    thread3.Abort();
                    thread4.Abort();

                    //Add it to DB
                    block.AddBlock();
                    Console.WriteLine();
                    Console.WriteLine("Found block! ID: " + block.Index + " in " + stopwatch.ElapsedMilliseconds + "ms");
                    Console.WriteLine("Hash: " + block.Hash);
                    Console.WriteLine("Previous Hash: " + block.PrevHash);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Enter to close");
            Console.ReadKey();
        }

        private static string AddData()
        {
            //Random strings, to randomize data
            Data data = new Data(RandomStringsHelper.RandomString(5), RandomStringsHelper.RandomString(15));
            data.SaveDataInDatabase();
            //Console.WriteLine("Added data, ID: " + data.ID);
            return HashHelper.SHA512HashData(data);
        }

        private static Blockchain GenerateBlock(int Difficulty, string Data)
        {
            Blockchain Block = new Blockchain();
            Block.PrevHash = Block.GetLastBlock().Hash;
            Block.Hash = ValidHash(ref Block, Difficulty);
            Block.TimeStamp = TimestampHelper.Get();

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
                BlockHash = HashHelper.SHA512HashBlock(ref block);
                if (!BlockHash.StartsWith(ProofOfWorkString))
                    block.Nonce++;
                else
                    break;
            }

            return BlockHash;
        }
    }
}
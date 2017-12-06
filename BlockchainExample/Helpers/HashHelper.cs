using BlockchainExample.Model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainExample.Helper
{
    public class HashHelper
    {
        public static string SHA512HashData(Data data)
        {
            SHA512 hasher = SHA512.Create();
            var hash = hasher.ComputeHash(ObjectToByteHelper.ObjectToByteArray(Encoding.Default.GetBytes(data.ID.ToString() + data.Name + data.SomeData)));
            return Convert.ToBase64String(hash);
        }

        public static string SHA512HashBlock(ref Blockchain block)
        {
            SHA512 hasher = SHA512Managed.Create();
            var hash = hasher.ComputeHash(ObjectToByteHelper.ObjectToByteArray(Encoding.Default.GetBytes(block.Data + block.Index + block.PrevHash + block.TimeStamp + block.Nonce)));
            return Convert.ToBase64String(hash);
        }
    }
}
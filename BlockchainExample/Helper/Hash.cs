using BlockchainExample.Model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainExample.Helper
{
    public class Hash
    {
        public static string SHA512HashUser(Users user)
        {
            SHA512 hasher = SHA512.Create();
            var hash = hasher.ComputeHash(ObjectToByte.ObjectToByteArray(Encoding.Default.GetBytes(user.ID.ToString() + user.Name + user.SomeData)));
            return Convert.ToBase64String(hash);
        }

        public static string SHA512HashBlock(Blockchain block)
        {
            SHA512 hasher = SHA512Managed.Create();
            var hash = hasher.ComputeHash(ObjectToByte.ObjectToByteArray(Encoding.Default.GetBytes(block.Data + block.Index + block.PrevHash + block.TimeStamp)));
            return Convert.ToBase64String(hash);
        }
    }
}
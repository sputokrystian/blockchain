using System;

namespace BlockchainExample.Helper
{
    public class Timestamp
    {
        public static int Get()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
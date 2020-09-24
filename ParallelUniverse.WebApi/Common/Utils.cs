using System;
using System.Security.Cryptography;

namespace ParallelUniverse.WebApi
{
    public static class Utils
    {
        public static string GetRandomString(int dataLen)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomData = new byte[dataLen];
            rng.GetNonZeroBytes(randomData);
            return Convert.ToBase64String(randomData);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgileTrace.Services
{
    public class SignService
    {
        public static string MakeApiSign(string securityKey, string time, string requestId)
        {
            var t = $"{requestId}{securityKey}{time}";
            return Md5(t).ToUpper();
        }

        public static string Md5(string plain)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(plain);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

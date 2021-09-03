using BoOp.Business.IO;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BoOp.UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string pass = "myPassword";

            Console.WriteLine(pass);

            string pass2 = "notmyPass";
            Console.WriteLine(pass2);

            if(Hash(pass) == Hash(pass2))
            {
                Console.WriteLine("the hashes are equal");
            }
            else
            {
                Console.WriteLine("the hashes are NOT equal");
            }
        }

        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
            Scanner scanner = new Scanner();

            scanner.BarcodeScanned += Scanner_BarcodeScanned;

        }

        private static void Scanner_BarcodeScanned(Barcode barcode)
        {
            Console.WriteLine("Barcode gefunden mit Text: {0}", barcode.Text);
        }
    }
}


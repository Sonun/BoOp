using BoOp.Business;
using BoOp.Business.IO;
using BoOp.DBAccessor.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BoOp.UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var bl = new List<BuchModel>();
            BuchModel bm;
            for (int i = 0; i < 10; i++)
            {
                bm = new BuchModel();
                bm.Titel = i % 2 == 0 ? "JK Rowling " + i : "Walter Tevis " + i*2;
                bl.Add(bm);
            }

            bl = Utils.SearchForWordInBooklist(bl, "wli");

            foreach(var book in bl)
            {
                Console.WriteLine(book.Titel);
            }
        }

        static void TestHash()
        {
            string pass = "myPassword";

            Console.WriteLine(pass);

            string pass2 = "notmyPass";
            Console.WriteLine(pass2);

            if (Hash(pass) == Hash(pass2))
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
        }

        private static void Scanner_BarcodeScanned(Barcode barcode)
        {
            Console.WriteLine("Barcode gefunden mit Text: {0}", barcode.Text);
        }
    }
}


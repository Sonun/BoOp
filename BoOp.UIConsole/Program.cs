using BoOp.Business;
using BoOp.Business.IO;
using BoOp.DBAccessor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace BoOp.UIConsole
{
    class Program
    {
        static private ILibrary _library;
        static void Main(string[] args)
        {
            testAddBook();
        }

        static void testAddBook()
        {
            var lib = new Library();
            var book = new BuchModel() { BasicInfos = new BasicBuchModel() { Altersvorschlag = "ab 12", Auflage = 2, Author = "Domi", ISBN="213231-3123", Regal= "5A",} };
            lib.AddBook(book);
            Console.WriteLine("Book added.");
            var books = lib.GetAllBooks();
            Console.ReadLine();

        }


        static void testbarcode()
        {
            var temp = Utils.GenerateUniqueBarcode("herr der ringe");
            Console.WriteLine(temp);
        }

        static void testsortign()
        {
            Library l = new Library();
            var bl = l.GetAllBooks();

            bl = Utils.SortedBookListByRating(bl);

            foreach (var book in bl)
            {
                Console.WriteLine(book.BasicInfos.Titel);
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


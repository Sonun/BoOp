using BoOp.Business;
using BoOp.DBAccessor.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BoOp.UIConsole
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: Program.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 1/09/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen, Aynur Sabri, Florian Heinebrodt
    //Beschreibung : Klasse zum Testen von Methoden aus der   Utils.cs   und der   Library.cs   klasse
    //              ( die meisten tests wurden gelöscht um einen überblick zu haben)
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    class Program
    {
        static private ILibrary _library;

        static void Main(string[] args)
        {
            testBarcodePDF();
        }

        static void testBarcodePDF()
        {
            List<(string barcode, string name, string klasse)> tupelList = new List<(string barcode, string name, string klasse)>();
            BuchModel n = new BuchModel();
            n.BasicInfos = new BasicBuchModel();
            n.BasicInfos.Id = 193;

            tupelList.Add((Utils.GenerateUniqueUserIDString(), "Thorsten Sommerfeld", "11BE10"));
            Utils.GenerateMultipleBarcodePDF(tupelList, true, "dominik", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            tupelList.Clear();
            tupelList.Add((Utils.GenerateUniqueBookBarcodeString(n), "Rapunzel und der Stein der Weisen", ""));
            tupelList.Add((Utils.GenerateUniqueBookBarcodeString(n), "Rapunzel und der Stein der Weisen, Teil 1: der Turm der Schande", ""));

            Utils.GenerateMultipleBarcodePDF(tupelList, false, "dominik", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        }

        static void testaddUser()
        {
            var geb = new DateTime(1996, 07, 20);

            var model = new PersonModel { Vorname="Dominik", Nachname="v.M.", EMail="sad@sad.sad", GeburtsdatumDateTime=geb, GeburtsdatumString = geb.ToString("d"), PasswortHash=Utils.HashSHA("qwe"), Rechte=Rechtelevel.ADMIN, Telefonnummer="123" };

            _library.AddUser(model);
        }

        static void testAddBook()
        {
            var lib = new Library();
            var book = new BuchModel() { BasicInfos = new BasicBuchModel() { Altersvorschlag = "ab 12", Auflage = 2, Author = "Domi",  ISBN="213231-3123", Titel="Das Leben.", Verlag="Selfmade", Regal= "5A" } };
            lib.AddBook(book);
            Console.WriteLine("Book added.");
            var books = lib.GetAllBooks();
            Console.ReadLine();

        }

        static void testbarcode()
        {
            var temp = Utils.GenerateUniqueBookBarcodeString(new BuchModel() { BasicInfos = new BasicBuchModel() { Altersvorschlag = "ab 12", Auflage = 2, Author = "Domi", ISBN = "213231-3123", Titel = "Das Leben.", Verlag = "Selfmade", Regal = "5A" } });
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
    }
}


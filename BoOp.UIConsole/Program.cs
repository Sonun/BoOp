using BoOp.Business.IO;
using System;

namespace BoOp.UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner();

            scanner.BarcodeScanned += Scanner_BarcodeScanned;

        }

        private static void Scanner_BarcodeScanned(Barcode barcode)
        {
            Console.WriteLine("Barcode gefunden mit Text: {0}", barcode.Text);
        }
    }
}

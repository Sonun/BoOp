using BoOp.Business;
using System;

namespace BoOp.UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(SQLDataAccess.GetConncetionString());
            Console.ReadLine();
        }
    }
}

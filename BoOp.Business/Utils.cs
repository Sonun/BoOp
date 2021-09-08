﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BoOp.DBAccessor.Models;

namespace BoOp.Business
{
    public static class Utils
    {
        /// <summary>
        /// create a SHA 256 Hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HashSHA(string input)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// sorted with bubblesort
        /// </summary>
        /// <param name="bookList"></param>
        /// <returns></returns>
        public static List<BuchModel> SortedBookListByTitel(List<BuchModel> bookList)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int comparison = String.Compare(bookList[i].Titel, bookList[i + 1].Titel, comparisonType: StringComparison.OrdinalIgnoreCase);

                    if (comparison > 0)
                    {
                        tempBook = bookList[i + 1];
                        bookList[i + 1] = bookList[i];
                        bookList[i] = tempBook;
                    }
                }
            }
            return bookList;
        }

        /// <summary>
        /// sorted with bubblesort
        /// </summary>
        /// <param name="bookList"></param>
        /// <returns></returns>
        public static List<BuchModel> SortedBookListByAuthor(List<BuchModel> bookList)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int comparison = String.Compare(bookList[i].Author, bookList[i + 1].Author, comparisonType: StringComparison.OrdinalIgnoreCase);

                    if (comparison > 0)
                    {
                        tempBook = bookList[i + 1];
                        bookList[i + 1] = bookList[i];
                        bookList[i] = tempBook;
                    }
                }
            }
            return bookList;
        }

        /// <summary>
        /// sorted with bubblesort
        /// </summary>
        /// <param name="bookList"></param>
        /// <returns></returns>
        public static List<BuchModel> SortedBookListByRating(List<BuchModel> bookList)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int throughCut1 = 0;

                    foreach (var eachRez in bookList[i].Rezensionen)
                    {
                        throughCut1 += eachRez.Sterne;
                    }

                    throughCut1 = throughCut1 / bookList[i].Rezensionen.Count;

                    int throughCut2 = 0;

                    foreach (var eachRez in bookList[i].Rezensionen)
                    {
                        throughCut2 += eachRez.Sterne;
                    }

                    throughCut2 = throughCut2 / bookList[i].Rezensionen.Count;

                    if (throughCut1 > throughCut2)
                    {
                        tempBook = bookList[i + 1];
                        bookList[i + 1] = bookList[i];
                        bookList[i] = tempBook;
                    }
                }
            }
            return bookList;
        }
    }
}

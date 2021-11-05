using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// searches vor a vorname in a userlist
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ObservableCollection<PersonModel> SearchVornameInUserlist(ObservableCollection<PersonModel> userlsit, string searchWord)
        {
            //get rid of the reference
            var useList = new ObservableCollection<PersonModel>(userlsit);

            var temp = useList
                .Where(x => x.Vorname.ToLower()
                .Contains(searchWord.ToLower()))
                .ToList();

            ObservableCollection<PersonModel> outList = new ObservableCollection<PersonModel>();
            foreach (var each in temp)
            {
                outList.Add(each);
                useList.Remove(each);
            }

            return SortedUserlistByVorname(outList);
        }
        

        /// <summary>
        /// sorts the userlist by vorname, bubblesort
        /// </summary>
        /// <param name="list"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static ObservableCollection<PersonModel> SortedUserlistByVorname(ObservableCollection<PersonModel> list, bool reverse = false)
        {
            PersonModel tempUser;

            for (int j = 0; j <= list.Count - 2; j++)
            {
                for (int i = 0; i <= list.Count - 2; i++)
                {
                    int comparison = string.Compare(list[i].Vorname, list[i + 1].Vorname, comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// sorts the userlist by nachname, bubblesort
        /// </summary>
        /// <param name="list"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static ObservableCollection<PersonModel> SortedUserlistByNachname(ObservableCollection<PersonModel> list, bool reverse = false)
        {
            PersonModel tempUser;

            for (int j = 0; j <= list.Count - 2; j++)
            {
                for (int i = 0; i <= list.Count - 2; i++)
                {
                    int comparison = string.Compare(list[i].Nachname, list[i + 1].Nachname, comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// sorts the userlist by rechte, bubblesort
        /// </summary>
        /// <param name="list"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static ObservableCollection<PersonModel> SortedUserlistByRechte(ObservableCollection<PersonModel> list, bool reverse = false)
        {
            PersonModel tempUser;

            for (int j = 0; j <= list.Count - 2; j++)
            {
                for (int i = 0; i <= list.Count - 2; i++)
                {
                    int comparison = string.Compare(list[i].Rechte.ToString().ToLower(), list[i + 1].Rechte.ToString().ToLower(), comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempUser = list[i + 1];
                            list[i + 1] = list[i];
                            list[i] = tempUser;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// search for all book titles containing a string
        /// </summary>
        /// <param name="bookList"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public static ObservableCollection<BuchModel> SearchForTitleInBooklist(ObservableCollection<BuchModel> bookList, string searchWord)
        {
            //get rid of the reference
            var useList = new ObservableCollection<BuchModel>(bookList);

            var temp = useList
                .Where(x => x.BasicInfos.Titel.ToLower()
                .Contains(searchWord.ToLower()))
                .ToList();

            ObservableCollection<BuchModel> outList = new ObservableCollection<BuchModel>();
            foreach (var each in temp)
            {
                outList.Add(each);
                useList.Remove(each);
            }

            return SortedBookListByTitel(outList);
        }

        /// <summary>
        /// search for all books containing a string
        /// </summary>
        /// <param name="bookList"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public static ObservableCollection<BuchModel> SearchForAuthorInBooklist(ObservableCollection<BuchModel> bookList, string searchWord)
        {
            //get rid of the reference
            var useList = new ObservableCollection<BuchModel>(bookList);

            var temp = useList
                .Where(x => x.BasicInfos.Author.ToLower()
                .Contains(searchWord.ToLower()))
                .ToList();

            ObservableCollection<BuchModel> outList = new ObservableCollection<BuchModel>();
            foreach (var each in temp)
            {
                outList.Add(each);
                useList.Remove(each);
            }

            return SortedBookListByTitel(outList);
        }

        /// <summary>
        /// search for all books containing a string
        /// </summary>
        /// <param name="bookList"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public static ObservableCollection<BuchModel> SearchForGenreInBooklist(ObservableCollection<BuchModel> bookList, string searchWord)
        {
            //get rid of the reference
            var useList = new ObservableCollection<BuchModel>(bookList);
            ObservableCollection<BuchModel> outList = new ObservableCollection<BuchModel>();

            foreach (var book in useList)
            {
                foreach (var genre in book.Genres)
                {
                    if (genre.ToLower().Equals(searchWord))
                    {
                        outList.Add(book);
                        break;
                    }
                }
            }

            return SortedBookListByTitel(outList);
        }

        /// <summary>
        /// search for all books containing a string
        /// </summary>
        /// <param name="bookList"></param>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        public static ObservableCollection<BuchModel> SearchForSchlagwortInBooklist(ObservableCollection<BuchModel> bookList, string searchWord)
        {
            //get rid of the reference
            var useList = new ObservableCollection<BuchModel>(bookList);
            ObservableCollection<BuchModel> outList = new ObservableCollection<BuchModel>();

            foreach (var book in useList)
            {
                foreach (var schlagwort in book.Schlagwoerter)
                {
                    if (schlagwort.ToLower().Equals(searchWord))
                    {
                        outList.Add(book);
                        break;
                    }
                }
            }

            return SortedBookListByTitel(outList);
        }

        /// <summary>
        /// sorted with bubblesort
        /// </summary>
        /// <param name="bookList"></param>
        /// <returns></returns>
        public static ObservableCollection<BuchModel> SortedBookListByTitel(ObservableCollection<BuchModel> bookList, bool reverse = false)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int comparison = string.Compare(bookList[i].BasicInfos.Titel, bookList[i + 1].BasicInfos.Titel, comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
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
        public static ObservableCollection<BuchModel> SortedBookListByAuthor(ObservableCollection<BuchModel> bookList, bool reverse = false)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int comparison = string.Compare(bookList[i].BasicInfos.Author, bookList[i + 1].BasicInfos.Author, comparisonType: StringComparison.OrdinalIgnoreCase);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
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
        public static ObservableCollection<BuchModel> SortedBookListByRating(ObservableCollection<BuchModel> bookList, bool reverse = false)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int throughCut1 = 0;

                    foreach (var eachRez in bookList[i].Rezensionen)
                    {
                        throughCut1 += eachRez.BasicInfos.Sterne;
                    }

                    if (bookList[i].Rezensionen.Count != 0)
                        throughCut1 /= bookList[i].Rezensionen.Count;

                    int throughCut2 = 0;

                    foreach (var eachRez in bookList[i + 1].Rezensionen)
                    {
                        throughCut2 += eachRez.BasicInfos.Sterne;
                    }

                    if (bookList[i + 1].Rezensionen.Count != 0)
                        throughCut2 /= bookList[i + 1].Rezensionen.Count;

                    if (!reverse)
                    {
                        if (throughCut2 > throughCut1)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
                    }
                    else
                    {
                        if (throughCut2 < throughCut1)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
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
        public static ObservableCollection<BuchModel> SortedBookListByISBN(ObservableCollection<BuchModel> bookList, bool reverse = false)
        {
            BuchModel tempBook;

            for (int j = 0; j <= bookList.Count - 2; j++)
            {
                for (int i = 0; i <= bookList.Count - 2; i++)
                {
                    int comparison = string.Compare(bookList[i].BasicInfos.ISBN, bookList[i + 1].BasicInfos.ISBN, comparisonType: StringComparison.OrdinalIgnoreCase);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            tempBook = bookList[i + 1];
                            bookList[i + 1] = bookList[i];
                            bookList[i] = tempBook;
                        }
                    }
                        
                }
            }
            return bookList;
        }

        /// <summary>
        /// BubbleSort algirithm for bookratings
        /// </summary>
        /// <param name="rezensionModels"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static List<RezensionModel> SortReviewsByRating(List<RezensionModel> rezensionModels, bool reverse = false)
        {
            RezensionModel tempReview;

            for (int j = 0; j <= rezensionModels.Count - 2; j++)
            {
                for (int i = 0; i <= rezensionModels.Count - 2; i++)
                {
                    if (!reverse)
                    {
                        if (rezensionModels[i].BasicInfos.Sterne < rezensionModels[i + 1].BasicInfos.Sterne)
                        {
                            tempReview = rezensionModels[i + 1];
                            rezensionModels[i + 1] = rezensionModels[i];
                            rezensionModels[i] = tempReview;
                        }
                    }
                    else
                    {
                        if (rezensionModels[i].BasicInfos.Sterne > rezensionModels[i + 1].BasicInfos.Sterne)
                        {
                            tempReview = rezensionModels[i + 1];
                            rezensionModels[i + 1] = rezensionModels[i];
                            rezensionModels[i] = tempReview;
                        }
                    }
                }
            }
            return rezensionModels;
        }

        /// <summary>
        /// create a unique abrcode and save pdf file in projet directory
        /// Example for a Barcode: BoOp0000010
        /// </summary>
        /// <param BuchModel="book">das buch für das ein neuer barcode erstellt werden soll</param>
        /// <returns>returns the new barcode for the book</returns>
        public static string GenerateUniqueBookBarcodeString(BuchModel book)
        {
            // Beispiel für einen Barcode:
            // BoOp0000010

            //add leading 0's untill the lenght is 6
            var barcodeId = book.BasicInfos.Id.ToString();
            while (barcodeId.Length <= 6)
            {
                barcodeId = "0" + barcodeId;
            }

            int exCount = 0;

            if (book.Exemplare != null)
            {
                for (int i = 0; i < book.Exemplare.Count; i++)
                {
                    if (book.Exemplare[i].BasicInfos.Barcode == "" || book.Exemplare[i].BasicInfos.Barcode == null)
                    {
                        exCount = i;
                        break;
                    }
                }
            }

            var barcodeAsString = "BoOp" + barcodeId + exCount;

            //GeneratedBarcode MyBarCode = BarcodeWriter.CreateBarcode(barcodeAsString, BarcodeWriterEncoding.Code128);
            //MyBarCode.SaveAsPdf(book.BasicInfos.Titel + ".pdf");

            return barcodeAsString;
        }

        public static string GenerateUniqueUserBarcodeString(PersonModel user)
        {
            // Beispiel für einen Barcode:
            // Usr0000010

            if (user != null)
            {
                return null;
            }

            //add leading 0's untill the lenght is 6
            var barcodeId = user.Id.ToString();
            while (barcodeId.Length <= 6)
            {
                barcodeId = "0" + barcodeId;
            }

            var barcodeAsString = "Usr" + barcodeId;

            //GeneratedBarcode MyBarCode = BarcodeWriter.CreateBarcode(barcodeAsString, BarcodeWriterEncoding.Code128);
            //MyBarCode.SaveAsPdf(user.Vorname + " " + user.Nachname + ".pdf");

            return barcodeAsString;
        }
    }
}
using BoOp.DBAccessor;
using BoOp.DBAccessor.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace BoOp.Business

{   ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: Library.cs
    //Author : Manuel Janzen
    //Erstellt am : 1/9/2021
    //Bearbeitet von : Aynur Sabri, Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Implementierung des Interfaces ILibrary, und somit die Schnittstelle zur Datenbank
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Library : ILibrary
    {
        private readonly string _connectionString;
        private SQLiteDataAccessor _db;

        public Library(string connectionString = null)
        {
            _db = new SQLiteDataAccessor();

            if (connectionString == null)
            {
                _connectionString = SQLiteDataAccessor.GetConncetionString();
            }
        }

        public ObservableCollection<BuchModel> GetAllBooks()
        {
            // ToDo: Map the references, genres, and key words to the books as wel
            string sqlBasicBooks = "SELECT * FROM Buecher";
            var basicBooks = _db.LoadData<BasicBuchModel, dynamic>(sqlBasicBooks, new { }, _connectionString);

            string sqlBasicBuchGenres = "SELECT * FROM BuchGenres";
            var basicBuchGenres = _db.LoadData<BasicBuchGenresModel, dynamic>(sqlBasicBuchGenres, new { }, _connectionString);

            string sqlBasicBuchSchlagwoerter = "SELECT * FROM BuchSchlagwoerter";
            var basicBuchSchlagwoerter = _db.LoadData<BasicBuchSchlagwoerterModel, dynamic>(sqlBasicBuchSchlagwoerter, new { }, _connectionString);

            string sqlBasicGenres = "SELECT * FROM Genre";
            var basicGenres = _db.LoadData<BasicGenreModel, dynamic>(sqlBasicGenres, new { }, _connectionString);

            string sqlBasicPersonen = "SELECT * FROM Personen";
            var basicPersonen = _db.LoadData<PersonModel, dynamic>(sqlBasicPersonen, new { }, _connectionString);

            string sqlBasicRezensionen = "SELECT * FROM Rezensionen";
            var basicRezensionen = _db.LoadData<BasicRezensionenModel, dynamic>(sqlBasicRezensionen, new { }, _connectionString);

            string sqlBasicSchlagwoerter = "SELECT * FROM Schlagwoerter";
            var basicSchlagwoerter = _db.LoadData<BasicSchlagwoerterModel, dynamic>(sqlBasicSchlagwoerter, new { }, _connectionString);

            string sqlBasicExemplare = "SELECT * FROM Exemplare";
            var basicExemplare = _db.LoadData<BasicExemplarModel, dynamic>(sqlBasicExemplare, new { }, _connectionString);

            var allBooks = new ObservableCollection<BuchModel>();
            basicBooks.ForEach(x => allBooks.Add(new BuchModel()
            {
                BasicInfos = x,
                Genres = (from buchgenres in basicBuchGenres
                          join book in basicBooks on buchgenres.BuchID equals book.Id
                          join genre in basicGenres on buchgenres.GenreID equals genre.Id
                          where x.Id == buchgenres.BuchID
                          select genre.Genrename).ToList(),
                Exemplare = (from exemplare in basicExemplare
                             join book in basicBooks on exemplare.BuchID equals book.Id
                             where x.Id == book.Id
                             select new ExemplarModel() 
                             { 
                                 BasicInfos = new BasicExemplarModel() 
                                 { 
                                     Id = exemplare.Id, 
                                     Barcode = exemplare.Barcode, 
                                     BuchID = exemplare.BuchID, 
                                     AusleiherID = exemplare.AusleiherID,
                                     AusleihDatumString = exemplare.AusleihDatumString
                                 },
                                 LendBy = (from personen in basicPersonen
                                           join exemplar in basicExemplare on personen.Id equals exemplar.AusleiherID
                                           where exemplare.AusleiherID == personen.Id
                                           select new PersonModel()
                                           {
                                               Id = personen.Id,
                                               EMail = personen.EMail,
                                               GeburtsdatumString = personen.GeburtsdatumString,
                                               Nachname = personen.Nachname,
                                               Rechte = personen.Rechte,
                                               Telefonnummer = personen.Telefonnummer,
                                               Vorname = personen.Vorname,
                                               AusweisID = personen.AusweisID
                                           }).FirstOrDefault()
                             }).ToList(),

                Rezensionen = (from buchrezensionen in basicRezensionen
                               join book in basicBooks on buchrezensionen.BuchID equals book.Id
                               where x.Id == buchrezensionen.BuchID
                               select new RezensionModel()
                               {
                                   BasicInfos = new BasicRezensionenModel() { BuchID = buchrezensionen.BuchID, PersonID = buchrezensionen.PersonID, Id = buchrezensionen.Id, Rezensionstext = buchrezensionen.Rezensionstext, Sterne = buchrezensionen.Sterne },
                                   Author = basicPersonen.Where(z => z.Id == buchrezensionen.PersonID).FirstOrDefault()
                               }).ToList(),

                Schlagwoerter = (from buchschlagwoerter in basicBuchSchlagwoerter
                                 join book in basicBooks on buchschlagwoerter.BuchID equals book.Id
                                 join schlagwoerter in basicSchlagwoerter on buchschlagwoerter.SchlagwortID equals schlagwoerter.Id
                                 where buchschlagwoerter.BuchID == x.Id
                                 select schlagwoerter.Wort).ToList()

            }));

            return allBooks;
        }

        public int GetIdByISBN(BuchModel book)
        {
            string sql = "SELECT Id FROM Buecher WHERE ISBN = @ISBN;";
            return _db.LoadData<int, dynamic>(sql, new { book.BasicInfos.ISBN }, _connectionString).FirstOrDefault();
        }

        public void AddBook(BuchModel book)
        {
            // Check if book is already in DB
            string sql = "SELECT * FROM Buecher WHERE ISBN = @ISBN;";
            var bookCheck  = _db.LoadData<BasicBuchModel, dynamic>(
                 sql,
                 new { book.BasicInfos.ISBN }, _connectionString).FirstOrDefault();

            if (bookCheck == null)
            {
                // Save the basic book
                sql = "insert into Buecher (Titel, Author, Verlag, Auflage, ISBN, Altersvorschlag, Regal, BildPfad, Beschreibung) values (@Titel, @Author, @Verlag, @Auflage, @ISBN, @Altersvorschlag, @Regal, @BildPfad, @Beschreibung);";
                _db.SaveData(sql,
                        new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Verlag, book.BasicInfos.Auflage, book.BasicInfos.ISBN, book.BasicInfos.Altersvorschlag, book.BasicInfos.Regal, book.BasicInfos.BildPfad, book.BasicInfos.Beschreibung },
                        _connectionString);
            }
            else
            {
                Debug.WriteLine("Book with same Titel, Author and ISBN exists already in DB. instead of adding, edited the book");
                throw new Exception("Buch mit dem Titel, Author und ISBN existiert bereits in der Datenbank.");
            }
        }

        public void ReturnBook(string bookBarcode)
        {
            string sqlString = "UPDATE Exemplare " +
                 "SET AusleiherID = NULL, " +
                 "AusleihDatumString = NULL " +
                 "WHERE Barcode = @bookBarcode";

            _db.SaveData(sqlString, new { bookBarcode = bookBarcode }, _connectionString);
        }

        public PersonModel GetUserByID(int id)
        {
            string sql = "SELECT * FROM Personen WHERE Id = @id";
            var person = _db.LoadData<PersonModel, dynamic>(sql, new { id }, _connectionString);

            return person.FirstOrDefault();
        }

        public void EditBasicBookDetails(BasicBuchModel basicBuchModel)
        {
            string sqlstring = $"UPDATE Buecher SET Id = @Id, ISBN =@ISBN, Regal=@Regal, Titel=@Titel, Auflage=@Auflage, Verlag=@Verlag, Altersvorschlag=@Altersvorschlag, BildPfad=@BildPfad, Beschreibung=@Beschreibung WHERE Id = @Id; ";
            _db.SaveData( sqlstring, new { basicBuchModel.Id, basicBuchModel.ISBN, basicBuchModel.Regal, basicBuchModel.Titel, basicBuchModel.Auflage, basicBuchModel.Verlag, basicBuchModel.Altersvorschlag, basicBuchModel.BildPfad, basicBuchModel.Beschreibung }, _connectionString );
        }

        public void AddReview(RezensionModel review)
        {
            //keep it in range
            if (review.BasicInfos.Sterne > 5) review.BasicInfos.Sterne = 5;
            if (review.BasicInfos.Sterne < 0) review.BasicInfos.Sterne = 0;

            string sql = "SELECT BuchID FROM Rezensionen WHERE PersonID = @id";
            var bookIdsOfPerson = _db.LoadData<int, dynamic>(sql, new { id = review.BasicInfos.PersonID }, _connectionString);

            bool personAlreadyRatedThisBook = false;

            foreach (var bookId in bookIdsOfPerson)
            {
                if (bookId == review.BasicInfos.BuchID)
                {
                    //person alrdy made a rez for this book
                    personAlreadyRatedThisBook = true;
                    break;
                }
            }

            string sqlString = "";
            if (personAlreadyRatedThisBook)
            {
                sqlString = "UPDATE Rezensionen " +
                    "SET RezensionsText = @RezensionsText, " +
                    "Sterne = @Sterne " +
                    "WHERE BuchID = @BuchID AND PersonID = @PersonID";
            }
            else
            {
                sqlString = "INSERT INTO Rezensionen(BuchID, Sterne, RezensionsText, PersonID) " +
                    "VALUES(@BuchID, @Sterne, @RezensionsText, @PersonID);";
            }

            _db.SaveData(sqlString, new { review.BasicInfos.BuchID, review.BasicInfos.Sterne, review.BasicInfos.Rezensionstext, review.BasicInfos.PersonID }, _connectionString);
        }

        public void AddUser(PersonModel user)
        {
            string sqlString = "INSERT INTO Personen(Vorname, Nachname, GeburtsdatumString, Telefonnummer, Rechte, Email, PasswortHASH, AusweisID, Klassenname) " +
                "VALUES(@Vorname, @Nachname, @GeburtsdatumString, @Telefonnummer, @Rechte, @EMail, @pass, @AusweisID, @Klassenname);";

            _db.SaveData(sqlString, new { user.Vorname, user.Nachname, user.GeburtsdatumString, user.Telefonnummer, pass=user.PasswortHash, Rechte = user.Rechte, user.EMail, user.AusweisID, user.Klassenname }, _connectionString);
        }

        public void RemoveUser(PersonModel user)
        {
            string sqlstring = $"DELETE From Personen Where Id=@Id";
            _db.SaveData( sqlstring, new { user.Id }, _connectionString );
        }

        public void EditUserDetails(PersonModel user)
        {
            string sqlstring = $"UPDATE Personen SET Vorname = @Vorname, Nachname =@Nachname, PasswortHASH=@pass, GeburtsdatumString=@GeburtsdatumString, Telefonnummer=@Telefonnummer, Rechte=@Rechte, AusweisID=@ausweis, EMail=@EMail, Klassenname=@klassenname WHERE Id = @Id; ";
            _db.SaveData( sqlstring, new { user.Id, user.Vorname, user.Nachname, user.GeburtsdatumString, user.Telefonnummer, pass = user.PasswortHash, user.Rechte, user.EMail, ausweis = user.AusweisID, klassenname = user.Klassenname }, _connectionString );
        }

        public void EditBookDetails (BuchModel book, bool editMode = false)
        {
            string sql = "";
            if (editMode)
            {
                sql = "UPDATE Buecher SET Titel = @Titel, Author = @Author, Verlag = @Verlag, Auflage = @Auflage, ISBN = @ISBN, Altersvorschlag = @Altersvorschlag, Regal = @Regal, BildPfad=@BildPfad, Beschreibung=@Beschreibung WHERE Id = @Id;";
                _db.SaveData(sql,
                        new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Verlag, book.BasicInfos.Auflage, book.BasicInfos.ISBN, book.BasicInfos.Altersvorschlag, book.BasicInfos.Regal, book.BasicInfos.Id, book.BasicInfos.BildPfad, book.BasicInfos.Beschreibung },
                        _connectionString);
            }
            else
            {
                sql = "SELECT * FROM Buecher WHERE Titel = @Titel AND Author = @Author AND ISBN = @ISBN;";
                book.BasicInfos.Id = _db.LoadData<BasicBuchModel, dynamic>(
                     sql,
                     new { Titel = book.BasicInfos.Titel, Author = book.BasicInfos.Author, ISBN = book.BasicInfos.ISBN },
                     _connectionString).FirstOrDefault().Id;
            }

            // Add Barcodes to DB
            if (book.Exemplare != null)
            {
                sql = "SELECT Barcode FROM Exemplare WHERE BuchID = @BuchID;";
                var unnecessaryBarcodes = _db.LoadData<string, dynamic>(
                     sql,
                     new { BuchID = book.BasicInfos.Id },
                     _connectionString);

                foreach (var exemplar in book.Exemplare)
                {
                    // Check if Barcode already exists in DB
                    sql = "SELECT * FROM Exemplare WHERE Barcode = @Barcode;";
                    var exemplarCheck = _db.LoadData<BasicExemplarModel, dynamic>(
                         sql,
                         new { BuchID = book.BasicInfos.Id, Barcode = exemplar.BasicInfos.Barcode },
                         _connectionString).FirstOrDefault();

                    if (exemplarCheck != null)
                    {
                        unnecessaryBarcodes.Remove(exemplarCheck.Barcode);
                    }

                    if (exemplarCheck == null)
                    {
                        sql = "INSERT INTO Exemplare (BuchID, Barcode) VALUES (@BuchID, @Barcode)";
                        _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, Barcode = exemplar.BasicInfos.Barcode }, _connectionString);
                    }
                    else
                    {
                        Debug.WriteLine("Barcode is already used for a book in DB.");
                    }
                }

                // Check if book has to many entry in DB --> Remove exemplar - book references
                if (unnecessaryBarcodes.Count > 0)
                {
                    foreach (var barcode in unnecessaryBarcodes)
                    {
                        sql = "DELETE FROM Exemplare WHERE Barcode = @barcode;";
                        _db.SaveData(sql, new { barcode }, _connectionString);
                    }
                }
            }

            // Add Schlagwoerter to DB
            if (book.Schlagwoerter != null)
            {
                sql = "SELECT SchlagwortID FROM BuchSchlagwoerter WHERE BuchID = @BuchID;";
                var unnecessarySchlagwortIDs = _db.LoadData<int, dynamic>(
                     sql,
                     new { BuchID = book.BasicInfos.Id },
                     _connectionString);

                foreach (var word in book.Schlagwoerter)
                {
                    // Check if word is already in DB
                    sql = "SELECT * FROM Schlagwoerter WHERE Wort = @Wort;";
                    var schlagwort = _db.LoadData<BasicSchlagwoerterModel, dynamic>(
                        sql,
                        new { Wort = word },
                        _connectionString).FirstOrDefault();

                    // Add word if it's not entered already
                    if (schlagwort == null)
                    {
                        sql = "INSERT INTO Schlagwoerter (Wort) VALUES (@Wort);";
                        _db.SaveData(sql, new { Wort = word }, _connectionString);

                        sql = "SELECT * FROM Schlagwoerter WHERE Wort = @Wort;";
                        schlagwort = _db.LoadData<BasicSchlagwoerterModel, dynamic>(
                            sql,
                            new { Wort = word },
                            _connectionString).FirstOrDefault();
                    }

                    // Check if word is already marked as Schlagwort for that book
                    if (schlagwort != null)
                    {
                        sql = "SELECT * FROM BuchSchlagwoerter WHERE BuchID = @BuchID AND SchlagwortID = @SchlagwortID;";
                        var buchSchlagwort = _db.LoadData<BasicBuchSchlagwoerterModel, dynamic>(
                        sql,
                        new { BuchID = book.BasicInfos.Id, SchlagwortID = schlagwort.Id },
                        _connectionString).FirstOrDefault();

                        if (buchSchlagwort == null)
                        {
                            // Create reference between word and book
                            sql = "INSERT INTO BuchSchlagwoerter (BuchID, SchlagwortID) VALUES (@BuchID, @SchlagwortID)";
                            _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, SchlagwortID = schlagwort.Id }, _connectionString);
                        }

                        unnecessarySchlagwortIDs.Remove(schlagwort.Id.GetValueOrDefault());
                    }
                }
                // Check if book has to many entry in DB --> Remove word - book references
                if (unnecessarySchlagwortIDs.Count > 0)
                {
                    foreach (var buchSchlagwortID in unnecessarySchlagwortIDs)
                    {
                        sql = "DELETE FROM BuchSchlagwoerter WHERE BuchID = @BuchID AND SchlagwortID = @SchlagwortID;";
                        _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, SchlagwortID = buchSchlagwortID }, _connectionString);
                    }
                }
            }

            // Add Genres to DB
            if (book.Genres != null)
            {
                sql = "SELECT GenreID FROM BuchGenres WHERE BuchID = @BuchID;";
                var unnecessaryGenreIDs = _db.LoadData<int, dynamic>(
                     sql,
                     new { BuchID = book.BasicInfos.Id },
                     _connectionString);

                foreach (var genre in book.Genres)
                {
                    // Check if word is already in DB
                    sql = "SELECT * FROM Genre WHERE Genrename = @Genrename;";
                    var basicGenre = _db.LoadData<BasicGenreModel, dynamic>(
                        sql,
                        new { Genrename = genre },
                        _connectionString).FirstOrDefault();

                    // Add word if it's not entered already
                    if (basicGenre == null)
                    {
                        sql = "INSERT INTO Genre (Genrename) VALUES (@Genrename);";
                        _db.SaveData(sql, new { Genrename = genre }, _connectionString);

                        sql = "SELECT * FROM Genre WHERE Genrename = @Genrename;";
                        basicGenre = _db.LoadData<BasicGenreModel, dynamic>(
                            sql,
                            new { Genrename = genre },
                            _connectionString).FirstOrDefault();
                    }

                    // Check if word is already marked as Schlagwort for that book
                    if (basicGenre != null)
                    {
                        sql = "SELECT * FROM BuchGenres WHERE BuchID = @BuchID AND GenreID = @GenreID;";
                        var buchGenre = _db.LoadData<BasicBuchGenresModel, dynamic>(
                        sql,
                        new { BuchID = book.BasicInfos.Id, GenreID = basicGenre.Id },
                        _connectionString).FirstOrDefault();

                        if (buchGenre == null)
                        {
                            // Create reference between word and book
                            sql = "INSERT INTO BuchGenres (BuchID, GenreID) VALUES (@BuchID, @GenreID)";
                            _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, GenreID = basicGenre.Id }, _connectionString);
                        }
                        unnecessaryGenreIDs.Remove(basicGenre.Id.GetValueOrDefault());

                    }
                }

                // Check if book has to many entry in DB --> Remove genre - book references
                if (unnecessaryGenreIDs.Count > 0)
                {
                    foreach (var buchGenreID in unnecessaryGenreIDs)
                    {
                        sql = "DELETE FROM BuchGenres WHERE BuchID = @BuchID AND GenreID = @GenreID;";
                        _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, GenreID = buchGenreID }, _connectionString);
                    }
                }
            }

        }

        public void LendBook(int? userId, string bookBarcode)
        {
            string sqlString = "UPDATE Exemplare " +
                "SET AusleiherID = @userId, " +
                "AusleihDatumString = @datum " +
                "WHERE Barcode = @bookBarcode";

            _db.SaveData(sqlString, new { userId, datum = DateTime.Now.ToString("d"), bookBarcode }, _connectionString);
        }

        public PersonModel GetUserByBarcode(string ausweisID)
        {
            var person = new PersonModel();
            try
            {
                string sql = "SELECT * FROM Personen";
                person = _db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString)
                    .Where(x => x.AusweisID != null)
                    .Single(x => { return x.AusweisID.Equals(ausweisID); });
            }
            catch (Exception)
            {
                throw new Exception("Benutzer konnte mit diesem Barcode nicht ermittelt werden.");
            }
            return person;
        }

        public int? GetBookIdByBarcode(string barcode)
        {
            string sql = "SELECT BuchID FROM Exemplare WHERE Barcode = @barcode;";
            return _db.LoadData<int?, dynamic>(sql, new { barcode }, _connectionString).FirstOrDefault();
        }

        public ObservableCollection<PersonModel> GetAllUsers()
        {
            string sqlBasicBooks = "SELECT * FROM Personen";
            var output = _db.LoadData<PersonModel, dynamic>(sqlBasicBooks, new { }, _connectionString);

            var personen = new ObservableCollection<PersonModel>();

            foreach (var p in output)
            {
                personen.Add(p);
            }

            return personen;
        }

        public void RemoveBook(BuchModel book)
        {
            try
            {
                // Remove Exemplare
                string sql = "DELETE FROM Exemplare WHERE BuchID = @BuchID;";
                _db.SaveData(sql, new { BuchID = book.BasicInfos.Id }, _connectionString);

                // Remove BuchSchlagwoerter
                sql = "DELETE FROM BuchSchlagwoerter WHERE BuchID = @BuchID;";
                _db.SaveData(sql, new { BuchID = book.BasicInfos.Id }, _connectionString);

                // Remove BuchGenres
                sql = "DELETE FROM BuchGenres WHERE BuchID = @BuchID;";
                _db.SaveData(sql, new { BuchID = book.BasicInfos.Id }, _connectionString);

                // Remove Rezensionen
                sql = "DELETE FROM Rezensionen WHERE BuchID = @BuchID;";
                _db.SaveData(sql, new { BuchID = book.BasicInfos.Id }, _connectionString);

                // Remove BasicBook
                sql = "DELETE FROM Buecher WHERE Id = @Id;";
                _db.SaveData(sql, new { Id = book.BasicInfos.Id }, _connectionString);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public void RemoveExemplar(ExemplarModel exemplar)
        {
            try
            {
                string sql = "DELETE FROM Exemplare WHERE Id = @Id";
                _db.SaveData(sql, new { Id = exemplar.BasicInfos.Id }, _connectionString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if UserID is available
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool CheckAvailabilityUserID(string userID)
        {
            string sql = "SELECT * FROM Personen WHERE AusweisID = @AusweisID";
            var userExists = _db.LoadData<PersonModel, dynamic>(sql, new { AusweisID = userID }, _connectionString).SingleOrDefault();

            return userExists == null;
        }
    }
}

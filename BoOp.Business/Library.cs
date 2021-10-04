using BoOp.DBAccessor;
using BoOp.DBAccessor.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business
{
    public class Library : ILibrary
    {
        private PersonModel _loggedInUnser;

        private readonly string _connectionString;
        private SQLDataAccessor _db;

        public Library(string connectionString = null)
        {
            _db = new SQLDataAccessor();

            if (connectionString == null)
            {
                _connectionString = SQLDataAccessor.GetConncetionString();
            }
        }

        public ObservableCollection<BuchModel> GetAllBooks()
        {
            // ToDo: Map the references, genres, and key words to the books as wel
            string sqlBasicBooks = "SELECT * FROM dbo.Buecher";
            var basicBooks = _db.LoadData<BasicBuchModel, dynamic>(sqlBasicBooks, new { }, _connectionString);

            string sqlBasicBuchGenres = "SELECT * FROM dbo.BuchGenres";
            var basicBuchGenres = _db.LoadData<BasicBuchGenresModel, dynamic>(sqlBasicBuchGenres, new { }, _connectionString);

            string sqlBasicBuchSchlagwoerter = "SELECT * FROM dbo.BuchSchlagwoerter";
            var basicBuchSchlagwoerter = _db.LoadData<BasicBuchSchlagwoerterModel, dynamic>(sqlBasicBuchSchlagwoerter, new { }, _connectionString);

            string sqlBasicGenres = "SELECT * FROM dbo.Genre";
            var basicGenres = _db.LoadData<BasicGenreModel, dynamic>(sqlBasicGenres, new { }, _connectionString);

            string sqlBasicPersonen = "SELECT * FROM dbo.Personen";
            var basicPersonen = _db.LoadData<PersonModel, dynamic>(sqlBasicPersonen, new { }, _connectionString);

            string sqlBasicRezensionen = "SELECT * FROM dbo.Rezensionen";
            var basicRezensionen = _db.LoadData<BasicRezensionenModel, dynamic>(sqlBasicRezensionen, new { }, _connectionString);

            string sqlBasicSchlagwoerter = "SELECT * FROM dbo.Schlagwoerter";
            var basicSchlagwoerter = _db.LoadData<BasicSchlagwoerterModel, dynamic>(sqlBasicSchlagwoerter, new { }, _connectionString);

            string sqlBasicExemplare = "SELECT * FROM dbo.Exemplare";
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
                                     LendByUserID = exemplare.LendByUserID
                                 },
                                 LendBy = (from personen in basicPersonen
                                           join exemplar in basicExemplare on personen.Id equals exemplar.LendByUserID
                                           where exemplare.LendByUserID == personen.Id
                                           select new PersonModel()
                                           {
                                               Id = personen.Id,
                                               EMail = personen.EMail,
                                               Geburtsdatum = personen.Geburtsdatum,
                                               Nachname = personen.Nachname,
                                               Rechte = personen.Rechte,
                                               Telefonnummer = personen.Telefonnummer,
                                               Vorname = personen.Vorname
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

            })) ;

            return allBooks;
        }




        public void AddBook(BuchModel book)
        {
            _loggedInUnser = null;
            if(_loggedInUnser != null)
                throw new NotImplementedException();

            // ToDO: Check if book is already in DB

            // Save the basic book
            string sql = "insert into dbo.Buecher (Titel, Author, Verlag, Auflage, ISBN, Altersvorschlag, Regal) values (@Titel, @Author, @Verlag, @Auflage, @ISBN, @Altersvorschlag, @Regal );";
            _db.SaveData(sql,
                        new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Verlag, book.BasicInfos.Auflage, book.BasicInfos.ISBN, book.BasicInfos.Altersvorschlag, book.BasicInfos.Regal},
                        _connectionString);

            sql = "SELECT * FROM dbo.Buecher WHERE Titel = @Titel AND Author = @Author AND ISBN = @ISBN;";
            book.BasicInfos.Id = _db.LoadData<BasicBuchModel, dynamic>(
                 sql,
                 new { Titel = book.BasicInfos.Titel, Author = book.BasicInfos.Author, ISBN = book.BasicInfos.ISBN },
                 _connectionString).FirstOrDefault().Id;

            // Add Barcodes to DB
            if (book.Exemplare != null)
            {
                foreach (var exemplar in book.Exemplare)
                {
                    sql = "INSERT INTO dbo.Exemplare (BuchID, Barcode) VALUES (@BuchID, @Barcode)";
                    _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, Barcode = exemplar.BasicInfos.Barcode }, _connectionString);
                }
            }

            if (book.Rezensionen != null)
            {
                foreach (var review in book.Rezensionen)
                {
                    sql = "INSERT INTO dbo.Rezensionen (BuchID, Sterne, Rezensionstext, PersonID) VALUES (@Id, @Sterne, @Rezensionstext, @PersonID)";
                    _db.SaveData(sql, new { book.BasicInfos.Id, review.BasicInfos.Sterne, review.BasicInfos.Rezensionstext, review.BasicInfos.PersonID }, _connectionString);
                }
            }

            // Add Schlagwoerter to DB
            if (book.Schlagwoerter != null)
            {
                foreach (var word in book.Schlagwoerter)
                {
                    // Check if word is already in DB
                    sql = "SELECT * FROM dbo.Schlagwoerter WHERE Wort = @Wort;";
                    var schlagwort = _db.LoadData<BasicSchlagwoerterModel, dynamic>(
                        sql,
                        new { Wort = word },
                        _connectionString).FirstOrDefault();

                    // Add word if it's not entered already
                    if (schlagwort == null)
                    {
                        sql = "INSERT INTO dbo.Schlagwoerter (Wort) VALUES (@Wort);";
                        _db.SaveData(sql, new { Wort = word }, _connectionString);

                        sql = "SELECT * FROM dbo.Schlagwoerter WHERE Wort = @Wort;";
                        schlagwort = _db.LoadData<BasicSchlagwoerterModel, dynamic>(
                            sql,
                            new { Wort = word },
                            _connectionString).FirstOrDefault();
                    }

                    // Check if word is already marked as Schlagwort for that book
                    if (schlagwort != null)
                    {
                        sql = "SELECT * FROM dbo.BuchSchlagwoerter WHERE BuchID = @BuchID AND SchlagwortID = @SchlagwortID;";
                        var buchSchlagwort = _db.LoadData<BasicBuchSchlagwoerterModel, dynamic>(
                        sql,
                        new { BuchID = book.BasicInfos.Id, SchlagwortID = schlagwort.Id },
                        _connectionString).FirstOrDefault();

                        if (buchSchlagwort == null)
                        {
                            // Create reference between word and book
                            sql = "INSERT INTO dbo.BuchSchlagwoerter (BuchID, SchlagwortID) VALUES (@BuchID, @SchlagwortID)";
                            _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, SchlagwortID = schlagwort.Id }, _connectionString);
                        }
                    }
                }
            }

            // ToDo: Add Genres to DB
            if (book.Genres != null)
            {

            }
        }

        public void LendBook(int userId, string bookBarcode)
        {
            string sql = "SELECT Id " +
                "FROM Exemplare " +
                "WHERE Barcode = @bookBarcode";
            var bookId = _db.LoadData<int, dynamic>(sql, new { bookBarcode }, _connectionString).FirstOrDefault();

            string sqlString = "UPDATE Exemplare " +
                "SET LendByUserID = @userId " +
                "WHERE Id = @buchId";

            _db.SaveData(sqlString, new { userId = userId, buchId = bookId }, _connectionString);
        }

        public void ReturnBook(string bookBarcode)
        {
            string sql = "SELECT Id " +
               "FROM Exemplare " +
               "WHERE Barcode = @bookBarcode";
            var bookId = _db.LoadData<int, dynamic>(sql, new { bookBarcode }, _connectionString).FirstOrDefault();

            string sqlString = "UPDATE Exemplare " +
                "SET LendByUserID = NULL " +
                "WHERE Id = @buchId";

            _db.SaveData(sqlString, new { buchId = bookId }, _connectionString);
        }

        public PersonModel GetUserByID(int id)
        {
            string sql = "SELECT * FROM Personen WHERE Id = @id";
            var person = _db.LoadData<PersonModel, dynamic>(sql, new { id }, _connectionString);

            return person.FirstOrDefault();
        }

        public void RemoveBook(string bookBarcode)
        {
            throw new NotImplementedException();
        }

        public void EditBookDetails(BuchModel bookModel)
        {
            throw new NotImplementedException();
        }

        public void AddReview(RezensionModel review)
        {
            throw new NotImplementedException();
        }

        public void AddUser(PersonModel user)
        {
            string sqlString = "INSERT INTO Personen(Vorname, Nachname, Geburtsdatum, Telefonnummer, Rechte, Email) " +
                "VALUES(@Vorname, @Nachname, @Geburtsdatum, @Telefonnummer, @Rechte, @EMail);";

            _db.SaveData(sqlString, new { user.Vorname, user.Nachname, user.Geburtsdatum, user.Telefonnummer, Rechte = user.Rechte, user.EMail }, _connectionString);
        }

        public void RemoveUser(PersonModel user)
        {
            throw new NotImplementedException();
        }

        public void EditUserDetails(PersonModel user)
        {
            throw new NotImplementedException();
        }
    }
}

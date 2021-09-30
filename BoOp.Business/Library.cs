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

            var allBooks = new ObservableCollection<BuchModel>();
            basicBooks.ForEach(x => allBooks.Add(new BuchModel()
            {
                BasicInfos = x,
                Genres = (from buchgenres in basicBuchGenres
                          join book in basicBooks on buchgenres.BuchID equals book.Id
                          join genre in basicGenres on buchgenres.GenreID equals genre.Id
                          where x.Id == buchgenres.BuchID
                          select genre.Genrename).ToList(),
                LendBy = (from personen in basicPersonen
                            join book in basicBooks on personen.Id equals book.Id
                            where x.PersonID == personen.Id
                            select new PersonModel() 
                            { 
                                Id = personen.Id, EMail = personen.EMail, Geburtsdatum = personen.Geburtsdatum, Nachname = personen.Nachname, Rechte = personen.Rechte, Telefonnummer = personen.Telefonnummer, Vorname = personen.Vorname 
                            }).FirstOrDefault()
                ,
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
        }

        public void LendBook(PersonModel user, BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void GetUserByID(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveBook(BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void EditBookDetails(BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void AddReview(RezensionModel review)
        {
            throw new NotImplementedException();
        }

        public void AddUser(PersonModel user)
        {
            throw new NotImplementedException();
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

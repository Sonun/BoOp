using BoOp.DBAccessor;
using BoOp.DBAccessor.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business
{
    public class Library : ILibrary
    {
        private UserModel _loggedInUnser;

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

        public List<BasicBuchModel> GetAllBooks()
        {
            // ToDo: Map the references, genres, and key words to the books as wel
            string sqlBasicBooks = "SELECT Id, Titel, Author, Verlag, Auflage, ISBN, Altersvorschlag, Regal FROM dbo.Buecher";
            var basicBooks = _db.LoadData<BasicBuchModel, dynamic>(sqlBasicBooks, new { }, _connectionString);

            string sqlBasicBuchGenres = "SELECT * FROM dbo.BuchGenres";
            var basicBuchGenres = _db.LoadData<BasicBuchGenresModel, dynamic>(sqlBasicBuchGenres, new { }, _connectionString);

            string sqlBasicBuchSchlagwoerter = "SELECT * FROM dbo.BuchSchlagwoerter";
            var basicBuchSchlagwoerter = _db.LoadData<BasicBuchSchlagwoerterModel, dynamic>(sqlBasicBuchSchlagwoerter, new { }, _connectionString);

            string sqlBasicGenres = "SELECT * FROM dbo.Genre";
            var basicGenres = _db.LoadData<BasicGenreModel, dynamic>(sqlBasicGenres, new { }, _connectionString);

            string sqlBasicPersonen = "SELECT * FROM dbo.Personen";
            var basicPersonen = _db.LoadData<BasicPersonenModel, dynamic>(sqlBasicPersonen, new { }, _connectionString);

            string sqlBasicRezensionen = "SELECT * FROM dbo.Rezensionen";
            var basicRezensionen = _db.LoadData<BasicRezensionenModel, dynamic>(sqlBasicRezensionen, new { }, _connectionString);

            string sqlBasicSchlagwoerter = "SELECT * FROM dbo.Schlagwoerter";
            var basicSchlagwoerter = _db.LoadData<BasicSchlagwoerterModel, dynamic>(sqlBasicSchlagwoerter, new { }, _connectionString);

            // ToDo: Create List of BuchModel, RezensionenModel, UserModel

            var allBooks = new List<BuchModel>();
            basicBooks.ForEach(x => allBooks.Add(new BuchModel()
            {
                BasicInfos = x,
                Genres = (from buchgenres in basicBuchGenres
                          join book in basicBooks on buchgenres.BuchID equals book.Id
                          join genre in basicGenres on buchgenres.GenreID equals genre.Id
                          select genre.Genrename).ToList()
            }));

            return basicBooks;
    }




        public void AddBook(BuchModel book)
        {
            _loggedInUnser = null;
            if(_loggedInUnser != null)
                throw new NotImplementedException();
        }

        public void LendBook(UserModel user, BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(BuchModel book)
        {
            throw new NotImplementedException();
        }
    }
}

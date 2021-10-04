using BoOp.DBAccessor;
using BoOp.DBAccessor.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            // Check if book is already in DB
            string sql = "SELECT * FROM dbo.Buecher WHERE Titel = @Titel AND Author = @Author AND ISBN = @ISBN;";
            var bookCheck  = _db.LoadData<BasicBuchModel, dynamic>(
                 sql,
                 new { Titel = book.BasicInfos.Titel, Author = book.BasicInfos.Author, ISBN = book.BasicInfos.ISBN },
                 _connectionString).FirstOrDefault();

            if (bookCheck == null)
            {
                // Save the basic book
                sql = "insert into dbo.Buecher (Titel, Author, Verlag, Auflage, ISBN, Altersvorschlag, Regal) values (@Titel, @Author, @Verlag, @Auflage, @ISBN, @Altersvorschlag, @Regal );";
                _db.SaveData(sql,
                            new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Verlag, book.BasicInfos.Auflage, book.BasicInfos.ISBN, book.BasicInfos.Altersvorschlag, book.BasicInfos.Regal },
                            _connectionString);
            }
            else
            {
                // throw new Exception("Book with same Titel, Author and ISBN exists already in DB.");

                Debug.WriteLine("Book with same Titel, Author and ISBN exists already in DB.");
            }

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
                    // Check if Barcode already exists in DB
                    sql = "SELECT * FROM dbo.Exemplare WHERE Barcode = @Barcode;";
                    var exemplarCheck = _db.LoadData<BasicExemplarModel, dynamic>(
                         sql,
                         new { BuchID = book.BasicInfos.Id, Barcode = exemplar.BasicInfos.Barcode },
                         _connectionString).FirstOrDefault();

                    if (exemplarCheck == null)
                    {
                        sql = "INSERT INTO dbo.Exemplare (BuchID, Barcode) VALUES (@BuchID, @Barcode)";
                        _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, Barcode = exemplar.BasicInfos.Barcode }, _connectionString);
                    }
                    else
                    {
                        Debug.WriteLine("Barcode is already used for a book in DB.");
                    }
                    
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

            // Add Genres to DB
            if (book.Genres != null)
            {
                foreach (var genre in book.Genres)
                {
                    // Check if word is already in DB
                    sql = "SELECT * FROM dbo.Genre WHERE Genrename = @Genrename;";
                    var basicGenre = _db.LoadData<BasicGenreModel, dynamic>(
                        sql,
                        new { Genrename = genre },
                        _connectionString).FirstOrDefault();

                    // Add word if it's not entered already
                    if (basicGenre == null)
                    {
                        sql = "INSERT INTO dbo.Genre (Genrename) VALUES (@Genrename);";
                        _db.SaveData(sql, new { Genrename = genre }, _connectionString);

                        sql = "SELECT * FROM dbo.Genre WHERE Genrename = @Genrename;";
                        basicGenre = _db.LoadData<BasicGenreModel, dynamic>(
                            sql,
                            new { Genrename = genre },
                            _connectionString).FirstOrDefault();
                    }

                    // Check if word is already marked as Schlagwort for that book
                    if (basicGenre != null)
                    {
                        sql = "SELECT * FROM dbo.BuchGenres WHERE BuchID = @BuchID AND GenreID = @GenreID;";
                        var buchGenre = _db.LoadData<BasicBuchGenresModel, dynamic>(
                        sql,
                        new { BuchID = book.BasicInfos.Id, GenreID = basicGenre.Id },
                        _connectionString).FirstOrDefault();

                        if (buchGenre == null)
                        {
                            // Create reference between word and book
                            sql = "INSERT INTO dbo.BuchGenres (BuchID, GenreID) VALUES (@BuchID, @GenreID)";
                            _db.SaveData(sql, new { BuchID = book.BasicInfos.Id, GenreID = basicGenre.Id }, _connectionString);
                        }
                    }
                }
            }

            //if (book.Rezensionen != null)
            //{
            //    foreach (var review in book.Rezensionen)
            //    {
            //        sql = "INSERT INTO dbo.Rezensionen (BuchID, Sterne, Rezensionstext, PersonID) VALUES (@Id, @Sterne, @Rezensionstext, @PersonID)";
            //        _db.SaveData(sql, new { book.BasicInfos.Id, review.BasicInfos.Sterne, review.BasicInfos.Rezensionstext, review.BasicInfos.PersonID }, _connectionString);
            //    }
            //}

        }

        public void LendBook(PersonModel user, BuchModel book)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(BuchModel book)
        {
            throw new NotImplementedException();
        }

        public PersonModel GetUserByID(int id)
        {
            string sId = id + "";
            string sql = "SELECT * FROM Personen WHERE Id = " + sId;
            var person = _db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString);

            return person.FirstOrDefault();
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
            string sqlString = "INSERT INTO Personen(Vorname, Nachname, Geburtsdatum, Telefonnummer, Rechte, Email) " +
                "VALUES(@Vorname, @Nachname, @Geburtsdatum, @Telefonnummer, @Rechte, @EMail);";

            _db.SaveData(sqlString, new { user.Vorname, user.Nachname, user.Geburtsdatum, user.Telefonnummer, user.Rechte, user.EMail }, _connectionString);
        }

        public void RemoveUser(PersonModel user)
        {
            throw new NotImplementedException();
        }

        public void EditUserDetails(PersonModel user)
        {
            throw new NotImplementedException();
        }


        ////////////////////
        ///

        //public FullContactModel GetFullContactById(int id)
        //{
        //    string sql = "select Id, FirstName, LastName from Contacts where Id = @Id";
        //    FullContactModel output = new FullContactModel();

        //    output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

        //    if (output.BasicInfo == null)
        //    {
        //        // do something to tell the user that the record was not found
        //        return null;
        //    }

        //    sql = @"select e.*
        //            from EmailAddresses e
        //            inner
        //            join ContactEmail ce on ce.EmailAddressId = e.Id
        //            where ce.ContactId = @Id";

        //    output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

        //    sql = @"select p.*
        //            from PhoneNumbers p
        //            inner join ContactPhoneNumbers cp on cp.PhoneNumberId = p.Id
        //            where cp.ContactId = @Id";

        //    output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

        //    return output;
        //}

        //public void CreateContact(FullContactModel contact)
        //{
        //    // Save the basic contact
        //    string sql = "insert into Contacts (FirstName, LastName) values (@FirstName, @LastName);";
        //    db.SaveData(sql,
        //                new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
        //                _connectionString);

        //    // Get the ID number of the contact
        //    sql = "select Id from Contacts where FirstName = @FirstName and LastName = @LastName;";
        //    int contactId = db.LoadData<IdLookupModel, dynamic>(
        //        sql,
        //        new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
        //        _connectionString).First().Id;

        //    foreach (var phoneNumber in contact.PhoneNumbers)
        //    {
        //        if (phoneNumber.Id == 0)
        //        {
        //            sql = "insert into PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
        //            db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

        //            sql = "select Id from PhoneNumbers where PhoneNumber = @PhoneNumber;";
        //            phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
        //                new { phoneNumber.PhoneNumber },
        //                _connectionString).First().Id;
        //        }

        //        sql = "insert into ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
        //        db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
        //    }

        //    foreach (var email in contact.EmailAddresses)
        //    {
        //        if (email.Id == 0)
        //        {
        //            sql = "insert into EmailAddresses (EmailAddress) values (@EmailAddress);";
        //            db.SaveData(sql, new { email.EmailAddress }, _connectionString);

        //            sql = "select Id from EmailAddresses where EmailAddress = @EmailAddress;";
        //            email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { email.EmailAddress }, _connectionString).First().Id;
        //        }

        //        sql = "insert into ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
        //        db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
        //    }
        //}

        //public void UpdateContactName(BasicContactModel contact)
        //{
        //    string sql = "update Contacts set FirstName = @FirstName, LastName = @LastName where Id = @Id";
        //    db.SaveData(sql, contact, _connectionString);
        //}

        //public void RemovePhoneNumberFromContact(int contactId, int phoneNumberId)
        //{
        //    string sql = "select Id, ContactId, PhoneNumberId from ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId;";
        //    var links = db.LoadData<ContactPhoneNumberModel, dynamic>(sql,
        //        new { PhoneNumberId = phoneNumberId },
        //        _connectionString);

        //    sql = "delete from ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId and ContactId = @ContactId";
        //    db.SaveData(sql, new { PhoneNumberId = phoneNumberId, ContactId = contactId }, _connectionString);

        //    if (links.Count == 1)
        //    {
        //        sql = "delete from PhoneNumbers where Id = @PhoneNumberId;";
        //        db.SaveData(sql, new { PhoneNumberId = phoneNumberId }, _connectionString);
        //    }
        //}
    }
}

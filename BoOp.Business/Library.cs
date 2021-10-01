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


            // Save the basic book
            string sql = $"insert into Buecher (Titel, Author, Verlag, PersonID, Auflage, ISBN, Altersvorschlag, Regal, Barcode) values ('{book.BasicInfos.Titel}', '{book.BasicInfos.Author}', '{book.BasicInfos.Verlag}', '{book.BasicInfos.PersonID}', '{book.BasicInfos.Auflage}', '{book.BasicInfos.ISBN}', '{book.BasicInfos.Altersvorschlag}', '{book.BasicInfos.Regal}', '{book.BasicInfos.Barcode}');";
            _db.SaveData(sql,
                        new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Verlag, book.BasicInfos.PersonID, book.BasicInfos.Auflage, book.BasicInfos.ISBN, book.BasicInfos.Altersvorschlag, book.BasicInfos.Regal, book.BasicInfos.Barcode},
                        _connectionString);


           // Get the ID number of the newly added book
           sql = $"select Id from Buecher where Titel = '{book.BasicInfos.Titel}' and Author = '{book.BasicInfos.Author}' and Barcode = '{book.BasicInfos.Barcode}';";
            int bookID = _db.LoadData<BasicBuchModel, dynamic>(
                sql,
                new { book.BasicInfos.Titel, book.BasicInfos.Author, book.BasicInfos.Barcode },
                _connectionString).First().Id;

            Console.WriteLine("");
            //foreach (var review in book.Rezensionen)
            //{
            //    if (phoneNumber.Id == 0)
            //    {
            //        sql = "insert into PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
            //        db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

            //        sql = "select Id from PhoneNumbers where PhoneNumber = @PhoneNumber;";
            //        phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
            //            new { phoneNumber.PhoneNumber },
            //            _connectionString).First().Id;
            //    }

            //    sql = "insert into ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
            //    db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
            //}

            //foreach (var email in contact.EmailAddresses)
            //{
            //    if (email.Id == 0)
            //    {
            //        sql = "insert into EmailAddresses (EmailAddress) values (@EmailAddress);";
            //        db.SaveData(sql, new { email.EmailAddress }, _connectionString);

            //        sql = "select Id from EmailAddresses where EmailAddress = @EmailAddress;";
            //        email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { email.EmailAddress }, _connectionString).First().Id;
            //    }

            //    sql = "insert into ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
            //    db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
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
                "VALUES('" + user.Vorname + "','" + user.Nachname + "','" + user.Geburtsdatum + "','" + user.Telefonnummer + "','" + user.Rechte + "','" + user.EMail + "');";

            _db.SaveData(sqlString, new { }, _connectionString);
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

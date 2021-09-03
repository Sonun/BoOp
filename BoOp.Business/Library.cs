using BoOp.Business.Models;
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
        private User _loggedInUnser;

        private readonly string _connectionString;
        private SQLDataAccess _db;



        public Library(string connectionString)
        {
            _connectionString = connectionString;
            _db = new SQLDataAccess();
        }

        public List<Book> GetAllBooks()
        {
            string sql = "select * from dbo.Buecher";

            return _db.LoadData<Book, dynamic>(sql, new { }, _connectionString);
        }

        //public Book GetBookById(int id)
        //{
        //    string sql = "select * from dbo.Contacts where Id = @id";
        //    var output = new Book();

        //    output.BasicInfo = _db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

        //    if (output.BasicInfo == null)
        //    {
        //        // do something to tell the user that the record was not found
        //        return null;
        //    }

        //    sql = @"select e.*
        //            from dbo.EmailAddresses e
        //            inner
        //            join dbo.ContactEmail ce on ce.EmailAddressId = e.Id
        //            where ce.ContactId = @Id";

        //    output.EmailAddresses = _db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

        //    sql = @"select p.*
        //            from dbo.PhoneNumbers p
        //            inner join dbo.ContactPhoneNumbers cp on cp.PhoneNumberId = p.Id
        //            where cp.ContactId = @Id";

        //    output.PhoneNumbers = _db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

        //    return output;
        //}

        //public void CreateContact(FullContactModel contact)
        //{
        //    // Save the basic contact
        //    string sql = "insert into dbo.Contacts (FirstName, LastName) values (@FirstName, @LastName);";
        //    _db.SaveData(sql,
        //                new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
        //                _connectionString);

        //    // Get the ID number of the contact
        //    sql = "select Id from dbo.Contacts where FirstName = @FirstName and LastName = @LastName;";
        //    int contactId = _db.LoadData<IdLookupModel, dynamic>(
        //        sql,
        //        new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
        //        _connectionString).First().Id;

        //    foreach (var phoneNumber in contact.PhoneNumbers)
        //    {
        //        if (phoneNumber.Id == 0)
        //        {
        //            sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
        //            _db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

        //            sql = "select Id from dbo.PhoneNumbers where PhoneNumber = @PhoneNumber;";
        //            phoneNumber.Id = _db.LoadData<IdLookupModel, dynamic>(sql,
        //                new { phoneNumber.PhoneNumber },
        //                _connectionString).First().Id;
        //        }

        //        sql = "insert into dbo.ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
        //        _db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
        //    }

        //    foreach (var email in contact.EmailAddresses)
        //    {
        //        if (email.Id == 0)
        //        {
        //            sql = "insert into dbo.EmailAddresses (EmailAddress) values (@EmailAddress);";
        //            _db.SaveData(sql, new { email.EmailAddress }, _connectionString);

        //            sql = "select Id from dbo.EmailAddresses where EmailAddress = @EmailAddress;";
        //            email.Id = _db.LoadData<IdLookupModel, dynamic>(sql, new { email.EmailAddress }, _connectionString).First().Id;
        //        }

        //        sql = "insert into dbo.ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
        //        _db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
        //    }
        //}

        //public void UpdateContactName(BasicContactModel contact)
        //{
        //    string sql = "update dbo.Contacts set FirstName = @FirstName, LastName = @LastName where Id = @Id";
        //    _db.SaveData(sql, contact, _connectionString);
        //}

        //public void RemovePhoneNumberFromContact(int contactId, int phoneNumberId)
        //{
        //    string sql = "select Id, ContactId, PhoneNumberId from dbo.ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId;";
        //    var links = _db.LoadData<ContactPhoneNumberModel, dynamic>(sql,
        //        new { PhoneNumberId = phoneNumberId },
        //        _connectionString);

        //    sql = "delete from dbo.ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId and ContactId = @ContactId";
        //    _db.SaveData(sql, new { PhoneNumberId = phoneNumberId, ContactId = contactId }, _connectionString);

        //    if (links.Count == 1)
        //    {
        //        sql = "delete from dbo.PhoneNumbers where Id = @PhoneNumberId;";
        //        _db.SaveData(sql, new { PhoneNumberId = phoneNumberId }, _connectionString);
        //    }
        //}


        public void AddBook(Book book)
        {
            _loggedInUnser = null;
            if(_loggedInUnser != null)
                throw new NotImplementedException();
        }

        public void LendBook(User user, Book book)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(Book book)
        {
            throw new NotImplementedException();
        }
    }
}

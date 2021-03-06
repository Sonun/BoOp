using BoOp.DBAccessor.Models;
using System.Collections.ObjectModel;

namespace BoOp.Business
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ILibrary.cs
    //Author : Manuel Janzen
    //Erstellt am : 1/9/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Interface klasse zur beschreibung einer Klasse zur datenbank connection
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface ILibrary
    {
        // Book related:

        //return a List of all books in the database
        public ObservableCollection<BuchModel> GetAllBooks();

        // Dependent on login
        public void LendBook(int? userId, string bookBarcode);

        // Independent on login
        public void ReturnBook(string bookBarcode);

        // Dependent on User Rights
        public void AddBook(BuchModel book);

        // removes a book from the datbase
        public void RemoveBook(BuchModel book);

        // Remove exemplar
        public void RemoveExemplar(ExemplarModel exemplar);

        // Edit book
        public void EditBookDetails(BuchModel bookModel, bool editMode = false);

        // Add review to a book
        public void AddReview(RezensionModel review);

        public int GetIdByISBN(BuchModel book);

        public int? GetBookIdByBarcode(string barcode);

        // User related:

        //return all users
        public ObservableCollection<PersonModel> GetAllUsers();

        // get user by id
        public PersonModel GetUserByID(int id);

        // get user by barcode
        public PersonModel GetUserByBarcode(string barcode); 

        // Add User to DB
        public void AddUser(PersonModel user);

        // Remove User from DB
        public void RemoveUser(PersonModel user);

        // Edit User Details
        public void EditUserDetails(PersonModel user);

        // Check if UserID already exists
        public bool CheckAvailabilityUserID(string userID);

    }
}

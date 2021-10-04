using BoOp.DBAccessor.Models;
using System.Collections.ObjectModel;

namespace BoOp.Business
{
    public interface ILibrary
    {
        // Book related:

        //return a List of all books in the database
        public ObservableCollection<BuchModel> GetAllBooks();

        // Dependent on login
        public void LendBook(int userId, string bookBarcode);

        // Independent on login
        public void ReturnBook(string bookBarcode);

        // Dependent on User Rights
        public void AddBook(BuchModel book);

        // Remove book
        public void RemoveBook(string bookBarcode);

        // Edit book
        public void EditBookDetails(string bookBarcode);

        // Add review to a book
        public void AddReview(RezensionModel review);


        // User related:

        // get user by id
        public PersonModel GetUserByID(int id);

        // Add User to DB
        public void AddUser(PersonModel user);

        // Remove User from DB
        public void RemoveUser(PersonModel user);

        // Edit User Details
        public void EditUserDetails(PersonModel user);



    }
}

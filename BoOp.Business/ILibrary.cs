using BoOp.DBAccessor.Models;
using System.Collections.ObjectModel;

namespace BoOp.Business
{
    public interface ILibrary
    {
        // Dependent on login
        public void LendBook(PersonModel user, BuchModel book);

        // Independent on login
        public void ReturnBook(BuchModel book);

        // Dependent on User Rights
        public void AddBook(BuchModel book);

        // get user by id
        public void GetUserByID(int id);

        //return a List of all books in the database
        public ObservableCollection<BuchModel> GetAllBooks();

    }
}

using BoOp.DBAccessor.Models;

namespace BoOp.Business
{
    public interface ILibrary
    {
        // Dependent on login
        public void LendBook(UserModel user, BuchModel book);

        // Independent on login
        public void ReturnBook(BuchModel book);

        // Dependent on User Rights
        public void AddBook(BuchModel book);

    }
}

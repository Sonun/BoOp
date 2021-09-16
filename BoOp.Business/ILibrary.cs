using BoOp.DBAccessor.Models;

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

    }
}

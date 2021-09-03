using BoOp.Business.Books;
using BoOp.Business.Users;

namespace BoOp.Business
{
    public interface ILibrary
    {
        // Dependent on login
        public void LendBook(User user, Book book);

        // Independent on login
        public void ReturnBook(Book book);

        // Dependent on User Rights
        public void AddBook(Book book);

    }
}

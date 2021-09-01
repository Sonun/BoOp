using BoOp.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business
{
    public class Library : ILibrary
    {
        private User _loggedInUnser;

        public void AddBook(Book book)
        {
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

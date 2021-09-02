using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Models
{
    public class User
    {
        public enum Rights
        {
            SIMPLEREADER, // Reservieren, Ausleihen, Zurückgeben
            HELPER, // Ausleihen und Zurückgeben für andere, Bücher und Benutzer anlegen (nicht löschen)
            BIBOTEAM, // wie Schulhelfer + Löschen von Büchern und Benutzer möglich
            ADMIN // Alles (BiboTeam anlegen und löschen)
        }

        public string ID { get; set; }
        public Rights Right { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public List<Book> LendedBooks { get; set; }
    }
}

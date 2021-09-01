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
            Leser, // Reservieren, Ausleihen, Zurückgeben
            Schulhelfer, // Ausleihen und Zurückgeben für andere, Bücher und Benutzer anlegen (nicht löschen)
            BiboTeam, // wie Schulhelfer + Löschen von Büchern und Benutzer möglich
            Admin // Alles (BiboTeam anlegen und löschen)
        }

        public string ID { get; set; }
        public Rights Right { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
    }
}

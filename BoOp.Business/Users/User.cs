using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Users
{
    public class User
    {
        /// <summary>
        /// Identifikationsnummer des Benutzers
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Benutzerrechte des Benutzers
        /// </summary>
        public UserLevels UserLevel { get; set; }
        /// <summary>
        /// Vorname des Benutzers
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Nachname des Benutzers
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Geburtstag des Benutzers
        /// </summary>
        public DateTime Birthdate { get; set; }
    }
}

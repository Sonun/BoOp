using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public enum Rechtelevel
    {
        /// <summary>
        /// Gibt dem Benutzer die Rechte Bücher zu reservieren, auszuleihen oder
        /// zurückzugeben
        /// </summary>
        LESER, // Reservieren, Ausleihen, Zurückgeben
        /// <summary>
        /// Gibt dem Benutzer die Rechte von Reader und kann diese Rechte auch für andere Ausführen
        /// </summary>
        HELFER, // Ausleihen und Zurückgeben für andere, Bücher und Benutzer anlegen (nicht löschen)
        /// <summary>
        /// Gibt dem Benutzer die Rechte von GUIDANCE und darf die Bücherdatendank anlegen
        /// </summary>
        BIBOTEAM, // wie Schulhelfer + Löschen von Büchern und Benutzer möglich
        /// <summary>
        /// Gibt de, Benutzer die Rechte von Staff und darf Rechte verwalten, sowie neue Mitarbeiter anlegen
        /// </summary>
        ADMIN // Alles (BiboTeam anlegen und löschen)
    }

    public class PersonModel
    {
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Telefonnummer { get; set; }
        public Rechtelevel Rechte { get; set; }
        public string EMail { get; set; }
    }
}

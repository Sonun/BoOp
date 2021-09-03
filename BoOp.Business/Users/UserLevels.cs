using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Users
{
    public enum UserLevels
    {
        /// <summary>
        /// Gibt dem Benutzer die Rechte Bücher zu reservieren, auszuleihen oder
        /// zurückzugeben
        /// </summary>
        READER, // Reservieren, Ausleihen, Zurückgeben
        /// <summary>
        /// Gibt dem Benutzer die Rechte von Reader und kann diese Rechte auch für andere Ausführen
        /// </summary>
        GUIDANCE, // Ausleihen und Zurückgeben für andere, Bücher und Benutzer anlegen (nicht löschen)
        /// <summary>
        /// Gibt dem Benutzer die Rechte von GUIDANCE und darf die Bücherdatendank anlegen
        /// </summary>
        STAFF, // wie Schulhelfer + Löschen von Büchern und Benutzer möglich
        /// <summary>
        /// Gibt de, Benutzer die Rechte von Staff und darf Rechte verwalten, sowie neue Mitarbeiter anlegen
        /// </summary>
        ADMIN // Alles (BiboTeam anlegen und löschen)
    }
}

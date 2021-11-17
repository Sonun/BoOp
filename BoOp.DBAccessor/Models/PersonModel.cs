using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: PersonModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 16/09/2021
    //Bearbeitet von : Manuel Janzen, Dominik von Michalkowsky
    //Beschreibung : dinge die das PersonModel enthalten muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PersonModel
    {
        public int? Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string VorUndNachname { get { return Vorname + " " + Nachname; } }
        public string PasswortHash { get; set; }
        //public DateTime Geburtsdatum { get; set; }
        //public string GeburtstagAsString { get { return Geburtsdatum.ToString("d");  } }
        public string GeburtsdatumString { get; set; }
        public DateTime GeburtsdatumDateTime { get { return DateTime.Parse(GeburtsdatumString); } set { GeburtsdatumDateTime = value; } }
        public string Telefonnummer { get; set; }
        public Rechtelevel Rechte { get; set; }
        public string EMail { get; set; }
        public string AusweisID { get; set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: PersonModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 19/09/2021
    //Bearbeitet von : Manuel Janzen, Dominik von Michalkowsky
    //Beschreibung : alle Mögliche Rechtelevel Der benutzer
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Flags]
    public enum Rechtelevel
    {
        /// <summary>
        /// Gibt dem Benutzer die Rechte Bücher zu reservieren, auszuleihen oder
        /// zurückzugeben
        /// </summary>
        LESER = 1, // Reservieren, Ausleihen, Zurückgeben
        /// <summary>
        /// Gibt dem Benutzer die Rechte von Reader und kann diese Rechte auch für andere Ausführen
        /// </summary>
        HELFER = 2, // Ausleihen und Zurückgeben für andere, Bücher und Benutzer anlegen (nicht löschen)
        /// <summary>
        /// Gibt dem Benutzer die Rechte von GUIDANCE und darf die Bücherdatendank anlegen
        /// </summary>
        BIBOTEAM = 4, // wie Schulhelfer + Löschen von Büchern und Benutzer möglich
        /// <summary>
        /// Gibt de, Benutzer die Rechte von Staff und darf Rechte verwalten, sowie neue Mitarbeiter anlegen
        /// </summary>
        ADMIN = 8 // Alles (BiboTeam anlegen und löschen)
    }
}

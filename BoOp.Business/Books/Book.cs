using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Books
{
    public class Book
    {
        // Necessary Properties
        /// <summary>
        /// Internationale Standardbuchnummer
        /// </summary>
        public int ISBN { get; set; }
        /// <summary>
        /// Identifikationsnummer des Buches
        /// </summary>
        public int BuchID { get; set; }
        /// <summary>
        /// Buchtitel
        /// </summary>
        public string Titel { get; set; }
        /// <summary>
        /// Buchautor
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Verlag des Buches
        /// </summary>
        public string Publisher { get; set; }

        // Obligatory Properties
        /// <summary>
        /// Gibt an in welchem Regal das Buch steht
        /// </summary>
        public string ShelfNumber { get; set; }
        /// <summary>
        /// Liste mit Genres des Buches
        /// </summary>
        public List<string> Category { get; set; }
        /// <summary>
        /// Liste mit Schlagwörtern
        /// </summary>
        public List<string> Keywords { get; set; }
        /// <summary>
        /// Auflage des Buches
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Empfohlenes Lesealter
        /// </summary>
        public int RecommendedAge { get; set; }
        /// <summary>
        /// Bewertung des Buches
        /// </summary>
        public List<BookReference> References {get; set;}

        /// <summary>
        /// Registriert das Buch in der Datenbank
        /// </summary>
        public void Register()
        {
            throw new NotImplementedException();
        }

    }
}

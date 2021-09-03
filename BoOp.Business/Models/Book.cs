using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Models
{
    public class Book
    {
        // Necessary Properties
        public int ISDN { get; set; }
        public string ID { get; set; }
        public string Titel { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; } // Verlag

        // Obligatory Properties
        public User LendBy { get; set; }
        public string ShelfNumber { get; set; }
        public List<string> Category { get; set; }
        public List<string> Keywords { get; set; } // Schlagwörter
        public int Version { get; set; } // Auflage
        public string RecommendedAge { get; set; }
        public List<BookReference> References {get; set;}

    }
}

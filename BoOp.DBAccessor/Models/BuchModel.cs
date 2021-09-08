using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BuchModel
    {
        public int Id { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Schlagwoerter { get; set; }
        public UserModel LendBy { get; set; }
        public string Titel { get; set; }
        public string Author { get; set; }
        public string Verlag { get; set; }
        public int Auflage { get; set; }
        public string ISBN { get; set; }
        public string Altersvorschlag { get; set; }
        public string Regal { get; set; }
        public List<RezensionModel> Rezensionen {get; set;}
    }
}

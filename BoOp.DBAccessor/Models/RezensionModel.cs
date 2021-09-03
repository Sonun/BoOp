using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class RezensionModel
    {
        public int Id { get; set; }
        public BuchModel Buch { get; set; }
        public int Sterne { get; set; }
        public string Rezensionstext { get; set; }
        public UserModel Author { get; set; }
    }
}

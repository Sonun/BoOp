using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BasicRezensionenModel
    {
        public int Id { get; set; }
        public int BuchID { get; set; }
        public int Sterne { get; set; }
        public string Rezensionstext { get; set; }
        public int PersonID { get; set; }
    }
}

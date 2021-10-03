using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BasicBuchSchlagwoerterModel
    {
        public int? Id { get; set; }
        public int BuchID { get; set; }
        public int SchlagwortID { get; set; }
    }
}

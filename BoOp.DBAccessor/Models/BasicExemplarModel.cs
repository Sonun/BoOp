using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BasicExemplarModel
    {
        public int? Id { get; set; }
        public int BuchID { get; set; }
        public string Barcode { get; set; }
        public int? LendByUserID { get; set; }
    }
}

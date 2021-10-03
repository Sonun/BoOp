using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class ExemplarModel
    {
        public BasicExemplarModel BasicInfos { get; set; }
        public PersonModel LendBy { get; set; }
    }
}

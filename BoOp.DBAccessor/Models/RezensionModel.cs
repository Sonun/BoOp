using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class RezensionModel
    {
        public BasicRezensionenModel BasicInfos { get; set; }
        public BasicPersonenModel Author { get; set; }
    }
}

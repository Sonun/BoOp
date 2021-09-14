using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BuchModel
    {
        public BasicBuchModel BasicInfos { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Schlagwoerter { get; set; }
        public UserModel LendBy { get; set; }
        public List<RezensionModel> Rezensionen {get; set;}
    }
}

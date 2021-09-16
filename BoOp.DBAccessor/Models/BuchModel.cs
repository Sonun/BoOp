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
        public PersonModel LendBy { get; set; }
        public List<RezensionModel> Rezensionen {get; set;}
        public double? RezensionenDurschschnitt
        {
            get
            {
                if (Rezensionen == null || Rezensionen.Count == 0)
                    return null;

                double temp = 0;

                foreach (var rez in Rezensionen)
                {
                    temp += rez.BasicInfos.Sterne;
                }

                temp /= Rezensionen.Count;

                return Math.Round(temp, 2);
            }
        }
        public string BuchBarcode { get; set; }
    }
}

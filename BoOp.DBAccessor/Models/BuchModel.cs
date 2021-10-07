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
        public List<ExemplarModel> Exemplare { get; set; }
        public List<RezensionModel> Rezensionen {get; set;}
        public int ExemplarAnzahl { get{ return Exemplare.Count; }}
        public int NichtVergebeneExemplare { 
            get 
            {
                if (Exemplare == null)
                    return 0;

                var nichtVergeben = Exemplare.Count;
                foreach (var ex in Exemplare)
                {
                    if(ex.LendBy != null)
                    {
                        nichtVergeben--;
                    }
                }

                return nichtVergeben;
            } 
        }
        public string ExemplarBindingForSuperAwsomeWPFLibraryView { get { return ExemplarAnzahl + " (" + NichtVergebeneExemplare + ")"; } }

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
    }
}

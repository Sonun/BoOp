using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ExemplarModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 3/10/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : dinge die das ExemplarModel enthalten muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ExemplarModel
    {
        public BasicExemplarModel BasicInfos { get; set; }
        public PersonModel LendBy { get; set; }
    }
}

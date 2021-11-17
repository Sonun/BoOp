using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: BasicSchlagwoerterModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 14/09/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : dinge die das BasicSchlagwoerterModel enthalten muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BasicSchlagwoerterModel
    {
        public int? Id { get; set; }
        public string Wort { get; set; }
    }
}

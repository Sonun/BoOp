using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    public class BasicBuchSchlagwoerterModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Dateiname: BasicBuchSchlagwoerterModel.cs
        //Author : Manuel Janzen
        //Erstellt am : 14/9/2021
        //Bearbeitet von : Manuel Janzen
        //Beschreibung : dinge die das BasicBuchSchlagwoerterModel enthalten muss
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int? Id { get; set; }
        public int BuchID { get; set; }
        public int SchlagwortID { get; set; }
    }
}

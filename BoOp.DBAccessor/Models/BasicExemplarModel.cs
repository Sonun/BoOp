using System;

namespace BoOp.DBAccessor.Models
{
    public class BasicExemplarModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Dateiname: BasicExemplarModel.cs
        //Author : Manuel Janzen
        //Erstellt am : 3/10/2021
        //Bearbeitet von : Manuel Janzen
        //Beschreibung : dinge die das BasicExemplarModel enthalten muss
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int? Id { get; set; }
        public int BuchID { get; set; }
        public string Barcode { get; set; }
        public int? AusleiherID { get; set; }
        public string AusleihDatumString { get; set; }
        public DateTime AusleihDatumDateTime { get { return DateTime.Parse(AusleihDatumString); } set {  } }
    }
}

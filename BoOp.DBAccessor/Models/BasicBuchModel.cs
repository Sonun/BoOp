namespace BoOp.DBAccessor.Models
{
    public class BasicBuchModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Dateiname: BasicBuchModel.cs
        //Author : Manuel Janzen
        //Erstellt am : 14/9/2021
        //Bearbeitet von : Manuel Janzen
        //Beschreibung : dinge die das BasicBuchModel enthalten muss
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int? Id { get; set; }
        public string Titel { get; set; }
        public string Author { get; set; }
        public string Verlag { get; set; }
        public int? Auflage { get; set; }
        public string ISBN { get; set; }
        public string Altersvorschlag { get; set; }
        public string Regal { get; set; }
    }
}

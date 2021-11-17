namespace BoOp.DBAccessor.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: RezensionModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 3/09/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : dinge die das RezensionModel enthalten muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class RezensionModel
    {
        public BasicRezensionenModel BasicInfos { get; set; }
        public PersonModel Author { get; set; }
    }
}

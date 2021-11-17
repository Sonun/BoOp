﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: BasicRezensionenModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 14/09/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : dinge die das BasicRezensionenModel enthalten muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BasicRezensionenModel
    {
        public int? Id { get; set; }
        public int BuchID { get; set; }
        public int Sterne { get; set; }
        public string Rezensionstext { get; set; }
        public int PersonID { get; set; }
    }
}

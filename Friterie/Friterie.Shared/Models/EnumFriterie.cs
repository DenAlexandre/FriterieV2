using System;
using System.Collections.Generic;
using System.Text;

namespace Friterie.Shared.Models
{
    public static class EnumFriterie
    {

        public enum ProductTypeEnum : ushort
        {
            All = 0,
            Burgers = 1,
            Viandes = 2,
            Sauces = 3, //
            Menus = 4, //
        }

        public enum StatusTypeEnum : ushort
        {
            Créé = 0,
            EnCoursDeCommande = 10,
            EnCoursDeFabrication = 20,
            Terminé = 30, //
            Annulé = 40,
            Inconnu = 100

        }
    }
}

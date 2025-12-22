using System;
using System.Collections.Generic;
using System.Text;

namespace Friterie.Shared.Models
{
    public class EnumFriterie
    {

        public enum ProductTypeEnum : ushort
        {

            Burgers = 1,
            Viandes = 2,
            Sauces = 10, //
            Menus = 20, //


        }

        public enum StatusTypeEnum : ushort
        {

            Pending = 0,
            Paid = 1, //
            Completed = 2, //
            Cancelled = 3,

        }
    }
}

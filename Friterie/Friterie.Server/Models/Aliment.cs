using Friterie.Server.Models;

namespace Friterie.Models
{
    public class Aliment : ISelectable
    {
        public GroupeAliment TGroupeAliment { get; set; }

        public int? TAlimentCode { get; set; }
        public string TAlimentNom { get; set; }
        public decimal? TProteines { get; set; }
        public decimal? TGlucides { get; set; }
        public decimal? TLipides { get; set; }
        public decimal? TEnergie { get; set; }
        public bool Selected { get; set; }
    }
}



namespace Friterie.Models
{
    public class GroupeAliment
    {
        public int TGroupeCode { get; set; }
        public string TGroupeNom { get; set; }
        public int TSsGroupeCode { get; set; }
        public string TSsGroupeNom { get; set; }
        public int TSsSsGroupeCode { get; set; }
        public string TSsSsGroupeNom { get; set; }
        public bool Selected { get; set; }
    }
}



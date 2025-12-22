namespace Friterie.Shared.Models
{
    public class Article : ISelectable
    {
        public GroupeArticle TGroupeArticle { get; set; }

        //SELECT a.art_id, a.art_nom, a.art_desc, a.art_prix, a.art_url_img, a.art_type, c.id_categorie, c.nom_categorie
        public int? TArticleId { get; set; }

        public string TArticleNom { get; set; }

        public string TArticleDesc { get; set; }

        public decimal? TArticlePrix { get; set; }

        public string TArticleURLIMg { get; set; }
        public bool Selected { get; set; }
    }
}



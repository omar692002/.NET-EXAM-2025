namespace GestionParfumerie.Application.DTOs
{
    public class ProduitDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Prix { get; set; }
        public int QuantiteEnStock { get; set; }
        public DateTime DateAjout { get; set; }

        public string CategorieNom { get; set; } = string.Empty;
        public string BrandNom { get; set; } = string.Empty;

        // Liste des familles olfactives du produit
        public List<string> FamillesOlfactives { get; set; } = new List<string>();

        public string? PhotoPath { get; set; }
    }
}

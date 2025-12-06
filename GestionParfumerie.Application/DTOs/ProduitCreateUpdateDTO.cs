namespace GestionParfumerie.Application.DTOs
{
    public class ProduitCreateUpdateDTO
    {
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Prix { get; set; }
        public int QuantiteEnStock { get; set; }

        public int CategorieId { get; set; }
        public int BrandId { get; set; }

        public List<int> FamillesOlfactivesIds { get; set; } = new();

        public string? PhotoPath { get; set; }
    }
}

namespace GestionParfumerie.Application.DTOs
{
    public class LigneCommandeDTO
    {
        public int ProduitId { get; set; }
        public string ProduitNom { get; set; } = string.Empty;
        public decimal PrixUnitaire { get; set; }
        public int Quantite { get; set; }
        public decimal TotalLigne { get; set; }
    }
}

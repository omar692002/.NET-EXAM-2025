namespace GestionParfumerie.Domain.Entities
{
    public class ProduitFamilleOlfactive
    {
        public int ProduitId { get; set; }
        public Produit Produit { get; set; } = null!;
        public int FamilleOlfactiveId { get; set; }
        public FamilleOlfactive FamilleOlfactive { get; set; }
    }
}
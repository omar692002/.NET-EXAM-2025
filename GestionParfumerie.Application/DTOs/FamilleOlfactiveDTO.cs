namespace GestionParfumerie.Application.DTOs
{
    public class FamilleOlfactiveDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        // Calculé
        public int NombreProduits { get; set; }
        public string? Description { get; set; } // ← Ajouté

        public List<ProduitDTO> Produits { get; set; } = new();

    }

}

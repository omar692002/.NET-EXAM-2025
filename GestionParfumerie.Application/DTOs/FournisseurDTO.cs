namespace GestionParfumerie.Application.DTOs
{
    public class FournisseurDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }

        // Calculé
        public int NombreProduits { get; set; }
        public List<ProduitDTO> Produits { get; set; } = new();

    }

}

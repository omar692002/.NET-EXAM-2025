namespace GestionParfumerie.Application.DTOs
{
    public class CategorieDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Calculé
        public int NombreProduits { get; set; }
    }
}

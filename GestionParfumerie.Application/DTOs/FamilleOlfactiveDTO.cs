namespace GestionParfumerie.Application.DTOs
{
    public class FamilleOlfactiveDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        // Calculé
        public int NombreProduits { get; set; }
    }
}

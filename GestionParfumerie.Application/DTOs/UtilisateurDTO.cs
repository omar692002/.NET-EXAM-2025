namespace GestionParfumerie.Application.DTOs
{
    public class UtilisateurDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<CommandeDTO> Commandes { get; set; } = new();

    }

}

using Microsoft.AspNetCore.Identity;


namespace GestionParfumerie.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Champs supplémentaires
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }

        public DateTime? CreatedAt { get; set; }
        // Relations
        public ICollection<Commande>? Commandes { get; set; }
    }
}

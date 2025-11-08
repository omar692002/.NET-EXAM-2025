using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Domain.Entities
{
    public class Commande
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCommande { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string Statut { get; set; } = "En cours";

        [Precision(18, 2)] 
        public decimal Total { get; set; }

        // --- Relation vers l'utilisateur (client qui a passé la commande) ---
        [Required]
        public string UtilisateurId { get; set; } = string.Empty;

        public ApplicationUser? Utilisateur { get; set; }

        // --- Navigation vers les lignes de commande ---
        public ICollection<LigneCommande> LigneCommandes { get; set; } = new List<LigneCommande>();
    }
}

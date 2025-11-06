using System;
using System.Collections.Generic;
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

        public decimal Total { get; set; }
        public string UtilisateurId { get; set; }
        public ApplicationUser Utilisateur { get; set; }

        // Navigation
        public ICollection<LigneCommande> LigneCommandes { get; set; }
    }
}

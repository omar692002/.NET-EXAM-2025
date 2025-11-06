using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace GestionParfumerie.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Champs supplémentaires
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }

        // Relations
        public ICollection<Commande>? Commandes { get; set; }
    }
}

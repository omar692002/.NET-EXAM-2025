using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Domain.Entities
{
    public class Fournisseur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nom { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string Telephone { get; set; }

        public required string Adresse { get; set; }

        public required ICollection<Produit> Produits { get; set; }
    }
}

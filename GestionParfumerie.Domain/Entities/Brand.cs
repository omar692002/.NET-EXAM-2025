using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Domain.Entities
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nom { get; set; }

        [EmailAddress]
        public required string Email { get; set; }


        public required string Adresse { get; set; }

        public ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}

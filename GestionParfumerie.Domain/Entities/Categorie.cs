using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Domain.Entities
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public required ICollection<Produit> Produits { get; set; }
    }
}

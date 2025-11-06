using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionParfumerie.Domain.Entities
{
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Prix { get; set; }

        public int QuantiteEnStock { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.Now;

        // Foreign Keys
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; }

        public int FamilleOlfactiveId { get; set; }
        public FamilleOlfactive FamilleOlfactive { get; set; }

        // Navigation
        public ICollection<LigneCommande> LigneCommandes { get; set; }
    }
}

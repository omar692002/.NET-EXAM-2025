using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionParfumerie.Domain.Entities
{
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nom { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prix { get; set; }

        public int QuantiteEnStock { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.Now;

        // --- Relations ---
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public string? PhotoPath { get; set; }

        // Many-to-Many avec Famille
        public ICollection<ProduitFamilleOlfactive> ProduitFamilles { get; set; }
            = new List<ProduitFamilleOlfactive>();

        // One-to-Many avec ligne commande
        public ICollection<LigneCommande> LigneCommandes { get; set; }
            = new List<LigneCommande>(); // ✔ CORRECTION
    }
}

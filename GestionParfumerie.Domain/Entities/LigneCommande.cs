using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Domain.Entities
{
    public class LigneCommande
    {
        [Key]
        public int Id { get; set; }

        public int CommandeId { get; set; }
        public Commande Commande { get; set; }

        public int ProduitId { get; set; }
        public Produit Produit { get; set; }

        public int Quantite { get; set; }

        public decimal PrixUnitaire { get; set; }
    }
}

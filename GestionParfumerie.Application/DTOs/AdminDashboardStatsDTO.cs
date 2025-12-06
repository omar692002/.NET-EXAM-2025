namespace GestionParfumerie.Application.DTOs
{
    public class AdminDashboardStatsDTO
    {
        // Statistiques globales
        public decimal ChiffreAffairesTotal { get; set; }
        public int NombreTotalCommandes { get; set; }
        public int NombreTotalClients { get; set; }
        public int NombreTotalProduits { get; set; }

        // Commandes par statut
        public int CommandesEnCours { get; set; }
        public int CommandesLivrees { get; set; }
        public int CommandesAnnulees { get; set; }

        // Statistiques du mois en cours
        public decimal ChiffreAffairesMoisCourant { get; set; }
        public int CommandesMoisCourant { get; set; }
        public int NouveauxClientsMoisCourant { get; set; }

        // Top produits et clients
        public List<TopProduitDTO> TopProduits { get; set; } = new();
        public List<TopClientDTO> TopClients { get; set; } = new();

        // Commandes récentes
        public List<CommandeDTO> CommandesRecentes { get; set; } = new();
    }

    public class TopProduitDTO
    {
        public int ProduitId { get; set; }
        public string ProduitNom { get; set; } = string.Empty;
        public int QuantiteVendue { get; set; }
        public decimal ChiffreAffaires { get; set; }
    }

    public class TopClientDTO
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public int NombreCommandes { get; set; }
        public decimal TotalDepense { get; set; }
    }
}
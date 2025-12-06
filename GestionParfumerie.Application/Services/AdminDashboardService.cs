using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Interfaces;

namespace GestionParfumerie.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ICommandesRepository _commandeRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;

        public AdminDashboardService(
            ICommandesRepository commandeRepository,
            IProduitRepository produitRepository,
            IUtilisateurRepository utilisateurRepository)
        {
            _commandeRepository = commandeRepository;
            _produitRepository = produitRepository;
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task<AdminDashboardStatsDTO> GetDashboardStatsAsync()
        {
            var stats = new AdminDashboardStatsDTO();

            // Date du début du mois courant
            var debutMoisCourant = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Récupérer toutes les commandes avec relations
            var commandes = (await _commandeRepository.GetAllWithDetailsAsync()).ToList();

            // Statistiques globales
            stats.ChiffreAffairesTotal = commandes.Sum(c => c.Total);
            stats.NombreTotalCommandes = commandes.Count;
            stats.NombreTotalClients = await _utilisateurRepository.GetTotalClientsCountAsync();
            stats.NombreTotalProduits = await _produitRepository.GetTotalCountAsync();

            // Commandes par statut
            stats.CommandesEnCours = commandes.Count(c => c.Statut.ToString() == "En cours");
            stats.CommandesLivrees = commandes.Count(c => c.Statut.ToString() == "Livrée");
            stats.CommandesAnnulees = commandes.Count(c => c.Statut.ToString() == "Annulée");

            // Statistiques du mois courant
            var commandesMoisCourant = commandes.Where(c => c.DateCommande >= debutMoisCourant).ToList();
            stats.ChiffreAffairesMoisCourant = commandesMoisCourant.Sum(c => c.Total);
            stats.CommandesMoisCourant = commandesMoisCourant.Count;

            // Nouveaux clients du mois
            stats.NouveauxClientsMoisCourant = await _utilisateurRepository.GetNewClientsCountSinceDateAsync(debutMoisCourant);

            // Top 5 des produits les plus vendus
            stats.TopProduits = commandes
                .SelectMany(c => c.LigneCommandes)
                .GroupBy(lc => new { lc.ProduitId, lc.Produit.Nom })
                .Select(g => new TopProduitDTO
                {
                    ProduitId = g.Key.ProduitId,
                    ProduitNom = g.Key.Nom,
                    QuantiteVendue = g.Sum(lc => lc.Quantite),
                    ChiffreAffaires = g.Sum(lc => lc.PrixUnitaire * lc.Quantite)
                })
                .OrderByDescending(p => p.QuantiteVendue)
                .Take(5)
                .ToList();

            // Top 5 des meilleurs clients
            stats.TopClients = commandes
                .GroupBy(c => new { c.UtilisateurId, c.Utilisateur.Email })
                .Select(g => new TopClientDTO
                {
                    ClientId = g.Key.UtilisateurId,
                    ClientEmail = g.Key.Email ?? "Inconnu",
                    NombreCommandes = g.Count(),
                    TotalDepense = g.Sum(c => c.Total)
                })
                .OrderByDescending(c => c.TotalDepense)
                .Take(5)
                .ToList();

            // 10 commandes les plus récentes
            stats.CommandesRecentes = commandes
                .OrderByDescending(c => c.DateCommande)
                .Take(10)
                .Select(c => new CommandeDTO
                {
                    Id = c.Id,
                    DateCommande = c.DateCommande,
                    Statut = c.Statut.ToString(),
                    UtilisateurEmail = c.Utilisateur.Email ?? "Inconnu",
                    UtilisateurId = c.UtilisateurId,
                    Total = c.Total,
                    NbArticles = c.LigneCommandes.Sum(lc => lc.Quantite),
                    LigneCommandes = c.LigneCommandes.Select(lc => new LigneCommandeDTO
                    {
                        ProduitId = lc.ProduitId,
                        ProduitNom = lc.Produit.Nom,
                        PrixUnitaire = lc.PrixUnitaire,
                        Quantite = lc.Quantite,
                        TotalLigne = lc.PrixUnitaire * lc.Quantite
                    }).ToList()
                })
                .ToList();

            return stats;
        }
    }
}
namespace GestionParfumerie.Application.DTOs
{
    public class UtilisateurStatsDTO
    {
        public string UtilisateurId { get; set; } = string.Empty;
        public int NombreCommandes { get; set; }
        public decimal TotalDepense { get; set; }
        public int CommandeEnCours { get; set; }
        public int CommandeLivree { get; set; }
        public int CommandeAnnulee { get; set; }
    }

}

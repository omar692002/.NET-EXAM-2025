namespace GestionParfumerie.Application.DTOs
{
    public class CommandeDTO
    {
        public int Id { get; set; } 
        public DateTime DateCommande { get; set; }
        public string Statut { get; set; } = "En cours";
        public string UtilisateurEmail { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int NbArticles { get; set; }
        public ICollection<LigneCommandeDTO> LigneCommandes { get; set; } = new List<LigneCommandeDTO>();
    }
}

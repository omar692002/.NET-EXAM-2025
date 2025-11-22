namespace GestionParfumerie.Application.DTOs
{
    public class CommandeCreateDTO
    {
        public ICollection<LigneCommandeCreateDTO> LigneCommandes { get; set; } = new List<LigneCommandeCreateDTO>();
    }
}

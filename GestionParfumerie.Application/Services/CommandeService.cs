using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Domain.Enums;
namespace GestionParfumerie.Application.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ICommandesRepository _repo;
        private readonly IMapper _mapper;

        public CommandeService(ICommandesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommandeDTO>> GetAllAsync()
        {
            var commandes = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CommandeDTO>>(commandes);
        }

        public async Task<CommandeDTO> GetByIdAsync(int id)
        {
            var commande = await _repo.GetByIdAsync(id);
            return _mapper.Map<CommandeDTO>(commande);
        }

        public async Task<CommandeDTO> CreateAsync(string utilisateurId, CommandeCreateDTO dto)
        {
            var commande = new Commande
            {
                UtilisateurId = utilisateurId,
                DateCommande = DateTime.UtcNow,
                Statut = StatutCommande.EnCours,
                LigneCommandes = dto.LigneCommandes.Select(l => new LigneCommande
                {
                    ProduitId = l.ProduitId,
                    Quantite = l.Quantite
                }).ToList()
            };

            await _repo.AddAsync(commande);
            return _mapper.Map<CommandeDTO>(commande);
        }


        public async Task<bool> UpdateStatutAsync(int id, string statut)
        {
            var commande = await _repo.GetByIdAsync(id);
            if (commande == null) return false;

            if (!Enum.TryParse<StatutCommande>(statut, true, out var nouveauStatut))
                throw new ArgumentException("Statut invalide");

            commande.Statut = nouveauStatut;
            await _repo.UpdateAsync(commande);
            return true;
        }


        public async Task<IEnumerable<CommandeDTO>> GetAllByUtilisateurIdAsync(string utilisateurId)
        {
            var commandes = await _repo.GetAllByUtilisateurIdAsync(utilisateurId);
            return _mapper.Map<IEnumerable<CommandeDTO>>(commandes);
        }
    }
}
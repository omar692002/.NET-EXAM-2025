using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Domain.Entities;

namespace GestionParfumerie.Application.Services
{
    public class ProduitService : IProduitService
    {
        private readonly IProduitRepository _repository;
        private readonly IMapper _mapper;

        public ProduitService(IProduitRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProduitDTO>> GetAllAsync()
        {
            var produits = await _repository.GetAllWithRelationsAsync();
            return _mapper.Map<IEnumerable<ProduitDTO>>(produits);
        }

        public async Task<ProduitDTO?> GetByIdAsync(int id)
        {
            var produit = await _repository.GetByIdWithRelationsAsync(id);
            return produit != null ? _mapper.Map<ProduitDTO>(produit) : null;
        }

        public async Task<ProduitDTO> CreateAsync(ProduitCreateUpdateDTO dto)
        {
            // Créer l'entité Produit
            var produit = new Produit
            {
                Nom = dto.Nom,
                Description = dto.Description,
                Prix = dto.Prix,
                QuantiteEnStock = dto.QuantiteEnStock,
                CategorieId = dto.CategorieId,
                BrandId = dto.BrandId,
                PhotoPath = dto.PhotoPath,
                DateAjout = DateTime.Now
            };

            // Ajouter le produit
            await _repository.AddAsync(produit);

            // Ajouter les familles olfactives
            if (dto.FamillesOlfactivesIds != null && dto.FamillesOlfactivesIds.Any())
            {
                await _repository.AddFamillesAsync(produit.Id, dto.FamillesOlfactivesIds);
            }

            // Recharger avec les relations
            var produitComplet = await _repository.GetByIdWithRelationsAsync(produit.Id);
            return _mapper.Map<ProduitDTO>(produitComplet!);
        }

        public async Task<bool> UpdateAsync(int id, ProduitCreateUpdateDTO dto)
        {
            var produit = await _repository.GetByIdAsync(id);
            if (produit == null)
                return false;

            // Mettre à jour les propriétés
            produit.Nom = dto.Nom;
            produit.Description = dto.Description;
            produit.Prix = dto.Prix;
            produit.QuantiteEnStock = dto.QuantiteEnStock;
            produit.CategorieId = dto.CategorieId;
            produit.BrandId = dto.BrandId;
            produit.PhotoPath = dto.PhotoPath;

            await _repository.UpdateAsync(produit);

            // Gérer les familles olfactives
            await _repository.RemoveAllFamillesAsync(id);

            if (dto.FamillesOlfactivesIds != null && dto.FamillesOlfactivesIds.Any())
            {
                await _repository.AddFamillesAsync(id, dto.FamillesOlfactivesIds);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var produit = await _repository.GetByIdAsync(id);
            if (produit == null)
                return false;

            // Vérifier si utilisé dans des commandes
            if (await _repository.HasOrdersAsync(id))
                throw new InvalidOperationException("Impossible de supprimer un produit déjà utilisé dans une commande.");

            // Supprimer les familles associées
            await _repository.RemoveAllFamillesAsync(id);

            // Supprimer le produit
            await _repository.DeleteAsync(produit);
            return true;
        }
    }
}
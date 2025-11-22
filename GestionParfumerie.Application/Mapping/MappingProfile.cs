using AutoMapper;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- PRODUIT ---
            CreateMap<Produit, ProduitDTO>()
                .ForMember(dest => dest.CategorieNom, opt => opt.MapFrom(src => src.Categorie.Nom))
                .ForMember(dest => dest.FournisseurNom, opt => opt.MapFrom(src => src.Fournisseur.Nom))
                .ForMember(dest => dest.FamilleOlfactiveNom, opt => opt.MapFrom(src => src.FamilleOlfactive.Nom));
            CreateMap<ProduitCreateUpdateDTO, Produit>();

            // --- CATEGORIE ---
            CreateMap<Categorie, CategorieDTO>()
                .ForMember(dest => dest.NombreProduits, opt => opt.MapFrom(src => src.Produits.Count));
            CreateMap<CategorieDTO, Categorie>();

            // --- FOURNISSEUR ---
            CreateMap<Fournisseur, FournisseurDTO>()
                .ForMember(dest => dest.NombreProduits, opt => opt.MapFrom(src => src.Produits.Count));
            CreateMap<FournisseurDTO, Fournisseur>();

            // --- FAMILLE OLFACTIVE ---
            CreateMap<FamilleOlfactive, FamilleOlfactiveDTO>()
                .ForMember(dest => dest.NombreProduits, opt => opt.MapFrom(src => src.Produits.Count));
            CreateMap<FamilleOlfactiveDTO, FamilleOlfactive>();

            // --- LIGNE COMMANDE ---
            CreateMap<LigneCommande, LigneCommandeDTO>()
              .ForMember(dest => dest.ProduitNom, opt => opt.MapFrom(src => src.Produit.Nom))
              .ForMember(dest => dest.PrixUnitaire, opt => opt.MapFrom(src => src.Produit.Prix))
              .ForMember(dest => dest.TotalLigne, opt => opt.MapFrom(src => src.Quantite * src.Produit.Prix));

            // --- Commande (lecture) ---
            CreateMap<Commande, CommandeDTO>()
                .ForMember(dest => dest.UtilisateurEmail, opt => opt.MapFrom(src => src.Utilisateur.Email))
                .ForMember(dest => dest.NbArticles, opt => opt.MapFrom(src => src.LigneCommandes.Sum(l => l.Quantite)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.LigneCommandes.Sum(l => l.Quantite * l.Produit.Prix)));
        }
    }
}
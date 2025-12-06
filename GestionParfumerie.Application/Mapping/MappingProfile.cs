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
              .ForMember(dest => dest.CategorieNom,
                  opt => opt.MapFrom(src => src.Categorie.Nom))
              .ForMember(dest => dest.BrandNom,
                  opt => opt.MapFrom(src => src.Brand.Nom))
              .ForMember(dest => dest.FamillesOlfactives,
                  opt => opt.MapFrom(src => src.ProduitFamilles
                      .Select(pf => pf.FamilleOlfactive.Nom)
                      .ToList()));

            // Mapping ProduitCreateUpdateDTO -> Produit (pour la création/modification)
            CreateMap<ProduitCreateUpdateDTO, Produit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categorie, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.ProduitFamilles, opt => opt.Ignore()) 
                .ForMember(dest => dest.LigneCommandes, opt => opt.Ignore());

            // --- CATEGORIE ---
            CreateMap<Categorie, CategorieDTO>()
             .ForMember(dest => dest.NombreProduits,
                 opt => opt.MapFrom(src => src.Produits.Count))
             .ForMember(dest => dest.Produits,
                 opt => opt.MapFrom(src => src.Produits));
            CreateMap<CategorieDTO, Categorie>();

            // --- BRAND ---
            CreateMap<Brand, BrandsDTO>()
                .ForMember(dest => dest.NombreProduits, opt => opt.MapFrom(src => src.Produits.Count))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom))
                .ForMember(dest => dest.Produits, opt => opt.Ignore());
            CreateMap<BrandsDTO, Brand>();

            // --- FAMILLE OLFACTIVE ---
            CreateMap<FamilleOlfactive, FamilleOlfactiveDTO>()
                .ForMember(dest => dest.NombreProduits, opt => opt.MapFrom(src => src.ProduitFamilles.Count))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<FamilleOlfactiveDTO, FamilleOlfactive>();

            // --- LIGNE COMMANDE ---
            CreateMap<LigneCommande, LigneCommandeDTO>()
                .ForMember(dest => dest.ProduitNom, opt => opt.MapFrom(src => src.Produit.Nom))
                .ForMember(dest => dest.PrixUnitaire, opt => opt.MapFrom(src => src.PrixUnitaire))
                .ForMember(dest => dest.TotalLigne, opt => opt.MapFrom(src => src.Quantite * src.PrixUnitaire));

            // --- COMMANDE ---
            CreateMap<Commande, CommandeDTO>()
                .ForMember(dest => dest.UtilisateurEmail, opt => opt.MapFrom(src => src.Utilisateur.Email))
                .ForMember(dest => dest.NbArticles, opt => opt.MapFrom(src => src.LigneCommandes.Sum(l => l.Quantite)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.LigneCommandes.Sum(l => l.Quantite * l.PrixUnitaire)));
        }
    }
}

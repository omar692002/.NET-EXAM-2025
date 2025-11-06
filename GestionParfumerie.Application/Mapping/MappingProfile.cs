using AutoMapper;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produit, ProduitDto>()
                .ForMember(dest => dest.CategorieNom, opt => opt.MapFrom(src => src.Categorie.Nom))
                .ForMember(dest => dest.FournisseurNom, opt => opt.MapFrom(src => src.Fournisseur.Nom))
                .ForMember(dest => dest.FamilleOlfactiveNom, opt => opt.MapFrom(src => src.FamilleOlfactive.Nom));

            CreateMap<ProduitDto, Produit>();
        }
    }
}

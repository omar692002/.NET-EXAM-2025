using System.ComponentModel.DataAnnotations;
using GestionParfumerie.Domain.Entities;
using Xunit;
namespace GestionParfumerie.Tests.Tests.Unitaire
{
    public class TestsDomaineCategorie
    {
        private IList<ValidationResult> Validate(Categorie c)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(c, new ValidationContext(c), results, true);
            return results;
        }

        [Fact]
        public void Categorie_Nom_Obligatoire()
        {
            var cat = new Categorie
            {
                Nom = null,
                Description = "Test",
                Produits = new List<Produit>()
            };

            var results = Validate(cat);

            Assert.Contains(results, r => r.MemberNames.Contains("Nom"));
        }

        [Fact]
        public void Categorie_Nom_Depasse_100_Caracteres()
        {
            var cat = new Categorie
            {
                Nom = new string('a', 101),
                Description = "Test",
                Produits = new List<Produit>()
            };

            var results = Validate(cat);

            Assert.Contains(results, r => r.MemberNames.Contains("Nom"));
        }

        [Fact]
        public void Categorie_Description_Depasse_300_Caracteres()
        {
            var cat = new Categorie
            {
                Nom = "Test",
                Description = new string('a', 301),
                Produits = new List<Produit>()
            };

            var results = Validate(cat);

            Assert.Contains(results, r => r.MemberNames.Contains("Description"));
        }
    }

}
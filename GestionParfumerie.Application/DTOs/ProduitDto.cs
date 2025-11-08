using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Application.DTOs
{
    public class ProduitDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public string CategorieNom { get; set; }
        public string FournisseurNom { get; set; }
        public string FamilleOlfactiveNom { get; set; }
    }
}

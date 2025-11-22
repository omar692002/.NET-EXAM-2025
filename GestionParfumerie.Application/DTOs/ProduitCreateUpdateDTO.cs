using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Application.DTOs
{
    public class ProduitCreateUpdateDTO
    {

        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public int Stock { get; set; }

        public int CategorieId { get; set; }
        public int FournisseurId { get; set; }
        public int FamilleOlfactiveId { get; set; }
    }
}

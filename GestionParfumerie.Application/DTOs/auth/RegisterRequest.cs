using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Application.DTOs.auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Adresse { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

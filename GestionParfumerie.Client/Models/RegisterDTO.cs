using System.ComponentModel.DataAnnotations;

namespace GestionParfumerie.Client.Models
{
    public class RegisterDTO

    {
        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; } = "";

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; } = "";

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password { get; set; } = "";

        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

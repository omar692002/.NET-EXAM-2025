using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Services;
using GestionParfumerie.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionParfumerie.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UtilisateursController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommandeService _commandeService;
        private readonly IMapper _mapper;

        public UtilisateursController(
            UserManager<ApplicationUser> userManager,
            ICommandeService commandeService,
            IMapper mapper)
        {
            _userManager = userManager;
            _commandeService = commandeService;
            _mapper = mapper;
        }

        // ==========================================
        // ADMIN : Récupérer tous les utilisateurs
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetAllUtilisateurs()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersDto = new List<UtilisateurDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersDto.Add(new UtilisateurDTO
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    DateNaissance = user.DateNaissance,
                    Adresse = user.Adresse,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return Ok(usersDto);
        }

        // ==========================================
        // ADMIN : Récupérer un utilisateur par ID
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUtilisateurById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UtilisateurDTO
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Nom = user.Nom,
                Prenom = user.Prenom,
                DateNaissance = user.DateNaissance,
                Adresse = user.Adresse,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return Ok(userDto);
        }

        // ==========================================
        // ADMIN : Récupérer les commandes d'un utilisateur
        // ==========================================
        [HttpGet("{id}/commandes")]
        public async Task<IActionResult> GetCommandesUtilisateur(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            var commandes = await _commandeService.GetAllByUtilisateurIdAsync(id);
            var commandesDto = _mapper.Map<IEnumerable<CommandeDTO>>(commandes);

            return Ok(commandesDto);
        }

        // ==========================================
        // ADMIN : Récupérer une commande spécifique avec ses lignes
        // ==========================================
        [HttpGet("{userId}/commandes/{commandeId}")]
        public async Task<IActionResult> GetCommandeDetails(string userId, int commandeId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            var commande = await _commandeService.GetByIdAsync(commandeId);
            if (commande == null)
                return NotFound("Commande non trouvée");

            // Vérifier que la commande appartient bien à cet utilisateur
            if (commande.UtilisateurId != userId)
                return BadRequest("Cette commande n'appartient pas à cet utilisateur");

            var commandeDto = _mapper.Map<CommandeDTO>(commande);
            return Ok(commandeDto);
        }

        // ==========================================
        // ADMIN : Rechercher des utilisateurs
        // ==========================================
        [HttpGet("search")]
        public async Task<IActionResult> SearchUtilisateurs([FromQuery] string? email, [FromQuery] string? nom)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(nom))
                query = query.Where(u => u.Nom.Contains(nom) || u.Prenom.Contains(nom));

            var users = await query.ToListAsync();

            var usersDto = new List<UtilisateurDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersDto.Add(new UtilisateurDTO
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    DateNaissance = user.DateNaissance,
                    Adresse = user.Adresse,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return Ok(usersDto);
        }

        // ==========================================
        // ADMIN : Statistiques d'un utilisateur
        // ==========================================
        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetUtilisateurStats(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            var commandes = await _commandeService.GetAllByUtilisateurIdAsync(id);

            var stats = new UtilisateurStatsDTO
            {
                UtilisateurId = id,
                NombreCommandes = commandes.Count(),
                TotalDepense = commandes.Sum(c => c.Total),
                CommandeEnCours = commandes.Count(c => c.Statut == "EnCours"),
                CommandeLivree = commandes.Count(c => c.Statut == "Livrée"),
                CommandeAnnulee = commandes.Count(c => c.Statut == "Annulée")
            };

            return Ok(stats);
        }
    }
}
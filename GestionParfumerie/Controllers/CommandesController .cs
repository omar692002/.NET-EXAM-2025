using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Services;
using GestionParfumerie.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionParfumerie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommandesController : ControllerBase
    {
        private readonly ICommandeService _commandeService;
        private readonly IMapper _mapper;

        public CommandesController(ICommandeService commandeService, IMapper mapper)
        {
            _commandeService = commandeService;
            _mapper = mapper;
        }

        // ==========================================
        // CLIENT : voir ses commandes
        // ==========================================
        [Authorize(Roles = "Client")]
        [HttpGet("mes-commandes")]
        public async Task<IActionResult> GetMesCommandes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var commandes = await _commandeService.GetAllByUtilisateurIdAsync(userId);
            var dto = _mapper.Map<IEnumerable<CommandeDTO>>(commandes);
            return Ok(dto);
        }

        // ==========================================
        // ADMIN : voir toutes les commandes avec les clients
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCommandes()
        {
            var commandes = await _commandeService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<CommandeDTO>>(commandes);
            return Ok(dto);
        }

        // ==========================================
        // CLIENT : passer une commande
        // ==========================================
        [Authorize(Roles = "Client")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCommande([FromBody] CommandeCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var commandeDto = await _commandeService.CreateAsync(userId, dto);
            return Ok(commandeDto);
        }

        // ==========================================
        // ADMIN : changer le statut d'une commande
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatut(int id, [FromBody] string statut)
        {
            var success = await _commandeService.UpdateStatutAsync(id, statut);
            if (!success) return NotFound();

            return NoContent();
        }


    }
}

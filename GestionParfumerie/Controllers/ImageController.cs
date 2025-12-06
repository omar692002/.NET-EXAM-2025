using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public ImagesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Aucun fichier fourni");

                // Vérifier la taille
                if (file.Length > MaxFileSize)
                    return BadRequest($"Le fichier est trop volumineux. Taille max : {MaxFileSize / 1024 / 1024} MB");

                // Vérifier l'extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    return BadRequest($"Extension non autorisée. Extensions autorisées : {string.Join(", ", _allowedExtensions)}");

                // Créer le dossier si n'existe pas
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Générer un nom unique
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Sauvegarder le fichier
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Retourner le chemin relatif
                var relativePath = $"/images/{uniqueFileName}";
                return Ok(new { path = relativePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur : {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteImage([FromQuery] string photoPath)
        {
            try
            {
                if (string.IsNullOrEmpty(photoPath))
                    return BadRequest("Chemin invalide");

                var fullPath = Path.Combine(_environment.WebRootPath, photoPath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Ok(new { message = "Image supprimée" });
                }

                return NotFound("Image non trouvée");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur : {ex.Message}");
            }
        }
    }
}
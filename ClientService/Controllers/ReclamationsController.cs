using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReclamationService.Data;
using ReclamationService.DTOs;
using ReclamationService.Models;
using System.Security.Claims;

namespace ReclamationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reclamations")]
    public class ReclamationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReclamationsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ResponsableSAV")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reclamation>>> GetReclamations()
        {
            return Ok(await _context.Reclamations.ToListAsync());
        }

        [Authorize(Roles = "Client")]
        [HttpGet("mes-reclamations")]
        public async Task<ActionResult<IEnumerable<Reclamation>>> GetMesReclamations()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var http = new HttpClient();
            var response = await http.GetAsync($"https://localhost:7266/api/Clients/by-userid/{userId}");
            if (!response.IsSuccessStatusCode)
                return BadRequest("Client non trouvé");

            var client = await response.Content.ReadFromJsonAsync<Client>();
            var clientId = client.Id;

            var reclamations = await _context.Reclamations
                .Where(r => r.ClientId == clientId)
                .ToListAsync();

            return Ok(reclamations);
        }

        [Authorize(Roles = "Client,ResponsableSAV")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Reclamation>> GetReclamation(int id)
        {
            var reclamation = await _context.Reclamations.FindAsync(id);
            if (reclamation == null)
                return NotFound();

            if (User.IsInRole("Client"))
            {
                var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (reclamation.ClientId != clientId)
                    return Forbid();
            }

            return Ok(reclamation);
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<ActionResult<Reclamation>> Create([FromBody] ReclamationDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var http = new HttpClient();
            var response = await http.GetAsync(
                $"https://localhost:7266/api/Clients/by-userid/{userId}"
            );

            if (!response.IsSuccessStatusCode)
                return BadRequest("Client non trouvé dans ClientService");

            var client = await response.Content.ReadFromJsonAsync<Client>();
            var clientId = client.Id;
            var reclamation = new Reclamation
            {
                Description = dto.Description,
                ArticleId = dto.ArticleId,
                ClientId = clientId,
                DateReclamation = DateTime.Now,
                Statut = "En attente"
            };

            _context.Reclamations.Add(reclamation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReclamation), new { id = reclamation.Id }, reclamation);
        }

        [Authorize(Roles = "ResponsableSAV")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Reclamation reclamation)
        {
            if (id != reclamation.Id)
                return BadRequest("ID mismatch");

            var existingReclamation = await _context.Reclamations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingReclamation == null)
                return NotFound();

            // 🔹 Conserver le Client et les champs sensibles
            reclamation.ClientId = existingReclamation.ClientId;
            reclamation.ArticleId = existingReclamation.ArticleId;
            reclamation.DateReclamation = existingReclamation.DateReclamation;

            // 🔹 IMPORTANT : ne pas forcer la navigation Client
            reclamation.Client = null;

            _context.Entry(reclamation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Reclamations.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }
        

        [Authorize(Roles = "ResponsableSAV")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reclamation = await _context.Reclamations.FindAsync(id);
            if (reclamation == null)
                return NotFound();

            _context.Reclamations.Remove(reclamation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

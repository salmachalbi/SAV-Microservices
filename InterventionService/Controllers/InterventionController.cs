using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InterventionService.Data;
using InterventionService.Models;
using System.Net.Http.Json;
using InterventionService.DTOs;
using System.Security.Claims;

namespace InterventionService.Controllers
{
    [Authorize]
    [Route("api/interventions")]
    [ApiController]
    public class InterventionController : ControllerBase
    {
        private readonly InterventionDbContext _context;

        public InterventionController(InterventionDbContext context)
        {
            _context = context;
        }

        

        // ==============================
        // RESPONSABLE SAV
        // ==============================

        [Authorize(Roles = "ResponsableSAV")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventions()
        {
            return Ok(await _context.Interventions.ToListAsync());
        }

        [Authorize(Roles = "ResponsableSAV")]
        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention([FromBody] Intervention intervention)
        {
            intervention.DateIntervention = DateTime.Now;

            // 🔹 Vérification garantie simulée
            DateTime dateAchatArticle = DateTime.Now.AddMonths(-10);
            int garantieMois = 12;
            bool sousGarantie = dateAchatArticle.AddMonths(garantieMois) >= DateTime.Now;
            intervention.SousGarantie = sousGarantie;
            intervention.MontantFacture = sousGarantie ? 0 : intervention.CoutPieces + intervention.CoutMainOeuvre;

            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIntervention), new { id = intervention.Id }, intervention);
        }

        [Authorize(Roles = "ResponsableSAV")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutIntervention(int id, [FromBody] Intervention intervention)
        {
            if (id != intervention.Id)
                return BadRequest("ID mismatch");

            _context.Entry(intervention).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [Authorize(Roles = "ResponsableSAV")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteIntervention(int id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
                return NotFound();

            _context.Interventions.Remove(intervention);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==============================
        // CLIENT
        // ==============================

        [Authorize(Roles = "Client")]
        [HttpGet("mes-interventions")]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetMesInterventions()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var http = new HttpClient();
            var response = await http.GetAsync($"https://localhost:7266/api/Clients/by-userid/{userId}");
            if (!response.IsSuccessStatusCode)
                return BadRequest("Client non trouvé");

            var client = await response.Content.ReadFromJsonAsync<ClientDto>();
            int clientId = client.Id;

            var interventions = await _context.Interventions
                .Where(i => i.ClientId == clientId)
                .ToListAsync();

            return Ok(interventions);
        }

        // ==============================
        // CLIENT + RESPONSABLE SAV
        // ==============================

        [Authorize(Roles = "Client,ResponsableSAV")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Intervention>> GetIntervention(int id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
                return NotFound();

            if (User.IsInRole("Client"))
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                using var http = new HttpClient();
                var response = await http.GetAsync($"https://localhost:7266/api/Clients/by-userid/{userId}");
                if (!response.IsSuccessStatusCode)
                    return BadRequest("Client non trouvé");

                var client = await response.Content.ReadFromJsonAsync<ClientDto>();
                int clientId = client.Id;

                if (intervention.ClientId != clientId)
                    return Forbid();
            }

            return Ok(intervention);
        }

        // ==============================
        // MÉTHODE PRIVÉE
        // ==============================

        private bool InterventionExists(int id)
        {
            return _context.Interventions.Any(e => e.Id == id);
        }
    }
}

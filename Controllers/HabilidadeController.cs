using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RPG_API.Data;
using RPG_API.Models;
using RPG_API.ViewModel;

namespace RPG_API.Controllers
{
    [ApiController]
    public class HabilidadeController : Controller
    {
        [Authorize(Roles = "User, Admin")]
        [HttpGet("habilidade/{nome}")]
        public async Task<IActionResult> GetAsync([FromRoute] string nome, [FromServices] AppDbContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                // Use "readonly" para a chave do cache
                var cacheKey = $"habilidadeCache_{nome}";

                if (!cache.TryGetValue(cacheKey, out Habilidade? habilidade))
                {
                    habilidade = await context.TabHabilidades
                        .FirstOrDefaultAsync(h => h.Nome.Equals(nome));

                    if (habilidade is null)
                        return NotFound();

                    cache.Set(cacheKey, habilidade, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }

                return Ok(habilidade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problema interno no servidor", details = ex.Message });
            }
        }


        [Authorize(Roles = "User, Admin")]
        [HttpGet("habilidade/{Id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute]int Id, [FromServices] AppDbContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                const string cacheKey = "habilidadeCache";

                if (!cache.TryGetValue(cacheKey, out Habilidade? habilidades))
                {
                    habilidades = await context.TabHabilidades.FindAsync(Id);

                    if (habilidades is null)
                        return NotFound();

                    cache.Set(cacheKey, habilidades, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }

                return Ok(habilidades);
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("habilidades")]
        public async Task<IActionResult> GetAllAsync([FromServices] AppDbContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                const string cacheKey = "habilidadesCache";

                if (!cache.TryGetValue(cacheKey, out List<Habilidade>? habilidades))
                {
                    habilidades = await context.TabHabilidades.ToListAsync();

                    cache.Set(cacheKey, habilidades, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }

                return Ok(habilidades);
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("habilidades")]

        public async Task<IActionResult> PostAsync([FromBody] CreateHabilidadeViewModel model, [FromServices] AppDbContext context)
        {
            var novaHabilidade = new Habilidade
            {
                Nome = model.Nome,
                Categoria = model.Categoria,
                Rank = model.Rank,
                TipoDeAcao = model.TipoDeAcao,
                Cooldown = model.Cooldown,
                Conjuracao = model.Conjuracao,
                DT = model.DT,
                Efeito = model.Efeito,
                CustoDeMana = AuxClass.descobrirCustoDeMana(model.Rank)
            };

            try
            {
                await context.TabHabilidades.AddAsync(novaHabilidade);
                await context.SaveChangesAsync();
                return Created($"/{novaHabilidade.Id}", model);
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("habilidades/{Id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int Id, [FromBody] UpdateHabilidadeViewModel model, [FromServices] AppDbContext context)
        {
            var aHabilidade = await context.TabHabilidades.FindAsync(Id);

            if (aHabilidade == null) { return NotFound(); }

            aHabilidade.Nome = model.Nome;
            aHabilidade.Categoria = model.Categoria;
            aHabilidade.Rank = model.Rank;
            aHabilidade.TipoDeAcao = model.TipoDeAcao;
            aHabilidade.Cooldown = model.Cooldown;
            aHabilidade.Conjuracao = model.Conjuracao;
            aHabilidade.DT = model.DT;
            aHabilidade.Efeito = model.Efeito;
            aHabilidade.CustoDeMana = AuxClass.descobrirCustoDeMana(model.Rank);

            try
            {
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("habilidades/{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int Id, [FromServices] AppDbContext context)
        {
            var aHabilidade = await context.TabHabilidades.FindAsync(Id);

            if (aHabilidade == null)
                return NotFound();

            try
            {
                context.TabHabilidades.Remove(aHabilidade);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }
    }

    public class AuxClass()
    {
        public static int descobrirCustoDeMana(char rank)
        {
            switch (rank)
            {
                case 'P': return 0;
                case 'D': return 1;
                case 'C': return 2;
                case 'B': return 3;
                case 'A': return 4;
                case 'S': return 5;
            }
            return 0;
        }
    }
}

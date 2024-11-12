using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RPG_API.Data;
using RPG_API.Models;
using RPG_API.Services;
using RPG_API.ViewModel;
using System.Text.RegularExpressions;

namespace RPG_API.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        [HttpPost("account/login")]
        public IActionResult Login([FromBody] UserLoginViewModel model, [FromServices] AppDbContext context, [FromServices] TokenService tokenService)
        {
            var user = context.TabUsers.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            if (Settings.generateHash(model.Password) != user.Senha)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            try
            {
                var token = tokenService.CreateToken(user);
                return Ok(new { token = token });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [HttpPost("account/signup")]
        public IActionResult Signup([FromBody] UserSignupViewModel model, [FromServices] AppDbContext context)
        {
            var user = context.TabUsers.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
                return StatusCode(401, new { message = "E-mail já cadastrado" });

            try
            {
                var userNew = new User
                {
                    Nome = model.Name,
                    Email = model.Email,
                    Senha = Settings.generateHash(model.Password),
                    Role = "User"
                };

                context.TabUsers.Add(userNew);
                context.SaveChanges();

                return Ok(new { userId = userNew.Id });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }
        
        [HttpPut("account/update")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Put([FromBody] UserUpdateViewModel model, [FromServices] AppDbContext context, [FromServices] TokenService tokenService)
        {
            var user = context.TabUsers.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            if (Settings.generateHash(model.Password) != user.Senha)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            try
            {
                user.Nome = model.Name;
                user.Senha = Settings.generateHash(model.newPassword);
;
                if (model.Image is not null)
                {
                    var fileName = $"{Guid.NewGuid()}.jpg";
                    var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Image, "");
                    var bytes = Convert.FromBase64String(data);
                    System.IO.File.WriteAllBytes($"wwwroot/images/{fileName}", bytes);

                    user.FotoPerfil = $"images/{fileName}";
                }

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }
        [HttpGet("account/{Id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Get([FromServices] AppDbContext context, [FromRoute] int id)
        {
            try
            {
                var umUser = await context.TabUsers.FindAsync(id);
                if (umUser == null)
                    return NotFound();

                return Ok(umUser);
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [HttpGet("account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromServices] AppDbContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                const string cacheKey = "accountCache";

                if (!cache.TryGetValue(cacheKey, out List<User>? users))
                {
                    users = await context.TabUsers.ToListAsync();

                    cache.Set(cacheKey, users, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [HttpDelete("account/{Id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int Id, [FromServices] AppDbContext context)
        {
            var user = await context.TabUsers.FindAsync(Id);

            if (user is null)
                return NotFound();

            try
            {
                context.Remove(user);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }

        [HttpGet("account/images/{path}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> getImage([FromRoute] string path, [FromServices] AppDbContext context)
        {
            try
            {
                var imagePath = Path.Combine("wwwroot", "images", path);

                if (!System.IO.File.Exists(imagePath))
                    return NotFound(new { message = "Imagem não encontrada" });

                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

                return File(imageBytes, "image/jpg");
            }
            catch
            {
                return StatusCode(500, new { message = "Problema interno no servidor" });
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RPG_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FotoPerfil {  get; set; } = string.Empty;
    }
}

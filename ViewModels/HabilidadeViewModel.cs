using System.ComponentModel.DataAnnotations;

namespace RPG_API.ViewModel
{
    public class CreateHabilidadeViewModel
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório!")]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O campo 'Categoria' é obrigatório!")]
        [AllowedValues("Buff", "Ataque", "Magia", "Debuff", "Ato", "Passiva")]
        public string Categoria { get; set; } = string.Empty;

        [AllowedValues('D', 'C', 'B', 'A', 'S', 'P')]
        public char Rank { get; set; }

        [AllowedValues("Completa", "Principal", "Livre", "Movimento", "Buff", "Reação")]
        public string TipoDeAcao { get; set; } = string.Empty;

        public string Cooldown { get; set; } = string.Empty;
        public string Conjuracao { get; set; } = string.Empty;
        public int DT { get; set; }

        [Required]
        public string Efeito { get; set; } = string.Empty;
    }

    public class UpdateHabilidadeViewModel
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Categoria' é obrigatório!")]
        [AllowedValues("Buff", "Ataque", "Magia", "Debuff", "Ato", "Passiva")]
        public string Categoria { get; set; } = string.Empty;

        [AllowedValues('D', 'C', 'B', 'A', 'S', 'P')]
        public char Rank { get; set; }

        [AllowedValues("Completa", "Principal", "Livre", "Movimento", "Buff", "Reação")]
        public string TipoDeAcao { get; set; } = string.Empty;
        public string Cooldown { get; set; } = string.Empty;
        public string Conjuracao { get; set; } = string.Empty;
        public int DT { get; set; }

        [Required]
        public string Efeito { get; set; } = string.Empty;
    }
}

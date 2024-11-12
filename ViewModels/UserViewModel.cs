using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace RPG_API.ViewModel
{
        public class UserReturnViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string  Image {  get; set; } = string.Empty;
        }

        public class UserSignupViewModel
        {
            [Required]
            public string Name { get; set; } = string.Empty;
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }

        public class UserLoginViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }

    public class UserUpdateViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string newPassword {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image {  get; set; } = string.Empty;
    }
}

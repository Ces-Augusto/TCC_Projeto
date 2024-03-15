using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCC_Projeto.Models.Usuario
{
    public class LoginViewModel
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }

    public class CadastroViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Inclement (adiciona campo no banco = 1, 2,...)
        public required int Id { get; set; } //Required pois não pode ser NN 

        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "O campo 'Confirmação de Senha' é obrigatório.")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public required string Password2 { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Sobrenome' é obrigatório.")]
        public required string Sobrenome { get; set; }

        // Remova a anotação [required] para esses campos
        public DateTime DataCriacao { get; set; }

        public bool Ativo { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace TCC_Projeto.Models.Usuario
{
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Inclement (adiciona campo no banco = 1, 2,...)
        public int Id { get; set; } //Required pois não pode ser NN 

        [StringLength(50)]
        public required string Nome { get; set; }

        [StringLength(50)]
        public required string Sobrenome { get; set; }

        [StringLength(50)]
        public required string Username { get; set; }

        [StringLength(64)]
        public required string Password { get; set; }

        [StringLength(50)]
        public required string Email { get; set; }

        public required DateTime DataCriacao { get; set; }

        public required Boolean Ativo { get; set; }
    }
}

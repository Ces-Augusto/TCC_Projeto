using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCC_Projeto.Models.Etiqueta
{
    public class Etiquetas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increment (adiciona campo no banco = 1, 2,...)
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public required string Nome { get; set; }

        [StringLength(150)]
        [Required]
        public required string Descricao { get; set; }

        [Required]
        public required byte[] Imagem { get; set; }
    }
}

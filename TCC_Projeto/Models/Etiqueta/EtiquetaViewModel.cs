using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TCC_Projeto.Models.Etiqueta
{
    public class EtiquetaViewModel
    {
        [Required]
        public required string Nome { get; set; }

        [Required]
        public required string Descricao { get; set; }

        [Required]
        public required IFormFile Imagem { get; set; }
    }

    public class PDFViewModel
    {
        [Required(ErrorMessage = "O número do pedido é obrigatório")]
        public required string NPedido { get; set; }

        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O arquivo PDF é obrigatório")]
        public required IFormFile PDF { get; set; }
    }
}

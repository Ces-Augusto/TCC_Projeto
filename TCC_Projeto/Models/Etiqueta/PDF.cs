using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCC_Projeto.Models.Etiqueta
{
    public class PDF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public required string NPedido { get; set; }

        public DateTime Data { get; set; }

        [StringLength(255)]
        public required string? CaminhoPDF { get; set; }
    }
}
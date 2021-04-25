using System;
using System.ComponentModel.DataAnnotations;

namespace Alura.LeilaoOnline.WebApp.Models
{
    public class Leilao
    {        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Título é obrigatório")]
        [MinLength(10)]
        [Display(Name = "Título", Prompt = "Digite o título do leilão")]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Início do Pregão")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime? Inicio { get; set; }

        [Display(Name = "Término do Pregão")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida")]
        public DateTime? Termino { get; set; }

        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; }
        public SituacaoLeilao Situacao { get; set; }
        public string PosterUrl => $"/images/poster-{Id}.jpg";
    }
}
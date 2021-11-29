using System.Collections.Generic;

namespace Alura.LeilaoOnline.WebApp.Models
{
    public class Categoria
    {
        public Categoria(int id, string descricao, string imagem, ICollection<Leilao> leiloes = null)
        {
            Id = id;
            Descricao = descricao;
            Imagem = imagem;
            Leiloes = new List<Leilao>();
        }

        //TODO - Implementar notification pattern - Quando um modificado está inválido ele recebe uma notificação

        public int Id { get; private set; }
        public string Descricao { get; private  set; }
        public string Imagem { get; private set; }
        public ICollection<Leilao> Leiloes { get; private set; }
    }

    public class CategoriaComInfoLeilao : Categoria
    {
        public CategoriaComInfoLeilao(int id, string descricao, string imagem, ICollection<Leilao> leiloes, int emRascunho, int emPregao, int finalizados) : base(id, descricao, imagem, leiloes)
        {
            EmRascunho = emRascunho;
            EmPregao = emPregao;
            Finalizados = finalizados;
        }
        public int EmRascunho { get; set; }
        public int EmPregao { get; set; }
        public int Finalizados { get; set; }
    }
}
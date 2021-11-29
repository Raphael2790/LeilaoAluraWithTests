using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Servicos.Handlers
{
    public class DefaultProdutoServico : IProdutoServico
    {
        private readonly ILeilaoRepositorio _leilaoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        public DefaultProdutoServico(ILeilaoRepositorio leilaoRepositorio, ICategoriaRepositorio categoriaRepositorio)
        {
            _leilaoRepositorio = leilaoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
        }

        public Categoria ConsultaCategoriaPorIdComLeiloesEmPregao(int id)
        {
            return _categoriaRepositorio.BuscarPorId(id);
        }

        public IEnumerable<CategoriaComInfoLeilao> ConsultaCategoriasComTotalDeLeiloesEmPregao()
        {
            return _categoriaRepositorio.BuscarTodos()
                        .Select(c => new CategoriaComInfoLeilao
                        (
                            c.Id,
                            c.Descricao,
                            c.Imagem,
                            c.Leiloes,
                            c.Leiloes.Where(x => x.Situacao == SituacaoLeilao.Rascunho).Count(),
                            c.Leiloes.Where(x => x.Situacao == SituacaoLeilao.Pregao).Count(),
                            c.Leiloes.Where(x => x.Situacao == SituacaoLeilao.Finalizado).Count()
                        ));
        }

        public IEnumerable<Leilao> PesquisarLeiloesEmPregaoPorTermo(string termo)
        {
            return _leilaoRepositorio.BuscarTodos()
                            .Where(c =>
                            c.Titulo.ToUpper().Contains(termo) ||
                            c.Descricao.ToUpper().Contains(termo) ||
                            c.Categoria.Descricao.ToUpper().Contains(termo));
        }
    }
}

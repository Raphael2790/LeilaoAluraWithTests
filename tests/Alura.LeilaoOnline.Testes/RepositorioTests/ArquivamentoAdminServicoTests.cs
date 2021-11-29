using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Dados.EFCore;
using Alura.LeilaoOnline.WebApp.Models;
using Alura.LeilaoOnline.WebApp.Servicos;
using Alura.LeilaoOnline.WebApp.Servicos.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Alura.LeilaoOnline.Testes.RepositorioTests
{
    public class ArquivamentoAdminServicoTests
    {
        private readonly AppDbContext _contexto;
        private readonly CategoriaRepositorioEFCore _categoriaRepositorio;
        private readonly LeilaoRepositorioEFCore _leilaoRepositorio;
        private readonly IAdminServico _adminServico;
        private readonly IUnitOfWork _unitOfWork;
        public ArquivamentoAdminServicoTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("CategoriaLeilaoContextoDb")
                .Options;

            _contexto = new AppDbContext(options);
            _unitOfWork = new UnitOfWork(_contexto);

            _categoriaRepositorio = new CategoriaRepositorioEFCore(_contexto);
            _leilaoRepositorio = new LeilaoRepositorioEFCore(_contexto, _unitOfWork);

            _adminServico = new ArquivamentoAdminServico(_leilaoRepositorio, _categoriaRepositorio);

            _contexto.Database.EnsureDeleted();
            SeedDatabase();
        }

        public static TheoryData<Leilao> LeilaoData => new TheoryData<Leilao>
        {
            new Leilao
            {
                Id = 1,
                Categoria = new Categoria(1, "Carros", "/imagem18228.png"), 
                Descricao = "Carros usados de 1990",
                IdCategoria = 1,
                Inicio = DateTime.Now,
                Situacao = SituacaoLeilao.Rascunho,
                Titulo = "Carros Usados"
            },
            new Leilao
            {
                Id = 2,
                Categoria = new Categoria(2, "Imóveis", "/imagem18227.png"),
                Descricao = "Imóveis antigos década de 70",
                IdCategoria = 2,
                Inicio = DateTime.Now,
                Situacao = SituacaoLeilao.Rascunho,
                Titulo = "Imóveis Antigos"
            }
        };

        [Theory]
        [MemberData(nameof(LeilaoData), MemberType = typeof(ArquivamentoAdminServicoTests))]
        public void CadastrarLeilaoQuandoSucessoDeveRetornarServiceResultSucess(Leilao leilao)
        {
            //arrange

            //act
            var result = _adminServico.CadastrarLeilao(leilao);
            var leilaoSalvo = _contexto.Leiloes.Find(leilao.Id);
            //assert
            Assert.True(result.Success);
            Assert.NotNull(leilaoSalvo);
        }

        private void SeedDatabase()
        {
            
        }
    }
}

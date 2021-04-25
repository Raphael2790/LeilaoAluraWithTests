using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Alura.LeilaoOnline.WebApp.Controllers;
using Alura.LeilaoOnline.WebApp.Servicos;
using Alura.LeilaoOnline.WebApp.Models;
using System;

namespace Alura.LeilaoOnline.Testes
{
    public class LeilaoControllerRemove
    {
        private readonly Mock<IAdminServico> _adminServicoMock = new Mock<IAdminServico>();
        private readonly LeilaoController _leilaoController;
        public LeilaoControllerRemove()
        {
            _leilaoController = new LeilaoController(_adminServicoMock.Object);
        }

        [Fact]
        public void DadoLeilaoInexistenteEntaoRetorna404()
        {
            // arrange
            var idLeilaoInexistente = 11232; // preciso entrar no banco para saber qual é inexistente!! teste deixa de ser automático...
            var actionResultEsperado = typeof(NotFoundResult);
            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoInexistente)).Returns((Leilao)null);

            // act
            var result = _leilaoController.Remove(idLeilaoInexistente);

            // assert
            Assert.IsType(actionResultEsperado, result);
        }

        [Fact]
        public void DadoLeilaoEmPregaoEntaoRetorna405()
        {
            // arrange
            var idLeilaoEmPregao = 11232; // qual leilao está em pregão???!! 
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Pregao;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmPregao)).Returns(leilao);

            // act
            var result = _leilaoController.Remove(idLeilaoEmPregao);

            // assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (result as StatusCodeResult).StatusCode;
            Assert.Equal(405, statusCodeResult);
        }

        [Fact]
        public void DadoLeilaoEmRascunhoEntaoExcluiORegistro()
        {
            // arrange
            var idLeilaoEmRascunho = 11232; // qual leilao está em rascunho???!!

            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmRascunho)).Returns(leilao);
            _adminServicoMock.Setup(ad => ad.RemoverLeilao(leilao)).Returns(new ServiceResult().SetSuccess());
            //act
            var result = _leilaoController.Remove(idLeilaoEmRascunho);

            // assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DadoLeilaoExistenteDeveRetornarBadResquestQuandoResultSetError()
        {
            // arrange
            var idLeilaoEmRascunho = 11232; // qual leilao está em rascunho???!!

            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";
            var exception = new Exception("Ocorreu um erro inesperado");

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmRascunho)).Returns(leilao);
            _adminServicoMock.Setup(ad => ad.RemoverLeilao(leilao)).Returns(new ServiceResult().SetError(exception));
            //act
            var result = _leilaoController.Remove(idLeilaoEmRascunho);

            // assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}

using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Alura.LeilaoOnline.WebApp.Controllers;
using Alura.LeilaoOnline.WebApp.Servicos;
using Alura.LeilaoOnline.WebApp.Models;
using System;

namespace Alura.LeilaoOnline.Testes
{
    public class LeilaoControllerTests
    {
        private readonly Mock<IAdminServico> _adminServicoMock = new Mock<IAdminServico>();
        private readonly LeilaoController _leilaoController;
        public LeilaoControllerTests()
        {
            _leilaoController = new LeilaoController(_adminServicoMock.Object);
        }

        #region ControllerRemoveTests
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
        #endregion

        #region ControllerFinalizaTestes
        [Theory]
        [InlineData(11232)]
        public void FinalizaLeilaoPorIdQuandoLeilaoNaoExistirDeveRetornarNotFound(int idLeilaoInexistente)
        {
            //arrange
            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoInexistente)).Returns((Leilao)null);

            // act
            var result = _leilaoController.Finaliza(idLeilaoInexistente);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(11232)]
        public void FinalizaLeilaoPorIdQuandoLeilaoEmPregaoRetornaStatusCode405(int idLeilaoEmPregao)
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmPregao)).Returns(leilao);

            //act
            var result = _leilaoController.Finaliza(idLeilaoEmPregao);

            //assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (result as StatusCodeResult).StatusCode;
            Assert.Equal(405, statusCodeResult);
        }

        [Theory]
        [InlineData(11232)]
        public void FinalizaLeilaoPorIdQuandoHouverErroDeveRetornarBadRequestObject(int idLeilaoEmRascunho)
        {
            // arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Pregao;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";
            var exception = new Exception("Ocorreu um erro inesperado");

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmRascunho)).Returns(leilao);
            _adminServicoMock.Setup(ad => ad.FinalizarPregaoDoLeilaoPorId(idLeilaoEmRascunho)).Returns(new ServiceResult().SetError(exception));
            //act
            var result = _leilaoController.Finaliza(idLeilaoEmRascunho);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(11232)]
        public void FinalizaLeilaoPorIdQuandoLeilaoEmPregaoDeveRetornarRedirectToAcionResult(int idLeilao)
        {
            // arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Pregao;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilao)).Returns(leilao);
            _adminServicoMock.Setup(ad => ad.FinalizarPregaoDoLeilaoPorId(idLeilao)).Returns(new ServiceResult().SetSuccess());

            //act
            var result = _leilaoController.Finaliza(idLeilao);

            //assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion

        #region ControllerIniciaTestes
        [Theory]
        [InlineData(11232)]
        public void IniciaLeilaoPorIdQuandoLeilaoNaoExistirDeveRetornarNotFound(int idLeilaoInexistente)
        {
            //arrange
            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoInexistente)).Returns((Leilao)null);

            // act
            var result = _leilaoController.Inicia(idLeilaoInexistente);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(11232)]
        public void IniciaLeilaoPorIdQuandoLeilaoEmPregaoRetornaStatusCode405(int idLeilaoEmPregao)
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Pregao;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilaoEmPregao)).Returns(leilao);

            //act
            var result = _leilaoController.Inicia(idLeilaoEmPregao);

            //assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (result as StatusCodeResult).StatusCode;
            Assert.Equal(405, statusCodeResult);
        }

        [Theory]
        [InlineData(11232)]
        public void IniciaLeilaoPorIdQuandoHouverErroDeveRetornarBadRequestObject(int idLeilaoEmRascunho)
        {
            // arrange
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
            _adminServicoMock.Setup(ad => ad.IniciarPregaoDoLeilaoPorId(idLeilaoEmRascunho)).Returns(new ServiceResult().SetError(exception));
            //act
            var result = _leilaoController.Inicia(idLeilaoEmRascunho);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(11232)]
        public void IniciaLeilaoPorIdQuandoLeilaoEmPregaoDeveRetornarRedirectToAcionResult(int idLeilao)
        {
            // arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ConsultarLeilaoPorId(idLeilao)).Returns(leilao);
            _adminServicoMock.Setup(ad => ad.IniciarPregaoDoLeilaoPorId(idLeilao)).Returns(new ServiceResult().SetSuccess());

            //act
            var result = _leilaoController.Inicia(idLeilao);

            //assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion

        #region ControllerEditTests
        [Fact]
        public void ModificarLeilaoQuandoModeloLeilaoInvalidoRetornaViewResult()
        {
            //arrange
            Leilao leilao = new Leilao();
            //act
            var result = _leilaoController.Edit(leilao);
            //assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ModificarLeilaoQuandoPostHouverErroDeveRetornarBadRequest()
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";
            var exception = new Exception("Ocorreu um erro inesperado");

            _adminServicoMock.Setup(ad => ad.ModificarLeilao(leilao)).Returns(new ServiceResult().SetError(exception));
            //act
            var result = _leilaoController.Edit(leilao);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ModificarLeilaoQuandoSucessoDeveRetornarRedirectToAction()
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.ModificarLeilao(leilao)).Returns(new ServiceResult().SetSuccess());
            //act
            var result = _leilaoController.Edit(leilao);
            //assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion

        #region ControllerInsertTests
        [Fact]
        public void CadastrarLeilaoQuandoModeloLeilaoInvalidoRetornaViewResult()
        {
            //arrange
            Leilao leilao = new Leilao();
            //act
            var result = _leilaoController.Insert(leilao);
            //assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CadastrarLeilaoQuandoPostHouverErroDeveRetornarBadRequest()
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";
            var exception = new Exception("Ocorreu um erro inesperado");

            _adminServicoMock.Setup(ad => ad.CadastrarLeilao(leilao)).Returns(new ServiceResult().SetError(exception));
            //act
            var result = _leilaoController.Insert(leilao);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CadastrarLeilaoQuandoSucessoDeveRetornarRedirectToAction()
        {
            //arrange
            var leilao = new Leilao();
            leilao.Categoria = new Categoria(1, "Carros", "....");
            leilao.Descricao = "Leilão de carros usados";
            leilao.IdCategoria = 1;
            leilao.Situacao = SituacaoLeilao.Rascunho;
            leilao.Inicio = new System.DateTime(2021, 02, 26);
            leilao.Id = 11232;
            leilao.Titulo = "Carros usados";

            _adminServicoMock.Setup(ad => ad.CadastrarLeilao(leilao)).Returns(new ServiceResult().SetSuccess());
            //act
            var result = _leilaoController.Insert(leilao);
            //assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion
    }
}

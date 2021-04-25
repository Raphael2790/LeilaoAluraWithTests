using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Servicos.Handlers
{
    public class DefaultAdminServico : IAdminServico
    {
        private readonly ILeilaoRepositorio _leilaoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        public DefaultAdminServico(ILeilaoRepositorio leilaoRepositorio, ICategoriaRepositorio categoriaRepositorio)
        {
            _leilaoRepositorio = leilaoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
        }

        public ServiceResult CadastrarLeilao(Leilao leilao)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                _leilaoRepositorio.Incluir(leilao);
                return result.SetSuccess();
            }
            catch (Exception ex)
            {

                return result.SetError(ex.Message);
            }
        }

        public IEnumerable<Categoria> ConsultarCategorias()
        {
            return _categoriaRepositorio.BuscarTodos();
        }

        public Leilao ConsultarLeilaoPorId(int id)
        {
            return _leilaoRepositorio.BuscarPorId(id);
        }

        public IEnumerable<Leilao> ConsultarLeiloes()
        {
            return _leilaoRepositorio.BuscarTodos();
        }

        public ServiceResult FinalizarPregaoDoLeilaoPorId(int id)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var leilao = _leilaoRepositorio.BuscarPorId(id);
                if (leilao != null && leilao.Situacao == SituacaoLeilao.Pregao)
                {
                    leilao.Situacao = SituacaoLeilao.Finalizado;
                    leilao.Termino = DateTime.Now;
                    _leilaoRepositorio.Atualizar(leilao);
                }

                return result.SetSuccess();
            }
            catch (Exception ex)
            {
                return result.SetError(ex.Message);   
            }
            
        }

        public ServiceResult IniciarPregaoDoLeilaoPorId(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var leilao = _leilaoRepositorio.BuscarPorId(id);
                if (leilao != null && leilao.Situacao == SituacaoLeilao.Rascunho)
                {
                    leilao.Situacao = SituacaoLeilao.Pregao;
                    leilao.Inicio = DateTime.Now;
                    _leilaoRepositorio.Atualizar(leilao);
                }

                return result.SetSuccess();
            }
            catch (Exception ex)
            {

                return result.SetError(ex.Message);
            }
        }

        public ServiceResult ModificarLeilao(Leilao leilao)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (leilao != null)
                    _leilaoRepositorio.Atualizar(leilao);

                return result.SetSuccess();
            }
            catch (Exception ex)
            {

                return result.SetError(ex.Message);
            }
           
        }

        public ServiceResult RemoverLeilao(Leilao leilao)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if(leilao != null && leilao.Situacao != SituacaoLeilao.Pregao)
                    _leilaoRepositorio.Excluir(leilao);

                return result.SetSuccess();
            }
            catch (Exception ex)
            {

                return result.SetError(ex.Message);
            }
        }
    }
}

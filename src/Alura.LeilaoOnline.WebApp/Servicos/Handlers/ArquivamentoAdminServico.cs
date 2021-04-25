using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Servicos.Handlers
{
    public class ArquivamentoAdminServico : IAdminServico
    {
        private readonly IAdminServico _defaultAdminServico;
        public ArquivamentoAdminServico(ILeilaoRepositorio leilaoRepositorio, ICategoriaRepositorio categoriaRepositorio)
        {
            _defaultAdminServico = new DefaultAdminServico(leilaoRepositorio, categoriaRepositorio);
        }
        public void CadastrarLeilao(Leilao leilao)
        {
            _defaultAdminServico.CadastrarLeilao(leilao);
        }

        public IEnumerable<Categoria> ConsultarCategorias()
        {
            return _defaultAdminServico.ConsultarCategorias();
        }

        public Leilao ConsultarLeilaoPorId(int id)
        {
            return _defaultAdminServico.ConsultarLeilaoPorId(id);
        }

        public IEnumerable<Leilao> ConsultarLeiloes()
        {
            return _defaultAdminServico.ConsultarLeiloes()
                        .Where(x => x.Situacao != SituacaoLeilao.Arquivado)
                        .ToList();
        }

        public void FinalizarPregaoDoLeilaoPorId(int id)
        {
            _defaultAdminServico.FinalizarPregaoDoLeilaoPorId(id);
        }

        public void IniciarPregaoDoLeilaoPorId(int id)
        {
            _defaultAdminServico.IniciarPregaoDoLeilaoPorId(id);
        }

        public void ModificarLeilao(Leilao leilao)
        {
            _defaultAdminServico.ModificarLeilao(leilao);
        }

        public ServiceResult RemoverLeilao(Leilao leilao)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                if (leilao != null && leilao.Situacao != SituacaoLeilao.Pregao)
                {
                    leilao.Situacao = SituacaoLeilao.Arquivado;
                    _defaultAdminServico.ModificarLeilao(leilao);
                }
                return result.SetSuccess();
            }
            catch (Exception ex)
            {

                return result.SetError(ex.Message);
            }
           
        }
    }
}

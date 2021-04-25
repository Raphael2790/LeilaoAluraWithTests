using Alura.LeilaoOnline.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Servicos
{
    public interface IAdminServico
    {
        IEnumerable<Categoria> ConsultarCategorias();
        IEnumerable<Leilao> ConsultarLeiloes();
        Leilao ConsultarLeilaoPorId(int id);
        ServiceResult CadastrarLeilao(Leilao leilao);
        ServiceResult ModificarLeilao(Leilao leilao);
        ServiceResult RemoverLeilao(Leilao leilao);
        ServiceResult IniciarPregaoDoLeilaoPorId(int id);
        ServiceResult FinalizarPregaoDoLeilaoPorId(int id);
    }
}

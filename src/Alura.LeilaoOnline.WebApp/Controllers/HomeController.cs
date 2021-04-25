using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Models;
using Microsoft.AspNetCore.Routing;
using Alura.LeilaoOnline.WebApp.Servicos;

namespace Alura.LeilaoOnline.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProdutoServico _servico;

        public HomeController(IProdutoServico service)
        {
            _servico = service;
        }

        public IActionResult Index()
        {
            var categorias = _servico.ConsultaCategoriasComTotalDeLeiloesEmPregao();
            return View(categorias);
        }

        [Route("[controller]/StatusCode/{statusCode}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            if (statusCode == 404) return View("404");
            return View(statusCode);
        }

        [Route("[controller]/Categoria/{categoria}")]
        public IActionResult Categoria(int categoriaId)
        {
            var categ = _servico.ConsultaCategoriaPorIdComLeiloesEmPregao(categoriaId);
            return View(categ);
        }

        [HttpPost]
        [Route("[controller]/Busca")]
        public IActionResult Busca(string termo)
        {
            ViewData["termo"] = termo;
            var termoNormalizado = termo.ToUpper();
            var leiloes = _servico.PesquisarLeiloesEmPregaoPorTermo(termoNormalizado);
            return View(leiloes);
        }
    }
}

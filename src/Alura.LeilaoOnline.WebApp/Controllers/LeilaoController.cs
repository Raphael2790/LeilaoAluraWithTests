using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Alura.LeilaoOnline.WebApp.Models;
using System;
using Alura.LeilaoOnline.WebApp.Servicos;

namespace Alura.LeilaoOnline.WebApp.Controllers
{
    public class LeilaoController : Controller
    {

        private readonly IAdminServico _servico;

        public LeilaoController(IAdminServico servico)
        {
            _servico = servico;
        }

        public IActionResult Index()
        {
            var leiloes = _servico.ConsultarLeiloes();
            return View(leiloes);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            ViewData["Categorias"] = _servico.ConsultarCategorias();
            ViewData["Operacao"] = "Inclusão";
            return View("Form");
        }

        [HttpPost]
        public IActionResult Insert(Leilao model)
        {
            if (ModelState.IsValid && (!string.IsNullOrEmpty(model.Titulo) || !string.IsNullOrEmpty(model.Descricao)))
            {
                var result  = _servico.CadastrarLeilao(model);

                if (!result.Success)
                    return BadRequest("Houve um erro inesperado,tente mais tarde");

                return RedirectToAction("Index");
            }
            ViewData["Categorias"] = _servico.ConsultarCategorias();
            ViewData["Operacao"] = "Inclusão";
            return View("Form", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["Categorias"] = _servico.ConsultarCategorias();
            ViewData["Operacao"] = "Edição";
            var leilao = _servico.ConsultarLeilaoPorId(id);
            if (leilao == null) return NotFound();
            return View("Form", leilao);
        }

        [HttpPost]
        public IActionResult Edit(Leilao model)
        {
            if (ModelState.IsValid && (!string.IsNullOrEmpty(model.Titulo) || !string.IsNullOrEmpty(model.Descricao)))
            {
                var result = _servico.ModificarLeilao(model);

                if (!result.Success)
                    return BadRequest("Houve um erro inesperado,tente mais tarde");

                return RedirectToAction("Index");
            }
            ViewData["Categorias"] = _servico.ConsultarCategorias();
            ViewData["Operacao"] = "Edição";
            return View("Form", model);
        }

        [HttpPost]
        public IActionResult Inicia(int id)
        {
            var leilao = _servico.ConsultarLeilaoPorId(id);
            if (leilao == null) 
                return NotFound();

            if (leilao.Situacao != SituacaoLeilao.Rascunho) 
                return StatusCode(405);

            var result = _servico.IniciarPregaoDoLeilaoPorId(id);

            if (!result.Success)
                return BadRequest("Houve um erro inesperado");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Finaliza(int id)
        {
            var leilao = _servico.ConsultarLeilaoPorId(id);
            if (leilao == null) return NotFound();

            if (leilao.Situacao != SituacaoLeilao.Pregao) 
                return StatusCode(405);

            var result = _servico.FinalizarPregaoDoLeilaoPorId(id);

            if (!result.Success) 
                return BadRequest("Houve um erro inesperado");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var leilao = _servico.ConsultarLeilaoPorId(id);
            if (leilao == null) return NotFound();
            if (leilao.Situacao == SituacaoLeilao.Pregao) return StatusCode(405);

            var result = _servico.RemoverLeilao(leilao);

            if(result.Success) return NoContent();

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Pesquisa(string termo)
        {
            ViewData["termo"] = termo;
            var leiloes = _servico.ConsultarLeiloes()
                            .Where(l => string.IsNullOrWhiteSpace(termo) || 
                            l.Titulo.ToUpper().Contains(termo.ToUpper()) || 
                            l.Descricao.ToUpper().Contains(termo.ToUpper()) ||
                            l.Categoria.Descricao.ToUpper().Contains(termo.ToUpper())
                );
            return View("Index", leiloes);
        }
    }
}

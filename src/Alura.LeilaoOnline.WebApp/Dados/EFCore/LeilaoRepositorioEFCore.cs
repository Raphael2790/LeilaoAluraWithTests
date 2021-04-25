using Alura.LeilaoOnline.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Alura.LeilaoOnline.WebApp.Dados.EFCore
{
    public class LeilaoRepositorioEFCore : ILeilaoRepositorio
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public LeilaoRepositorioEFCore(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public Leilao BuscarPorId(int id)
        {
            return _context.Leiloes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Leilao> BuscarTodos()
        {
            return _context.Leiloes.Include(x => x.Categoria).ToList();
        }

        public void Incluir(Leilao leilao)
        {
            _context.Leiloes.Add(leilao);
            _unitOfWork.Commit();
        }

        public void Atualizar(Leilao leilao)
        {
            _context.Leiloes.Update(leilao);
            _unitOfWork.Commit();
        }

        public void Excluir(Leilao leilao)
        {
            _context.Leiloes.Remove(leilao);
            _unitOfWork.Commit();
        }
    }
}

using Alura.LeilaoOnline.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Dados.EFCore
{
    public class CategoriaRepositorioEFCore : ICategoriaRepositorio
    {
        private readonly AppDbContext _context;
        public CategoriaRepositorioEFCore(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Categoria> BuscarTodos()
        {
            return _context.Categorias.Include(x => x.Leiloes).ToList();
        }

        public Categoria BuscarPorId(int id)
        {
            return _context.Categorias.Include(x => x.Leiloes).FirstOrDefault(x => x.Id == id);
        }
    }
}

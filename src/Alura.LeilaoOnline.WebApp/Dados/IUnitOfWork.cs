using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.LeilaoOnline.WebApp.Dados
{
    public interface IUnitOfWork
    {
        void Commit();
        void CommitAsync();
    }
}

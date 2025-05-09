using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioProtech.Domain.Entities;

namespace DesafioProtech.Domain.Interfaces
{
    public interface IAnimeRepository
    {
        Task<Anime> AdicionarAsync(Anime anime);
        Task<IEnumerable<Anime>> ListarAsync(string? nome, string? diretor,string? resumo, int pagina, int paginaTamanho);
        Task AtualizarAsync (Anime anime);
        Task RemoverLogicamenteAsync (int id);
        Task<Anime?> ObterPorIdAsync(int id);

    }
}

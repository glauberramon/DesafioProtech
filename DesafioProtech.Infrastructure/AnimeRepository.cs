using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using DesafioProtech.Domain.Interfaces;
using DesafioProtech.Domain.Entities;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

using DesafioProtech.Infrastructure.Data;
using System.Linq.Expressions;
using DesafioProtech.Domain.Exceptions;
using System.Xml.Linq;

namespace DesafioProtech.Infrastructure
{
    public class AnimeRepository : IAnimeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AnimeRepository> _logger;

        public AnimeRepository(AppDbContext context, ILogger<AnimeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Anime> AdicionarAsync(Anime anime)
        {
            try
            {
                await _context.Set<Anime>().AddAsync(anime);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Anime adicionado com sucesso. ID:{AnimeId}, Nome: {Nome}", anime.Id, anime.Nome);
                return anime;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Falha ao adicionar anime no banco. ID:{AnimeId}, Nome: {Nome}", anime.Id, anime.Nome);
                throw new Exception("Erro de persistência no banco de dados", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar anime");
                throw;
            }

        }

        public async Task AtualizarAsync(Anime anime)
        {
            try
            {
                var animeDb = await ObterPorIdAsync(anime.Id);

                if (animeDb == null)
                    throw new NotFoundException($"Anime não encontrado. Id:{anime.Id}");

                _logger.LogInformation("Anime encontrado com sucesso. ID:{AnimeId}. Nome:{Nome}", animeDb.Id, animeDb.Nome);

                if (anime.Nome != null)
                    animeDb.Nome = anime.Nome;

                if (anime.Diretor != null)
                    animeDb.Diretor = anime.Diretor;

                if (anime.Resumo != null)
                    animeDb.Resumo = anime.Resumo;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Anime atualizado. ID:{AnimeId}", anime.Id);
            }
            catch (NotFoundException ex) 
            {
                _logger.LogWarning(ex.Message); 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar anime. ID:{AnimeId}", anime.Id);
                throw;
            }


        }

        public async Task<IEnumerable<Anime>> ListarAsync(string? nome, string? diretor, string? resumo, int pagina, int paginaTamanho)
        {
            try
            {
                var query = _context.Set<Anime>().Where(a => a.Ativo);

                if (!string.IsNullOrEmpty(nome))
                    query = query.Where(a => a.Nome.Contains(nome));

                if (!string.IsNullOrEmpty(diretor))
                    query = query.Where(a => a.Diretor.Contains(diretor));

                if (!string.IsNullOrEmpty(resumo))
                    query = query.Where(a => a.Resumo.Contains(resumo));


                var result = await query.Skip((pagina - 1) * paginaTamanho).Take(paginaTamanho).ToListAsync();
                _logger.LogInformation("Listagem de animes. Total:{Quantidade}", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao listar animes.");
                throw;
            }

        }

        public async Task<Anime?> ObterPorIdAsync(int id)
        {
            try
            {

                return await _context.Set<Anime>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar anime. ID:{AnimeId}", id);
                throw;
            }

        }

        public async Task RemoverLogicamenteAsync(int id)
        {
            try
            {
                var animeDb = await ObterPorIdAsync(id);

                if (animeDb == null)
                    throw new NotFoundException($"Anime não encontrado. Id:{id}");


                animeDb.Ativo = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Anime desativado logicamente. ID:{AnimeId}", id);

            }
            catch (NotFoundException ex) 
            {
                _logger.LogWarning(ex.Message); 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao desativar anime. ID:{AnimeId}", id);
                throw;
            }



        }
    }
}

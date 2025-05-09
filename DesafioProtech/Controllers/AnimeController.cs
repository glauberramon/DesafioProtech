using DesafioProtech.API.DTOs;
using DesafioProtech.Domain.Entities;
using DesafioProtech.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DesafioProtech.Domain.Exceptions;

namespace DesafioProtech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Gestão de Animes")]
    public class AnimeController : ControllerBase
    {
        private readonly IAnimeRepository _repository;
        private readonly ILogger<AnimeController> _logger;

        public AnimeController(IAnimeRepository repository, ILogger<AnimeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Lista os animes cadastrados com filtros e paginação
        /// </summary>
        /// <remarks>
        /// Exemplo completo:
        /// 
        /// GET /api/anime?nome=Naruto&amp;diretor=Masashi Kishimoto&amp;resumo=Ninja&amp;pagina=1&amp;tamanhoPagina=10
        /// </remarks>
        /// <param name="nome">Filtro por nome (opcional)</param>
        /// <param name="diretor">Filtro por diretor (opcional)</param>
        /// <param name="resumo">Filtro por palavra no resumo (opcional)</param>
        /// <param name="pagina">Número da página (inicia em 1)</param>
        /// <param name="tamanhoPagina">Itens por página (máx. 50)</param>
        /// <returns>Lista paginada de animes ativos</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        /// <response code="400">Parâmetros de paginação inválidos</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Anime>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar(
            [FromQuery] string? nome,
            [FromQuery] string? diretor,
            [FromQuery] string? resumo,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            try
            {
                if (pagina < 1 || tamanhoPagina > 50)
                    return BadRequest("Paginação inválida");

                var animes = await _repository.ListarAsync(nome, diretor, resumo, pagina, tamanhoPagina);
                return Ok(animes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao listar animes");
                return StatusCode(500, "Erro interno");
            }
        }

        /// <summary>
        /// Obtém detalhes de um anime específico
        /// </summary>
        /// <remarks>
        /// Exemplo: GET /api/anime/1
        /// </remarks>
        /// <param name="id">ID do anime</param>
        /// <returns>Dados completos do anime</returns>
        /// <response code="200">Anime encontrado</response>
        /// <response code="404">Anime não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Anime), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Anime>> ObterPorId(int id)
        {
            try
            {
                var anime = await _repository.ObterPorIdAsync(id);
                if (anime == null)
                    throw new NotFoundException($"Anime ID {id} não encontrado");

                return Ok(anime);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar anime ID {id}", id);
                return StatusCode(500, "Erro interno");
            }
        }

        /// <summary>
        /// Cadastra um novo anime
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     POST /api/anime
        ///     {
        ///         "nome": "Attack on Titan",
        ///         "diretor": "Tetsurō Araki",
        ///         "resumo": "Humanidade vive em cidades protegidas..."
        ///     }
        /// </remarks>
        /// <param name="animeDto">Dados do anime</param>
        /// <returns>Anime criado</returns>
        /// <response code="201">Anime criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        [ProducesResponseType(typeof(Anime), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Cadastrar([FromBody] CreateAnimeDto animeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var anime = new Anime
                {
                    Nome = animeDto.Nome,
                    Diretor = animeDto.Diretor,
                    Resumo = animeDto.Resumo,
                    Ativo = true
                };

                await _repository.AdicionarAsync(anime);
                return CreatedAtAction(nameof(ObterPorId), new { id = anime.Id }, anime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar anime: {Nome}", animeDto.Nome);
                return StatusCode(500, new { Mensagem = "Erro interno" });
            }
        }

        /// <summary>
        /// Atualiza os dados de um anime existente
        /// </summary>
        /// <remarks>
        /// Campos não informados permanecerão inalterados
        /// 
        /// Exemplo:
        /// 
        ///     PUT /api/anime/1
        ///     {
        ///         "nome": "Novo Nome",
        ///         "resumo": "Descrição atualizada"
        ///     }
        /// </remarks>
        /// <param name="id">ID do anime a ser atualizado</param>
        /// <param name="dto">Dados parciais para atualização</param>
        /// <response code="204">Atualização bem-sucedida</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Anime não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateAnimeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var animeDb = await _repository.ObterPorIdAsync(id);
                if (animeDb == null)
                    throw new NotFoundException($"Anime ID {id} não encontrado");

                if (!string.IsNullOrEmpty(dto.Nome))
                    animeDb.Nome = dto.Nome;

                if (!string.IsNullOrEmpty(dto.Diretor))
                    animeDb.Diretor = dto.Diretor;

                if (!string.IsNullOrEmpty(dto.Resumo))
                    animeDb.Resumo = dto.Resumo;

                await _repository.AtualizarAsync(animeDb);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar anime ID {id}", id);
                return StatusCode(500, "Erro interno");
            }
        }

        /// <summary>
        /// Desativa um anime (exclusão lógica)
        /// </summary>
        /// <remarks>
        /// O registro permanece no banco com status inativo
        /// 
        /// Exemplo: DELETE /api/anime/1
        /// </remarks>
        /// <param name="id">ID do anime a desativar</param>
        /// <response code="204">Desativação bem-sucedida</response>
        /// <response code="404">Anime não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoverLogicamente(int id)
        {
            try
            {
                await _repository.RemoverLogicamenteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar anime ID {id}", id);
                return StatusCode(500, new { Mensagem = "Erro interno" });
            }
        }
    }
}
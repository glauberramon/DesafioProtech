using DesafioProtech.API.DTOs;
using DesafioProtech.Domain.Entities;
using DesafioProtech.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

using DesafioProtech.Domain.Exceptions;

namespace DesafioProtech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly IAnimeRepository _repository;
        private readonly ILogger<AnimeController> _logger;

        public AnimeController(IAnimeRepository repository, ILogger<AnimeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? nome,
            [FromQuery] string? diretor,
            [FromQuery] string? resumo,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            try
            {
                var animes = await _repository.ListarAsync(nome, diretor, resumo, pagina, tamanhoPagina);
                return Ok(animes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao listar animes");
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Anime>> ObterPorId(int id)
        {
            try
            {
                var anime = await _repository.ObterPorIdAsync(id);
                if (anime == null)
                    throw new NotFoundException($"Anime não encontrado. Id:{id}");
                return Ok(anime);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar anime Id {id}", id);
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpPost]
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
                _logger.LogError(ex, "Erro ao cadastrar anime");
                return StatusCode(500, new { Mensagem = "Erro interno" });
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateAnimeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var anime = new Anime { Resumo = dto.Resumo, Nome = dto.Nome, Diretor = dto.Diretor, Id = id };
                await _repository.AtualizarAsync(anime);
                return NoContent();
            }
            catch (NotFoundException ex) 
            {
                _logger.LogWarning(ex.Message); 
                return NotFound(ex.Message); 
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro ao atualizar anime");
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                _logger.LogError(ex, "Erro ao desativar anime");
                return StatusCode(500, new { Mensagem = "Erro interno" });
            }
            
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}

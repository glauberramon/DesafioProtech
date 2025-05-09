using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

using DesafioProtech.Domain.Interfaces;
using DesafioProtech.API.Controllers;
using Microsoft.Extensions.Logging;
using DesafioProtech.API.DTOs;
using DesafioProtech.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using DesafioProtech.Domain.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace DesafioProtech.Tests.Controllers
{
    public class AnimeControllerTests
    {
        private readonly Mock<IAnimeRepository> _mockRepository;
        private readonly AnimeController _controller;
        private readonly Mock<ILogger<AnimeController>> _mockLogger;
        public AnimeControllerTests()
        {
            _mockRepository = new Mock<IAnimeRepository>();
            _mockLogger = new Mock<ILogger<AnimeController>>();
            _controller = new AnimeController(_mockRepository.Object, _mockLogger.Object);
        }

        #region Testes Cadastrar
        [Fact]
        public async Task Cadastrar_DeveRetornar201_QuandoDtoValido()
        {
            // Arrange
            var dto = new CreateAnimeDto
            {
                Nome = "Naruto",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };

            var animeEsperado = new Anime
            {
                Id = 1,
                Nome = dto.Nome,
                Diretor = dto.Diretor,
                Resumo = dto.Resumo,
                Ativo = true
            };

            _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Anime>()))
                          .ReturnsAsync(animeEsperado);

            // Act
            var result = await _controller.Cadastrar(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Cadastrar_DeveLogarErro500_QuandoOcorrerExcecao()
        {
            // Arrange
            var dto = new CreateAnimeDto
            {
                Nome = "Naruto",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };

            _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Anime>()))
                          .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Cadastrar(dto);

            // Assert
            Assert.IsType<ObjectResult>(result);

            // Verifique se o erro foi logado
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao cadastrar anime")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarErro400_QuandoDtoInvalido()
        {
            // Arrange 
            var dto = new CreateAnimeDto
            {
                Nome = "",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };

            _controller.ModelState.AddModelError("Nome", "O nome é obrigatório");

            // Act
            var result = await _controller.Cadastrar(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Assert do Logger  
            _mockLogger.Verify(
       x => x.Log(
           LogLevel.Warning,
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("DTO inválido")),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
       Times.AtLeastOnce);
        }
        #endregion

        #region Testes Listar
        [Fact]
        public async Task Listar_DeveRetornar200_QuandoAnimesExistem()
        {
            // Arrange
            var animes = new List<Anime>
            {
                new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um ninja adolescente..." },
                new Anime { Id = 2, Nome = "One Piece", Diretor = "Eiichiro Oda", Resumo = "Aventura em alto mar..." }
            };
            _mockRepository.Setup(r => r.ListarAsync(null, null, null, 1, 10))
                          .ReturnsAsync(animes);
            // Act
            var result = await _controller.Listar(null, null, null, 1, 10);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
       x => x.Log(
           LogLevel.Information,
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Listando animes")),
           It.IsAny<Exception>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()),
           Times.AtLeastOnce);

        }

        [Fact]
        public async Task Listar_DeveRetornar400_QuandoPaginicaoInvalida()
        {
            // Arrange
            var animes = new List<Anime>
            {
                new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um ninja adolescente..." },
                new Anime { Id = 2, Nome = "One Piece", Diretor = "Eiichiro Oda", Resumo = "Aventura em alto mar..." }
            };
            _mockRepository.Setup(r => r.ListarAsync(null, null, null, 0, 10))
                          .ReturnsAsync(animes);
            // Act
            var result = await _controller.Listar(null, null, null, 0, 10);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(x =>
            x.Log(LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("paginação inválidos")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Listar_DeveRetornar500_QuandoOcorrerExcecao()
        {
            // Arrange
            _mockRepository.Setup(r => r.ListarAsync(null, null, null, 1, 10))
                          .ThrowsAsync(new Exception());
            // Act
            var result = await _controller.Listar(null, null, null, 1, 10);

            // Assert 
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno", objectResult.Value);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Falha ao listar animes")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        #endregion

        #region Testes Atualizar
        [Fact]
        public async Task Atualizar_DeveRetornar204_QuandoAnimeExistir()
        {
            // Arrange
            var anime = new Anime
            {
                Id = 1,
                Nome = "Naruto",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };
            var dto = new UpdateAnimeDto
            {
                Nome = "Naruto Shippuden",
                Diretor = "Hayato Date",
                Resumo = "Continuação da história..."
            };
            _mockRepository.Setup(r => r.ObterPorIdAsync(anime.Id))
                          .ReturnsAsync(anime);

            // Act
            var result = await _controller.Atualizar(anime.Id, dto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("atualizado com sucesso")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar404_QuandoAnimeNaoExistir()
        {
            // Arrange
            var dto = new UpdateAnimeDto
            {
                Nome = "Naruto Shippuden",
                Diretor = "Hayato Date",
                Resumo = "Continuação da história..."
            };
            _mockRepository.Setup(r => r.ObterPorIdAsync(1))
                          .ThrowsAsync(new NotFoundException($"Anime ID {1} não encontrado"));
            // Act
            var result = await _controller.Atualizar(1, dto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("não encontrado")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar400_QuandoDtoInvalido()
        {
            // Arrange
            var dto = new UpdateAnimeDto
            {
                Nome = "",
                Diretor = "Hayato Date",
                Resumo = "Continuação da história..."
            };
            _controller.ModelState.AddModelError("Nome", "O nome é obrigatório");

            // Act
            var result = await _controller.Atualizar(1, dto);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("DTO inválido")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar500_QuandoOcorrerExcecao()
        {
            // Arrange
            var dto = new UpdateAnimeDto
            {
                Nome = "Naruto Shippuden",
                Diretor = "Hayato Date",
                Resumo = "Continuação da história..."
            };
            _mockRepository.Setup(r => r.ObterPorIdAsync(1))
                          .ThrowsAsync(new Exception());
            // Act
            var result = await _controller.Atualizar(1, dto);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno", objectResult.Value);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao atualizar anime ID {1}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        #endregion

        #region Testes ObterPorId
        [Fact]
        public async Task ObterPorId_DeveRetornar200_QuandoAnimeEncontrado()
        {
            //Arrange
            var anime = new Anime
            {
                Id = 1,
                Nome = "Naruto",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };
            _mockRepository.Setup(r => r.ObterPorIdAsync(anime.Id))
                          .ReturnsAsync(anime);

            //Act
            var result = await _controller.ObterPorId(anime.Id);

            //Assert

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedAnime = Assert.IsType<Anime>(okResult.Value);

            Assert.Equal(anime.Id, returnedAnime.Id);
            Assert.Equal(anime.Nome, returnedAnime.Nome);
            Assert.Equal(anime.Diretor, returnedAnime.Diretor);
            Assert.Equal(anime.Resumo, returnedAnime.Resumo);



            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Anime encontrado: {anime.Nome}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornar404_QuandoAnimeNaoEncontrado()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.ObterPorIdAsync(id))
                          .ThrowsAsync(new NotFoundException($"Anime ID {id} não encontrado"));
            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Anime ID {id} não encontrado")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornar500_QuandoOcorrerExcecao()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.ObterPorIdAsync(id))
                          .ThrowsAsync(new Exception());
            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno", objectResult.Value);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao buscar anime ID {id}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        #endregion

        #region Testes RemoverLogicamente
        [Fact]
        public async Task RemoverLogicamente_DeveRetornar204_QuandoAnimeExistir()
        {
            // Arrange
            var id = 1;
            var anime = new Anime
            {
                Id = id,
                Nome = "Naruto",
                Diretor = "Hayato Date",
                Resumo = "Um ninja adolescente..."
            };
            _mockRepository.Setup(r => r.AdicionarAsync(anime))
                          .ReturnsAsync(anime);
            // Act
            var result = await _controller.RemoverLogicamente(anime.Id);
            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Anime ID {anime.Id} desativado com sucesso")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task RemoverLogicamente_DeveRetornar404_QuandoAnimeNaoExistir()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.RemoverLogicamenteAsync(id))
                          .ThrowsAsync(new NotFoundException($"Anime ID {id} não encontrado"));
            // Act
            var result = await _controller.RemoverLogicamente(id);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Anime ID {id} não encontrado")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task RemoverLogicamente_DeveRetornar500_QuandoOcorrerExcecao()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.RemoverLogicamenteAsync(id))
                          .ThrowsAsync(new Exception());
            // Act
            var result = await _controller.RemoverLogicamente(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno", objectResult.Value);

            // Assert do Logger
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao desativar anime ID {id}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        #endregion
    }
}
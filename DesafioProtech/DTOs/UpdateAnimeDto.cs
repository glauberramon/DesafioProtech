using System.ComponentModel.DataAnnotations;

namespace DesafioProtech.API.DTOs
{
    /// <summary>
    /// DTO para atualização de informações de um anime.
    /// Todas as propriedades são opcionais, permitindo atualizações parciais.
    /// </summary>
    public class UpdateAnimeDto
    {
        /// <summary>
        /// Nome do anime (mínimo 3 caracteres)
        /// </summary>
        /// <example>One Piece</example>
        [StringLength(100, MinimumLength = 3)]
        public string? Nome { get; set; }

        /// <summary>
        /// Diretor do anime (máximo 50 caracteres)
        /// </summary>
        /// <example>Eichi Oda</example>
        [StringLength(50)]
        public string? Diretor { get; set; }

        /// <summary>
        /// Resumo do anime (máximo 500 caracteres)
        /// </summary>
        /// <example>Anime de aventura e piratas</example>
        [StringLength(500)]
        public string? Resumo { get; set; }
    }
}
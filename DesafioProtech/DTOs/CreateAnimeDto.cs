using System.ComponentModel.DataAnnotations;

namespace DesafioProtech.API.DTOs
{
    // DesafioProtech.API/DTOs/CreateAnimeDto.cs

    /// <summary>
    /// DTO para criação de um novo anime
    /// </summary>
    public class CreateAnimeDto
    {
        /// <summary>
        /// Nome do anime (mínimo 3 caracteres)
        /// </summary>
        /// <example>Attack on Titan</example>
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter 3 a 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Diretor do anime (máximo 50 caracteres)
        /// </summary>
        /// <example>Tetsurö Araki</example>
        [Required]
        [StringLength(50)]
        public string Diretor { get; set; }

        /// <summary>
        /// Resumo do anime (máximo 500 caracteres)
        /// </summary>
        /// <example>Anime de lutas e gigantes</example>
        [Required]
        [StringLength(500)]
        public string Resumo { get; set; }
    }
}

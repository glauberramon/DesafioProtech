using System.ComponentModel.DataAnnotations;

namespace DesafioProtech.API.DTOs
{
    public class UpdateAnimeDto
    {
        [StringLength(100, MinimumLength = 3)]
        public string? Nome { get; set; }

        [StringLength(50)]
        public string? Diretor { get; set; }

        [StringLength(500)]
        public string? Resumo { get; set; }
    }
}

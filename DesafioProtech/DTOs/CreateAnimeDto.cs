using System.ComponentModel.DataAnnotations;

namespace DesafioProtech.API.DTOs
{
    public class CreateAnimeDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100,MinimumLength =3, ErrorMessage ="Nome deve ter 3 a 100 caracteres")]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Diretor { get; set; }

        [Required]
        [StringLength(500)]
        public string Resumo { get; set; }
    }
}

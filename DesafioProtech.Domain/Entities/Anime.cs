using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioProtech.Domain.Entities
{
    /// <summary>
    /// Classe que representa a entidade Anime no domínio da aplicação.
    /// </summary>
    /// <remarks>
    /// Esta classe mapeia a tabela de Animes no banco de dados e contém as propriedades
    /// que definem as características de um anime no sistema.
    /// </remarks>
    [Table("Animes")]
    public class Anime
    {
        /// <summary>
        /// Identificador único do anime (chave primária).
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do anime. Campo obrigatório com tamanho entre 3 e 100 caracteres.
        /// </summary>
        /// <example>Attack on Titan</example>
        [Required]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Resumo ou sinopse do anime. Tamanho máximo de 500 caracteres.
        /// </summary>
        /// <example>Em um mundo onde a humanidade vive cercada por muralhas...</example>
        [StringLength(500)]
        public string Resumo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do diretor responsável pelo anime. Tamanho máximo de 50 caracteres.
        /// </summary>
        /// <example>Tetsurō Araki</example>
        [StringLength(50)]
        public string Diretor { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o anime está ativo no sistema (disponível para visualização).
        /// Valor padrão é true ao criar um novo registro.
        /// </summary>
        /// <example>true</example>
        public bool Ativo {  get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioProtech.Domain.Entities
{
    public class Anime
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Resumo { get; set; } = string.Empty;
        public string Diretor { get; set; } = string.Empty;
        public bool Ativo {  get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambarina.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public decimal MargemLucro { get; set; } // O % que você digita na tela
        public int EstoqueMinimo { get; set; }
        public int EstoqueAtual { get; set; }  // Alimentado pela Produção

        // Propriedade calculada (para uso interno no C#)
        public decimal PrecoSugerido { get; set; }
    }
}

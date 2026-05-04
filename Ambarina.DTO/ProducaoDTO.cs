using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambarina.DTO
{
    public class ProducaoDTO
    {
        public int Id { get; set; }
        public int IdProduto { get; set; } // A vela finalizada
        public int IdInsumo { get; set; }  // O que foi usado
        public decimal QuantidadeUtilizada { get; set; }
        public DateTime DataProducao { get; set; }
        public string Lote { get; set; }
    }
}

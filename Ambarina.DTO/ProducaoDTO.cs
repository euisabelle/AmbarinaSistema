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
        public int IdProduto { get; set; }     // A vela/home spray finalizado
        public DateTime DataProducao { get; set; }
        public int QtdeProduzida { get; set; }  // Quantidade inteira produzida
        public string Lote { get; set; }       // Código sequencial ex: "001"
        public string Status { get; set; }     // "EM CURA", "PRONTA" ou "EMBALADA"
    }
}

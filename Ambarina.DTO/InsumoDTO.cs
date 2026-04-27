using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambarina.DTO
{
    public class InsumoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal QtdeInicial { get; set; }
        public decimal CustoInicial { get; set; }
        public decimal EstoqueMinimo { get; set; }
    }
}
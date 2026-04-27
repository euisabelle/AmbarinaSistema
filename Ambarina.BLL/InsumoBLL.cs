using Ambarina.DAL;
using Ambarina.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambarina.BLL
{
    public class InsumoBLL
    {
        InsumoDAL insumoDAL = new InsumoDAL();

        public void SalvarInsumo(InsumoDTO insumo)
        {
            if (string.IsNullOrEmpty(insumo.Nome))
            {
                throw new Exception("O nome do insumo é obrigatório!");
            }
            //podemos adicionar outras validações aqui
            insumoDAL.Salvar(insumo);
        }
        public DataTable ListarInsumos()
        {
            return insumoDAL.ListarInsumos();
        }
        public void ExcluirInsumo(int id) { insumoDAL.ExcluirInsumo(id); }
        public void EditarInsumo(InsumoDTO insumo) { insumoDAL.EditarInsumo(insumo); }
    }
}

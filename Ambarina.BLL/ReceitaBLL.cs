using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ambarina.DAL;
using Ambarina.DTO;

namespace Ambarina.BLL
{
    public class ReceitaBLL
    {
        ReceitaDAL receitaDAL = new ReceitaDAL();

        // Método para carregar a nova Grid 
        public DataTable ListarReceitas()
        {
            return receitaDAL.ListarReceitas();
        }

        public DataTable ListarAromas()
        {
            return receitaDAL.ListarAromas();
        }

        // Método para excluir
        public void ExcluirReceita(int idReceita)
        {
            if (idReceita <= 0)
            {
                throw new Exception("Selecione uma receita válida para excluir.");
            }
            receitaDAL.Excluir(idReceita);
        }

        public DataTable ListarItensDaReceita(int idReceita)
        {
            return receitaDAL.ListarItensDaReceita(idReceita);
        }

        public void SalvarReceitaCompleta(ReceitaDTO receita, List<ItensReceitaDTO> itens)
        {
            if (receita.IdProduto <= 0)
                throw new Exception("Selecione um produto base válido.");

            if (itens == null || itens.Count == 0)
                throw new Exception("A receita precisa de pelo menos um insumo.");

            new ReceitaDAL().SalvarNovaReceita(receita, itens);
        }

        public void EditarReceitaCompleta(ReceitaDTO receita, List<ItensReceitaDTO> itens)
        {
            if (receita.Id <= 0)
                throw new Exception("ID da receita inválido.");

            if (receita.IdProduto <= 0)
                throw new Exception("Selecione um produto base válido.");

            if (itens == null || itens.Count == 0)
                throw new Exception("A receita precisa de pelo menos um insumo.");

            new ReceitaDAL().EditarReceita(receita, itens);
        }
    }
}

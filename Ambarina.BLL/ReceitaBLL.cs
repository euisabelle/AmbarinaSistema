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

        // Aqui também entrarão os métodos de Salvar e Editar Receita
    }
}

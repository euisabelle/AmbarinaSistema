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
    public class ProdutoBLL
    {
        // Instanciamos a DAL para poder usar os métodos dela
        ProdutoDAL produtoDAL = new ProdutoDAL();

        public DataTable ListarProdutosCombo()
        {
            // Apenas repassa a solicitação para a DAL
            return produtoDAL.ListarProdutosCombo();
        }

        public void AdicionarEstoqueProduto(int idProduto, int quantidade)
        {
            // Validação simples: não faz sentido produzir 0 ou negativo
            if (quantidade <= 0)
            {
                throw new Exception("A quantidade produzida deve ser maior que zero.");
            }

            produtoDAL.AumentarEstoque(idProduto, quantidade);
        }

        public void SalvarProduto(ProdutoDTO produto)
        {
            // Validação simples antes de enviar para o banco
            if (string.IsNullOrEmpty(produto.Nome))
            {
                throw new Exception("O nome do produto é obrigatório!");
            }

            if (produto.MargemLucro < 0)
            {
                throw new Exception("A margem de lucro não pode ser negativa.");
            }

            produtoDAL.Salvar(produto);
        }
    }
}

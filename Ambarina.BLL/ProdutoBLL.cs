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

        public List<ProdutoDTO> ListarProdutos()
        {
            // Deve retornar uma lista com todos os produtos cadastrados
            return produtoDAL.ListarProdutos();
        }

        public void EditarProduto(ProdutoDTO produto)
        {
            // Deve atualizar o produto no banco
            produtoDAL.EditarProduto(produto);
        }

        public void ExcluirProduto(int id)
        {
            // Deve deletar o produto do banco
            produtoDAL.ExcluirProduto(id);
        }
        public DataTable FiltrarEstoqueProntaEntrega(string buscaNome, string filtroCategoria, string filtroStatus)
        {
            // Passa os parâmetros limpos para a DAL executar
            return produtoDAL.ListarEstoqueComFiltros(buscaNome?.Trim(), filtroCategoria, filtroStatus);
        }

        public void AjustarQuantidadeFisica(int idProduto, int idReceita, int novaQuantidade)
        {
            if (idProduto <= 0) throw new Exception("Produto inválido.");
            if (idReceita <= 0) throw new Exception("Receita inválida.");
            if (novaQuantidade < 0) throw new Exception("O estoque não pode ser negativo.");

            produtoDAL.AtualizarQuantidadeEstoque(idProduto, idReceita, novaQuantidade);
        }
    }
}

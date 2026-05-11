using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambarina.DTO;



namespace Ambarina.DAL
{
    public class ProdutoDAL
    {
        Conexao conexao = new Conexao();

        public DataTable ListarProdutosCombo()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();
                string sql = "SELECT id_produto, nome FROM produtos ORDER BY nome ASC";
                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public void AumentarEstoque(int idProduto, int quantidade)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE produtos SET estoque_atual = estoque_atual + @qtd WHERE id_produto = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@qtd", quantidade);
                cmd.Parameters.AddWithValue("@id", idProduto);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public void Salvar(ProdutoDTO produto)
        {
            try
            {
                conexao.AbrirConexao();
                // O SQL foca nos dados base que definem o modelo
                string sql = "INSERT INTO produtos (nome, categoria, margem_lucro, estoque_minimo, estoque_atual) " +
                             "VALUES (@nome, @cat, @margem, @min, 0)";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@cat", produto.Categoria);
                cmd.Parameters.AddWithValue("@margem", produto.MargemLucro);
                cmd.Parameters.AddWithValue("@min", produto.EstoqueMinimo);
                // O estoque_atual começa em 0 porque nada foi produzido ainda

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar modelo no banco: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        public List<ProdutoDTO> ListarProdutos()
        {
            try
            {
                conexao.AbrirConexao();
                List<ProdutoDTO> listaProdutos = new List<ProdutoDTO>();
                string sql = "SELECT id_produto, nome, categoria, margem_lucro, estoque_minimo FROM produtos ORDER BY nome ASC";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProdutoDTO produto = new ProdutoDTO();
                    produto.Id = Convert.ToInt32(reader["id_produto"]);
                    produto.Nome = reader["nome"].ToString();
                    produto.Categoria = reader["categoria"].ToString();
                    produto.MargemLucro = Convert.ToDecimal(reader["margem_lucro"]);
                    produto.EstoqueMinimo = Convert.ToInt32(reader["estoque_minimo"]);

                    listaProdutos.Add(produto);
                }

                reader.Close();
                return listaProdutos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar produtos: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        public void EditarProduto(ProdutoDTO produto)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE produtos SET nome = @nome, categoria = @cat, margem_lucro = @margem, estoque_minimo = @min WHERE id_produto = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@id", produto.Id);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@cat", produto.Categoria);
                cmd.Parameters.AddWithValue("@margem", produto.MargemLucro);
                cmd.Parameters.AddWithValue("@min", produto.EstoqueMinimo);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar produto: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        public void ExcluirProduto(int id)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "DELETE FROM produtos WHERE id_produto = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir produto: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }
    }
}

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
        // Método para listar com filtros dinâmicos e calcular preço de custo + venda em tempo de execução
        public DataTable ListarEstoqueComFiltros(string buscaNome, string filtroCategoria, string filtroStatus)
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();

                // Query Sênior: Consolida os dados a partir do estoque físico real por aroma
                string sql = @"
                    SELECT 
                        epe.produtos_id_produto AS id_produto,
                        epe.receitas_id_receita AS id_receita,
                        p.nome AS nome_base,
                        CONCAT(p.nome, ' (', r.aroma_padrao, ')') AS nome_produto,
                        p.categoria,
                        p.estoque_minimo,
                        p.margem_lucro,
                        epe.quantidade_atual AS estoque_atual,
                        IFNULL(custo_calculado.total_custo, 0) AS custo_fabricacao,
                        (IFNULL(custo_calculado.total_custo, 0) * (1 + (p.margem_lucro / 100))) AS preco_venda_sugerido
                    FROM estoque_pronta_entrega epe
                    INNER JOIN produtos p ON epe.produtos_id_produto = p.id_produto
                    INNER JOIN receitas r ON epe.receitas_id_receita = r.id_receita
                    LEFT JOIN (
                        SELECT 
                            ri.receitas_id_receita,
                            SUM(ri.quantidade * (i.custo_unitario / i.quantidade_inicial)) AS total_custo
                        FROM receita_insumos ri
                        INNER JOIN insumos i ON ri.insumos_id_insumo = i.id_insumo
                        GROUP BY ri.receitas_id_receita
                    ) custo_calculado ON r.id_receita = custo_calculado.receitas_id_receita
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(buscaNome))
                {
                    sql += " AND (p.nome LIKE @busca OR r.aroma_padrao LIKE @busca)";
                }
                if (!string.IsNullOrEmpty(filtroCategoria) && filtroCategoria != "TODAS" && filtroCategoria != "TODOS" && filtroCategoria != "")
                {
                    sql += " AND p.categoria = @categoria";
                }
                if (filtroStatus == "EM ESTOQUE")
                {
                    sql += " AND epe.quantidade_atual > 0";
                }
                else if (filtroStatus == "ESGOTADOS")
                {
                    sql += " AND epe.quantidade_atual = 0";
                }
                else if (filtroStatus == "ESTOQUE BAIXO")
                {
                    sql += " AND epe.quantidade_atual <= p.estoque_minimo AND epe.quantidade_atual > 0";
                }

                sql += " ORDER BY p.nome ASC, r.aroma_padrao ASC";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);

                if (!string.IsNullOrEmpty(buscaNome))
                    cmd.Parameters.AddWithValue("@busca", "%" + buscaNome + "%");
                if (!string.IsNullOrEmpty(filtroCategoria) && filtroCategoria != "TODAS" && filtroCategoria != "TODOS" && filtroCategoria != "")
                    cmd.Parameters.AddWithValue("@categoria", filtroCategoria);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao filtrar estoque da pronta entrega: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // Método rápido para atualização direta da quantidade na Grid
        public void AtualizarQuantidadeEstoque(int idProduto, int idReceita, int novaQuantidade)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = @"UPDATE estoque_pronta_entrega 
                       SET quantidade_atual = @qtd 
                       WHERE produtos_id_produto = @idProd AND receitas_id_receita = @idRec";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@qtd", novaQuantidade);
                cmd.Parameters.AddWithValue("@idProd", idProduto);
                cmd.Parameters.AddWithValue("@idRec", idReceita);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ajustar estoque na pronta entrega: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }
    }
}

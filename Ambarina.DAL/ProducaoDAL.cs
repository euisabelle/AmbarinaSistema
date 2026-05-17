using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ambarina.DTO;
using MySql.Data.MySqlClient;


namespace Ambarina.DAL
{
    public class ProducaoDAL
    {
        Conexao conexao = new Conexao();

        // 1. Busca o último lote VARCHAR e calcula o próximo sequencial
        public string ObterProximoLote()
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT MAX(CAST(lote AS UNSIGNED)) FROM producao";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);

                object resultado = cmd.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return "001";
                }

                int ultimoNumero = Convert.ToInt32(resultado);
                int proximoNumero = ultimoNumero + 1;

                return proximoNumero.ToString("D3");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular o próximo lote: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 2. Registra a produção e BAIXA OS INSUMOS (Ajustado para 'estoque_atual' na tabela insumos)
        public void RegistrarProducaoCompleta(ProducaoDTO producao, List<ItensReceitaDTO> insumosParaBaixar)
        {
            MySqlTransaction transacao = null;

            try
            {
                conexao.AbrirConexao();
                transacao = conexao.conectar.BeginTransaction();

                // PASSO 1: Inserir na tabela 'producao' (Coluna real de quantidade se chama 'quantidade')
                string sqlProducao = @"INSERT INTO producao (id_produto, data_producao, quantidade, lote, status) 
                                       VALUES (@idProd, @data, @qtd, @lote, @status)";

                MySqlCommand cmdProd = new MySqlCommand(sqlProducao, conexao.conectar, transacao);
                cmdProd.Parameters.AddWithValue("@idProd", producao.IdProduto);
                cmdProd.Parameters.AddWithValue("@data", producao.DataProducao);
                cmdProd.Parameters.AddWithValue("@qtd", producao.QtdeProduzida);
                cmdProd.Parameters.AddWithValue("@lote", producao.Lote);
                cmdProd.Parameters.AddWithValue("@status", producao.Status);
                cmdProd.ExecuteNonQuery();

                // PASSO 2: Dar baixa na tabela 'insumos' -> Coluna corrigida para 'estoque_atual'
                foreach (var insumo in insumosParaBaixar)
                {
                    string sqlBaixaInsumo = @"UPDATE insumos 
                                              SET estoque_atual = estoque_atual - @qtdConsumida 
                                              WHERE TRIM(nome) = TRIM(@nomeInsumo)";

                    MySqlCommand cmdInsumo = new MySqlCommand(sqlBaixaInsumo, conexao.conectar, transacao);
                    cmdInsumo.Parameters.AddWithValue("@qtdConsumida", insumo.Quantidade);
                    cmdInsumo.Parameters.AddWithValue("@nomeInsumo", insumo.NomeInsumo);

                    int linhasAfetadas = cmdInsumo.ExecuteNonQuery();
                    if (linhasAfetadas == 0)
                    {
                        throw new Exception($"Insumo '{insumo.NomeInsumo}' não encontrado para baixa de estoque.");
                    }
                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new Exception("Erro ao registrar produção e baixar insumos: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 3. Query com INNER JOIN para listar na dgvProducao (Mapeia 'quantidade' da tabela como 'qtde_produzida' para o C#)
        public DataTable ListarProducoes()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();

                string sql = @"SELECT pr.id_producao, pr.data_producao, p.nome AS nome_produto, 
                                      r.aroma_padrao, pr.lote, pr.status, pr.id_produto, pr.quantidade AS qtde_produzida
                               FROM producao pr
                               INNER JOIN produtos p ON pr.id_produto = p.id_produto
                               LEFT JOIN receitas r ON p.id_produto = r.id_produto
                               ORDER BY pr.id_producao DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw new Exception("Erro ao listar produções: " + ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        // 4. Atualiza o Status e altera o 'estoque_atual' da tabela 'produtos' quando virar EMBALADA
        public void AtualizarStatusProducao(int idProducao, string novoStatus, int idProduto, int qtdProduzida)
        {
            MySqlTransaction transacao = null;
            try
            {
                conexao.AbrirConexao();
                transacao = conexao.conectar.BeginTransaction();

                // 1. Atualiza o status na tabela 'producao'
                string sqlStatus = "UPDATE producao SET status = @status WHERE id_producao = @id";
                MySqlCommand cmdStatus = new MySqlCommand(sqlStatus, conexao.conectar, transacao);
                cmdStatus.Parameters.AddWithValue("@status", novoStatus);
                cmdStatus.Parameters.AddWithValue("@id", idProducao);
                cmdStatus.ExecuteNonQuery();

                // 2. Se mudou para EMBALADA, adiciona as unidades na tabela 'produtos' (coluna real: estoque_atual)
                if (novoStatus.Trim().ToUpper() == "EMBALADA")
                {
                    string sqlEstoque = "UPDATE produtos SET estoque_atual = estoque_atual + @qtd WHERE id_produto = @idProd";
                    MySqlCommand cmdEstoque = new MySqlCommand(sqlEstoque, conexao.conectar, transacao);
                    cmdEstoque.Parameters.AddWithValue("@qtd", qtdProduzida);
                    cmdEstoque.Parameters.AddWithValue("@idProd", idProduto);
                    cmdEstoque.ExecuteNonQuery();
                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new Exception("Erro ao atualizar status do lote: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 5. Excluir e estornar insumos (Somando de volta na coluna 'estoque_atual' da tabela 'insumos')
        public void ExcluirEEstornarProducao(int idProducao, int idProduto, int qtdProduzida, List<ItensReceitaDTO> insumosParaDevolver)
        {
            MySqlTransaction transacao = null;
            try
            {
                conexao.AbrirConexao();
                transacao = conexao.conectar.BeginTransaction();

                foreach (var insumo in insumosParaDevolver)
                {
                    string sqlEstornoInsumo = @"UPDATE insumos 
                                                SET estoque_atual = estoque_atual + @qtdDevolvida 
                                                WHERE TRIM(nome) = TRIM(@nomeInsumo)";

                    MySqlCommand cmdInsumo = new MySqlCommand(sqlEstornoInsumo, conexao.conectar, transacao);
                    cmdInsumo.Parameters.AddWithValue("@qtdDevolvida", insumo.Quantidade);
                    cmdInsumo.Parameters.AddWithValue("@nomeInsumo", insumo.NomeInsumo);
                    cmdInsumo.ExecuteNonQuery();
                }

                string sqlDelete = "DELETE FROM producao WHERE id_producao = @id";
                MySqlCommand cmdDelete = new MySqlCommand(sqlDelete, conexao.conectar, transacao);
                cmdDelete.Parameters.AddWithValue("@id", idProducao);
                cmdDelete.ExecuteNonQuery();

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new Exception("Erro crítico ao estornar insumos e deletar produção: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }
    }
}

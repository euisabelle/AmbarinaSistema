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

        // Busca o último lote VARCHAR e calcula o próximo sequencial
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

        // Registra a produção e BAIXA OS INSUMOS (Ajustado para 'estoque_atual' na tabela insumos)
        public void RegistrarProducaoCompleta(ProducaoDTO producao, int idReceita)
        {
            MySqlTransaction transacao = null;
            try
            {
                conexao.AbrirConexao();
                transacao = conexao.conectar.BeginTransaction();

                // 1. Salva a produção com a referência direta à receita (aroma) correspondente
                string sqlProducao = @"INSERT INTO producao (id_produto, id_receita, data_producao, quantidade, lote, status) 
                               VALUES (@idProd, @idRec, @data, @qtd, @lote, @status)";

                MySqlCommand cmdProd = new MySqlCommand(sqlProducao, conexao.conectar, transacao);
                cmdProd.Parameters.AddWithValue("@idProd", producao.IdProduto);
                cmdProd.Parameters.AddWithValue("@idRec", idReceita); // <-- Nova vinculação de parâmetro
                cmdProd.Parameters.AddWithValue("@data", producao.DataProducao);
                cmdProd.Parameters.AddWithValue("@qtd", producao.QtdeProduzida);
                cmdProd.Parameters.AddWithValue("@lote", producao.Lote);
                cmdProd.Parameters.AddWithValue("@status", "EM CURA");
                cmdProd.ExecuteNonQuery();

                // 2. Busca e consome os insumos da receita específica
                string sqlItens = @"SELECT insumos_id_insumo, quantidade FROM receita_insumos WHERE receitas_id_receita = @idRec";
                MySqlCommand cmdItens = new MySqlCommand(sqlItens, conexao.conectar, transacao);
                cmdItens.Parameters.AddWithValue("@idRec", idReceita);

                DataTable dtItens = new DataTable();
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmdItens))
                {
                    da.Fill(dtItens);
                }

                foreach (DataRow row in dtItens.Rows)
                {
                    int idInsumo = Convert.ToInt32(row["insumos_id_insumo"]);
                    decimal qtdUnitaria = Convert.ToDecimal(row["quantidade"]);
                    decimal qtdTotalConsumida = qtdUnitaria * producao.QtdeProduzida;

                    string sqlBaixaInsumo = @"UPDATE insumos SET estoque_atual = estoque_atual - @qtdConsumida WHERE id_insumo = @idInsumo";
                    MySqlCommand cmdBaixa = new MySqlCommand(sqlBaixaInsumo, conexao.conectar, transacao);
                    cmdBaixa.Parameters.AddWithValue("@qtdConsumida", qtdTotalConsumida);
                    cmdBaixa.Parameters.AddWithValue("@idInsumo", idInsumo);
                    cmdBaixa.ExecuteNonQuery();
                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new Exception("Erro ao registrar produção: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // Alimenta a Pronta Entrega apenas quando muda para EMBALADA
        public DataTable ListarProducoes()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();

                // O JOIN agora amarra a produção diretamente à receita exata do lote
                string sql = @"SELECT pr.id_producao, pr.data_producao, p.nome AS nome_produto, 
                              r.aroma_padrao, pr.lote, pr.status, pr.id_produto, pr.quantidade AS qtde_produzida
                       FROM producao pr
                       INNER JOIN produtos p ON pr.id_produto = p.id_produto
                       INNER JOIN receitas r ON pr.id_receita = r.id_receita
                       ORDER BY pr.id_producao DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar produções: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // Atualiza o Status e altera o 'estoque_atual' da tabela 'produtos' quando virar EMBALADA
        public void AtualizarStatusProducao(int idProducao, string novoStatus, int idProduto, int quantidade, int idReceita)
        {
            MySqlTransaction transacao = null;
            try
            {
                conexao.AbrirConexao();
                transacao = conexao.conectar.BeginTransaction();

                // 1. Atualiza o status na tabela producao
                string sqlUpdateStatus = "UPDATE producao SET status = @status WHERE id_producao = @id";
                MySqlCommand cmdStatus = new MySqlCommand(sqlUpdateStatus, conexao.conectar, transacao);
                cmdStatus.Parameters.AddWithValue("@status", novoStatus);
                cmdStatus.Parameters.AddWithValue("@id", idProducao);
                cmdStatus.ExecuteNonQuery();

                // 2. Se mudou para EMBALADA, alimenta a nova tabela estoque_pronta_entrega
                if (novoStatus.Trim().ToUpper() == "EMBALADA")
                {
                    string sqlVerifica = "SELECT COUNT(*) FROM estoque_pronta_entrega WHERE produtos_id_produto = @idProd AND receitas_id_receita = @idRec";
                    MySqlCommand cmdVerifica = new MySqlCommand(sqlVerifica, conexao.conectar, transacao);
                    cmdVerifica.Parameters.AddWithValue("@idProd", idProduto);
                    cmdVerifica.Parameters.AddWithValue("@idRec", idReceita);
                    int existe = Convert.ToInt32(cmdVerifica.ExecuteScalar());

                    if (existe > 0)
                    {
                        string sqlUpdateEstoque = @"UPDATE estoque_pronta_entrega 
                                            SET quantidade_atual = quantidade_atual + @qtd 
                                            WHERE produtos_id_produto = @idProd AND receitas_id_receita = @idRec";
                        MySqlCommand cmdUp = new MySqlCommand(sqlUpdateEstoque, conexao.conectar, transacao);
                        cmdUp.Parameters.AddWithValue("@qtd", quantidade);
                        cmdUp.Parameters.AddWithValue("@idProd", idProduto);
                        cmdUp.Parameters.AddWithValue("@idRec", idReceita);
                        cmdUp.ExecuteNonQuery();
                    }
                    else
                    {
                        string sqlInsertEstoque = @"INSERT INTO estoque_pronta_entrega (produtos_id_produto, receitas_id_receita, quantidade_atual) 
                                            VALUES (@idProd, @idRec, @qtd)";
                        MySqlCommand cmdIns = new MySqlCommand(sqlInsertEstoque, conexao.conectar, transacao);
                        cmdIns.Parameters.AddWithValue("@idProd", idProduto);
                        cmdIns.Parameters.AddWithValue("@idRec", idReceita);
                        cmdIns.Parameters.AddWithValue("@qtd", quantidade);
                        cmdIns.ExecuteNonQuery();
                    }
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

        // Excluir e estornar insumos (Somando de volta na coluna 'estoque_atual' da tabela 'insumos')
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

using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Ambarina.DAL
{
    public class DashboardDAL
    {
        Conexao conexao = new Conexao();

        // 1. Quantidade de Insumos abaixo do estoque mínimo (Almoxarifado)
        public int ObterInsumosAbaixoDoMinimo()
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT COUNT(*) FROM insumos WHERE estoque_atual < estoque_minimo";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao contabilizar insumos críticos: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 2. Quantidade total de itens no Estoque de Pronta Entrega
        public int ObterTotalProntaEntrega()
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT SUM(quantidade_atual) FROM estoque_pronta_entrega";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);

                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value) return 0;

                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao somar estoque de pronta entrega: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 3. NOVO: Quantidade total de produtos atualmente em produção ("EM CURA" ou "PRONTA")
        public int ObterTotalEmProducaoAtiva()
        {
            try
            {
                conexao.AbrirConexao();
                string sql = @"SELECT SUM(p.quantidade) 
                       FROM producao p
                       INNER JOIN receitas r ON p.id_receita = r.id_receita
                       WHERE p.status IN ('EM CURA', 'PRONTA')";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                object result = cmd.ExecuteScalar();

                // Verificação de segurança sénior contra Nulls provenientes do SUM()
                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao somar produtos em produção ativa: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }

        // 4. Grid de Produções Ativas (Apenas EM CURA e PRONTA)
        public DataTable ObterProducoesAtivasGrid()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();

                // Query limpa e perfeitamente sintonizada com o seu modelo de dados
                string sql = @"SELECT p.id_producao, p.id_produto, p.id_receita, p.lote as 'Lote', 
                   CONCAT(prod.nome, ' (', r.aroma_padrao, ')') as 'Produto', 
                   p.status as 'Status', p.quantidade as 'Qtd', p.data_producao as 'DataFabrica'
                   FROM producao p
                   INNER JOIN produtos prod ON p.id_produto = prod.id_produto
                   INNER JOIN receitas r ON p.id_receita = r.id_receita
                   WHERE p.status IN ('EM CURA', 'PRONTA')
                   ORDER BY p.data_producao ASC, p.lote ASC";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar produções ativas para o dashboard: " + ex.Message);
            }
            finally
            {
                conexao.FecharConexao();
            }
        }
    }
}
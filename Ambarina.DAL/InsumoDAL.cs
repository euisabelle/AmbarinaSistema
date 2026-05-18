using Ambarina.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambarina.DAL
{
    public class InsumoDAL
    {
        Conexao conexao = new Conexao();

        public void Salvar(InsumoDTO insumo)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "INSERT INTO insumos (nome, categoria, unidade_medida, estoque_atual, quantidade_inicial, custo_unitario, estoque_minimo) " +
                             "VALUES (@nome, @cat, @un, @estAtual, @qtdInicial, @custo, @min)";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@nome", insumo.Nome);
                cmd.Parameters.AddWithValue("@cat", insumo.Categoria);
                cmd.Parameters.AddWithValue("@un", insumo.UnidadeMedida);

                // Vincula os valores certos coletados da tela
                cmd.Parameters.AddWithValue("@estAtual", insumo.EstoqueAtual);
                cmd.Parameters.AddWithValue("@qtdInicial", insumo.QtdeInicial);
                cmd.Parameters.AddWithValue("@custo", insumo.CustoInicial);
                cmd.Parameters.AddWithValue("@min", insumo.EstoqueMinimo);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar insumo: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public DataTable ListarInsumos()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();

                // Trazemos o custo_unitario bruto para o C# usar no Editar,
                // e calculamos o 'Custo Unit' (por grama) e o 'Total' proporcional para a Grid.
                string sql = @"SELECT 
                        id_insumo as 'ID', 
                        nome as 'Nome', 
                        categoria as 'Categoria', 
                        unidade_medida as 'Unidade', 
                        quantidade_inicial as 'Qtd Embalagem', 
                        estoque_atual as 'Estoque Atual', 
                        custo_unitario as 'Custo Total',
                        ROUND((custo_unitario / quantidade_inicial), 4) as 'Custo Unit', 
                        estoque_minimo as 'Mínimo', 
                        ROUND(estoque_atual * (custo_unitario / quantidade_inicial), 2) as 'Total' 
                       FROM insumos";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar insumos: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public void ExcluirInsumo(int id)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "DELETE FROM insumos WHERE id_insumo = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir insumo: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public void EditarInsumo(InsumoDTO insumo)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE insumos SET nome=@nome, categoria=@cat, unidade_medida=@un, " +
                             "estoque_atual=@est, quantidade_inicial=@qtdIni, custo_unitario=@custo, estoque_minimo=@min WHERE id_insumo=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@nome", insumo.Nome);
                cmd.Parameters.AddWithValue("@cat", insumo.Categoria);
                cmd.Parameters.AddWithValue("@un", insumo.UnidadeMedida);

                cmd.Parameters.AddWithValue("@est", insumo.EstoqueAtual);
                cmd.Parameters.AddWithValue("@qtdIni", insumo.QtdeInicial);
                cmd.Parameters.AddWithValue("@custo", insumo.CustoInicial);
                cmd.Parameters.AddWithValue("@min", insumo.EstoqueMinimo);
                cmd.Parameters.AddWithValue("@id", insumo.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar insumo: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public void BaixarEstoque(int idInsumo, decimal quantidade)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE insumos SET estoque_atual = estoque_atual - @qtd WHERE id_insumo = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@qtd", quantidade);
                cmd.Parameters.AddWithValue("@id", idInsumo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao baixar estoque: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public DataTable ListarParaCombo()
        {
            try
            {
                conexao.AbrirConexao();
                DataTable dt = new DataTable();
                string sql = "SELECT id_insumo, nome FROM insumos ORDER BY nome ASC";
                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar lista de insumos: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        public string ObterUnidadeMedida(int idInsumo)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT unidade_medida FROM insumos WHERE id_insumo = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@id", idInsumo);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }
    }
}
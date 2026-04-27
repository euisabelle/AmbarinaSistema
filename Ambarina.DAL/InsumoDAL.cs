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
                string sql = "INSERT INTO insumos (nome, categoria, unidade_medida, estoque_atual, custo_unitario, estoque_minimo) " +
                             "VALUES (@nome, @cat, @un, @est, @custo, @min)";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@nome", insumo.Nome);
                cmd.Parameters.AddWithValue("@cat", insumo.Categoria);
                cmd.Parameters.AddWithValue("@un", insumo.UnidadeMedida);
                cmd.Parameters.AddWithValue("@est", insumo.QtdeInicial);
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
                string sql = "SELECT id_insumo as 'ID', nome as 'Nome', categoria as 'Categoria', " +
                "unidade_medida as 'Unidade', estoque_atual as 'Estoque', " +
                "custo_unitario as 'Custo (R$)', estoque_minimo as 'Mínimo', " +
                "(estoque_atual * custo_unitario) as 'Total' FROM insumos";

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

        // Método para Deletar
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

        // Método para Editar (Update)
        public void EditarInsumo(InsumoDTO insumo)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE insumos SET nome=@nome, categoria=@cat, unidade_medida=@un, " +
                             "estoque_atual=@est, custo_unitario=@custo, estoque_minimo=@min WHERE id_insumo=@id";

                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@nome", insumo.Nome);
                cmd.Parameters.AddWithValue("@cat", insumo.Categoria);
                cmd.Parameters.AddWithValue("@un", insumo.UnidadeMedida);
                cmd.Parameters.AddWithValue("@est", insumo.QtdeInicial);
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
    }
}

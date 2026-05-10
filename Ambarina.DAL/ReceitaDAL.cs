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
    public class ReceitaDAL
    {
        Conexao conexao = new Conexao();

        public DataTable ListarReceitas()
        {
            try
            {
                conexao.AbrirConexao();
                // Buscamos o nome do produto (fazendo um JOIN) e o aroma padrão
                string sql = "SELECT r.id_receita, p.nome as 'Produto', r.aroma_padrao as 'Aroma' " +
                             "FROM receitas r " +
                             "INNER JOIN produtos p ON r.id_produto = p.id_produto " +
                             "ORDER BY p.nome ASC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }
        public void Excluir(int idReceita)
        {
            try
            {
                conexao.AbrirConexao();
                // Primeiro deletamos os itens da receita (filhos) para não dar erro de chave estrangeira
                string sqlItens = "DELETE FROM itens_receita WHERE id_receita = @id";
                MySqlCommand cmdItens = new MySqlCommand(sqlItens, conexao.conectar);
                cmdItens.Parameters.AddWithValue("@id", idReceita);
                cmdItens.ExecuteNonQuery();

                // Depois deletamos a receita em si (pai)
                string sqlReceita = "DELETE FROM receitas WHERE id_receita = @id";
                MySqlCommand cmdReceita = new MySqlCommand(sqlReceita, conexao.conectar);
                cmdReceita.Parameters.AddWithValue("@id", idReceita);
                cmdReceita.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public DataTable ListarItensDaReceita(int idReceita)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT i.nome as Insumo, ir.quantidade as Qtd, i.unidade_medida as Unid " +
                             "FROM itens_receita ir " +
                             "INNER JOIN insumos i ON ir.id_insumo = i.id_insumo " +
                             "WHERE ir.id_receita = @id";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.SelectCommand.Parameters.AddWithValue("@id", idReceita);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public void SalvarNovaReceita(ReceitaDTO receita, List<ItensReceitaDTO> itens)
        {
            try
            {
                conexao.AbrirConexao();
                // 1. Salva o cabeçalho da receita e pega o ID gerado
                string sqlReceita = "INSERT INTO receitas (id_produto, aroma_padrao) VALUES (@idProd, @aroma); SELECT LAST_INSERT_ID();";
                MySqlCommand cmdRec = new MySqlCommand(sqlReceita, conexao.conectar);
                cmdRec.Parameters.AddWithValue("@idProd", receita.IdProduto);
                cmdRec.Parameters.AddWithValue("@aroma", receita.AromaPadrao);

                int idReceitaGerada = Convert.ToInt32(cmdRec.ExecuteScalar());

                // 2. Salva os itens vinculados a esse ID
                foreach (var item in itens)
                {
                    // Precisamos buscar o id_insumo pelo nome que está na grid
                    string sqlItens = "INSERT INTO itens_receita (id_receita, id_insumo, quantidade) " +
                                      "VALUES (@idRec, (SELECT id_insumo FROM insumos WHERE nome = @nomeInsumo), @qtd)";
                    MySqlCommand cmdItem = new MySqlCommand(sqlItens, conexao.conectar);
                    cmdItem.Parameters.AddWithValue("@idRec", idReceitaGerada);
                    cmdItem.Parameters.AddWithValue("@nomeInsumo", item.NomeInsumo);
                    cmdItem.Parameters.AddWithValue("@qtd", item.Quantidade);
                    cmdItem.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }
    }
}

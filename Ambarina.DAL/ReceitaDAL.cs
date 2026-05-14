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

        public DataTable ListarAromas()
        {
            try
            {
                conexao.AbrirConexao();
                // Lista todos os aromas cadastrados (valores únicos)
                string sql = "SELECT DISTINCT aroma_padrao FROM receitas ORDER BY aroma_padrao ASC";
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
                // Usando o nome CORRETO da tabela: receita_insumos
                string sqlItens = "DELETE FROM receita_insumos WHERE receitas_id_receita = @id";
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
                // Ajustado para os nomes exatos do seu novo diagrama: receita_insumos
                string sql = "SELECT i.nome as Insumo, ri.quantidade as Qtd, i.unidade_medida as Unid " +
                             "FROM receita_insumos ri " +
                             "INNER JOIN insumos i ON ri.insumos_id_insumo = i.id_insumo " +
                             "WHERE ri.receitas_id_receita = @id";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao.conectar);
                da.SelectCommand.Parameters.AddWithValue("@id", idReceita);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw new Exception("Erro ao carregar insumos: " + ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public void SalvarNovaReceita(ReceitaDTO receita, List<ItensReceitaDTO> itens)
        {
            try
            {
                conexao.AbrirConexao();

                // 1. Salva o cabeçalho (sem tentar salvar insumo aqui!)
                string sqlReceita = "INSERT INTO receitas (id_produto, aroma_padrao) VALUES (@idProd, @aroma); SELECT LAST_INSERT_ID();";
                MySqlCommand cmdRec = new MySqlCommand(sqlReceita, conexao.conectar);
                cmdRec.Parameters.AddWithValue("@idProd", receita.IdProduto);
                cmdRec.Parameters.AddWithValue("@aroma", receita.AromaPadrao);

                int idReceitaGerada = Convert.ToInt32(cmdRec.ExecuteScalar());

                // Salva os itens na tabela intermediária 
                foreach (var item in itens)
                {
                    // Buscamos o ID do insumo pelo nome com TRIM para evitar erros de espaço
                    string sqlBuscaInsumo = "SELECT id_insumo FROM insumos WHERE TRIM(nome) = TRIM(@nomeInsumo) LIMIT 1";
                    MySqlCommand cmdBusca = new MySqlCommand(sqlBuscaInsumo, conexao.conectar);
                    cmdBusca.Parameters.AddWithValue("@nomeInsumo", item.NomeInsumo);

                    object idInsumoObj = cmdBusca.ExecuteScalar();
                    if (idInsumoObj == null)
                    {
                        throw new Exception($"Insumo '{item.NomeInsumo}' não encontrado no banco de dados!");
                    }

                    int idInsumo = Convert.ToInt32(idInsumoObj);

                    // Inserir na tabela 'receita_insumos' com os nomes definidos no diagrama
                    string sqlItens = "INSERT INTO receita_insumos (receitas_id_receita, insumos_id_insumo, quantidade) " +
                                      "VALUES (@idRec, @idInsumo, @qtd)";
                    MySqlDataAdapter da = new MySqlDataAdapter(sqlItens, conexao.conectar);
                    MySqlCommand cmdItem = new MySqlCommand(sqlItens, conexao.conectar);
                    cmdItem.Parameters.AddWithValue("@idRec", idReceitaGerada);
                    cmdItem.Parameters.AddWithValue("@idInsumo", idInsumo);
                    cmdItem.Parameters.AddWithValue("@qtd", item.Quantidade);
                    cmdItem.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }

        public void EditarReceita(ReceitaDTO receita, List<ItensReceitaDTO> itens)
        {
            try
            {
                conexao.AbrirConexao();

                // 1. Atualiza o cabeçalho da receita
                string sqlReceita = "UPDATE receitas SET id_produto = @idProd, aroma_padrao = @aroma WHERE id_receita = @id";
                MySqlCommand cmdRec = new MySqlCommand(sqlReceita, conexao.conectar);
                cmdRec.Parameters.AddWithValue("@idProd", receita.IdProduto);
                cmdRec.Parameters.AddWithValue("@aroma", receita.AromaPadrao);
                cmdRec.Parameters.AddWithValue("@id", receita.Id);
                int rowsAffected = cmdRec.ExecuteNonQuery();

                // Verificar se a receita foi encontrada
                if (rowsAffected == 0)
                {
                    throw new Exception($"A receita com ID {receita.Id} não foi encontrada no banco de dados.");
                }

                // 2. Deleta os itens antigos usando a tabela CORRETA
                string sqlDeleteItens = "DELETE FROM receita_insumos WHERE receitas_id_receita = @id";
                MySqlCommand cmdDel = new MySqlCommand(sqlDeleteItens, conexao.conectar);
                cmdDel.Parameters.AddWithValue("@id", receita.Id);
                cmdDel.ExecuteNonQuery();

                // 3. Insere os novos itens
                foreach (var item in itens)
                {
                    // Primeiro busca o ID do insumo pelo nome
                    string sqlBuscaInsumo = "SELECT id_insumo FROM insumos WHERE TRIM(nome) = TRIM(@nomeInsumo) LIMIT 1";
                    MySqlCommand cmdBusca = new MySqlCommand(sqlBuscaInsumo, conexao.conectar);
                    cmdBusca.Parameters.AddWithValue("@nomeInsumo", item.NomeInsumo);

                    object idInsumoObj = cmdBusca.ExecuteScalar();
                    if (idInsumoObj == null)
                    {
                        throw new Exception($"Insumo '{item.NomeInsumo}' não encontrado no banco de dados!");
                    }

                    int idInsumo = Convert.ToInt32(idInsumoObj);

                    // Usar a tabela CORRETA e as colunas corretas
                    string sqlItens = "INSERT INTO receita_insumos (receitas_id_receita, insumos_id_insumo, quantidade) " +
                                      "VALUES (@idRec, @idInsumo, @qtd)";
                    MySqlCommand cmdItem = new MySqlCommand(sqlItens, conexao.conectar);
                    cmdItem.Parameters.AddWithValue("@idRec", receita.Id);
                    cmdItem.Parameters.AddWithValue("@idInsumo", idInsumo);
                    cmdItem.Parameters.AddWithValue("@qtd", item.Quantidade);
                    cmdItem.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { conexao.FecharConexao(); }
        }
    }
}

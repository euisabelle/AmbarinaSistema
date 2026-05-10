using Ambarina.DTO;
using MySql.Data.MySqlClient;
using System;

namespace Ambarina.DAL
{
    public class UsuarioDAL
    {
        Conexao conexao = new Conexao();

        public UsuarioDTO ValidarLogin(string usuario, string senha)
        {
            try
            {
                conexao.AbrirConexao();
                // Query para buscar o usuário
                string sql = "SELECT * FROM usuarios WHERE usuario = @user AND senha = @pass";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@user", usuario);
                cmd.Parameters.AddWithValue("@pass", senha);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read()) // Se encontrou alguém
                {
                    UsuarioDTO user = new UsuarioDTO();
                    user.Id = Convert.ToInt32(dr["id_usuario"]);
                    user.Nome = dr["nome"].ToString();
                    user.NivelAcesso = dr["nivel_acesso"].ToString();
                    return user;
                }
                return null; // Não encontrou
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao validar login: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        // NOVO: altera a senha do usuário. Retorna true se conseguiu alterar.
        public bool AlterarSenha(string usuario, string novaSenha)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "UPDATE usuarios SET senha = @pass WHERE usuario = @user";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@pass", novaSenha);
                cmd.Parameters.AddWithValue("@user", usuario);

                int linhas = cmd.ExecuteNonQuery();
                return linhas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar senha: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }

        // NOVO: obtém a senha atual do usuário para validação
        public string ObterSenhaAtual(string usuario)
        {
            try
            {
                conexao.AbrirConexao();
                string sql = "SELECT senha FROM usuarios WHERE usuario = @user";
                MySqlCommand cmd = new MySqlCommand(sql, conexao.conectar);
                cmd.Parameters.AddWithValue("@user", usuario);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return dr["senha"].ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter senha atual: " + ex.Message);
            }
            finally { conexao.FecharConexao(); }
        }
    }
}
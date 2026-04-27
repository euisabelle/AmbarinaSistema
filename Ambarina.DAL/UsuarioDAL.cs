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
    }
}
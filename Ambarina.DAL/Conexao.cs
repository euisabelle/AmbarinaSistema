using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Ambarina.DAL
{
    public class Conexao
    {
        // A "String de Conexão": o endereço da casa do banco
        // server=localhost (seu PC), database=ambarina, user=root, password=vazio
        private string strConexao = "server=localhost;database=ambarina;uid=root;pwd=;port=3306";
        public MySqlConnection conectar;

        public void AbrirConexao()
        {
            try
            {
                conectar = new MySqlConnection(strConexao);
                conectar.Open();
            }
            catch (Exception ex)
            {
                // Em vez de mostrar na tela aqui, nós lançamos a exceção
                // para que a UI (onde está o botão) decida como mostrar o erro.
                throw new Exception("Erro na DAL (Conexão): " + ex.Message);
            }
        }

        public void FecharConexao()
        {
            try
            {
                if (conectar.State == System.Data.ConnectionState.Open)
                {
                    conectar.Close();
                }
            }
            catch (Exception ex)
            {
                // Em vez de mostrar na tela aqui, nós lançamos a exceção
                // para que a UI (onde está o botão) decida como mostrar o erro.
                throw new Exception("Erro na DAL (Conexão): " + ex.Message);
            }
        }
    }
}

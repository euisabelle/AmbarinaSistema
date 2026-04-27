using Ambarina.DAL;
using Ambarina.DTO;
using System;

namespace Ambarina.BLL
{
    public class UsuarioBLL
    {
        UsuarioDAL loginDAL = new UsuarioDAL();

        public UsuarioDTO Autenticar(string usuario, string senha)
        {
            // Validação básica antes de ir ao banco
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                throw new Exception("Usuário e senha são obrigatórios!");
            }

            return loginDAL.ValidarLogin(usuario, senha);
        }
    }
}
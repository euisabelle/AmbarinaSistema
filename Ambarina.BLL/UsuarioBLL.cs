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
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                throw new Exception("Usuário e senha são obrigatórios!");
            }

            return loginDAL.ValidarLogin(usuario, senha);
        }

        public bool AlterarSenha(string usuario, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(novaSenha))
            {
                throw new Exception("Usuário e nova senha são obrigatórios.");
            }

            return loginDAL.AlterarSenha(usuario, novaSenha);
        }

        public bool ValidarSenhaAnterior(string usuario, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(novaSenha))
            {
                throw new Exception("Usuário e nova senha são obrigatórios.");
            }

            string senhaAnterior = loginDAL.ObterSenhaAtual(usuario);
            
            if (senhaAnterior == novaSenha)
            {
                return false;
            }

            return true;
        }

        /// Valida se o usuário fornecido está logado.
        public static string ValidarNomeUsuarioLogado(UsuarioDTO usuarioLogado)
        {
            if (usuarioLogado == null || string.IsNullOrEmpty(usuarioLogado.Nome))
            {
                throw new InvalidOperationException("Nenhum usuário está logado no momento.");
            }

            return usuarioLogado.Nome;
        }
    }
}
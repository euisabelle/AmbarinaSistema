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

        //expõe alteração de senha para UI após validação
        public bool AlterarSenha(string usuario, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(novaSenha))
            {
                throw new Exception("Usuário e nova senha são obrigatórios.");
            }

            // Aqui poderia entrar hashing; por compatibilidade com o projeto atual salvamos direto.
            return loginDAL.AlterarSenha(usuario, novaSenha);
        }

        // Valida se a nova senha é diferente da senha anterior
        public bool ValidarSenhaAnterior(string usuario, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(novaSenha))
            {
                throw new Exception("Usuário e nova senha são obrigatórios.");
            }

            string senhaAnterior = loginDAL.ObterSenhaAtual(usuario);
            
            // Se a senha anterior é igual à nova, retorna false
            if (senhaAnterior == novaSenha)
            {
                return false;
            }

            return true;
        }
    }
}
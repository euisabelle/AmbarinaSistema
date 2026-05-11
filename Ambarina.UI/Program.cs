using Ambarina.DTO;

namespace Ambarina.UI
{
    internal static class Program
    {
       
        /// Armazena o usuário atualmente logado na aplicação
        public static UsuarioDTO UsuarioLogado { get; set; }

        ///  The main entry point for the application.
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmSplash());
        }
    }
}
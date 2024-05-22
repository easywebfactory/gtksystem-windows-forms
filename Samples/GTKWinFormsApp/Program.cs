using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("ExecutablePath:" + Application.ExecutablePath);
            Console.WriteLine("ExecutablePath:" + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);

           // Console.WriteLine("CommonAppDataPath:" + Application.CommonAppDataPath);
           // Console.WriteLine("UserAppDataPath:" + Application.UserAppDataPath);
           // Console.WriteLine("LocalUserAppDataPath:" + Application.LocalUserAppDataPath);
            Console.WriteLine("StartupPath:" + Application.StartupPath);
            Console.WriteLine("UserAppDataPath:" + Application.UserAppDataPath);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3());

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyOtGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new Menu());
            Application.Run(new GameForm());
        }
    }
}

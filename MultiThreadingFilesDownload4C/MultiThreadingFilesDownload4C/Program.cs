using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace LearningDemo
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

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();

            Application.Run(new MultpleFilesBreakPointDownload());
        }
    }
}
﻿using System;
using System.Windows.Forms;

namespace StoneAge.WinForms
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var appSettings = new AppSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (appSettings.ShowSplashScreen)
                Application.Run(new WelcomeScreenForm());
            var game = new Core.Game();
            if (appSettings.ShowChoosePlayersScreen)
                Application.Run(new ChoosePlayersForm(game));
            Application.Run(new MainForm(game));
        }
    }
}

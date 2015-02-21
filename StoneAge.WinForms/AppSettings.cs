using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.WinForms
{
    public class AppSettings
    {
        public bool ShowSplashScreen
        {
            get
            {
                return false;
            }
        }

        public bool ShowChoosePlayersScreen
        {
            get
            {
                return true;
            }
        }
    }
}

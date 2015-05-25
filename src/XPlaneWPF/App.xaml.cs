using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace XPlaneWPF
{
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            var window = new AppWindow();

            window.Show();
        }
    }
}

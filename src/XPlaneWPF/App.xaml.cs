using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace XPlaneWPF
{
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            XPlaneWPF.Extensions.SVG.SvgExtension.Directory = System.IO.Path.Combine("Icons", "SVG");

            var IsXPMediaCenter = SystemParameters.IsMediaCenter;
            var hasMouse = SystemParameters.IsMousePresent;
            var hasMouseWheel = SystemParameters.IsMouseWheelPresent;
            var isGlassEnabled = SystemParameters.IsGlassEnabled;
            var isRemoteControlled = SystemParameters.IsRemotelyControlled;
            var isRemoteSession = SystemParameters.IsRemoteSession;
            var isSlowMachine = SystemParameters.IsSlowMachine;
            var isTablet = SystemParameters.IsTabletPC;
            var themeColor = SystemParameters.UxThemeColor;
            var themeName = SystemParameters.UxThemeName;
            var glassBrush = SystemParameters.WindowGlassBrush;
            var glassColor = SystemParameters.WindowGlassColor;
            var VirtualWidth = SystemParameters.VirtualScreenWidth;
            var VirtualHeight = SystemParameters.VirtualScreenHeight;
            var PrimaryWidth = SystemParameters.PrimaryScreenWidth;
            var primaryHeight = SystemParameters.PrimaryScreenHeight;

            if (isRemoteSession || isRemoteControlled)
            {
                MessageBox.Show("This Application does not support remote connections. Performance may be drastically reduced");
            }

            if (!hasMouseWheel)
            {
                MessageBox.Show("You do not have a mouse wheel. Experience may be hindered");
            }
            


            



            if (VirtualWidth > PrimaryWidth || VirtualHeight > primaryHeight)
            {
                //MessageBox.Show("Multiple Monitors detected");

                // Span entire resolution?

                // Separate Windows per monitor?

                // Powerpoint'ish Setup

                //new Window().ShowDialog();
            }

            //Console.WriteLine("Total Screen Resolution: {0} x {1}", SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
            //Console.WriteLine("Primary Screen Resolution: {0} x {1}", SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            new AppWindow().Show();
        }
    }
}

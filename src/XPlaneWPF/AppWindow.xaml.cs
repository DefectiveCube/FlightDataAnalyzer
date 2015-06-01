using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using XPlaneWPF.Pages;

namespace XPlaneWPF
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow :  NavigationWindow
    {
        public AppWindow()
        {
            InitializeComponent();

            Loaded += ViewModel.AppWindow_Loaded;
        }

        private void TaskbarDrag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

/*        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            if (WindowState == System.Windows.WindowState.Normal)
            {
                NormalTop = Top;
                NormalLeft = Left;
                NormalWidth = ActualWidth;
                NormalHeight = ActualHeight;
                Width = SystemParameters.VirtualScreenWidth;
                Height = SystemParameters.VirtualScreenHeight;
                Top = 0;
                Left = 0;
                WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                Width = NormalWidth;
                Height = NormalHeight;
                Top = NormalTop;
                Left = NormalLeft;
                WindowState = System.Windows.WindowState.Normal;
            }
        }*/

        /*
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            base.WindowState = System.Windows.WindowState.Minimized;
        }

        private void navWindow_StateChanged(object sender, EventArgs e)
        {
            Console.WriteLine("StateChanged");
        }

        private void navWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine("SizeChanged");
        }  

        private void Minimize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Maximize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Normal)
            {
                NormalTop = Top;
                NormalLeft = Left;
                NormalWidth = ActualWidth;
                NormalHeight = ActualHeight;
                Width = SystemParameters.VirtualScreenWidth;
                Height = SystemParameters.VirtualScreenHeight;
                Top = 0;
                Left = 0;
                WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                Width = NormalWidth;
                Height = NormalHeight;
                Top = NormalTop;
                Left = NormalLeft;
                WindowState = System.Windows.WindowState.Normal;
            }
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FDA.IO;
using XPlaneWPF.Commands;
using XPlaneWPF.Pages;

namespace XPlaneWPF.ViewModels
{
    public class AppWindowViewModel : INotifyPropertyChanged
    {
        private string pageName;

        public event PropertyChangedEventHandler PropertyChanged;

        public AppWindowViewModel()
        {
            PageChangeCommand = new Command<string>(Navigate);
            MinimizeCommand = new Command(Minimize);
            MaximizeCommand = new Command(Maximize);
            FloatMenuOpenCommand = new Command(FloatMenuToggle);
            CloseCommand = new Command(Close);           
        }

        internal void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a type based off an XML document
            TypeCreater.LoadModels(@"C:\Users\KirkDietz\Documents\FlightDataAnalyzer\models");

            var asms = Assembly.GetExecutingAssembly().GetReferencedAssemblies();


            var window = sender as AppWindow;
            View = window;

            var obj = window.Template.FindName("FloatMenu", window) as Popup;

            if (obj != null)
            {
                obj.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(PlaceFloatMenu);
            }
        }

        public string PageName
        {
            get { return pageName; }
            set
            {
                if (pageName == value) return;
                pageName = value;
                OnPropertyChanged("PageName");
            }
        }

        public ICommand PageChangeCommand { get; set; }

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand WindowStateCycleCommand { get; set; }

        public ICommand FloatMenuOpenCommand { get; set; }

        public WindowState WindowState { get; set; }

        public Window View { get; set; }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    
        public void Navigate(string parameter)
        {
            var window = App.Current.MainWindow as NavigationWindow;

            Page page;

            switch (parameter)
            {
                case"Home":
                    page = new StartupPage();
                    break;
                case "ImportData":
                    page = new ImportDataPage();
                    break;
                case "ExportData":
                    page = new ExportDataPage();
                    break;
                case "ViewModels":
                    page = new DataModelPage();
                    break;
                case "BuildModel":
                    page = new DataModelBuilderPage();
                    break;
                case "ViewQueries":
                    page = new QueryPage();
                    break;
                case "Calibration":
                    page = new CalibrationPage();
                    break;
                default:
                    Console.WriteLine("Unable to Navigate. Parameter: {0}", parameter);
                    return;
            }

            window.NavigationService.Navigate(page);
        }


        public CustomPopupPlacement[] PlaceFloatMenu(Size popupSize, Size targetSize, Point offset)
        {
            var placement1 = new CustomPopupPlacement(new Point(-50, 100), PopupPrimaryAxis.Vertical);
            var placement2 = new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);

            return new CustomPopupPlacement[] { placement1, placement2 };
        }

        public void FloatMenuToggle(object obj)
        {
            AppWindow window = View as AppWindow;

            var popup = window.Template.FindName("SubFloatMenu",window) as Popup;

            popup.IsOpen = !popup.IsOpen;            
        }

        public void Maximize(object obj)
        {

        }

        public void Minimize(object obj)
        {

        }

        public void Close(object obj)
        {
            App.Current.Shutdown();
        }
    }
}

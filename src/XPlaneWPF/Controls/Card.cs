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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPlaneWPF.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XPlaneWPF.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XPlaneWPF.Controls;assembly=XPlaneWPF.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Card/>
    ///
    /// </summary>
    [ContentProperty("Content")]
    public class Card : Control
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Card), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register("Subtitle", typeof(string), typeof(Card), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(Card), new UIPropertyMetadata(null));
        public static readonly DependencyProperty AdmissiveProperty = DependencyProperty.Register("Admissive", typeof(UIElement), typeof(Card), new UIPropertyMetadata(null));
        public static readonly DependencyProperty DismissiveProperty = DependencyProperty.Register("Dismissive", typeof(UIElement), typeof(Card), new UIPropertyMetadata(null));

        static Card()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Card), new FrameworkPropertyMetadata(typeof(Card)));            
        }

        public Card()
        {
            DataContext = this;
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public string Subtitle
        {
            get { return (string)this.GetValue(SubtitleProperty); }
            set { this.SetValue(SubtitleProperty, value); }
        }

        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public UIElement Dismissive
        {
            get { return (UIElement)GetValue(DismissiveProperty); }
            set { SetValue(DismissiveProperty, value); }
        }

        public UIElement Admissive
        {
            get { return (UIElement)GetValue(AdmissiveProperty); }
            set { SetValue(AdmissiveProperty, value); }
        }
    }
}

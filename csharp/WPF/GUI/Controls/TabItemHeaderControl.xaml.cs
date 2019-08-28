using System.Windows.Controls;
using System.Windows;
using System;
using System.Windows.Media.Imaging;

namespace WPF.GUI.Controls
{
    /// <summary>
    /// Interaction logic for TabItemHeaderControl.xaml
    /// </summary>
    public partial class TabItemHeaderControl : UserControl
    {
        public const string EmptyString = "";

        public TabItemHeaderControl(string header, string iconPath = EmptyString)
        {
            InitializeComponent();
            Header.Text = header;

            if (string.IsNullOrEmpty(iconPath))
                Icon.Visibility = Visibility.Collapsed;
            else
                Icon.Source = new BitmapImage(new Uri(iconPath));
        }
    }
}

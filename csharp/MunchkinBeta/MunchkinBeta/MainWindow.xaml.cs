using MunchkinLib.Models;
using System;
using System.Reflection;
using System.Windows;

namespace MunchkinBeta
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player player;
        private MainPage mainPage;

        /// <summary>
        /// MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetTitle();
        }

        private void SetTitle()
        {
            Title = "Munchkin ";
            try
            {
                Title += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception)
            { }
#if DEBUG
            Title += "Beta";
#endif
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            var isFemale = IsFemaleCheckbox.IsChecked ?? false;

            player = new Player(isFemale);
            mainPage = new MainPage(this, player);
            Content = mainPage;
            Height = mainPage.Height;
            Width = mainPage.Width;
        }
    }
}

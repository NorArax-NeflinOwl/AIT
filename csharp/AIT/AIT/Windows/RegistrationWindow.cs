using AITLib.Constants;
using AIT.Interfaces;
using System;
using System.Windows;
using AIT.Pages;
using AIT.Pages.PartlyPages;

namespace AIT.Windows
{
    public partial class RegistrationWindow : Window, ISubscribe, IDisposable
    {
        private AccountFieldFrame frame;
        public RegistrationWindow()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            frame = new AccountFieldFrame();

            aitAccountFrame.Content = frame;
            Title = AitStrings.REGISTRATION_T;
            TitleLabel.Content = AitStrings.REGISTRATION_T;
            aitCreateBtn.Content = AitStrings.CREATE;

            frame.aitFName.Focus();
        }

        public void Subscribe()
        {
            aitCreateBtn.Click += AitCreateBtn_Click;
        }

        private async void AitCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (frame.ControlsValidation())
            {
                if(await frame.PersonRejestracion())
                    Close();
            }
        }

        public void Unsubscribe()
        {
            aitCreateBtn.Click -= AitCreateBtn_Click;
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        } 
    }
}
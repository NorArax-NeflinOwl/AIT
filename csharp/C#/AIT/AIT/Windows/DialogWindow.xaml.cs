using AIT.Helpers;
using AIT.Interfaces;
using AIT.Models;
using AITLib.Constants;
using AITLib.Helpers;
using System;
using System.Diagnostics;
using System.Windows;

namespace AIT.Windows
{
    public partial class DialogWindow : Window, ISubscribe, IDisposable
    {
        private AitDialogModel m_Model;
        private object AnswerParam;
        public Action<bool, object> AnswerHandler { get; set; }

        public DialogWindow()
        {
            InitializeComponent();

            Init();
            Subscribe();
        }

        public DialogWindow(AitDialogModel model)
        {
            m_Model = model;

            InitializeComponent();

            Init();
            Subscribe();
        }

        public DialogWindow(AitDialogModel model, Action<bool, object> handler, object param = null)
        {
            m_Model = model;
            AnswerHandler = handler;
            AnswerParam = param;

            InitializeComponent();

            Init();
            Subscribe();
        }

        public void Init()
        {
            Title = m_Model.Title;
            aitImage.Source = m_Model.Image.Source;
            
            aitMessageContent.Text = m_Model.Message;
            if (m_Model.OneButtonPanel)
            {
                aitOneButtonPanel.Visibility = Visibility.Visible;
                aitTwoButtonPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                aitOneButtonPanel.Visibility = Visibility.Collapsed;
                aitTwoButtonPanel.Visibility = Visibility.Visible;
            }
        }

        public void Subscribe()
        {
            if(m_Model.OneButtonPanel)
            {
                aitOkErrButton.Click += AitOkErrButton_Click;
            }
            else
            {
                aitCancelButton.Click += AitCancelButton_Click;
                aitOkButton.Click += AitOkButton_Click;
            }
            Closed += Dialog_Closed;
        }

        private void AitOkButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerHandler?.Invoke(true, AnswerParam);
            Close();
        }

        private void AitCancelButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerHandler?.Invoke(false, AnswerParam);
            Close();
        }

        private void AitOkErrButton_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if(AnswerParam is string str && str.Equals(AitStrings.CLOSE))
            {
                try
                {
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
                    Environment.Exit(0);
                }
            }
        }

        private void Dialog_Closed(object sender, EventArgs e)
        {
            if (AitFeatureManager.Instance.Disable_Feature[Feature.SERIALIZATION_SESSION])
                AnswerHandler?.Invoke(false, AnswerParam);
        }

        public void Unsubscribe()
        {
            if (m_Model.OneButtonPanel)
            {
                aitOkErrButton.Click -= AitOkErrButton_Click;
            }
            else
            {
                aitCancelButton.Click -= AitCancelButton_Click;
                aitOkButton.Click -= AitOkButton_Click;
            }
            Closed -= Dialog_Closed;
        }
        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        }
    }
}

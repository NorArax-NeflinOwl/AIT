using AIT.DataBases;
using AIT.DataBases.DBModel;
using AIT.Helpers;
using AIT.Interfaces;
using AIT.Models;
using AITLib.Constants;
using AITLib.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace AIT.Windows
{
    public partial class QuickNoteWindow : Window, ISubscribe, IDisposable
    {
        private readonly Action m_Refresh;

        public QuickNoteWindow()
        {
            InitializeComponent();
        }

        public QuickNoteWindow(Action refresh)
        {
            m_Refresh = refresh;

            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        }

        public void Init()
        {
            Title = $"QuickNote-{DateTime.Now.ToString("HH:mm_dd-MM-yyyy")}";
            aitTitleNote.Text = Title;

            Show();
        }

        public void Subscribe()
        {
            aitQuickNoteSaveBtn.Click += AitQuickNoteSaveBtn_Click;
            aitCryptMeCheckBox.Checked += AitCryptMeCheckBox_Checked;
            aitCryptMeCheckBox.Unchecked += AitCryptMeCheckBox_Checked;
        }

        private void AitCryptMeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            aitPasswordNote.IsEnabled = aitCryptMeCheckBox.IsChecked == true;
        }

        private async void AitQuickNoteSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(aitQuickNoteContent.Text))
            {
                try
                {
                    var password = string.Empty;
                    var note = string.Empty;
                    if (aitCryptMeCheckBox.IsChecked == true && !string.IsNullOrEmpty(aitPasswordNote.Password))
                    {
                        password = AitCryptJsonManager.Instance.Serialize(aitPasswordNote.Password);
                        note = AitCryptJsonManager.Instance.Serialize(aitQuickNoteContent.Text, aitPasswordNote.Password);
                    }
                    else
                        note = aitQuickNoteContent.Text;

                    var person = AitSessionManager.GetSession.Person;
                    var quickNote = new AitQuickNote
                    {
                        Title = aitTitleNote.Text,
                        PersonID = person.ID,
                        Note = note,
                        CDate = DateTime.Now,
                        Password = password,
                        Person = person
                    };

                    person.QuickNotes.Add(quickNote);
                    await AitDBContextInstance.Instance.SaveChangesAsync();

                    var msg = string.Format(AitStrings.CREATE_QUICKNOTE_SUCCESS, aitTitleNote.Text);
                    var success = new DialogWindow(new AitDialogModel(DialogType.INFORMATION, msg));
                    success.Show();

                    m_Refresh?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(string.Format(AitStrings.CREATE_QUICKNOTE_CRASH, Title));
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                    var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                    error.Show();
                }
                finally
                {
                    Close();
                }
            }
            else
            {
                var msg = AitStrings.EMPTY_QUICKNOTE;
                var dialog = new DialogWindow(new AitDialogModel(DialogType.ERROR, msg));
                dialog.Show();
            }
        }

        public void Unsubscribe()
        {
            aitQuickNoteSaveBtn.Click -= AitQuickNoteSaveBtn_Click;
            aitCryptMeCheckBox.Checked -= AitCryptMeCheckBox_Checked;
            aitCryptMeCheckBox.Unchecked -= AitCryptMeCheckBox_Checked;
        }
    }
}

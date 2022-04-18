using AIT.DataBases;
using AIT.DataBases.DBModel;
using AIT.Helpers;
using AIT.Interfaces;
using AIT.Windows;
using AITLib.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class QuickNoteExplorerPage : Page, ISubscribe, IDisposable
    {
        private readonly MainWindow m_MainWindow;
        private Action<bool, object> m_Question;
        private Dictionary<int, KeyValuePair<AitQuickNote, bool>> quickNotes;

        public QuickNoteExplorerPage()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public QuickNoteExplorerPage(Window window)
        {
            m_MainWindow = window as MainWindow;
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
            var person = AitSessionManager.GetSession.Person;
            int i = 0;
            quickNotes = new Dictionary<int, KeyValuePair<AitQuickNote, bool>>();
            foreach (var item in person.QuickNotes)
            {
                KeyValuePair<AitQuickNote, bool> pair = new KeyValuePair<AitQuickNote, bool>(item, item.IsCrypted);
                quickNotes.Add(i, pair);
                i++;
            }
            InitNote();
        }

        private void InitNote(int? index = null, bool? decrypt = null)
        {
            m_MainWindow.aitProgressGrid.Visibility = Visibility.Visible;

            var person = AitSessionManager.GetSession.Person;
            if (person != null)
            {
                aitQuickNoteListView.Items.Clear();
                for (var i = 0; i < quickNotes.Count; i++)
                {
                    if(index != null && i == index)
                    {
                        string password;
                        var note = (AitQuickNote)quickNotes[i].Key;

                        if(!string.IsNullOrWhiteSpace(note.Note))
                        {
                            if (decrypt == true)
                            {
                                password = AitCryptJsonManager.Instance.Deserialize<string>(note.Password);
                                note.Note = AitCryptJsonManager.Instance.Deserialize<string>(note.Note, password);
                                quickNotes[i] = new KeyValuePair<AitQuickNote, bool>(note, false);
                            }
                            else if (decrypt == false)
                            {
                                password = AitCryptJsonManager.Instance.Serialize(note.Password);
                                note.Note = AitCryptJsonManager.Instance.Serialize(note.Note, password);
                                quickNotes[i] = new KeyValuePair<AitQuickNote, bool>(note, true);
                            }
                        }

                        aitQuickNoteListView.Items.Add(note);
                    }
                    else
                        aitQuickNoteListView.Items.Add((AitQuickNote)quickNotes[i].Key.Clone());
                }
            }

            aitQuickNoteListView.SelectedIndex = -1;

            //await Task.Delay(TimeSpan.FromSeconds(0.5));
            m_MainWindow.aitProgressGrid.Visibility = Visibility.Collapsed;
        }

        public void Subscribe()
        {
            m_MainWindow.Refresh += Refresh;
            aitQuickNoteListView.MouseDoubleClick += AitQuickNoteListView_MouseDoubleClick;
            aitQuickNoteListView.MouseRightButtonUp += AitQuickNoteListView_MouseRightButtonUp;
        }

        private void AitQuickNoteListView_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CryptNote();
        }

        private void CryptNote()
        {
            if (aitQuickNoteListView.SelectedItem is AitQuickNote note)
            {
                var index = aitQuickNoteListView.SelectedIndex;
                if (note.IsCrypted)
                {
                    InitNote(index, false);
                }
            }
        }

        private void AitQuickNoteListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(aitQuickNoteListView.SelectedItem is AitQuickNote note)
            {
                var index = aitQuickNoteListView.SelectedIndex;
                if (note.IsCrypted)
                {
                    InitNote(index, true);
                }
                else
                {
                    m_Question += QuestionRequest;
                    var msg = string.Format(AitStrings.DELETE_NOTE_QUE, note.Title);
                    var dialog = new DialogWindow(new Models.AitDialogModel(Models.DialogType.QUESTION, msg), m_Question, index);
                    dialog.Show();
                }
            }
        }

        private async void QuestionRequest(bool obj, object param)
        {
            m_Question -= QuestionRequest;

            if(obj && param is int index)
            {
                m_MainWindow.aitProgressGrid.Visibility = Visibility.Visible;

                var person = AitSessionManager.GetSession.Person;
                if (person != null)
                {
                    var i = 0;
                    foreach(AitQuickNote note in person.QuickNotes)
                    {
                        if(i == index)
                        {
                            person.QuickNotes.Remove(note);
                            break;
                        }
                        i++;
                    }
                    await AitDBContextInstance.Instance.SaveChangesAsync();
                    InitNote(null);
                }

                aitQuickNoteListView.SelectedIndex = -1;
                m_MainWindow.aitProgressGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void Refresh()
        {
            InitNote(null);
        }

        public void Unsubscribe()
        {
            m_MainWindow.Refresh -= Refresh;
            aitQuickNoteListView.MouseDoubleClick -= AitQuickNoteListView_MouseDoubleClick;
            aitQuickNoteListView.MouseRightButtonUp -= AitQuickNoteListView_MouseRightButtonUp;
        }
    }
}

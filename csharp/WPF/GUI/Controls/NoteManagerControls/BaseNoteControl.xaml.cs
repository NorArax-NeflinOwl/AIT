using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF.Managers;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;

namespace WPF.GUI.Controls.NoteManagerControls
{
    /// <summary>
    /// Interaction logic for BaseNoteControl.xaml
    /// </summary>
    public partial class BaseNoteControl : UserControl, IBaseNoteManagerControl
    {
        public bool IsCorrectlyFilled { get; set; }
        public FileTypeModel NoteType { get; set; }
        public bool IsDisposed { get; set; }
        public IProperties Properties { get; }

        public BaseNoteControl(FileTypeModel type)
        {
            if (type != null)
                NoteType = type;
            else
                NoteType = FileTypesManager.GetType(FileTypesEnum.NOTE);

            InitializeComponent();
            Init();
            Subscribe();
        }

        public void ClearContentAction()
        {
            MessageContent.IsEnabled = true;
            MessageContent.Text = string.Empty;
        }

        public void Dispose()
        {
            MessageContent.LostFocus -= MessageContent_LostFocus;

            IsDisposed = true;
            GC.Collect();
        }

        public void Subscribe()
        {
            MessageContent.LostFocus += MessageContent_LostFocus;
        }

        public void EditContentBtn_Click()
        {
            MessageContent.IsEnabled = !MessageContent.IsEnabled;
        }

        public void Init()
        {
            MessageContent.MaxLines = int.MaxValue;
            if (NoteType.EnumType.Equals(FileTypesEnum.NOTE) || NoteType.EnumType.Equals(FileTypesEnum.ACTIVATION_CODE)
                || NoteType.EnumType.Equals(FileTypesEnum.TRACE) || NoteType.EnumType.Equals(FileTypesEnum.EXCEPTION))
            {
                MessageContentInfo.Text = WPF.Properties.Resources.NOTE_DESC;
            }
            else
            {
                MessageContentInfo.Text = string.Empty;
                MessageContentInfo.Visibility = Visibility.Collapsed;
            }
        }

        public void Load()
        {
        }

        public string SerializableControl()
        {
            return CryptoJsonManager.Instance.Serialize(new MessageInfoModel(MessageContent.Text));
        }

        public void SetOneNoteContentAction(NoteListViewItemControl ctrl)
        {
            try
            {
                if (ctrl.Note.Content.StartsWith("[{"))
                {
                    MessageContent.Text = string.Empty;
                    var objs = CryptoJsonManager.Instance.Deserialize<List<MessageInfoModel>>(ctrl.Note.Content, false);
                    if (objs != null)
                    {
                        foreach (var obj in objs)
                        {
                            FillNoteFieldsFromNote(ctrl.Note.Type, obj);
                        }
                    }
                }
                else
                {
                    var obj = CryptoJsonManager.Instance.Deserialize<MessageInfoModel>(ctrl.Note.Content, false);
                    if (obj != null)
                    {
                        FillNoteFieldsFromNote(ctrl.Note.Type, obj);
                    }
                }
            }
            catch (Exception)
            {
                MessageContent.Text = ctrl.Note.Content;
            }

            MessageContent.IsEnabled = false;
        }

        public bool TypeAllowToEmptyContent()
        {
            return NoteType.AllowToEmptyContent;
        }

        public bool ValidateNotDefaultNote()
        {
            return !string.IsNullOrEmpty(MessageContent.Text);
        }

        public bool ValidateRequiredFieldFillCorrectly()
        {
            return !string.IsNullOrEmpty(MessageContent.Text);
        }

        private void MessageContent_LostFocus(object sender, RoutedEventArgs e)
        {
            IsCorrectlyFilled = !string.IsNullOrEmpty(MessageContent.Text);
            WindowsExtension.GetMainWindow()?.MainTabControlManager?.GetNoteManager()?.RefreshLayout();
        }

        private void FillNoteFieldsFromNote(FileTypesEnum type, MessageInfoModel obj)
        {
            if (FileTypesEnum.EXCEPTION.Equals(type))
            {
                foreach (var exception in obj.ExceptionInfo.ToList())
                    MessageContent.Text += exception;
            }
            else if (FileTypesEnum.TRACE.Equals(type) || FileTypesEnum.KEYLOGGER.Equals(type))
            {
                foreach (var element in obj.Array.ToList())
                    MessageContent.Text += element;
            }
            else
            {
                MessageContent.Text = obj.Message;
            }
        }
    }
}

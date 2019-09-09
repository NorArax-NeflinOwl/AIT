using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF.Managers;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;

namespace WPF.GUI.Controls.NoteManagerControls
{
    /// <summary>
    /// Interaction logic for LottoNoteControl.xaml
    /// </summary>
    public partial class LottoNoteControl : UserControl, IBaseNoteManagerControl
    {
        private List<string> luckyNumbersToSave;
        
        public bool IsDisposed { get; set; }
        public bool IsCorrectlyFilled { get; set; }
        public IProperties Properties { get; }
        public FileTypeModel Type { get; set; }

        public LottoNoteControl(FileTypeModel type)
        {
            InitializeComponent();

            Type = type;
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            MessageContentList.SelectionChanged -= MessageContentList_SelectionChanged;
            MessageContent.LostFocus -= MessageContent_LostFocus;
            
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            luckyNumbersToSave = new List<string>();
            MessageContent.MaxLines = int.MaxValue;
        }

        public void Subscribe()
        {
            MessageContentList.SelectionChanged += MessageContentList_SelectionChanged;
            MessageContent.LostFocus += MessageContent_LostFocus;
        }

        public string SerializableControl()
        {
            var array = new List<string>();
            foreach (ListViewItem item in MessageContentList.Items)
            {
                if (item.Content is TextBox textbox && !string.IsNullOrEmpty(textbox.Text))
                {
                    array.Add(textbox.Text);
                }
            }

            return CryptoJsonManager.Instance.Serialize(new MessageInfoModel(luckyNumbersToSave)
            {
                Message = MessageContent.Text,
                Array = array.ToArray()
            });
        }

        public bool ValidateNotDefaultNote()
        {
            return !string.IsNullOrEmpty(MessageContent.Text) && TypeAllowToEmptyContent()
                || (luckyNumbersToSave.Any() && Type != null && FileTypesEnum.LOTTO_NOTE.Equals(Type.EnumType));
        }

        public bool ValidateRequiredFieldFillCorrectly()
        {
            return (IsCorrectlyFilled || TypeAllowToEmptyContent())
                && (!luckyNumbersToSave.Any() || (luckyNumbersToSave.Any() && Type != null && FileTypesEnum.LOTTO_NOTE.Equals(Type.EnumType)));
        }

        public bool TypeAllowToEmptyContent()
        {
            return Type.AllowToEmptyContent;
        }

        public void ClearContentAction()
        {
            MessageContent.Text = string.Empty;
            luckyNumbersToSave.Clear();
            MessageContent.IsEnabled = true;
            MessageContentList.Visibility = Visibility.Collapsed;
            ClearMessageContentListView();
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

            foreach (ListViewItem element in MessageContentList.Items)
            {
                if (element?.Content is TextBox textbox)
                    textbox.IsEnabled = false;
            }
        }

        public void EditContentBtn_Click()
        {
            MessageContent.IsEnabled = !MessageContent.IsEnabled;

            foreach (ListViewItem item in MessageContentList.Items)
            {
                if (item?.Content is TextBox textBox)
                    textBox.IsEnabled = !textBox.IsEnabled;
            }
        }

        private void MessageContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = MessageContentList.SelectedItem as ListViewItem;
            if (item?.Content is TextBox textBox)
                textBox.Focus();
        }

        private bool ValidateLottoNewLuckyNumber(List<string> tab)
        {
            tab.ForEach(q => q = q.Replace(" ", string.Empty));
            tab = tab.Distinct().ToList();

            if (tab == null || tab.Count != 6)
                return false;

            foreach (var input in tab)
            {
                var number = -1;
                if (!int.TryParse(input, out number) || number < 0 || number > 50)
                {
                    return false;
                }
            }

            return true;
        }

        private string ConvertTab2String(List<string> tab)
        {
            var result = string.Empty;
            var index = 0;
            foreach (var value in tab)
            {
                result += index > 0 ? ", " + value : "" + value;
                index++;
            }

            return result;
        }
        
        private void LottoTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var textBox = sender as TextBox;
                if (!string.IsNullOrEmpty(textBox?.Text))
                {
                    if (textBox.Text.Contains(",") || textBox.Text.Contains(" "))
                    {
                        var spaceTab = textBox.Text.Split(' ').ToList();
                        var dotTab = textBox.Text.Split(',').ToList();
                        var spaceDetected = ValidateLottoNewLuckyNumber(spaceTab);
                        if (ValidateLottoNewLuckyNumber(dotTab) || spaceDetected)
                        {
                            if (spaceDetected)
                                dotTab = spaceTab;

                            luckyNumbersToSave.Add(ConvertTab2String(dotTab));

                            AddNewLottoTextBox();
                        }
                        else
                        {
                            throw new AitAccountExceptions.InvalidValueException(string.Format(WPF.Properties.Resources.INVALID_LOTTO_NUMBER, textBox.Text));
                        }
                    }
                    else
                    {
                        throw new AitAccountExceptions.InvalidValueException(string.Format(WPF.Properties.Resources.INVALID_LOTTO_NUMBER, textBox.Text));
                    }
                }

                if (string.IsNullOrEmpty(textBox.Text) && MessageContentList.Items.Count != 1)
                {
                    MessageContentList.Items.Remove(sender);
                }

                ValidateNotDefaultNote();
                ValidateRequiredFieldFillCorrectly();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }
        }

        private void AddNewLottoTextBox(string text = "")
        {
            var lottoTextbox = new TextBox();
            lottoTextbox.LostFocus += LottoTextbox_LostFocus;
            lottoTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            lottoTextbox.MinWidth = 150;
            lottoTextbox.MaxLines = 1;
            lottoTextbox.Background = Brushes.AliceBlue;
            
            var toAdd = true;
            if (!string.IsNullOrEmpty(text))
            {
                lottoTextbox.Text = text;
                lottoTextbox.IsEnabled = false;

                var anyFilled = false;
                foreach (ListViewItem item in MessageContentList.Items)
                {
                    var textBox = item?.Content as TextBox;
                    if (!string.IsNullOrEmpty(textBox.Text))
                        anyFilled = true;
                }

                if (!anyFilled)
                    MessageContentList.Items.Clear();
            }
            else
            {
                foreach (ListViewItem item in MessageContentList.Items)
                {
                    var textBox = item?.Content as TextBox;
                    if (string.IsNullOrEmpty(textBox.Text))
                        toAdd = false;
                }
            }

            if (toAdd)
            {
                MessageContentList.Items.Add(new ListViewItem { Content = lottoTextbox });
            }
        }

        private void ClearMessageContentListView(bool fullClear = false)
        {
            foreach (ListViewItem item in MessageContentList.Items)
            {
                if (item?.Content is TextBox textbox)
                {
                    textbox.LostFocus -= LottoTextbox_LostFocus;
                }
            }
            luckyNumbersToSave.Clear();
            MessageContentList.Items.Clear();

            if (!fullClear)
                AddNewLottoTextBox();
        }

        private void FillNoteFieldsFromNote(FileTypesEnum type, MessageInfoModel obj)
        {
            if (luckyNumbersToSave == null)
                throw new Exception();

            luckyNumbersToSave.Clear();

            if (FileTypesEnum.EXCEPTION.Equals(type))
            {
                foreach (var exception in obj.ExceptionInfo.ToList())
                    MessageContent.Text += exception;
            }
            else if (FileTypesEnum.TRACE.Equals(type))
            {
                foreach (var element in obj.Array.ToList())
                    MessageContent.Text += element;
            }
            else if (FileTypesEnum.LOTTO_NOTE.Equals(type))
            {
                MessageContent.Text = obj.Message;

                if (obj.Array != null)
                {
                    foreach (var line in obj.Array.ToList())
                    {
                        luckyNumbersToSave.Add(line);
                        AddNewLottoTextBox(line);
                    }
                }
            }
            else
            {
                MessageContent.Text = obj.Message;
            }
        }

        private void MessageContent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageContent.Text))
                IsCorrectlyFilled = true;
            else
                IsCorrectlyFilled = false;

            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();
        }
    }
}

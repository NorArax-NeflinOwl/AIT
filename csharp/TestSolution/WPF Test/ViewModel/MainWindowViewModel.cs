using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using AIT_Lib.Converters;
using AIT_Lib.Constant;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Media;
using AIT_Lib.Interfaces;
using AIT_Lib.Models;
using AIT_Lib.Helpers;

namespace WPF_Test.ViewModel
{
    public class MainWindowViewModel
    {
        private Calendar m_aitCalendar;
        private TextBox m_aitTimeTextBox;
        private ComboBox m_aitNoteTypeComboBox;
        private ComboBox m_aitDateTypeComboBox;
        private TextBox m_aitTitleTextBox;
        private TextBox m_aitNoteTextBox;

        private ComboBox m_aitTrainigTypeComboBox;
        private CheckBox m_aitTimeOfDayCheckBox;
        private ComboBox m_aitTimeOfDayComboBox;
        private CheckBox m_aitSeriesCheckBox;
        private TextBox m_aitSeriesTextBox;
        private CheckBox m_aitRepeatChechBox;
        private TextBox m_aitRepeatTextBox;

        private TreeView m_aitLeftPanelTreeView;
        private Button m_aitAddButton;
        private Button m_aitRemoveButton;

        private Button m_aitCancelButton;
        private Button m_aitInsertButton;

        private ListView m_aitRightPanelListView;
        private Button m_aitClearButton;
        private Button m_aitSaveButton;

        private CheckBox m_aitEditCheckBox;
        private Button m_aitEditButton;
        private Button m_aitDeleteButton;

        private Dictionary<string, Tuple<Control, bool>> m_ControlList;
        private Dictionary<DateTime, IList<object>> m_aitCalendarLists; // TODO singleton

        public MainWindowViewModel(Calendar aitCalendar, TextBox aitTimeTextBox, 
            ComboBox aitNoteTypeComboBox, ComboBox aitDateTypeComboBox, TextBox aitTitleTextBox, TextBox aitNoteTextBox, 
            ComboBox aitTrainigTypeComboBox, CheckBox aitTimeOfDayCheckBox, ComboBox aitTimeOfDayComboBox, 
            CheckBox aitSeriesCheckBox, TextBox aitSeriesTextBox, CheckBox aitRepeatChechBox, TextBox aitRepeatTextBox,
            TreeView aitLeftPanelTreeView, Button aitAddButton, Button aitRemoveButton, 
            Button aitCancelButton, Button aitInsertButton, 
            ListView aitRightPanelListView, Button aitClearButton, Button aitSaveButton,
            CheckBox aitEditCheckBox, Button aitEditButton, Button aitDeleteButton)
        {
            m_aitCalendar = aitCalendar;
            m_aitTimeTextBox = aitTimeTextBox;
            m_aitNoteTypeComboBox = aitNoteTypeComboBox;
            m_aitDateTypeComboBox = aitDateTypeComboBox;
            m_aitTitleTextBox = aitTitleTextBox;
            m_aitNoteTextBox = aitNoteTextBox;

            m_aitTrainigTypeComboBox = aitTrainigTypeComboBox;
            m_aitTimeOfDayCheckBox = aitTimeOfDayCheckBox;
            m_aitTimeOfDayComboBox = aitTimeOfDayComboBox;
            m_aitSeriesCheckBox = aitSeriesCheckBox;
            m_aitSeriesTextBox = aitSeriesTextBox;
            m_aitRepeatChechBox = aitRepeatChechBox;
            m_aitRepeatTextBox = aitRepeatTextBox;

            m_aitLeftPanelTreeView = aitLeftPanelTreeView;
            m_aitAddButton = aitAddButton;
            m_aitRemoveButton = aitRemoveButton;

            m_aitCancelButton = aitCancelButton;
            m_aitInsertButton = aitInsertButton;

            m_aitRightPanelListView = aitRightPanelListView;
            m_aitClearButton = aitClearButton;
            m_aitSaveButton = aitSaveButton;

            m_aitEditCheckBox = aitEditCheckBox;
            m_aitEditButton = aitEditButton;
            m_aitDeleteButton = aitDeleteButton;

            InitDicCtr();
        }

        public void Init()
        {
            ComboBoxItem comboItem = null;

            comboItem = new ComboBoxItem();
            comboItem.Content = aitType2StringConverter.NoteType2StringConverter(aitNotesTypesModel.SIMPLE_NOTE_MODEL);
            comboItem.IsSelected = true;
            m_aitNoteTypeComboBox.Items.Add(comboItem);
            m_aitNoteTypeComboBox.Items.Add(aitType2StringConverter.NoteType2StringConverter(aitNotesTypesModel.TREE_VIEW_MODEL));
            m_aitNoteTypeComboBox.Items.Add(aitType2StringConverter.NoteType2StringConverter(aitNotesTypesModel.TRAINING_MODEL));

            comboItem = new ComboBoxItem();
            comboItem.Content = aitType2StringConverter.DateType2StringConverter(aitDateTimeType.DATE);
            comboItem.IsSelected = true;
            m_aitDateTypeComboBox.Items.Add(comboItem);
            m_aitDateTypeComboBox.Items.Add(aitType2StringConverter.DateType2StringConverter(aitDateTimeType.DATE_TIME));
            m_aitDateTypeComboBox.Items.Add(aitType2StringConverter.DateType2StringConverter(aitDateTimeType.TIME_NO_DATE));
            m_aitDateTypeComboBox.Items.Add(aitType2StringConverter.DateType2StringConverter(aitDateTimeType.NO_DATE));

            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.PUMP));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.SQUATS));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.DUMBBELL));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.BENCH));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.BIKE));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.RUN));
            m_aitTrainigTypeComboBox.Items.Add(aitType2StringConverter.TrainingType2StringConverter(aitTrainingTypes.WALK));

            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MWU));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MMT));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MBB));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MAB));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MBL));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.MAL));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.ABD));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.AAD));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.NBB));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.NAB));
            m_aitTimeOfDayComboBox.Items.Add(aitType2StringConverter.TimeOfDay2StringConverter(aitTimeOfDay.NBS));

            // simple note
            m_aitCalendar.SelectedDatesChanged += aitCalendar_SelectedDatesChanged;
            m_aitNoteTypeComboBox.SelectionChanged += aitNoteTypeComboBox_SelectionChanged;
            m_aitDateTypeComboBox.SelectionChanged += aitDateTypeComboBox_SelectionChanged;

            // training note
            m_aitTimeOfDayCheckBox.Checked += aitTimeOfDayCheckBox_Checked;
            m_aitTimeOfDayCheckBox.Unchecked += aitTimeOfDayCheckBox_Unchecked;
            m_aitSeriesCheckBox.Checked += aitSeriesCheckBox_Checked;
            m_aitSeriesCheckBox.Unchecked += aitSeriesCheckBox_Unchecked;
            m_aitRepeatChechBox.Checked += aitRepeatChechBox_Checked;
            m_aitRepeatChechBox.Unchecked += aitRepeatChechBox_Unchecked;

            // tree note
            m_aitAddButton.Click += aitAddButton_Click;
            m_aitRemoveButton.Click += aitRemoveButton_Click;

            // insert list
            m_aitCancelButton.Click += aitCancelButton_Click;
            m_aitInsertButton.Click += aitInsertButton_Click;

            // save list
            m_aitRightPanelListView.SelectionChanged += aitRightPanelListView_SelectionChanged;
            m_aitRightPanelListView.MouseDoubleClick += aitRightPanelListView_MouseDoubleClick;
            m_aitClearButton.Click += aitClearButton_Click;
            m_aitSaveButton.Click += aitSaveButton_Click;
            
            // edit list
            m_aitEditCheckBox.Checked += aitEditCheckBox_Checked;
            m_aitEditCheckBox.Unchecked += aitEditCheckBox_Unchecked;
            m_aitEditButton.Click += aitEditButton_Click;

        }

        private void InitDicCtr()
        {
            m_ControlList = new Dictionary<string, Tuple<Control, bool>>();
            var tuple = new Tuple<Control, bool>(m_aitTimeTextBox, true);
            m_ControlList.Add("m_aitTimeTextBox", tuple);
            tuple = new Tuple<Control, bool>(m_aitTitleTextBox, true);
            m_ControlList.Add("m_aitTitleTextBox", tuple);
            tuple = new Tuple<Control, bool>(m_aitNoteTextBox, true);
            m_ControlList.Add("m_aitNoteTextBox", tuple);
            tuple = new Tuple<Control, bool>(m_aitSeriesTextBox, true);
            m_ControlList.Add("m_aitSeriesTextBox", tuple);
            tuple = new Tuple<Control, bool>(m_aitRepeatTextBox, true);
            m_ControlList.Add("m_aitRepeatTextBox", tuple);
            tuple = new Tuple<Control, bool>(m_aitCalendar, true);
            m_ControlList.Add("m_aitCalendar", tuple);

            m_aitCalendarLists = new Dictionary<DateTime, IList<object>>();

            var datetime = DateTime.Now.Date.AddMonths(-3);
            while (true)
            {
                if (datetime.Date > DateTime.Now.Date.AddMonths(3)) return;

                m_aitCalendarLists.Add(datetime, new List<object>());
                datetime = datetime.AddDays(1);
            }
        }
        
        private void aitCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = m_aitCalendar.SelectedDate;
            if (selectedDate != null)
            {
                if (CheckSelectionDate(selectedDate))
                {
                    // TODO Show error information dialog
                    var msg = "Selected date error " + selectedDate.Value.ToString("yyyy-MM-dd");
                    Debug.WriteLine(msg);
                    aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = msg });

                    m_aitCalendar.SelectedDate = m_aitCalendar.DisplayDate = DateTime.Now;
                }
                else
                {
                    m_aitRightPanelListView.Items.Clear();
                    foreach (aitDataBaseInterface item in m_aitCalendarLists[selectedDate.Value.Date])
                        m_aitRightPanelListView.Items.Add(item.Clone());
                }
            }
        }


        // todo...
        private void aitRightPanelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (m_aitRightPanelListView.SelectedItem != null)
            {
                EnableAllCtr();
                m_aitEditCheckBox.IsEnabled = false;
                m_aitEditCheckBox.IsChecked = false;
                m_aitEditButton.IsEnabled = false;
                aitCancelButton_Click(null, null);
                m_aitRightPanelListView.SelectedItem = null;
            }
        }

        private void aitRightPanelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = m_aitRightPanelListView.SelectedItem as aitSimpleNoteModel;
            if (selectedItem != null)
            {
                m_aitTitleTextBox.Text = selectedItem.Title;
                m_aitNoteTextBox.Text = selectedItem.Note;
                if (m_aitTimeTextBox.IsEnabled)
                    m_aitTimeTextBox.Text = selectedItem.Date.Value.TimeOfDay.ToString().Substring(0, 5);
                else
                    m_aitTimeTextBox.Clear();
                m_aitNoteTypeComboBox.SelectedIndex = (int)selectedItem.NoteType;
                m_aitDateTypeComboBox.SelectedIndex = (int)selectedItem.DateTimeType;

                switch (selectedItem.NoteType)
                {
                    default:
                    case aitNotesTypesModel.SIMPLE_NOTE_MODEL:
                        m_aitTrainigTypeComboBox.SelectedIndex = -1;
                        m_aitTimeOfDayCheckBox.IsChecked = false;
                        m_aitTimeOfDayComboBox.SelectedIndex = -1;
                        m_aitSeriesCheckBox.IsChecked = false;
                        m_aitSeriesTextBox.Clear();
                        m_aitRepeatChechBox.IsChecked = false;
                        m_aitRepeatTextBox.Clear();
                        m_aitLeftPanelTreeView.Items.Clear();
                        aitNoteTypeComboBox_SelectionChanged(null, null);
                        aitDateTypeComboBox_SelectionChanged(null, null);
                        aitTimeOfDayCheckBox_Unchecked(null, null);
                        aitSeriesCheckBox_Unchecked(null, null);
                        aitRepeatChechBox_Unchecked(null, null);
                        break;
                    case aitNotesTypesModel.TRAINING_MODEL:
                        var trainingItem = selectedItem as aitTrainingModel;
                        if (trainingItem != null)
                        {
                            m_aitTrainigTypeComboBox.SelectedIndex = (int)trainingItem.TrainingType - 1;
                            m_aitTimeOfDayComboBox.SelectedIndex = (int)trainingItem.TimeOfDay - 1;
                            m_aitSeriesTextBox.Text = trainingItem.Series.ToString();
                            m_aitRepeatTextBox.Text = trainingItem.Repeat.ToString();
                        }
                        break;
                }
                DisableAllCtr();
                m_aitEditCheckBox.IsEnabled = true;
                m_aitEditButton.IsEnabled = false;
            }
        }

        private void aitAddButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void aitRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void aitCancelButton_Click(object sender, RoutedEventArgs e)
        {
            m_aitCalendar.SelectedDate = DateTime.Now;
            m_aitNoteTypeComboBox.SelectedIndex = 0;
            m_aitDateTypeComboBox.SelectedIndex = 0;
            m_aitTrainigTypeComboBox.SelectedIndex = -1;
            m_aitTimeOfDayCheckBox.IsChecked = false;
            m_aitTimeOfDayComboBox.SelectedIndex = -1;
            m_aitSeriesCheckBox.IsChecked = false;
            m_aitSeriesTextBox.Clear();
            m_aitRepeatChechBox.IsChecked = false;
            m_aitRepeatTextBox.Clear();
            m_aitTimeTextBox.Clear();
            m_aitTitleTextBox.Clear();
            m_aitNoteTextBox.Clear();
            m_aitLeftPanelTreeView.Items.Clear();
            aitNoteTypeComboBox_SelectionChanged(null, null);
            aitDateTypeComboBox_SelectionChanged(null, null);
            aitTimeOfDayCheckBox_Unchecked(null, null);
            aitSeriesCheckBox_Unchecked(null, null);
            aitRepeatChechBox_Unchecked(null, null);
            m_aitEditCheckBox.IsEnabled = false;
            m_aitEditButton.IsEnabled = false;
        }

        private void aitInsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateValues())
            {
                DateTime selD;
                if (m_aitCalendar.SelectedDate.HasValue)
                {
                    // mark singles days
                    selD = m_aitCalendar.SelectedDate.Value;
                }
                else
                {
                    // mark range of days
                    selD = DateTime.Now;
                }

                // Set note prop
                DateTime? date = null;
                aitDateTimeType dateType;
                switch(m_aitDateTypeComboBox.SelectedIndex)
                {
                    default:
                    case 0:
                        date = selD;
                        dateType = aitDateTimeType.DATE;
                        break;
                    case 1:
                        var time = DateTime.Parse(m_aitTimeTextBox.Text);
                        date = new DateTime(selD.Year, selD.Month, selD.Day, time.Hour, time.Minute, 0);
                        dateType = aitDateTimeType.DATE_TIME;
                        break;
                    case 2:
                        date = DateTime.Parse(m_aitTimeTextBox.Text);
                        dateType = aitDateTimeType.TIME_NO_DATE;
                        break;
                    case 3:
                        dateType = aitDateTimeType.NO_DATE;
                        break;
                }

                aitTrainingTypes trainingType = aitTrainingTypes.UNSPECIFIED;
                if(m_aitTrainigTypeComboBox.SelectedIndex != -1)
                    trainingType = aitNumber2TypeCoverter.Number2TrainingTypeConverter(m_aitTrainigTypeComboBox.SelectedIndex);

                aitTimeOfDay timeOfDay = aitTimeOfDay.UNF;
                if(m_aitTimeOfDayComboBox.IsEnabled)
                    timeOfDay = aitNumber2TypeCoverter.Number2TimeOfDayConverter(m_aitTimeOfDayComboBox.SelectedIndex);

                var series = m_aitSeriesTextBox.IsEnabled ? UInt32.Parse(m_aitSeriesTextBox.Text) : 0;
                var repeat = m_aitRepeatTextBox.IsEnabled ? UInt32.Parse(m_aitRepeatTextBox.Text) : 0;

                // Create note instance
                aitDataBaseInterface note;
                switch (m_aitNoteTypeComboBox.SelectedIndex)
                {
                    default:
                    case 0:
                        note = new aitSimpleNoteModel(m_aitTitleTextBox.Text, m_aitNoteTextBox.Text, date, dateType);
                        break;
                    case 1:
                        note = new aitNodeNoteModel(m_aitTitleTextBox.Text, m_aitNoteTextBox.Text, date, dateType); // TODO Tworzenie drzewa a nie wezle wraz z caly lewym treeview
                        break;
                    case 2:
                        note = new aitTrainingModel(m_aitTitleTextBox.Text, m_aitNoteTextBox.Text, date, series, repeat, trainingType, timeOfDay, dateType);
                        break;
                }

                // Insert note
                m_aitRightPanelListView.Items.Add(note);

                // Insertd list to calendatList
                var key = m_aitCalendar.SelectedDate.HasValue ? m_aitCalendar.SelectedDate.Value.Date : DateTime.Now.Date;
                m_aitCalendarLists[key].Clear();
                foreach(ICloneable item in m_aitRightPanelListView.Items)
                {
                    var clone = item.Clone();

                    m_aitCalendarLists[key].Add(item.Clone());
                }
                aitCancelButton_Click(null, null);
            }
        }

        private void aitClearButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Question dialog 
            if (m_aitCalendar.SelectedDate.HasValue)
                m_aitCalendarLists[m_aitCalendar.SelectedDate.Value.Date].Clear();
            else
                m_aitCalendarLists[DateTime.Now.Date].Clear();

            m_aitRightPanelListView.Items.Clear();
            aitCancelButton_Click(null, null);
            EnableAllCtr();
        }

        private void aitSaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Save m_calendarList to db
            System.Windows.Application.Current.Shutdown();
        }

        private void aitEditButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void aitNoteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = m_aitNoteTypeComboBox.SelectedItem.ToString();
            switch (selected)
            {
                default:
                case aitStrings.SIMPLE_NOTE:
                    m_aitTrainigTypeComboBox.IsEnabled = false;
                    m_aitTimeOfDayCheckBox.IsEnabled = false;
                    m_aitTimeOfDayComboBox.IsEnabled = false;
                    m_aitSeriesCheckBox.IsEnabled = false;
                    m_aitSeriesTextBox.IsEnabled = false;
                    m_aitRepeatChechBox.IsEnabled = false;
                    m_aitRepeatTextBox.IsEnabled = false;

                    m_aitLeftPanelTreeView.IsEnabled = false;
                    m_aitAddButton.IsEnabled = false;
                    m_aitRemoveButton.IsEnabled = false;
                    return;
                case aitStrings.TREE_VIEW:
                    m_aitTrainigTypeComboBox.IsEnabled = false;
                    m_aitTimeOfDayCheckBox.IsEnabled = false;
                    m_aitTimeOfDayComboBox.IsEnabled = false;
                    m_aitSeriesCheckBox.IsEnabled = false;
                    m_aitSeriesTextBox.IsEnabled = false;
                    m_aitRepeatChechBox.IsEnabled = false;
                    m_aitRepeatTextBox.IsEnabled = false;

                    m_aitLeftPanelTreeView.IsEnabled = true;
                    m_aitAddButton.IsEnabled = true;
                    m_aitRemoveButton.IsEnabled = true;
                    return;
                case aitStrings.TRAINING_NOTE:
                    m_aitTrainigTypeComboBox.IsEnabled = true;
                    m_aitTimeOfDayCheckBox.IsEnabled = true;
                    m_aitTimeOfDayComboBox.IsEnabled = m_aitTimeOfDayCheckBox.IsChecked == true;
                    m_aitSeriesCheckBox.IsEnabled = true;
                    m_aitSeriesTextBox.IsEnabled = m_aitSeriesCheckBox.IsChecked == true;;
                    m_aitRepeatChechBox.IsEnabled = true;
                    m_aitRepeatTextBox.IsEnabled = m_aitRepeatChechBox.IsChecked == true;

                    m_aitLeftPanelTreeView.IsEnabled = false;
                    m_aitAddButton.IsEnabled = false;
                    m_aitRemoveButton.IsEnabled = false;
                    return;
            }
        }

        private void aitDateTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = m_aitDateTypeComboBox.SelectedItem.ToString();
            switch (selected)
            {
                default:
                case aitStrings.DATE:
                    m_aitCalendar.IsEnabled = true;
                    m_aitTimeTextBox.IsEnabled = false;
                    return;
                case aitStrings.DATE_TIME:
                    m_aitCalendar.IsEnabled = true;
                    m_aitTimeTextBox.IsEnabled = true;
                    return;
                case aitStrings.TIME_NO_DATE:
                    m_aitCalendar.IsEnabled = false;
                    m_aitTimeTextBox.IsEnabled = true;
                    return;
                case aitStrings.NO_DATE:
                    m_aitCalendar.IsEnabled = false;
                    m_aitTimeTextBox.IsEnabled = false;
                    return;
            }
        }

        private void aitTimeOfDayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            m_aitTimeOfDayComboBox.IsEnabled = true;
        }

        private void aitTimeOfDayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            m_aitTimeOfDayComboBox.IsEnabled = false;
        }

        private void aitSeriesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            m_aitSeriesTextBox.IsEnabled = true;
        }

        private void aitSeriesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            m_aitSeriesTextBox.IsEnabled = false;
        }

        private void aitRepeatChechBox_Checked(object sender, RoutedEventArgs e)
        {
            m_aitRepeatTextBox.IsEnabled = true;
        }

        private void aitRepeatChechBox_Unchecked(object sender, RoutedEventArgs e)
        {
            m_aitRepeatTextBox.IsEnabled = false;
        }

        private void aitEditCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            m_aitEditButton.IsEnabled = true;

            m_aitTitleTextBox.IsEnabled = true;
            m_aitNoteTextBox.IsEnabled = true;
            m_aitNoteTypeComboBox.IsEnabled = true;
            m_aitDateTypeComboBox.IsEnabled = true;

            if ((m_aitRightPanelListView.SelectedItem as aitSimpleNoteModel).NoteType == aitNotesTypesModel.TRAINING_MODEL)
            {
                m_aitTrainigTypeComboBox.IsEnabled = true;
                m_aitTimeOfDayCheckBox.IsEnabled = true;
                m_aitSeriesCheckBox.IsEnabled = true;
                m_aitRepeatChechBox.IsEnabled = true;
            }
            else
            {
                m_aitTrainigTypeComboBox.IsEnabled = false;
                m_aitTimeOfDayCheckBox.IsEnabled = false;
                m_aitSeriesCheckBox.IsEnabled = false;
                m_aitRepeatChechBox.IsEnabled = false;
            }

            if ((m_aitRightPanelListView.SelectedItem as aitSimpleNoteModel).NoteType == aitNotesTypesModel.TREE_VIEW_MODEL)
            {
                m_aitLeftPanelTreeView.IsEnabled = true;
                m_aitAddButton.IsEnabled = true;
                m_aitRemoveButton.IsEnabled = true;
            }
            else
            {
                m_aitLeftPanelTreeView.IsEnabled = false;
                m_aitAddButton.IsEnabled = false;
                m_aitRemoveButton.IsEnabled = false;
            }
        }

        private void aitEditCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            m_aitEditButton.IsEnabled = false;
            DisableAllCtr();
        }

        private bool ValidateValues()
        {
            int number;
            DateTime time;
            if (!DateTime.TryParse(m_aitTimeTextBox.Text, out time) && m_aitTimeTextBox.IsEnabled && !string.IsNullOrEmpty(m_aitTimeTextBox.Text)) 
                m_ControlList["m_aitTimeTextBox"] = new Tuple<Control,bool>(m_aitTimeTextBox, false);
            else m_ControlList["m_aitTimeTextBox"] = new Tuple<Control, bool>(m_aitTimeTextBox, true);

            if (string.IsNullOrEmpty(m_aitTitleTextBox.Text)) 
                m_ControlList["m_aitTitleTextBox"] = new Tuple<Control, bool>(m_aitTitleTextBox, false);
            else m_ControlList["m_aitTitleTextBox"] = new Tuple<Control, bool>(m_aitTitleTextBox, true);

            if (string.IsNullOrEmpty(m_aitNoteTextBox.Text)) 
                m_ControlList["m_aitNoteTextBox"] = new Tuple<Control, bool>(m_aitNoteTextBox, false);
            else m_ControlList["m_aitNoteTextBox"] = new Tuple<Control, bool>(m_aitNoteTextBox, true);

            if (!Int32.TryParse(m_aitSeriesTextBox.Text, out number) && m_aitSeriesTextBox.IsEnabled && !string.IsNullOrEmpty(m_aitSeriesTextBox.Text)) 
                m_ControlList["m_aitSeriesTextBox"] = new Tuple<Control, bool>(m_aitSeriesTextBox, false);
            else m_ControlList["m_aitSeriesTextBox"] = new Tuple<Control, bool>(m_aitSeriesTextBox, true);

            if (!Int32.TryParse(m_aitRepeatTextBox.Text, out number) && m_aitRepeatTextBox.IsEnabled && !string.IsNullOrEmpty(m_aitRepeatTextBox.Text)) 
                m_ControlList["m_aitRepeatTextBox"] = new Tuple<Control, bool>(m_aitRepeatTextBox, false);
            else m_ControlList["m_aitRepeatTextBox"] = new Tuple<Control, bool>(m_aitRepeatTextBox, true);

            if(m_aitCalendar.SelectedDate.HasValue && m_aitCalendar.SelectedDate.Value.Date < DateTime.Now.Date && m_aitCalendar.IsEnabled)
                m_ControlList["m_aitCalendar"] = new Tuple<Control, bool>(m_aitCalendar, false);
            else m_ControlList["m_aitCalendar"] = new Tuple<Control, bool>(m_aitCalendar, true);

            if (m_ControlList.Any(q => !q.Value.Item2))
            {
                // Set error bg
                foreach (var control in m_ControlList.Where(q => !q.Value.Item2))
                    control.Value.Item1.Background = Brushes.Red;

                // TODO Show error information dialog
                return false;
            }

            // Set default bg
            foreach (var control in m_ControlList)
                control.Value.Item1.Background = Brushes.White;

            return true;
        }

        private bool CheckSelectionDate(DateTime? selectionDate)
        {
            var futureChecker = selectionDate.Value.Date <= DateTime.Now.Date.AddMonths(3);
            return !futureChecker;
        }

        private void EnableAllCtr()
        {
            m_aitNoteTypeComboBox.SelectedIndex = 0;
            m_aitDateTypeComboBox.SelectedIndex = 0;

            m_aitTitleTextBox.IsEnabled = true;
            m_aitNoteTextBox.IsEnabled = true;
            m_aitInsertButton.IsEnabled = true;
            m_aitCancelButton.IsEnabled = true;
            m_aitNoteTypeComboBox.IsEnabled = true;
            m_aitDateTypeComboBox.IsEnabled = true;

            m_aitTrainigTypeComboBox.IsEnabled = false;
            m_aitTimeOfDayCheckBox.IsEnabled = false;
            m_aitTimeOfDayComboBox.IsEnabled = false;
            m_aitSeriesCheckBox.IsEnabled = false;
            m_aitSeriesTextBox.IsEnabled = false;
            m_aitRepeatChechBox.IsEnabled = false;
            m_aitRepeatTextBox.IsEnabled = false;

            m_aitLeftPanelTreeView.IsEnabled = false;
            m_aitAddButton.IsEnabled = false;
            m_aitRemoveButton.IsEnabled = false;
        }

        private void DisableAllCtr()
        {
            m_aitTitleTextBox.IsEnabled = false;
            m_aitNoteTextBox.IsEnabled = false;
            m_aitInsertButton.IsEnabled = false;
            m_aitCancelButton.IsEnabled = false;
            m_aitNoteTypeComboBox.IsEnabled = false;
            m_aitDateTypeComboBox.IsEnabled = false;

            m_aitTrainigTypeComboBox.IsEnabled = false;
            m_aitTimeOfDayCheckBox.IsEnabled = false;
            m_aitTimeOfDayComboBox.IsEnabled = false;
            m_aitSeriesCheckBox.IsEnabled = false;
            m_aitSeriesTextBox.IsEnabled = false;
            m_aitRepeatChechBox.IsEnabled = false;
            m_aitRepeatTextBox.IsEnabled = false;

            m_aitLeftPanelTreeView.IsEnabled = false;
            m_aitAddButton.IsEnabled = false;
            m_aitRemoveButton.IsEnabled = false;
        }
        
        private TreeViewItem Find(TreeViewItem item, string key)
        {
            if (item == null)
                return null;

            if (item.Header.Equals(key))
                return item;

            foreach (TreeViewItem it in item.Items)
            {
                var found = Find(it, key);
                if (found != null)
                    return found;
            }

            return null;
        }

        private void ExpandedTree(TreeViewItem root, TreeViewItem item)
        {
            if (item == null)
                return;

            if (!item.IsExpanded)
                item.IsExpanded = true;

            if (item.Equals(root) && item.Parent == null || root.Equals(item.Parent))
                return;

            ExpandedTree(root, item.Parent as TreeViewItem);
        }

        /*
        private void aitTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return))
                aitAddButton_Click(sender, null);
        }
        private void aitAddButton_Click(object sender, RoutedEventArgs e)
        {
            var newItem = new TreeViewItem();
            newItem.Header = m_aitTextBox.Text;

            var selectedItem = m_aitTreeView.SelectedValue as TreeViewItem;
            if (selectedItem != null)
                selectedItem.Items.Add(newItem);
            else
                m_aitTreeView.Items.Add(newItem);

            m_aitTextBox.Focus();
            m_aitTextBox.Text = string.Empty;
            if (selectedItem != null)
                ExpandedTree(selectedItem, newItem);
        }

        private void aitRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = m_aitTreeView.SelectedValue as TreeViewItem;
            if (selectedItem != null)
            {
                var parent = selectedItem.Parent as TreeViewItem;
                if (parent != null)
                    parent.Items.Remove(selectedItem);
                else
                    m_aitTreeView.Items.Remove(selectedItem);
            }
        }

        private void aitFindButton_Clik(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = null;
            if (m_aitTreeView.HasItems)
            {
                if (m_aitTreeView.Items.Count > 1)
                {
                    foreach (TreeViewItem it in m_aitTreeView.Items)
                    {
                        if (it.Header.Equals(m_aitTextBox.Text))
                            item = it;
                        else if (it.HasItems)
                            item = Find(it, m_aitTextBox.Text);
                    }
                }
                else
                    item = Find(m_aitTreeView.Items[0] as TreeViewItem, m_aitTextBox.Text);
            }

            if (item != null && item.Header.Equals(m_aitTextBox.Text) && !item.IsSelected)
            {
                m_aitTextBox.Focus();
                m_aitTextBox.Text = string.Empty;
                item.IsSelected = true;

                var root = m_aitTreeView.Items[0] as TreeViewItem;
                if (root.Equals(item))
                    ExpandedTree(root, item);
            }
        }*/
    }
}

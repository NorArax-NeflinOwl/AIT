using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Test.ViewModel;

namespace WPF_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel m_ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            m_ViewModel = new MainWindowViewModel(aitCalendar, aitTimeTextBox, aitNoteTypeComboBox, aitDateTypeComboBox, aitTitleTextBox, aitNoteTextBox, // simple note
            aitTrainigTypeComboBox, aitTimeOfDayCheckBox, aitTimeOfDayComboBox, aitSeriesCheckBox, aitSeriesTextBox, aitRepeatChechBox, aitRepeatTextBox, // training note
            aitLeftPanelTreeView, aitAddButton, aitRemoveButton,                                                                                          // tree note
            aitCancelButton, aitInsertButton,                                                                                                             // insert to list
            aitRightPanelListView, aitClearButton, aitSaveButton,                                                                                         // list save
            aitEditCheckBox, aitEditButton, aitDeleteButton);                                                                                             // editing list
            m_ViewModel.Init();
        }
    }
}

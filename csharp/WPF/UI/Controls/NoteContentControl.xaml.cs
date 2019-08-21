using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.Managers;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.UI.Controls
{
    /// <summary>
    /// Interaction logic for NoteContentControl.xaml
    /// </summary>
    public partial class NoteContentControl : UserControl, IDisposableExtended, Initializable
    {
        private FileTypesEnum? type;
        private BackgroundWorker backgroundWorker;

        public bool IsNotDefault { get; set; }
        public bool IsCorrectFilled { get; set; }
        public bool IsDisposed { get; set; }

        public NoteContentControl()
        {
            InitializeComponent();
            Init();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
        }

        public NoteContentControl(int selectedIndex = -1)
        {
            InitializeComponent();
            Init();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();

            type = FileTypesManager.SetType(selectedIndex);
        }

        public NoteContentControl(string content, FileTypesEnum type)
        {
            InitializeComponent();
            Init();
            this.type = type;
            try
            {
                var obj = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(content, null, false);
                if (obj != null)
                {
                    Date.Text = obj.Date.ToString();
                    MessageContent.Text = obj.Message.ToString();
                }
            }
            catch(Exception)
            {
                // FIX ME
                Date.Text = Properties.Resources.UNKNOWN;
                MessageContent.Text = content;
            }
        }

        public string SerializableControl()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            MessageTitle.Text = Properties.Resources.MESSAGE;
            DateTitle.Text = Properties.Resources.DATE_S;
        }

        private void StartTimeTicker(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.DoWork -= StartTimeTicker;
            Dispatcher.Invoke(async () =>
            {
                while(true)
                {
                    Date.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    await Task.Delay(1000);
                }
            });
        }
    }
}

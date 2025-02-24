using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppSearch.MVC.Models
{
    public class SimpleNodeModel : INotifyPropertyChanged
    {
        public SimpleNodeModel? Parent { get; private set; }
        public string NodeName { get; private set; }
        public string? TargetPath { get; private set; }
        public ObservableCollection<SimpleNodeModel>? Childs { get; private set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public SimpleNodeModel(SimpleNodeModel? parent, string nodeName,
            string? targetPath = null)
        {
            Parent = parent;
            NodeName = nodeName;
            TargetPath = targetPath;

            Childs = [];
        }

        public SimpleNodeModel(SimpleNodeModel? parent, string nodeName,
            ObservableCollection<SimpleNodeModel>? childs)
        {
            Parent = parent;
            NodeName = nodeName;
            TargetPath = null;

            if (childs?.Any() == true)
            {
                Childs = childs;
            }
            else
            {
                Childs = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using WPF.Managers;

namespace WPF.Models
{
    public class FileResultModel
    {
        private static readonly int DefaultLineNumber = -1;

        public int LineNumber { get; } = DefaultLineNumber;
        public bool FindInName { get => LineNumber == DefaultLineNumber; }
        public string LineContent { get; } = string.Empty;
        public string FilePath { get; }

        public FileResultModel(string filePath)
        {
            FilePath = filePath;
        }

        public FileResultModel(string filePath, int lineNumber, string lineContent)
        {
            FilePath = filePath;
            LineNumber = lineNumber;
            LineContent = lineContent;
        }

        public override string ToString()
        {
            return CryptoJsonManager.Instance.Serialize(this);
        }
    }
}

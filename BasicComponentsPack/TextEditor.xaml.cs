using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using VisualCore;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// </summary>
    public partial class TextEditor : UserControl, ISaveable, ITabContent
    {
        private string _originalFileName;
        public TextEditor()
        {
            InitializeComponent();
        }

        public bool IsChange { get; private set; }

        public void Save()
        {
            if (IsChange)
            {
                var textRange = new TextRange(
                        ContentEditor.Document.ContentStart,
                        ContentEditor.Document.ContentEnd
                    );
                File.WriteAllText(_originalFileName, textRange.Text);
                IsChange = false;
            }
        }

        public void SetContent(FileInfo file)
        {
            ContentEditor.Document.Blocks.Clear();
            ContentEditor.AppendText(File.ReadAllText(file.FullName));
            IsChange = false;
            _originalFileName = file.FullName;
        }

        private void ChangeContent(object sender, TextChangedEventArgs e)
        {
            IsChange = true;
        }

        public void ReLoad()
        {
            var file = new FileInfo(_originalFileName);
            SetContent(file);
        }
    }
}

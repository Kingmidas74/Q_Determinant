using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;

namespace ModernControls
{
    /// <summary>
    /// Interaction logic for BasicTextEditor.xaml
    /// </summary>
    public partial class BasicTextEditor : UserControl, IEditable
    {
        public bool IsChange {get ; private set; }
        private string _filePath;
        public BasicTextEditor()
        {
            InitializeComponent();
        }

        public void SetText(string filePath)
        {
            _filePath = filePath;
            EditableText.AppendText(File.ReadAllText(filePath, Encoding.UTF8));
            SetIsChange(false);

        }

        private void SetIsChange(bool status)
        {
            IsChange = status;
        }

        private void OnChanged(object sender, TextChangedEventArgs e)
        {
            SetIsChange(true);
        }

        public void SaveFile()
        {
            if (IsChange)
            {
                var tempFilePath = (new StringBuilder(_filePath)).Append(@".tmp").ToString();
                var textRange = new TextRange(
                    // TextPointer to the start of content in the RichTextBox.
                        EditableText.Document.ContentStart,
                    // TextPointer to the end of content in the RichTextBox.
                        EditableText.Document.ContentEnd
                    );
                try
                {
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(textRange.Text);
                    xDoc.Save(tempFilePath);

                }
                catch
                {
                    File.WriteAllText(tempFilePath, textRange.Text);
                }
                finally
                {
                    File.Delete(_filePath);
                    File.Copy(tempFilePath, _filePath);
                    SetIsChange(false);
                }
            }
        }


    }
}

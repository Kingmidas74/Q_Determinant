using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            EditableText.Text = File.ReadAllText(filePath, Encoding.UTF8);
            SetIsChange(false);
        }

        private void SetIsChange(bool status)
        {
            IsChange = status;
            //MessageBox.Show(((this as UserControl).Parent as object).ToString());
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
                try
                {
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(EditableText.Text);
                    xDoc.Save(tempFilePath);

                }
                catch
                {
                    File.WriteAllText(tempFilePath, EditableText.Text);
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

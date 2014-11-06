using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    public class TextEditor : TextBox
    {
        private string _filePath;
        private string _tempFilePath;
        public bool IsChange { get; private set; }

        public TextEditor()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(TextEditor), new FrameworkPropertyMetadata(typeof(TextEditor)));
            DefaultStyleKey = typeof (TextEditor);
        }


        public void SetFile(string FilePath)
        {
            IsChange = false;
            _filePath = FilePath;
            Text = File.ReadAllText(FilePath);
            MessageBox.Show(Text);
        }
        
    }
}

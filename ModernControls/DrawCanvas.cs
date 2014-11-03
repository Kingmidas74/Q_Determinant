using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Core;

namespace ModernControls
{
    public class DrawCanvas : Canvas
    {
      //  public BlockTypes CurrentBlockType { get; set; }
        
        static DrawCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawCanvas), new FrameworkPropertyMetadata(typeof(DrawCanvas)));
        }

        public override void OnApplyTemplate()
        {
       //     MessageBox.Show(CurrentBlockType.ToString());
            base.OnApplyTemplate();
        }
        
    }
}

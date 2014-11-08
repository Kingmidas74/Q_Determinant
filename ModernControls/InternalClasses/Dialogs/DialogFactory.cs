using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ModernControls.InternalClasses.Dialogs
{
    public class DialogFactory
    {
        private static OpenFileDialog _openDialog;
        private static SaveFileDialog _saveDialog;
        public static OpenFileDialog CallOpenDialog(DialogTypes type)
        {
            _openDialog = new OpenFileDialog();
            switch (type)
            {
                case DialogTypes.FlowChart :
                    OpenFlowChartDialog();
                    break;
                case DialogTypes.QDeterminant:
                    OpenQDeterminantDialog();
                    break;
                case DialogTypes.ImplementationPlan:
                    OpenImplementationPlanDialog();
                    break;
                case DialogTypes.Solution:
                    OpenSolutionDialog();
                    break;
            }
            return _openDialog;
        }

        public static SaveFileDialog CallSaveDialog(DialogTypes type)
        {
            _saveDialog = new SaveFileDialog();
            switch (type)
            {
                case DialogTypes.FlowChart:
                    SaveFlowChartDialog();
                    break;
                case DialogTypes.QDeterminant:
                    SaveQDeterminantDialog();
                    break;
                case DialogTypes.ImplementationPlan:
                    SaveImplementationPlanDialog();
                    break;
            }
            return _saveDialog;
        }

        private static void SaveImplementationPlanDialog()
        {
            _saveDialog.DefaultExt = ".xnl";
            _saveDialog.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }

        private static void SaveQDeterminantDialog()
        {
            _saveDialog.DefaultExt = ".xml";
            _saveDialog.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }

        private static void SaveFlowChartDialog()
        {
            _saveDialog.DefaultExt = ".xml";
            _saveDialog.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }

        private static void OpenSolutionDialog()
        {
            _openDialog.DefaultExt = ".qsln";
            _openDialog.Filter = "SolutionFiles (*.qsln)|*.qsln";
        }

        private static void OpenImplementationPlanDialog()
        {
            _openDialog.DefaultExt = ".ip";
            _openDialog.Filter = "ImplementationPlan (*.ip)|*.ip|XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }

        private static void OpenQDeterminantDialog()
        {
            _openDialog.DefaultExt = ".qd";
            _openDialog.Filter = "QDeterminant (*.qd)|*.ip|XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }

        private static void OpenFlowChartDialog()
        {
            _openDialog.DefaultExt = ".fc";
            _openDialog.Filter = "FlowChart (*.fc)|*.fc|XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
        }
    }
}

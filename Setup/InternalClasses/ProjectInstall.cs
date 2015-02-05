using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Setup.Properties;

namespace Setup.InternalClasses
{
    internal class ProjectInstall
    {

        private static string _startupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static readonly string DocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        internal static bool SetupUpdateService = !ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals("MidasUpdaterService"));
        internal static Dictionary<string, bool> AssociateFiles = new Dictionary<string, bool>
        {
            {".qsln", true},
            {".qpr", true},
            {".fc", true},
            {".ip", true},
            {".qd", true},
        };
        internal static bool CreateIcon = true;
        internal static bool CreateFolder = true;
        

        internal static string GetPathToQStudio()
        {
            return Path.Combine(_startupDirectory, Settings.Default.QStudioEXE);
        }

        internal static string GetPathToIcon()
        {
            return Path.Combine(_startupDirectory, Settings.Default.ProgramIcon);
        }

        internal static string GetPathToCompiler()
        {
            return Path.Combine(_startupDirectory, Settings.Default.CompilerEXE);
        }

        internal static string GetPathToUnInstaller()
        {
            return Path.Combine(_startupDirectory, Settings.Default.UnInstallerEXE);
        }

        internal static string GetFullPathToStartupDirectory()
        {
            return Path.Combine(_startupDirectory, Settings.Default.ProgramName);
        }

        internal static string GetShortPathToStartupDirectory()
        {
            return _startupDirectory;
        }

        internal static void SetStartupDirectory(string pathToDirectory)
        {
            _startupDirectory = pathToDirectory;
        }

        internal static string GetPathToDocuments()
        {
            return Path.Combine(DocumentsDirectory, Settings.Default.ProgramName);
        }



    }
}


using System;
using System.Linq.Expressions;
using Microsoft.Win32;

namespace UnInstaller.InternalClasses
{
    internal class Associate
    {
        internal static bool IsAssociated(string fileExtension)
        {
            return (Registry.ClassesRoot.OpenSubKey(fileExtension, false) != null);
        }

        internal static void RemoveAssociate(string fileExtension, string programName)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(fileExtension);
            Registry.ClassesRoot.DeleteSubKeyTree(programName);
        }

        internal static void RemoveFromUnInstallPanel(string programName)
        {
            try
            {
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + programName);
            }
            catch (Exception e)
            {
            }
            try
            {
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" +
                                                   programName);
            }
            catch (Exception e) { }
        }
    }
}

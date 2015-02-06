using Microsoft.Win32;

namespace Setup.InternalClasses
{
    internal class Associate
    {
        internal static void SetAssociate(string fileExtension, string programPath, string programName, string description, string iconPath)
        {
            Registry.ClassesRoot.CreateSubKey(fileExtension).SetValue("", programName);

            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(programName))
            {
                key.SetValue("", description);
                key.CreateSubKey("DefaultIcon").SetValue("", iconPath);
                key.CreateSubKey(@"Shell\Open\Command").SetValue("", programPath + " \"%1\"");
            }
        }

        internal static void AddToUnInstallPanel(string pathToUnInstaller, string programName, string pathToIcon=null, string companyName=null, string instalLocation=null)
        {
            using (var subkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\"+programName))
            {

                subkey.SetValue("DisplayName", programName);
                subkey.SetValue("UninstallString", pathToUnInstaller);
                subkey.SetValue("DisplayIcon", pathToIcon);
                subkey.SetValue("Publisher", companyName);
                subkey.SetValue("InstallLocation", instalLocation);
            }
        }


        internal static bool IsAssociated(string fileExtension)
        {
            return (Registry.ClassesRoot.OpenSubKey(fileExtension, false) != null);
        }

        internal static void RemoveAssociate(string fileExtension, string programName)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(fileExtension);
            Registry.ClassesRoot.DeleteSubKeyTree(programName);
        }
    }
}

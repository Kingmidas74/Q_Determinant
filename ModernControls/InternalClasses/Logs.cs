using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernControls.InternalClasses
{
    public class Logs
    {
        private static Logs _instance;
        private static StringBuilder _allDebugInfo;

        public static Logs Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logs();
                    _allDebugInfo = new StringBuilder("");
                }
                return _instance;
            }
            private set { } 
        }  

        Logs()
        {
            
        }
        public static string AllDebugInfo
        {
            get
            {
                return _allDebugInfo.ToString();
            }
        }

        public void WriteLog(string message, LogType type)
        {
            _allDebugInfo.AppendLine(message);
        }
        
    }
}

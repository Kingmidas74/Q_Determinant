using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NetworkAdapter
{
    public class AcesssToken
    {
        public static bool GetAccess(string login, string password, out string message)
        {
            message = string.Empty;
            try
            {
                var request =
                    new Request(string.Format("http://qdet.kingmidas.ru/api/api.php/access/{0}/{1}", login, password),
                        "GET");
                message = request.GetResponse();
                var response = JObject.Parse(message);
                return bool.Parse((string)response["accessStatus"]);
            }
            catch (Exception e)
            {
                message = string.Empty;
                return false;
            }
        }
    }
}

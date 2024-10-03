using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;

namespace InventoryWebService.BLL
{
    public static class Utils
    {
        public static string hexToASCII(string hexValue)
        {
            StringBuilder output = new StringBuilder("");

           
                for (int i = 0; i < hexValue.Length; i += 2)
                {
                    string str = hexValue.Substring(i, 2);
                    output.Append((char)Convert.ToInt32(str, 16));
                }
           
            return output.ToString();
        }
    }
}

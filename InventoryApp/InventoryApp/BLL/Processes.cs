using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace InventoryApp.BLL
{
    public static class Processes
    {
        public static bool IsNumeric(this String s)
        {
            return s.All(Char.IsDigit);
        }

        public static string cleanSql(string query) 
        {
            Regex whiteList = new Regex("('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)");
            return whiteList.Replace(query, "");
        }

        public static string ASCIITohex(string ASCIIValue)
        {
            byte[] ba = Encoding.Default.GetBytes(ASCIIValue.Trim());
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");

            return hexString;
        }

        public static string hexToASCII(string hexValue)
        {
            StringBuilder output = new StringBuilder("");

            try
            {
                for (int i = 0; i < hexValue.Length; i += 2)
                {
                    string str = hexValue.Substring(i, 2);
                    output.Append((char)Convert.ToInt32(str, 16));
                }
            }
            catch
            {
            }
            return output.ToString();
        }

        public static bool parseInt(string source, out int result)
        {
            result = -1;
            try {
                result = int.Parse(source);
            } catch(Exception e)
            { 
                return false; 
            }
            return true;
        }

        public static string XmlEscape(string unescaped)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public static string XmlUnescape(string escaped)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        }

        public static bool CheckConection()        
        {
            string url = Config.WebService;
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                myRequest.Timeout = 6000;
               
                using (System.Net.WebResponse myResponse = myRequest.GetResponse())
                {                   
                    myResponse.Close();               
                }
             
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }


    }
}

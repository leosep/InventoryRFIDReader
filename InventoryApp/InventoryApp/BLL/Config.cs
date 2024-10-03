using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace InventoryApp.BLL
{
    public class Config
    {
        private static NameValueCollection m_settings;
        private static string m_settingsPath;

        public static string WebService
        {
            get { return m_settings.Get("WebService"); }
            set { m_settings.Set("WebService", value); }
        }

        static Config()
        {
            // Path
            m_settingsPath = Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            m_settingsPath += @"\Config.xml";

            if (!File.Exists(m_settingsPath))
                throw new FileNotFoundException(
                                  m_settingsPath + "Config.xml could not be found.");

            System.Xml.XmlDocument xdoc = new XmlDocument();
            xdoc.Load(m_settingsPath);
            XmlElement root = xdoc.DocumentElement;
            System.Xml.XmlNodeList nodeList = root.ChildNodes.Item(0).ChildNodes;

            // Add configuration
            m_settings = new NameValueCollection();
            m_settings.Add("WebService", nodeList.Item(0).Attributes["value"].Value);
           
        }
    }
}

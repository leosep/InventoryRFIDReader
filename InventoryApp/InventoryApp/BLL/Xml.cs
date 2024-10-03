using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace InventoryApp.BLL
{
    public class Xml
    {
        public static string EncodeTexto(string unescapedTexto)
        {
            if (string.IsNullOrEmpty(unescapedTexto))
            {
                return unescapedTexto;
            }

            var builder = new StringBuilder(unescapedTexto.Length);
            using (var writer = XmlTextWriter.Create(builder, new XmlWriterSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment
            }))
            {
                writer.WriteValue(unescapedTexto);
            }
            return builder.ToString();
        }
    }
}

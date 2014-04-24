using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DAL.XmlWrapper
{
    public static class XmlWriterWrapper
    {
        /// <summary>
        /// Writes xml document to file
        /// </summary>
        /// <param name="document">The XmlDocument object to write</param>
        /// <param name="destinationPath">Path of the ouput file</param>
        /// <returns>True if writing succeeded, throws exception otherwise</returns>
        public static bool WriteXml(XmlDocument document, string destinationPath)
        {
            using (var writer = XmlWriter.Create(destinationPath))
            {
                document.WriteTo(writer);
            }

            return true;
        }
    }
}

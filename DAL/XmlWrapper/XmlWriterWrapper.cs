using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
            try
            {
                // Create the directory in case doesn't exist yet
                Directory.CreateDirectory(new FileInfo(destinationPath).DirectoryName);

                // Export the XML file with indentation for easy read
                using (var writer = XmlWriter.Create(destinationPath, new XmlWriterSettings() { Indent = true }))
                {
                    document.WriteTo(writer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to create XML file at path: {0}", destinationPath), ex);
            }

            // If no exception occured, operation successful
            return true;
        }
    }
}

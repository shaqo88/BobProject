using BL.SchemaLogic.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL.XmlLogic
{
    public static class XmlImportLogic
    {
        public static XmlSchemaElementWrapper XmlDocumentToSchemaWrapper(XmlDocument xmlDoc)
        {
            
            return null;
        }

        public static Version GetVersionOfXml(XmlDocument xmlDoc)
        {
            Version ver;

            var dec = xmlDoc.ChildNodes.OfType<XmlDeclaration>().FirstOrDefault();

            if (dec != null && Version.TryParse(dec.Version, out ver))
                return ver;

            return new Version(0, 0);
        }
    }
}

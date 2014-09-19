using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite
{
    public class XmlSchemaSimpleTypeWrapper : IXmlSchemaTypeWrapper
    {
        #region Properties

        public string Name { get; private set; }
        public XmlSchemaSimpleType SchemaType { get; private set; }
        public Type DotNetType { get; private set; } // The restriction base, relevant only for simple type
        public string Pattern { get; private set; }

        #endregion

        #region Constructor

        public XmlSchemaSimpleTypeWrapper(XmlSchemaSimpleType type)
        {
            this.Name = type.Name;
            this.SchemaType = type;
            this.DotNetType = SchemaType.Datatype.ValueType;
            GetPattern();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Factory create type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static IXmlSchemaTypeWrapper SchemaWrappersFactory(XmlSchemaType baseType)
        {
            if (baseType is XmlSchemaComplexType)
                return new XmlSchemaComplexTypeWrapper(baseType as XmlSchemaComplexType);
            else if (baseType is XmlSchemaSimpleType)
                return new XmlSchemaSimpleTypeWrapper(baseType as XmlSchemaSimpleType);
            else
                throw new Exception(string.Format("Unknown type: {0}", baseType.Name));
        }

        private void GetPattern()
        {
            var restriction = this.SchemaType.Content as XmlSchemaSimpleTypeRestriction;

            if (restriction != null && restriction.Facets.Count > 0)
            {
                var pattern = restriction.Facets[0] as XmlSchemaPatternFacet;
                Pattern = pattern.Value;
            }
        }

        public static Type GetDotNetType(IXmlSchemaTypeWrapper type)
        {
            if (type is XmlSchemaSimpleType)
                return type.DotNetType;

            return null;
        }

        #endregion
    }
}

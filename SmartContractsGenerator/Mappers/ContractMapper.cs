using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartContractsGenerator.Mappers
{
    public class ContractMapper
    {
        public Contract MapXmlDocumentToContract(XmlDocument document)
        {
            if (document != null)
            {
                var c = new Contract();
                var contractNode = document.SelectSingleNode("contract");
                if (contractNode != null)
                {
                    var nameNode = contractNode.SelectSingleNode("name");
                    if (nameNode != null)
                    {
                        c.Name = nameNode.InnerText;
                    }

                    var constructorNode = contractNode.SelectSingleNode("constructor");
                    if (constructorNode != null)
                    {
                        var constructor = new Constructor();
                        var visibilityNode = constructorNode.SelectSingleNode("visibility");
                        if (visibilityNode != null && visibilityNode.InnerText.All(c => char.IsDigit(c)))
                        {
                            constructor.Visibility = (Model.Enums.Visibility)Convert.ToInt32(visibilityNode.InnerText);
                        }

                        c.Constructor = constructor;
                    }
                }
                return c;
            }
            else
            {
                return null;
            }
        }
    }
}

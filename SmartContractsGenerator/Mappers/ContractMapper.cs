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
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("gxml", "https://developers.google.com/blockly/xml");
                XmlNode root = document.DocumentElement;

                var contractNodes = root.SelectNodes("descendant::gxml:block[@type=\"contract\"]", nsmgr);
                if (contractNodes.Count > 0)
                {
                    return GetContractFromXmlNode(contractNodes.Item(0), nsmgr);
                }
            }
            return null;
        }

        public Contract GetContractFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            Contract c = new Contract();

            var nameNode = node.SelectSingleNode("gxml:field", nsmgr);
            if (nameNode != null)
            {
                c.Name = nameNode.InnerText;
            }

            return c;
        }
    }
}

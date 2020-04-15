using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.Enums;
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

            var nameNode = node.SelectSingleNode("gxml:field[@name=\"Name\"]", nsmgr);
            if (nameNode != null)
            {
                c.Name = nameNode.InnerText;
            }

            var constructorNode = node.SelectSingleNode("gxml:statement[@name=\"Constructor\"]/gxml:block[@type=\"contract_constructor\"]", nsmgr);
            if (constructorNode != null)
            {
                c.Constructor = GetConstructorFromXmlNode(constructorNode, nsmgr);
            }

            return c;
        }

        public Constructor GetConstructorFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            Constructor c = new Constructor();

            var visibilityNode = node.SelectSingleNode("gxml:field[@name=\"Visibility\"]", nsmgr);
            if (visibilityNode != null)
            {
                c.Visibility = GetVisibilityFromXmlNode(visibilityNode);
            }

            return c;
        }

        public Visibility? GetVisibilityFromXmlNode(XmlNode node)
        {
            if (node.InnerText.All(c => char.IsDigit(c)))
            {
                return (Visibility)Convert.ToInt32(node.InnerText);
            }

            return null;
        }
    }
}

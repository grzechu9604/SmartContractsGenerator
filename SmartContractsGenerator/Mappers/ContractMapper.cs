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
            Contract c = new Contract()
            {
                Name = GetNameForElementNode(node, nsmgr)
            };

            var propertiesRootNode = node.SelectSingleNode("gxml:statement[@name=\"Properties\"]/gxml:block[@type=\"contract_property\"]", nsmgr);
            if (propertiesRootNode != null)
            {
                c.Properties = GetPropertiesFromXmlNode(propertiesRootNode, nsmgr);
            }

            var constructorNode = node.SelectSingleNode("gxml:statement[@name=\"Constructor\"]/gxml:block[@type=\"contract_constructor\"]", nsmgr);
            if (constructorNode != null)
            {
                c.Constructor = GetConstructorFromXmlNode(constructorNode, nsmgr);
            }

            var functionsRootNode = node.SelectSingleNode("gxml:statement[@name=\"Functions\"]/gxml:block[@type=\"contract_function\"]", nsmgr);
            if (functionsRootNode != null)
            {
                c.Functions = GetFunctionFromXmlNode(functionsRootNode, nsmgr);
            }

            return c;
        }

        public Constructor GetConstructorFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            Constructor c = new Constructor()
            {
                Visibility = GetVisibilityForElementNode(node, nsmgr)
            };

            return c;
        }

        public Visibility? GetVisibilityForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {

            var visibilityNode = node.SelectSingleNode("gxml:field[@name=\"Visibility\"]", nsmgr);
            if (visibilityNode != null)
            {
                if (visibilityNode.InnerText.All(c => char.IsDigit(c)))
                {
                    return (Visibility)Convert.ToInt32(visibilityNode.InnerText);
                }
            }

            return null;
        }

        public Variable GetVariableForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var variableNode = node.SelectSingleNode("gxml:value[@name=\"Variable\"]/gxml:block[@type=\"variable_declaration\"]", nsmgr);
            if (variableNode != null)
            {
                return new Variable()
                {
                    Name = GetNameForElementNode(variableNode, nsmgr),
                    Type = GeTypeForElementNode(variableNode, nsmgr)
                };
            }

            return null;
        }

        public List<ContractFunction> GetFunctionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var functions = new List<ContractFunction>();

            while (node != null)
            {
                var f = new ContractFunction()
                {
                    Name = GetNameForElementNode(node, nsmgr),
                    Visibility = GetVisibilityForElementNode(node, nsmgr)
                };

                functions.Add(f);

                node = node.SelectSingleNode("gxml:next/gxml:block[@type=\"contract_function\"]", nsmgr);
            }

            return functions;
        }

        public List<ContractProperty> GetPropertiesFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var properties = new List<ContractProperty>();

            while (node != null)
            {
                var cp = new ContractProperty()
                {
                    Visibility = GetVisibilityForElementNode(node, nsmgr),
                    Variable = GetVariableForElementNode(node, nsmgr)
                };

                properties.Add(cp);

                node = node.SelectSingleNode("gxml:next/gxml:block[@type=\"contract_property\"]", nsmgr);
            }

            return properties;
        }


        public string GetNameForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetValueFromFieldForElementNode(node, nsmgr, "Name");
        }

        public string GeTypeForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetValueFromFieldForElementNode(node, nsmgr, "Type");
        }

        private string GetValueFromFieldForElementNode(XmlNode node, XmlNamespaceManager nsmgr, string fieldName)
        {
            var nameNode = node.SelectSingleNode($"gxml:field[@name=\"{fieldName}\"]", nsmgr);
            if (nameNode != null)
            {
                return nameNode.InnerText;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetValueForConstantValueNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var nameNode = node.SelectSingleNode("gxml:field[@name=\"Name\"]", nsmgr);
            if (nameNode != null)
            {
                return nameNode.InnerText;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

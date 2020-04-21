using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartContractsGenerator.Mappers
{
    public class ContractMapper
    {
        private const string ContractBlockType = "contract";
        private const string ContractPropertyBlockType = "contract_property";
        private const string ContractConstructorBlockType = "contract_constructor";
        private const string ContractFunctionBlockType = "contract_function";
        private const string AssignmentBlockType = "assignment";
        private const string VariableBlockType = "variable";
        private const string ConstantValueBlockType = "constant_value";
        private const string OperationBlockType = "operation";
        private const string VariableDeclarationBlockType = "variable_declaration";

        private const string PropertiesStatementName = "Properties";
        private const string ConstructorStatementName = "Constructor";
        private const string FunctionsStatementName = "Functions";
        private const string InstructionsStatementName = "Instructions";
        private const string DestinationStatementName = "Destination";
        private const string SourceStatementName = "Source";

        private const string LeftSideValueName = "left_side";
        private const string RightSideValueName = "right_side";

        private const string VisibilityFieldName = "Visibility";
        private const string OperatorFieldName = "Operator";
        private const string VariableFieldName = "Variable";
        private const string NameFieldName = "Name";
        private const string TypeFieldName = "Type";
        


        public Contract MapXmlDocumentToContract(XmlDocument document)
        {
            if (document != null)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("gxml", "https://developers.google.com/blockly/xml");
                XmlNode root = document.DocumentElement;

                var contractNodes = root.SelectNodes($"descendant::gxml:block[@type=\"{ContractBlockType}\"]", nsmgr);
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

            var propertiesRootNode = node.SelectSingleNode($"gxml:statement[@name=\"{PropertiesStatementName}\"]/gxml:block[@type=\"{ContractPropertyBlockType}\"]", nsmgr);
            if (propertiesRootNode != null)
            {
                c.Properties = GetPropertiesFromXmlNode(propertiesRootNode, nsmgr);
            }

            var constructorNode = node.SelectSingleNode($"gxml:statement[@name=\"{ConstructorStatementName}\"]/gxml:block[@type=\"{ContractConstructorBlockType}\"]", nsmgr);
            if (constructorNode != null)
            {
                c.Constructor = GetConstructorFromXmlNode(constructorNode, nsmgr);
            }

            var functionsRootNode = node.SelectSingleNode($"gxml:statement[@name=\"{FunctionsStatementName}\"]/gxml:block[@type=\"{ContractFunctionBlockType}\"]", nsmgr);
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
                Visibility = GetVisibilityForElementNode(node, nsmgr),
                Instructions = GetInstructionsListFromXmlNode(node, nsmgr)
            };

            return c;
        }

        public InstructionsList GetInstructionsListFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var instructions = new InstructionsList();

            var instructionsNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);

            while (instructionsNode != null)
            {
                var instruction = GetInstructionFromXmlNode(instructionsNode, nsmgr);
                instructions.AppendInstruction(instruction);
                instructionsNode = instructionsNode.SelectSingleNode("gxml:next/gxml:block", nsmgr);
            }

            return instructions;
        }

        public IInstruction GetInstructionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node.Attributes["type"] != null)
            {
                switch (node.Attributes["type"].Value)
                {
                    case AssignmentBlockType:
                        return GetAssignmentFromXmlNode(node, nsmgr);
                }
            }

            return null;
        }

        public Assignment GetAssignmentFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var destinationNode = node.SelectSingleNode($"gxml:value[@name=\"{DestinationStatementName}\"]/gxml:block", nsmgr);
            var sourceNode = node.SelectSingleNode($"gxml:value[@name=\"{SourceStatementName}\"]/gxml:block", nsmgr);

            return new Assignment()
            {
                Destination = GetValueContainerFromXmlNode(destinationNode, nsmgr),
                Source = GetAssignableFromXmlNode(sourceNode, nsmgr)
            };
        }

        public IValueContainer GetValueContainerFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node.Attributes["type"] != null)
            {
                switch (node.Attributes["type"].Value)
                {
                    case VariableBlockType:
                        return GetVariableUsageForElementNode(node, nsmgr);
                }
            }

            return null;
        }

        public IAssignable GetAssignableFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null && node.Attributes["type"] != null)
            {
                switch (node.Attributes["type"].InnerText)
                {
                    case ConstantValueBlockType:
                        return GetConstantValueFromElementNode(node, nsmgr);
                    case OperationBlockType:
                        return GetOperationFromElementNode(node, nsmgr);
                }
            }

            return null;
        }

        public ConstantValue GetConstantValueFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new ConstantValue()
                {
                    Value = node.InnerText
                };
            }

            return null;
        }

        public Operation GetOperationFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var left = node.SelectSingleNode($"gxml:value[@name=\"{LeftSideValueName}\"]/gxml:block", nsmgr);
                var right = node.SelectSingleNode($"gxml:value[@name=\"{RightSideValueName}\"]/gxml:block", nsmgr);

                return new Operation()
                {
                    LeftSide = GetAssignableFromXmlNode(left, nsmgr),
                    RightSide = GetAssignableFromXmlNode(right, nsmgr),
                    Operator = GetOperatorForElementNode(node, nsmgr)
                };
            }

            return null;
        }

        public Visibility? GetVisibilityForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {

            var visibilityNode = node.SelectSingleNode($"gxml:field[@name=\"{VisibilityFieldName}\"]", nsmgr);
            if (visibilityNode != null)
            {
                if (visibilityNode.InnerText.All(c => char.IsDigit(c)))
                {
                    return (Visibility)Convert.ToInt32(visibilityNode.InnerText);
                }
            }

            return null;
        }

        public OperationOperator? GetOperatorForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {

            var operatorNode = node.SelectSingleNode($"gxml:field[@name=\"{OperatorFieldName}\"]", nsmgr);
            if (operatorNode != null)
            {
                //TODO Add proper mapper
                return OperationOperator.Plus;
            }

            return null;
        }

        public Variable GetVariableDeclarationForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var variableNode = node.SelectSingleNode($"gxml:value[@name=\"{VariableFieldName}\"]/gxml:block[@type=\"{VariableDeclarationBlockType}\"]", nsmgr);
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

        public Variable GetVariableUsageForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new Variable()
                {
                    Name = GetNameForElementNode(node, nsmgr)
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

                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractFunctionBlockType}\"]", nsmgr);
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
                    Variable = GetVariableDeclarationForElementNode(node, nsmgr)
                };

                properties.Add(cp);

                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractPropertyBlockType}\"]", nsmgr);
            }

            return properties;
        }


        public string GetNameForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetValueFromFieldForElementNode(node, nsmgr, NameFieldName);
        }

        public string GeTypeForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetValueFromFieldForElementNode(node, nsmgr, TypeFieldName);
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
            var nameNode = node.SelectSingleNode($"gxml:field[@name=\"{NameFieldName}\"]", nsmgr);
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

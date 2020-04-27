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
        private const string ContractEventBlockType = "contract_event";
        private const string AssignmentBlockType = "assignment";
        private const string VariableBlockType = "variable";
        private const string ConstantValueBlockType = "constant_value";
        private const string OperationBlockType = "operation";
        private const string VariableDeclarationBlockType = "variable_declaration";
        private const string IfStatementBlockType = "if_statement";
        private const string ContractLoopBlockType = "contract_loop";
        private const string RequirementBlockType = "requirement";

        private const string PropertiesStatementName = "Properties";
        private const string ConstructorStatementName = "Constructor";
        private const string FunctionsStatementName = "Functions";
        private const string EventsStatementName = "Events";
        private const string InstructionsStatementName = "Instructions";
        private const string DestinationStatementName = "Destination";
        private const string SourceStatementName = "Source";
        private const string TrueInstructionsStatementName = "true_instructions";
        private const string FalseInstructionsStatementName = "false_instructions";

        private const string LeftSideValueName = "left_side";
        private const string RightSideValueName = "right_side";
        private const string ConditionValueName = "condition";
        private const string InitialAssignmentValueName = "Initial_assignment";
        private const string BreakConditionValueName = "break_condition";
        private const string StepInstructionValueName = "step_instruction";

        private const string VisibilityFieldName = "Visibility";
        private const string OperatorFieldName = "Operator";
        private const string VariableFieldName = "Variable";
        private const string NameFieldName = "Name";
        private const string TypeFieldName = "Type";
        private const string ErrorMessageFieldName = "ErrorMessage";

        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IAssignable>> AssignableMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IValueContainer>> ValueContainerMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IInstruction>> InstructionMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IOneLineInstruction>> OneLineInstructionMappers;

        public ContractMapper()
        {
            AssignableMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IAssignable>>()
            {
                { ConstantValueBlockType, GetConstantValueFromElementNode },
                { OperationBlockType, GetOperationFromElementNode },
                { VariableBlockType, GetVariableFormXmlNode }
            };

            ValueContainerMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IValueContainer>>()
            {
                { VariableBlockType, GetVariableFormXmlNode },
                { VariableDeclarationBlockType, GetVariableDeclarationFromXmlNode }
            };

            InstructionMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IInstruction>>()
            {
                {AssignmentBlockType, GetAssignmentFromXmlNode },
                {IfStatementBlockType, GetIfStatementFromXmlNode },
                {ContractLoopBlockType, GetContractLoopFromXmlNode },
                {RequirementBlockType, GetRequirementFromXmlNode }
            };

            OneLineInstructionMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IOneLineInstruction>>()
            {
                {AssignmentBlockType, GetAssignmentFromXmlNode }
            };
        }
        
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

            var eventsRootNode = node.SelectSingleNode($"gxml:statement[@name=\"{EventsStatementName}\"]/gxml:block[@type=\"{ContractEventBlockType}\"]", nsmgr);
            if (eventsRootNode != null)
            {
                c.Events = GetContractEventsFromXmlNode(eventsRootNode, nsmgr);
            }

            return c;
        }

        public Constructor GetConstructorFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);
            Constructor c = new Constructor()
            {
                Visibility = GetVisibilityForElementNode(node, nsmgr),
                Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr)
            };

            return c;
        }

        public InstructionsList GetInstructionsListFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var instructions = new InstructionsList();

            while (node != null)
            {
                var instruction = GetInstructionFromXmlNode(node, nsmgr);
                instructions.AppendInstruction(instruction);
                node = node.SelectSingleNode("gxml:next/gxml:block", nsmgr);
            }

            return instructions;
        }

        public IInstruction GetInstructionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null && node.Attributes["type"] != null)
            {
                return InstructionMappers[node.Attributes["type"].Value].Invoke(node, nsmgr);
            }

            return null;
        }

        public IOneLineInstruction GetOneLineInstructionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null && node.Attributes["type"] != null)
            {
                return OneLineInstructionMappers[node.Attributes["type"].Value].Invoke(node, nsmgr);
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

        public IfStatement GetIfStatementFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var conditionNode = node.SelectSingleNode($"gxml:value[@name=\"{ConditionValueName}\"]/gxml:block", nsmgr);
                var trueInstructionsRoot = node.SelectSingleNode($"gxml:statement[@name=\"{TrueInstructionsStatementName}\"]/gxml:block", nsmgr);
                var falseInstructionsRoot = node.SelectSingleNode($"gxml:statement[@name=\"{FalseInstructionsStatementName}\"]/gxml:block", nsmgr);
                return new IfStatement()
                {
                    Condition = GetConditionFromXmlNode(conditionNode, nsmgr),
                    TrueInstructions = GetInstructionsListFromXmlNode(trueInstructionsRoot, nsmgr),
                    FalseInstructions = GetInstructionsListFromXmlNode(falseInstructionsRoot, nsmgr)
                };
            }

            return null;
        }

        public ContractLoop GetContractLoopFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var conditionNode = node.SelectSingleNode($"gxml:value[@name=\"{BreakConditionValueName}\"]/gxml:block", nsmgr);
                var initialNode = node.SelectSingleNode($"gxml:statement[@name=\"{InitialAssignmentValueName}\"]/gxml:block", nsmgr);
                var instructionsNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);
                var stepInstructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{StepInstructionValueName}\"]/gxml:block", nsmgr);

                return new ContractLoop()
                {
                    BreakCondition = GetConditionFromXmlNode(conditionNode, nsmgr),
                    InitialAssignment = GetAssignmentFromXmlNode(initialNode, nsmgr),
                    Instructions = GetInstructionsListFromXmlNode(instructionsNode, nsmgr),
                    StepInstruction = GetOneLineInstructionFromXmlNode(stepInstructionNode, nsmgr)
                };
            }

            return null;
        }

        public Requirement GetRequirementFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var conditionNode = node.SelectSingleNode($"gxml:value[@name=\"{ConditionValueName}\"]/gxml:block", nsmgr);
                var errorMessageNode = node.SelectSingleNode($"gxml:field[@name=\"{ErrorMessageFieldName}\"]", nsmgr);

                return new Requirement()
                {
                    Condition = GetConditionFromXmlNode(conditionNode, nsmgr),
                    ErrorMessage = GetErrorMessageFromElementNode(errorMessageNode, nsmgr)
                };
            }

            return null;
        }

        public IValueContainer GetValueContainerFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null && node.Attributes["type"] != null)
            {
                return ValueContainerMappers[node.Attributes["type"].Value].Invoke(node, nsmgr);
            }

            return null;
        }

        public IAssignable GetAssignableFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null && node.Attributes["type"] != null)
            {
                return AssignableMappers[node.Attributes["type"].InnerText].Invoke(node, nsmgr);
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

        public string GetErrorMessageFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                //TODO escape special chars
                return $"\"{node.InnerText}\"";
            }

            return null;
        }

        public Condition GetConditionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var conditionNode = node.SelectSingleNode($"gxml:value[@name=\"{ConditionValueName}\"]//gxml:block[@type=\"{OperationBlockType}\"]", nsmgr);
                return new Condition()
                {
                    ConditionOperation = GetOperationFromElementNode(conditionNode, nsmgr)
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
                return EnumMappers.MapBlocklyCodeToVisibility(visibilityNode.InnerText);
            }

            return null;
        }

        public OperationOperator? GetOperatorForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var operatorNode = node.SelectSingleNode($"gxml:field[@name=\"{OperatorFieldName}\"]", nsmgr);
            if (operatorNode != null)
            {
                return EnumMappers.MapBlocklyCodeToOperationOperator(operatorNode.InnerText);
            }

            return null;
        }

        public Declaration GetVariableDeclarationFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new Declaration()
                {
                    Variable = GetVariableFormXmlNode(node, nsmgr)
                };
            }

            return null;
        }

        public Variable GetVariableFormXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new Variable()
                {
                    Name = GetNameForElementNode(node, nsmgr),
                    Type = GeTypeForElementNode(node, nsmgr)
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

        public List<ContractEvent> GetContractEventsFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var events = new List<ContractEvent>();

            while (node != null)
            {
                var e = new ContractEvent()
                {
                    Name = GetNameForElementNode(node, nsmgr)
                };

                events.Add(e);

                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractEventBlockType}\"]", nsmgr);
            }

            return events;
        }

        public List<ContractProperty> GetPropertiesFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var properties = new List<ContractProperty>();

            while (node != null)
            {
                var variableNode = node.SelectSingleNode($"gxml:value[@name=\"{VariableFieldName}\"]/gxml:block[@type=\"{VariableDeclarationBlockType}\"]", nsmgr);
                var cp = new ContractProperty()
                {
                    Visibility = GetVisibilityForElementNode(node, nsmgr),
                    Variable = GetVariableFormXmlNode(variableNode, nsmgr)
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

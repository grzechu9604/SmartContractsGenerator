﻿using SmartContractsGenerator.Helpers;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.BuiltinFunctionCalls;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGenerator.Model.SpecialFunctions;
using System;
using System.Collections.Generic;
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
        private const string TrueConstantBlockType = "true_const";
        private const string FalseConstantBlockType = "false_const";
        private const string OperationBlockType = "operation";
        private const string LogicOperationBlockType = "logic_operation";
        private const string VariableDeclarationBlockType = "variable_declaration";
        private const string IfStatementBlockType = "if_statement";
        private const string ContractLoopBlockType = "contract_loop";
        private const string RequirementBlockType = "requirement";
        private const string EventCallBlockType = "event_call";
        private const string CallReturnableFunctionBlockType = "call_returnable_function";
        private const string CallVoidFunctionBlockType = "call_void_function";
        private const string ModifierBlockType = "modifier";
        private const string ReturnBlockType = "return";
        private const string ModifierApplianceBlockType = "modifier_appliance";
        private const string BreakStatementBlockType = "break_statement";
        private const string SpecialValueCallBlockType = "special_value_call";
        private const string BalanceCallBlockType = "balance_call";
        private const string TransferCallBlockType = "transfer_call";

        private const string PropertiesStatementName = "Properties";
        private const string ConstructorStatementName = "Constructor";
        private const string FunctionsStatementName = "Functions";
        private const string EventsStatementName = "Events";
        private const string InstructionsStatementName = "Instructions";
        private const string DestinationStatementName = "Destination";
        private const string SourceStatementName = "Source";
        private const string TrueInstructionsStatementName = "true_instructions";
        private const string FalseInstructionsStatementName = "false_instructions";
        private const string ModifiersStatementName = "Modifiers";

        private const string LeftSideValueName = "left_side";
        private const string RightSideValueName = "right_side";
        private const string ConditionValueName = "condition";
        private const string InitialAssignmentValueName = "Initial_assignment";
        private const string BreakConditionValueName = "break_condition";
        private const string StepInstructionValueName = "step_instruction";
        private const string ModifierValueName = "Modifier";
        private const string ReturnValueValueName = "ReturnValue";
        private const string AddressValueName = "Address";
        private const string AmountValueName = "Amount";

        private const string VisibilityFieldName = "Visibility";
        private const string OperatorFieldName = "Operator";
        private const string VariableFieldName = "Variable";
        private const string NameFieldName = "NAME";
        private const string TypeFieldName = "TYPE";
        private const string ErrorMessageFieldName = "ErrorMessage";
        private const string ModifierFieldName = "Modifier";
        private const string StateModificationFieldName = "StateModification";
        private const string LCNameFieldName = "name";
        private const string LCTypeFieldName = "type";
        private const string AcceptsEthersFieldName = "AcceptsEthers";
        private const string ValueFieldName = "value";

        private const string BlocklyBoolTrue = "TRUE";

        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IAssignable>> AssignableMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IValueContainer>> ValueContainerMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IInstruction>> InstructionMappers;
        private readonly Dictionary<string, Func<XmlNode, XmlNamespaceManager, IOneLineInstruction>> OneLineInstructionMappers;

        public ContractMapper()
        {
            AssignableMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IAssignable>>()
            {
                { ConstantValueBlockType, GetConstantValueFromElementNode },
                { TrueConstantBlockType, GetLogicTrueConstantValueFromElementNode },
                { FalseConstantBlockType, GetLogicFalseConstantValueFromElementNode },
                { OperationBlockType, GetMathOperationFromElementNode },
                { LogicOperationBlockType, GetLogicOperationFromElementNode },
                { VariableBlockType, GetVariableFormXmlNode },
                { CallReturnableFunctionBlockType, GetFunctionCallFromXmlNode },
                { SpecialValueCallBlockType, GetSpecialValueCallFromXmlNode },
                { BalanceCallBlockType, GetBalanceCallFromXmlNode }
            };

            ValueContainerMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IValueContainer>>()
            {
                { VariableBlockType, GetVariableFormXmlNode },
                { VariableDeclarationBlockType, GetVariableDeclarationFromXmlNode }
            };

            InstructionMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IInstruction>>()
            {
                { AssignmentBlockType, GetAssignmentFromXmlNode },
                { IfStatementBlockType, GetIfStatementFromXmlNode },
                { ContractLoopBlockType, GetContractLoopFromXmlNode },
                { RequirementBlockType, GetRequirementFromXmlNode },
                { EventCallBlockType, GetEventCallFromXmlNode },
                { CallVoidFunctionBlockType, GetFunctionCallFromXmlNode },
                { ReturnBlockType, GetReturnStatementFromXmlNode },
                { BreakStatementBlockType, GetBreakStatementFromXmlNode },
                { TransferCallBlockType, GetTransferCallFromXmlNode }
            };

            OneLineInstructionMappers = new Dictionary<string, Func<XmlNode, XmlNamespaceManager, IOneLineInstruction>>()
            {
                { AssignmentBlockType, GetAssignmentFromXmlNode }
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
            if (node != null)
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
                    c.Functions = GetFunctionsFromXmlNode(functionsRootNode, nsmgr);
                }

                var eventsRootNode = node.SelectSingleNode($"gxml:statement[@name=\"{EventsStatementName}\"]/gxml:block[@type=\"{ContractEventBlockType}\"]", nsmgr);
                if (eventsRootNode != null)
                {
                    c.Events = GetContractEventsFromXmlNode(eventsRootNode, nsmgr);
                }

                var modifiersRootNode = node.SelectSingleNode($"gxml:statement[@name=\"{ModifiersStatementName}\"]/gxml:block[@type=\"{ModifierBlockType}\"]", nsmgr);
                if (modifiersRootNode != null)
                {
                    c.Modifiers = GetModifiersFromXmlNode(modifiersRootNode, nsmgr);
                }

                var fallbackRootNode = node.SelectSingleNode($"gxml:statement[@name=\"FallbackFunction\"]/gxml:block[@type=\"fallback_function\"]", nsmgr);
                if (fallbackRootNode != null)
                {
                    c.FallbackFunction = GetFallbackFunctionFromXmlNode(fallbackRootNode, nsmgr);
                }
                
                var receiveRootNode = node.SelectSingleNode($"gxml:statement[@name=\"ReceiveFunction\"]/gxml:block[@type=\"receive_function\"]", nsmgr);
                if (receiveRootNode != null)
                {
                    c.ReceiveFunction = GetReceiveFunctionFromXmlNode(receiveRootNode, nsmgr);
                }

                return c;
            }

            return null;
        }

        public Constructor GetConstructorFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);
                var parametersNode = node.SelectSingleNode($"gxml:mutation", nsmgr);

                Constructor c = new Constructor()
                {
                    Visibility = GetVisibilityForElementNode(node, nsmgr),
                    Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr),
                    Parameters = GetParametersListFromXmlNode(parametersNode, nsmgr)
                };

                return c;
            }

            return null;
        }

        public ReceiveFunction GetReceiveFunctionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);

                return new ReceiveFunction()
                {
                    Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr)
                };
            }

            return null;
        }

        public FallbackFunction GetFallbackFunctionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);

                return new FallbackFunction()
                {
                    Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr),
                    IsPayable = GetAcceptsEthersForElementNode(node, nsmgr)
                };
            }

            return null;
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
            if (node != null)
            {
                var destinationNode = node.SelectSingleNode($"gxml:value[@name=\"{DestinationStatementName}\"]/gxml:block", nsmgr);
                var sourceNode = node.SelectSingleNode($"gxml:value[@name=\"{SourceStatementName}\"]/gxml:block", nsmgr);

                return new Assignment()
                {
                    Destination = GetValueContainerFromXmlNode(destinationNode, nsmgr),
                    Source = GetAssignableFromXmlNode(sourceNode, nsmgr)
                };
            }

            return null;
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
                var type = GeTypeForElementNode(node, nsmgr);
                var valueText = GetValueFromFieldForElementNode(node, nsmgr, ValueFieldName);

                return new ConstantValue()
                {
                    Value = GetValueForConstantValue(valueText, type)
                };
            }

            return null;
        }

        public string GetValueForConstantValue(string value, SolidityType? type)
        {
            try
            {
                switch (type)
                {
                    case SolidityType.Int:
                        return Convert.ToInt64(value).ToString();
                    case SolidityType.Fixed:
                        return Convert.ToDecimal(value).ToString();
                    case SolidityType.String:
                        return SolidityStringsEscaper.EscapeString(value);
                    default:
                        break;
                }
            }
            catch (FormatException fe)
            {
                throw new InvalidOperationException($"Couldn't convert value: {value} to {type}", fe);
            }
            return SolidityStringsEscaper.EscapeString(value);
        }

        public ConstantValue GetLogicTrueConstantValueFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetConstantValueFromElementNodeWithPredefinedValue(node, nsmgr, "true");
        }

        public ConstantValue GetLogicFalseConstantValueFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetConstantValueFromElementNodeWithPredefinedValue(node, nsmgr, "false");
        }
        
        private ConstantValue GetConstantValueFromElementNodeWithPredefinedValue(XmlNode node, XmlNamespaceManager nsmgr, string value)
        {
            if (node != null)
            {
                return new ConstantValue()
                {
                    Value = value
                };
            }

            return null;
        }

        public string GetErrorMessageFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return SolidityStringsEscaper.EscapeString(node.InnerText);
            }

            return null;
        }

        public Condition GetConditionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var conditionNode = node.SelectSingleNode($"gxml:value[@name=\"{ConditionValueName}\"]//gxml:block[@type=\"{LogicOperationBlockType}\"]", nsmgr);
                return new Condition()
                {
                    ConditionOperation = GetLogicOperationFromElementNode(conditionNode, nsmgr)
                };
            }

            return null;
        }

        public Operation GetLogicOperationFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetOperationFromElementNode(node, nsmgr, GetLogicOperatorForElementNode);
        }

        public Operation GetMathOperationFromElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetOperationFromElementNode(node, nsmgr, GetMathOperatorForElementNode);
        }

        private Operation GetOperationFromElementNode(XmlNode node, XmlNamespaceManager nsmgr, Func<XmlNode, XmlNamespaceManager, OperationOperator?> operatorFromNodeGetter)
        {
            if (node != null)
            {
                var left = node.SelectSingleNode($"gxml:value[@name=\"{LeftSideValueName}\"]/gxml:block", nsmgr);
                var right = node.SelectSingleNode($"gxml:value[@name=\"{RightSideValueName}\"]/gxml:block", nsmgr);

                return new Operation()
                {
                    LeftSide = GetAssignableFromXmlNode(left, nsmgr),
                    RightSide = GetAssignableFromXmlNode(right, nsmgr),
                    Operator = operatorFromNodeGetter(node, nsmgr)
                };
            }

            return null;
        }

        public Visibility? GetVisibilityForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var visibilityNode = node.SelectSingleNode($"gxml:field[@name=\"{VisibilityFieldName}\"]", nsmgr);
                if (visibilityNode != null)
                {
                    return EnumMappers.MapBlocklyCodeToVisibility(visibilityNode.InnerText);
                }
            }

            return null;
        }

        public OperationOperator? GetMathOperatorForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetOperatorForElementNode(node, nsmgr, EnumMappers.MapBlocklyCodeToMathOperationOperator);
        }

        public OperationOperator? GetLogicOperatorForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            return GetOperatorForElementNode(node, nsmgr, EnumMappers.MapBlocklyCodeToLogicOperationOperator);
        }

        private OperationOperator? GetOperatorForElementNode(XmlNode node, XmlNamespaceManager nsmgr, Func<string, OperationOperator> mapper)
        {
            if (node != null)
            {
                var operatorNode = node.SelectSingleNode($"gxml:field[@name=\"{OperatorFieldName}\"]", nsmgr);
                if (operatorNode != null)
                {
                    return mapper(operatorNode.InnerText);
                }
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

        public List<ContractFunction> GetFunctionsFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var functions = new List<ContractFunction>();

            while (node != null)
            {
                functions.Add(GetFunctionFromXmlNode(node, nsmgr));
                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractFunctionBlockType}\"]", nsmgr);
            }

            return functions;
        }

        public ContractFunction GetFunctionFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);
                var parametersNode = node.SelectSingleNode($"gxml:mutation", nsmgr);
                var modifierApplianceNode = node.SelectSingleNode($"gxml:value[@name=\"{ModifierValueName}\"]/gxml:block[@type=\"{ModifierApplianceBlockType}\"]", nsmgr);

                return new ContractFunction()
                {
                    Name = GetNameForElementNode(node, nsmgr),
                    Visibility = GetVisibilityForElementNode(node, nsmgr),
                    Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr),
                    Modifier = GetModifierApplianceFromXmlNode(modifierApplianceNode, nsmgr),
                    Parameters = GetParametersListFromXmlNode(parametersNode, nsmgr),
                    ReturningType = GeTypeForElementNode(node, nsmgr),
                    ModificationType = GetModificationTypeForElementNode(node, nsmgr),
                    IsPayable = GetAcceptsEthersForElementNode(node, nsmgr)
                };
            }

            return null;
        }

        public ParametersList GetParametersListFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var pList = new List<Variable>();
                foreach(XmlNode argNode in node.SelectNodes($"gxml:arg", nsmgr))
                {
                    pList.Add(GetVariableFromArgumentNode(argNode, nsmgr));
                }
                
                return new ParametersList()
                {
                    Parameters = pList
                };
            }

            return null;
        }

        public Variable GetVariableFromArgumentNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new Variable()
                {
                    Name = node.Attributes[LCNameFieldName].Value,
                    Type = EnumMappers.MapBlocklyCodeToSolidityType(node.Attributes[LCTypeFieldName].Value)
                };
            }

            return null;
        }

        public List<Modifier> GetModifiersFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var modifiers = new List<Modifier>();

            while (node != null)
            {
                modifiers.Add(GetModifierFromXmlNode(node, nsmgr));
                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ModifierBlockType}\"]", nsmgr);
            }

            return modifiers;
        }

        public Modifier GetModifierFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var instructionNode = node.SelectSingleNode($"gxml:statement[@name=\"{InstructionsStatementName}\"]/gxml:block", nsmgr);
                var parametersNode = node.SelectSingleNode($"gxml:mutation", nsmgr);

                return new Modifier()
                {
                    Name = GetNameForElementNode(node, nsmgr),
                    Instructions = GetInstructionsListFromXmlNode(instructionNode, nsmgr),
                    Parameters = GetParametersListFromXmlNode(parametersNode, nsmgr)
                };
            }

            return null;
        }

        public ModifierAppliance GetModifierApplianceFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var name = GetNameForCallFromXmlNode(node, nsmgr);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var paramList = GetParametersForCallFromXmlNode(node, nsmgr);
                    return new ModifierAppliance()
                    {
                        ModifierToApply = new Modifier()
                        {
                            Name = name
                        },
                        Parameters = paramList
                    };
                }
            }

            return null;
        }

        public FunctionCall GetFunctionCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var name = GetNameForCallFromXmlNode(node, nsmgr);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var paramList = GetParametersForCallFromXmlNode(node, nsmgr);
                    return new FunctionCall()
                    {
                        FunctionToCall = new ContractFunction()
                        {
                            Name = name
                        },
                        Parameters = paramList
                    };
                }
            }

            return null;
        }

        public ReturnStatement GetReturnStatementFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var objectToReturnNode = node.SelectSingleNode($"gxml:value[@name=\"{ReturnValueValueName}\"]/gxml:block", nsmgr);

                return new ReturnStatement()
                {
                    ToReturn = GetAssignableFromXmlNode(objectToReturnNode, nsmgr)
                };
            }
            return null;
        }

        public SpecialValueCall GetSpecialValueCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return new SpecialValueCall()
                {
                    PropertyToCall = GetBlockOrTransactionPropertyForElementNode(node, nsmgr)
                };
            }
            return null;
        }

        public BreakStatement GetBreakStatementFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr) => node != null ? new BreakStatement() : null;

        public List<ContractEvent> GetContractEventsFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var events = new List<ContractEvent>();

            while (node != null)
            {
                events.Add(GetContractEventFromXmlNode(node, nsmgr));
                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractEventBlockType}\"]", nsmgr);
            }

            return events;
        }

        public ContractEvent GetContractEventFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var parametersNode = node.SelectSingleNode($"gxml:mutation", nsmgr);

                return new ContractEvent()
                {
                    Name = GetNameForElementNode(node, nsmgr),
                    Parameters = GetParametersListFromXmlNode(parametersNode, nsmgr)
                };
            }

            return null;
        }

        public EventCall GetEventCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var name = GetNameForCallFromXmlNode(node, nsmgr);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var paramList = GetParametersForCallFromXmlNode(node, nsmgr);
                    return new EventCall()
                    {
                        EventToCall = new ContractEvent()
                        {
                            Name = name
                        },
                        Parameters = paramList
                    };
                }
            }

            return null;
        }

        public List<ContractProperty> GetPropertiesFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var properties = new List<ContractProperty>();

            while (node != null)
            {
                properties.Add(GetPropertyFromXmlNode(node, nsmgr));
                node = node.SelectSingleNode($"gxml:next/gxml:block[@type=\"{ContractPropertyBlockType}\"]", nsmgr);
            }

            return properties;
        }

        public ContractProperty GetPropertyFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var variableNode = node.SelectSingleNode($"gxml:value[@name=\"{VariableFieldName}\"]/gxml:block[@type=\"{VariableDeclarationBlockType}\"]", nsmgr);
                return new ContractProperty()
                {
                    Visibility = GetVisibilityForElementNode(node, nsmgr),
                    Variable = GetVariableFormXmlNode(variableNode, nsmgr)
                };
            }

            return null;
        }

        public TransferCall GetTransferCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var addressNode = node.SelectSingleNode($"gxml:value[@name=\"{AddressValueName}\"]/gxml:block", nsmgr);
                var amountNode = node.SelectSingleNode($"gxml:value[@name=\"{AmountValueName}\"]/gxml:block", nsmgr);

                return new TransferCall()
                {
                    Address = GetAssignableFromXmlNode(addressNode, nsmgr),
                    ValueToTransfer = GetAssignableFromXmlNode(amountNode, nsmgr)
                };
            }

            return null;
        }

        public BalanceCall GetBalanceCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var addressNode = node.SelectSingleNode($"gxml:value[@name=\"{AddressValueName}\"]/gxml:block", nsmgr);

                return new BalanceCall()
                {
                    Address = GetAssignableFromXmlNode(addressNode, nsmgr)
                };
            }

            return null;
        }

        public string GetNameForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return GetValueFromFieldForElementNode(node, nsmgr, NameFieldName);
            }

            return null;
        }

        public bool GetAcceptsEthersForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                return GetBoolValueFromFieldForElementNode(node, nsmgr, AcceptsEthersFieldName);
            }

            return false;
        }

        public Modifier GetModifierForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var mName = GetValueFromFieldForElementNode(node, nsmgr, ModifierFieldName);
                if (!string.IsNullOrWhiteSpace(mName))
                {
                    return new Modifier()
                    {
                        Name = mName
                    };
                }
            }

            return null;
        }

        public ModificationType GetModificationTypeForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var mName = GetValueFromFieldForElementNode(node, nsmgr, StateModificationFieldName);
                if (!string.IsNullOrWhiteSpace(mName))
                {
                    return EnumMappers.MapBlocklyCodeToModificationType(mName);
                }
            }

            return ModificationType.None;
        }

        public BlockOrTransactionProperty GetBlockOrTransactionPropertyForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var propertyId = GetValueFromFieldForElementNode(node, nsmgr, ValueFieldName);
                return EnumMappers.MapBlocklyCodeToBlockOrTransactionProperty(propertyId);
            }

            throw new InvalidOperationException("Node is required in this function!");
        }

        public SolidityType? GeTypeForElementNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var typeId = GetValueFromFieldForElementNode(node, nsmgr, TypeFieldName);
                if (!string.IsNullOrWhiteSpace(typeId))
                {
                    return EnumMappers.MapBlocklyCodeToSolidityType(typeId);
                }
            }

            return null;
        }

        public bool GetBoolValueFromFieldForElementNode(XmlNode node, XmlNamespaceManager nsmgr, string fieldName)
        {
            return GetValueFromFieldForElementNode(node, nsmgr, fieldName) == BlocklyBoolTrue;
        }

        public string GetValueFromFieldForElementNode(XmlNode node, XmlNamespaceManager nsmgr, string fieldName)
        {
            if (node != null)
            {
                var nameNode = node.SelectSingleNode($"gxml:field[@name=\"{fieldName}\"]", nsmgr);
                if (nameNode != null)
                {
                    return nameNode.InnerText;
                }
            }
                
            return string.Empty;
        }

        public string GetNameForCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            if (node != null)
            {
                var mutationNode = node.SelectSingleNode($"gxml:mutation", nsmgr);
                if (mutationNode != null && mutationNode.Attributes != null && mutationNode.Attributes["name"] != null)
                {
                    return mutationNode.Attributes["name"].Value;
                }
            }

            return null;
        }

        public CallingParametersList GetParametersForCallFromXmlNode(XmlNode node, XmlNamespaceManager nsmgr)
        {
            var cpl = new List<IAssignable>();

            if (node != null)
            {
                var parametersCount = node.SelectNodes("gxml:mutation/gxml:arg", nsmgr).Count;

                for (int i = 0; i < parametersCount; i++)
                {
                    IAssignable toAdd = null;

                    var currentArgBlock = node.SelectSingleNode($"gxml:value[@name=\"ARG{i}\"]/gxml:block", nsmgr);
                    if (currentArgBlock != null)
                    {
                        toAdd = GetAssignableFromXmlNode(currentArgBlock, nsmgr);
                    }

                    cpl.Add(toAdd ?? new ConstantValue() { Value = string.Empty });
                }
            }

            return new CallingParametersList() { Parameters = cpl };
        }
    }
}

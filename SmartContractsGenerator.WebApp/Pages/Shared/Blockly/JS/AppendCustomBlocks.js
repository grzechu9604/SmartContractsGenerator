var iAssignables = ["variable", "operation", "constant_value", "special_value", "call_returnable_function", "balance_call", "logic_operation"];
var iValueContainers = ["variable", "variable_declaration"];
var solidityTypes = [["Bool", "0"], ["Int", "1"], ["UInt", "2"], ["Fixed", "3"], ["UFixed", "4"], ["Address", "5"], ["Address Payable", "6"], ["String", "7"]];
var constantTypes = [["Int", "1"], ["Fixed", "3"], ["String", "7"]];

Blockly.MyDynamicInputs = {
    allDefinitionsOfType_: function (root, type) {
        var blocks = root.getAllBlocks(false);
        var elements = [];
        for (var i = 0; i < blocks.length; i++) {
            if (blocks[i].type && blocks[i].type == type) {
                if (blocks[i].type == "contract_function") {
                    elements.push([blocks[i].getProcedureDef(), blocks[i].getFieldValue("ApplyReturns") == "TRUE"]);
                }
                else {
                    elements.push(blocks[i].getProcedureDef());
                }
            }
        }
        return elements;
    },

    allProcedures: function (root) {
        return Blockly.MyDynamicInputs.allDefinitionsOfType_(root, "contract_function");
    },
    allEvents: function (root) {
        return Blockly.MyDynamicInputs.allDefinitionsOfType_(root, "contract_event");
    },
    allModifiers: function (root) {
        return Blockly.MyDynamicInputs.allDefinitionsOfType_(root, "modifier");
    },
    copyVariable: function (variable) {
        return {
            type: variable.type, name: variable.name, id: variable.getId(), getId: function () { return this.id }
        };
    },
    mutationToDom: function (opt_paramIds) {
        var container = Blockly.utils.xml.createElement('mutation');
        if (opt_paramIds) {
            container.setAttribute('name', this.getFieldValue('NAME'));
        }
        for (var i = 0; i < this.argumentVarModels_.length; i++) {
            var parameter = Blockly.utils.xml.createElement('arg');
            var argModel = this.argumentVarModels_[i];
            parameter.setAttribute('name', argModel.name);
            parameter.setAttribute('varid', argModel.getId());
            parameter.setAttribute('type', argModel.type);
            if (opt_paramIds && this.paramIds_) {
                parameter.setAttribute('paramId', this.paramIds_[i]);
            }
            container.appendChild(parameter);
        }

        return container;
    },
    domToMutation: function (xmlElement) {
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        for (var i = 0, childNode; (childNode = xmlElement.childNodes[i]); i++) {
            if (childNode.nodeName.toLowerCase() == 'arg') {
                var varName = childNode.getAttribute('name');
                var varId = childNode.getAttribute('varid') || childNode.getAttribute('varId');
                this.arguments_.push(varName);
                var variable = Blockly.Variables.getOrCreateVariablePackage(
                    this.workspace, varId, varName, '');
                if (variable != null) {
                    var modelVariable = Blockly.MyDynamicInputs.copyVariable(variable);
                    modelVariable.type = childNode.getAttribute('type');
                    this.argumentVarModels_.push(modelVariable);
                } else {
                    console.log('Failed to create a variable with name ' + varName + ', ignoring.');
                }
            }
        }
        this.updateParams_();

        if (this.performCallersMutation) {
            Blockly.Procedures.mutateCallers(this);
        }
    },
    decompose: function (workspace) {
        var containerBlockNode = Blockly.utils.xml.createElement('block');
        containerBlockNode.setAttribute('type', 'my_procedures_mutatorcontainer');
        var statementNode = Blockly.utils.xml.createElement('statement');
        statementNode.setAttribute('name', 'STACK');
        containerBlockNode.appendChild(statementNode);

        var node = statementNode;
        for (var i = 0; i < this.argumentVarModels_.length; i++) {
            var argBlockNode = Blockly.utils.xml.createElement('block');
            argBlockNode.setAttribute('type', 'my_procedures_mutatorarg');
            var fieldNode = Blockly.utils.xml.createElement('field');
            fieldNode.setAttribute('name', 'NAME');
            var argumentName = Blockly.utils.xml.createTextNode(this.argumentVarModels_[i].name);
            fieldNode.appendChild(argumentName);
            argBlockNode.appendChild(fieldNode);

            fieldNode = Blockly.utils.xml.createElement('field');
            fieldNode.setAttribute('name', 'TYPE');
            var argumentType = Blockly.utils.xml.createTextNode(this.argumentVarModels_[i].type);
            fieldNode.appendChild(argumentType);
            argBlockNode.appendChild(fieldNode);

            var nextNode = Blockly.utils.xml.createElement('next');
            argBlockNode.appendChild(nextNode);

            node.appendChild(argBlockNode);
            node = nextNode;
        }

        var containerBlock = Blockly.Xml.domToBlock(containerBlockNode, workspace);

        // Initialize procedure's callers with blank IDs.
        if (this.performCallersMutation) {
            Blockly.Procedures.mutateCallers(this);
        }
        return containerBlock;
    },
    compose: function (containerBlock) {
        // Parameter list.
        this.arguments_ = [];
        this.paramIds_ = [];
        this.argumentVarModels_ = [];
        var paramBlock = containerBlock.getInputTargetBlock('STACK');
        while (paramBlock) {
            var varName = paramBlock.getFieldValue('NAME');
            this.arguments_.push(varName);
            var variable = this.workspace.getVariable(varName, '');
            var modelVariable = Blockly.MyDynamicInputs.copyVariable(variable);
            modelVariable.type = paramBlock.getFieldValue('TYPE');
            this.argumentVarModels_.push(modelVariable);

            this.paramIds_.push(paramBlock.id);
            paramBlock = paramBlock.nextConnection &&
                paramBlock.nextConnection.targetBlock();
        }
        this.updateParams_();
        if (this.performCallersMutation) {
            Blockly.Procedures.mutateCallers(this);
        }
    },
    populateElements(elementsList, blockType) {
        var xmlList = [];
        for (var i = 0; i < elementsList.length; i++) {
            var name = elementsList[i][0];
            var args = elementsList[i][1];
            var block = Blockly.utils.xml.createElement('block');
            block.setAttribute('type', blockType);
            block.setAttribute('gap', 16);
            var mutation = Blockly.utils.xml.createElement('mutation');
            mutation.setAttribute('name', name);
            block.appendChild(mutation);
            for (var j = 0; j < args.length; j++) {
                var arg = Blockly.utils.xml.createElement('arg');
                arg.setAttribute('name', args[j]);
                mutation.appendChild(arg);
            }
            xmlList.push(block);
        }
        return xmlList;
    }
};

Blockly.Blocks['contract'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Contract");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldTextInput("[insert contract name]"), "NAME");
        this.appendStatementInput("Properties")
            .setCheck("contract_property")
            .appendField("Properties");
        this.appendStatementInput("Events")
            .setCheck('contract_event')
            .appendField("Events");
        this.appendStatementInput("Modifiers")
            .setCheck('modifier')
            .appendField("Modifiers");
        this.appendStatementInput("Constructor")
            .setCheck("contract_constructor")
            .appendField("Constructor");
        this.appendStatementInput("FallbackFunction")
            .setCheck("fallback_function")
            .appendField("Default function");
        this.appendStatementInput("ReceiveFunction")
            .setCheck("receive_function")
            .appendField("On receive Ether function");
        this.appendStatementInput("Functions")
            .setCheck('contract_function')
            .appendField("Functions");
        this.setColour(0);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['contract_constructor'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Constructor")
            .appendField('', 'PARAMS')
            .appendField(new Blockly.FieldDropdown([["Public", "1"], ["Internal", "2"]]), "Visibility");
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction');
        this.setPreviousStatement(true, "contract_constructor");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.performCallersMutation = false;
    },

    updateParams_: Blockly.Blocks['procedures_defnoreturn'].updateParams_,
    mutationToDom: Blockly.MyDynamicInputs.mutationToDom,
    domToMutation: Blockly.MyDynamicInputs.domToMutation,
    decompose: Blockly.MyDynamicInputs.decompose,
    compose: Blockly.MyDynamicInputs.compose,
    getVars: Blockly.Blocks['procedures_defnoreturn'].getVars,
    getVarModels: Blockly.Blocks['procedures_defnoreturn'].getVarModels,
    renameVarById: Blockly.Blocks['procedures_defnoreturn'].renameVarById,
    updateVarName: Blockly.Blocks['procedures_defnoreturn'].updateVarName,
    displayRenamedVar_: Blockly.Blocks['procedures_defnoreturn'].displayRenamedVar_,
    customContextMenu: Blockly.Blocks['procedures_defnoreturn'].customContextMenu
};

Blockly.Blocks['contract_property'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Property")
            .appendField(new Blockly.FieldDropdown([["External", "0"], ["Public", "1"], ["Private", "3"], ["Internal", "2"]]), "Visibility");
        this.appendValueInput("Variable")
            .setCheck("variable_declaration")
            .appendField("Variable");
        this.setPreviousStatement(true, "contract_property");
        this.setNextStatement(true, "contract_property");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['contract_event'] = {
    init: function () {
        var nameField = new Blockly.FieldTextInput('unnamed',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField("Contract event")
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.setPreviousStatement(true, 'contract_event');
        this.setNextStatement(true, 'contract_event');
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.performCallersMutation = true;
    },

    updateParams_: Blockly.Blocks['procedures_defnoreturn'].updateParams_,
    mutationToDom: Blockly.MyDynamicInputs.mutationToDom,
    domToMutation: Blockly.MyDynamicInputs.domToMutation,
    decompose: Blockly.MyDynamicInputs.decompose,
    compose: Blockly.MyDynamicInputs.compose,
    getProcedureDef: Blockly.Blocks['procedures_defnoreturn'].getProcedureDef,
    getVars: Blockly.Blocks['procedures_defnoreturn'].getVars,
    getVarModels: Blockly.Blocks['procedures_defnoreturn'].getVarModels,
    renameVarById: Blockly.Blocks['procedures_defnoreturn'].renameVarById,
    updateVarName: Blockly.Blocks['procedures_defnoreturn'].updateVarName,
    displayRenamedVar_: Blockly.Blocks['procedures_defnoreturn'].displayRenamedVar_,
    customContextMenu: Blockly.Blocks['procedures_defnoreturn'].customContextMenu
};

Blockly.Blocks['constant_value'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Value: ")
            .appendField(new Blockly.FieldTextInput("[value]"), "value")
            .appendField(new Blockly.FieldDropdown(constantTypes), "TYPE");
        this.setOutput(true, "constant_value");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['assignment'] = {
    init: function () {
        this.appendValueInput("Destination")
            .setCheck(iValueContainers);
        this.appendDummyInput()
            .appendField("=");
        this.appendValueInput("Source")
            .setCheck(iAssignables);
        this.setPreviousStatement(true, ['IInstruction', 'assignment']);
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['event_call'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Emit")
            .appendField(this.id, 'NAME');
        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.quarkConnections_ = {};
        this.quarkIds_ = null;
        this.previousEnabledState_ = true;
    },

    getProcedureCall: Blockly.Blocks['procedures_callnoreturn'].getProcedureCall,
    renameProcedure: Blockly.Blocks['procedures_callnoreturn'].renameProcedure,
    setProcedureParameters_:
        Blockly.Blocks['procedures_callnoreturn'].setProcedureParameters_,
    updateShape_: Blockly.Blocks['procedures_callnoreturn'].updateShape_,
    mutationToDom: Blockly.Blocks['procedures_callnoreturn'].mutationToDom,
    domToMutation: Blockly.Blocks['procedures_callnoreturn'].domToMutation,
    getVarModels: Blockly.Blocks['procedures_callnoreturn'].getVarModels,
    //onchange: Blockly.Blocks['procedures_callnoreturn'].onchange,
    customContextMenu:
        Blockly.Blocks['procedures_callnoreturn'].customContextMenu,
    defType_: 'call_void_function'
};

Blockly.Blocks['break_statement'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Break loop")
        this.setPreviousStatement(true, 'IInstruction');
        this.setColour(120);
    }
};

Blockly.Blocks['call_returnable_function'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Execute")
            .appendField(this.id, 'NAME');
        this.setOutput(true, 'call_returnable_function');
        this.setColour(60);
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.quarkConnections_ = {};
        this.quarkIds_ = null;
        this.previousEnabledState_ = true;
    },

    getProcedureCall: Blockly.Blocks['procedures_callnoreturn'].getProcedureCall,
    renameProcedure: Blockly.Blocks['procedures_callnoreturn'].renameProcedure,
    setProcedureParameters_:
        Blockly.Blocks['procedures_callnoreturn'].setProcedureParameters_,
    updateShape_: Blockly.Blocks['procedures_callnoreturn'].updateShape_,
    mutationToDom: Blockly.Blocks['procedures_callnoreturn'].mutationToDom,
    domToMutation: Blockly.Blocks['procedures_callnoreturn'].domToMutation,
    getVarModels: Blockly.Blocks['procedures_callnoreturn'].getVarModels,
    //onchange: Blockly.Blocks['procedures_callnoreturn'].onchange,
    customContextMenu:
        Blockly.Blocks['procedures_callnoreturn'].customContextMenu,
    defType_: 'call_void_function'
};

Blockly.Blocks['call_void_function'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Execute")
            .appendField(this.id, 'NAME');
        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.quarkConnections_ = {};
        this.quarkIds_ = null;
        this.previousEnabledState_ = true;
    },

    getProcedureCall: Blockly.Blocks['procedures_callnoreturn'].getProcedureCall,
    renameProcedure: Blockly.Blocks['procedures_callnoreturn'].renameProcedure,
    setProcedureParameters_:
        Blockly.Blocks['procedures_callnoreturn'].setProcedureParameters_,
    updateShape_: Blockly.Blocks['procedures_callnoreturn'].updateShape_,
    mutationToDom: Blockly.Blocks['procedures_callnoreturn'].mutationToDom,
    domToMutation: Blockly.Blocks['procedures_callnoreturn'].domToMutation,
    getVarModels: Blockly.Blocks['procedures_callnoreturn'].getVarModels,
    //onchange: Blockly.Blocks['procedures_callnoreturn'].onchange,
    customContextMenu:
        Blockly.Blocks['procedures_callnoreturn'].customContextMenu,
    defType_: 'call_void_function'
};

Blockly.Blocks['condition'] = {
    init: function () {
        this.appendValueInput("condition")
            .setCheck("logic_operation")
            .appendField("Condition")
            .appendField("");
        this.setOutput(true, "condition");
        this.setColour(315);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['operation'] = {
    init: function () {
        this.appendValueInput("left_side")
            .setCheck(iAssignables);
        this.appendDummyInput()
            .appendField(new Blockly.FieldDropdown([["+", "0"], ["-", "1"], ["%", "2"], ["/", "3"], ["*", "4"]]), "Operator");
        this.appendValueInput("right_side")
            .setCheck(iAssignables);
        this.setOutput(true, "operation");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['logic_operation'] = {
    init: function () {
        var operatorsDropdown = new Blockly.FieldDropdown([["!", "5"], ["||", "6"], ["&&", "7"], ["^", "8"], ["==", "9"], ["!=", "10"], ["<", "11"], ["<=", "12"], [">", "13"], [">=", "14"]], function (option) {
            this.sourceBlock_.updateLeftSideInput_(option == 5);
        });

        this.appendValueInput("left_side")
            .setCheck(iAssignables);
        this.appendDummyInput("operator_input")
            .appendField(operatorsDropdown, "Operator");
        this.appendValueInput("right_side")
            .setCheck(iAssignables);
        this.setOutput(true, "logic_operation");
        this.setColour(315);
        this.setTooltip("");
        this.setHelpUrl("");
        this.setInputsInline(true);
    },

    updateLeftSideInput_: function (isUnaryOperator) {
        var inputExists = this.getInput('left_side');
        if (!isUnaryOperator) {
            if (!inputExists) {
                this.appendValueInput("left_side")
                    .setCheck(iAssignables);
                this.moveInputBefore('left_side', 'operator_input');
            }
        } else if (inputExists) {
            this.removeInput('left_side');
        }
    },
};

Blockly.Blocks['requirement'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Condition of execution");
        this.appendValueInput("condition")
            .setCheck("condition")
            .appendField("Condition");
        this.appendDummyInput()
            .appendField("Error message:")
            .appendField(new Blockly.FieldTextInput("[message]"), "ErrorMessage");
        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['variable'] = {
    init: function () {
        this.appendDummyInput()
            .appendField(new Blockly.FieldVariable(), "NAME");
        this.setOutput(true, "variable");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['contract_loop'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Loop");
        this.appendStatementInput("Initial_assignment")
            .setCheck("assignment")
            .appendField("Initial assignment");
        this.appendStatementInput("step_instruction")
            .setCheck("assignment")
            .appendField("Step instruction");
        this.appendValueInput("break_condition")
            .setCheck("condition")
            .appendField("Break contition");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction')
            .appendField("Instructions");
        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['if_statement'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Conditional instruction");
        this.appendValueInput("condition")
            .setCheck("condition")
            .appendField("Condition");
        this.appendStatementInput("true_instructions")
            .setCheck('IInstruction')
            .appendField("If true");
        this.appendStatementInput("false_instructions")
            .setCheck('IInstruction')
            .appendField("If false");
        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['contract_function'] = {
    init: function () {
        var modifierCheckbox = new Blockly.FieldCheckbox("FALSE", function (applyModifierChecked) {
            this.sourceBlock_.updateModifierInput_(applyModifierChecked == "TRUE");
        });
        var returnsCheckbox = new Blockly.FieldCheckbox("FALSE", function (applyReturnsChecked) {
            this.sourceBlock_.updateReturnsInput_(applyReturnsChecked == "TRUE");
        });
        var acceptsEthersCheckbox = new Blockly.FieldCheckbox("FALSE", function (acceptsEthersChecked) {
            this.sourceBlock_.updateStateModificationInput_(acceptsEthersChecked == "TRUE");
        });
        this.appendDummyInput()
            .appendField("Function")
            .appendField(new Blockly.FieldDropdown([["External", "0"], ["Public", "1"], ["Private", "3"], ["Internal", "2"]]), "Visibility");
        var nameField = new Blockly.FieldTextInput('unnamed',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField(Blockly.Msg['PROCEDURES_DEFRETURN_TITLE'])
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Accepts ethers")
            .appendField(acceptsEthersCheckbox, "AcceptsEthers");
        this.appendDummyInput()
            .appendField("Apply modifier")
            .appendField(modifierCheckbox, "ApplyModifier");
        this.appendDummyInput("StateModification")
            .appendField("State modification type")
            .appendField(new Blockly.FieldDropdown([["ReadWrite", "0"], ["ReadOnly", "1"], ["Calculation only", "2"]]), "StateModification");
        this.appendDummyInput("ApplyReturns")
            .appendField("Return value")
            .appendField(returnsCheckbox, "ApplyReturns");
        this.appendDummyInput("InstructionsLabel")
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction');
        this.setPreviousStatement(true, 'contract_function');
        this.setNextStatement(true, 'contract_function');
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.statementConnection_ = null;
        this.performCallersMutation = true;
    },

    updateModifierInput_: function (applyModifierInput) {
        var inputExists = this.getInput("Modifier");
        if (applyModifierInput) {
            if (!inputExists) {
                this.appendValueInput("Modifier")
                    .appendField("Modifier: ")
                    .setCheck(["modifier_appliance"]);

                if (this.getInput("StateModification")) {
                    this.moveInputBefore('Modifier', 'StateModification');
                }
                else {
                    this.moveInputBefore('Modifier', 'ApplyReturns');
                }
            }
        } else if (inputExists) {
            this.removeInput('Modifier');
        }
    },

    updateReturnsInput_: function (applyReturnsInput) {
        var inputExists = this.getInput("ReturningType");
        if (applyReturnsInput) {
            if (!inputExists) {
                this.appendDummyInput("ReturningType")
                    .appendField("Returning type")
                    .appendField(new Blockly.FieldDropdown(solidityTypes), "TYPE");
                this.moveInputBefore('ReturningType', 'InstructionsLabel');
            }
        } else if (inputExists) {
            this.removeInput('ReturningType');
        }
    },

    updateStateModificationInput_: function (acceptsEthersChecked) {
        var inputExists = this.getInput("StateModification");
        if (!acceptsEthersChecked) {
            if (!inputExists) {
                this.appendDummyInput("StateModification")
                    .appendField("State modification type")
                    .appendField(new Blockly.FieldDropdown([["ReadWrite", "0"], ["ReadOnly", "1"], ["Calculation only", "2"]]), "StateModification");
                this.moveInputBefore('StateModification', 'ApplyReturns');
            }
        } else if (inputExists) {
            this.removeInput('StateModification');
        }
    },

    updateParams_: Blockly.Blocks['procedures_defnoreturn'].updateParams_,
    mutationToDom: Blockly.MyDynamicInputs.mutationToDom,
    domToMutation: Blockly.MyDynamicInputs.domToMutation,
    decompose: Blockly.MyDynamicInputs.decompose,
    compose: Blockly.MyDynamicInputs.compose,
    getProcedureDef: Blockly.Blocks['procedures_defnoreturn'].getProcedureDef,
    getVars: Blockly.Blocks['procedures_defnoreturn'].getVars,
    getVarModels: Blockly.Blocks['procedures_defnoreturn'].getVarModels,
    renameVarById: Blockly.Blocks['procedures_defnoreturn'].renameVarById,
    updateVarName: Blockly.Blocks['procedures_defnoreturn'].updateVarName,
    displayRenamedVar_: Blockly.Blocks['procedures_defnoreturn'].displayRenamedVar_,
    customContextMenu: Blockly.Blocks['procedures_defnoreturn'].customContextMenu,
    callType_: 'procedures_callreturn'
};

Blockly.Blocks['modifier'] = {
    init: function () {
        var nameField = new Blockly.FieldTextInput('unnamed',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField("Modifier")
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction');
        this.setPreviousStatement(true, "modifier");
        this.setNextStatement(true, 'modifier');
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.performCallersMutation = true;
    },

    updateParams_: Blockly.Blocks['procedures_defnoreturn'].updateParams_,
    mutationToDom: Blockly.MyDynamicInputs.mutationToDom,
    domToMutation: Blockly.MyDynamicInputs.domToMutation,
    decompose: Blockly.MyDynamicInputs.decompose,
    compose: Blockly.MyDynamicInputs.compose,
    getProcedureDef: Blockly.Blocks['procedures_defnoreturn'].getProcedureDef,
    getVars: Blockly.Blocks['procedures_defnoreturn'].getVars,
    getVarModels: Blockly.Blocks['procedures_defnoreturn'].getVarModels,
    renameVarById: Blockly.Blocks['procedures_defnoreturn'].renameVarById,
    updateVarName: Blockly.Blocks['procedures_defnoreturn'].updateVarName,
    displayRenamedVar_: Blockly.Blocks['procedures_defnoreturn'].displayRenamedVar_,
    customContextMenu: Blockly.Blocks['procedures_defnoreturn'].customContextMenu
};

Blockly.Blocks['modifier_appliance'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Use modifier")
            .appendField(this.id, 'NAME');
        this.setOutput(true, 'modifier_appliance');
        this.setStyle('procedure_blocks');
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.quarkConnections_ = {};
        this.quarkIds_ = null;
        this.previousEnabledState_ = true;
        this.setColour(30);
    },

    getProcedureCall: Blockly.Blocks['procedures_callnoreturn'].getProcedureCall,
    renameProcedure: Blockly.Blocks['procedures_callnoreturn'].renameProcedure,
    setProcedureParameters_:
        Blockly.Blocks['procedures_callnoreturn'].setProcedureParameters_,
    updateShape_: Blockly.Blocks['procedures_callnoreturn'].updateShape_,
    mutationToDom: Blockly.Blocks['procedures_callnoreturn'].mutationToDom,
    domToMutation: Blockly.Blocks['procedures_callnoreturn'].domToMutation,
    getVarModels: Blockly.Blocks['procedures_callnoreturn'].getVarModels,
    //onchange: Blockly.Blocks['procedures_callnoreturn'].onchange,
    customContextMenu:
        Blockly.Blocks['procedures_callnoreturn'].customContextMenu,
    defType_: 'modifier_appliance'
};

Blockly.Blocks['variable_declaration'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Variable declaration");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable(), "NAME");
        this.appendDummyInput()
            .appendField("type:")
            .appendField(new Blockly.FieldDropdown(solidityTypes), "TYPE");
        this.setInputsInline(false);
        this.setOutput(true, "variable_declaration");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['special_value_call'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Special values:")
            .appendField(new Blockly.FieldDropdown([["Block Coinbase", "0"], ["Block Difficulty", "1"], ["BlockGaslimit", "2"], ["Block Number", "3"], ["Block Timestamp", "4"], ["Gasleft", "5"], ["Message Data", "6"], ["Message Sender", "7"], ["Message Sig", "8"], ["Message Value", "9"], ["Now", "10"], ["Transaction Gasprice", "11"], ["Transaction Origin", "12"]]), "value");
        this.setInputsInline(false);
        this.setOutput(true, "special_value");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['return'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Break function")

        var returnValueCheckbox = new Blockly.FieldCheckbox("FALSE", function (returnValueChecked) {
            this.sourceBlock_.updateReturnValueInput_(returnValueChecked == "TRUE");
        });
        this.appendDummyInput()
            .appendField("Return value")
            .appendField(returnValueCheckbox, "ReturnValueCheckbox");

        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(false, null);
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    },

    updateReturnValueInput_: function (returnValueChecked) {
        var inputExists = this.getInput("ReturnValue");
        if (returnValueChecked) {
            if (!inputExists) {
                this.appendValueInput("ReturnValue")
                    .appendField("Value")
                    .setCheck(iAssignables);
            }
        } else if (inputExists) {
            this.removeInput('ReturnValue');
        }
    },
};

Blockly.Blocks['fallback_function'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Default function");
        this.appendDummyInput()
            .appendField("Accepts ethers")
            .appendField(new Blockly.FieldCheckbox("FALSE"), "AcceptsEthers");
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction');
        this.setPreviousStatement(true, "fallback_function");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['receive_function'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("On receive Ether function");
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck('IInstruction');
        this.setPreviousStatement(true, "receive_function");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['balance_call'] = {
    init: function () {
        this.appendValueInput("Address")
            .setCheck(iAssignables)
            .appendField("Check balance");
        this.setOutput(true, "balance_call");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['transfer_call'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Transfer Ethers");

        this.appendValueInput("Address")
            .appendField("Address")
            .setCheck(iAssignables);

        this.appendValueInput("Amount")
            .appendField("Amount")
            .setCheck(iAssignables);

        this.setPreviousStatement(true, 'IInstruction');
        this.setNextStatement(true, 'IInstruction');
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    },

    updateReturnValueInput_: function (returnValueChecked) {
        var inputExists = this.getInput("ReturnValue");
        if (returnValueChecked) {
            if (!inputExists) {
                this.appendValueInput("ReturnValue")
                    .appendField("Value")
                    .setCheck(iAssignables);
            }
        } else if (inputExists) {
            this.removeInput('ReturnValue');
        }
    },
};

Blockly.Blocks['true_const'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("true")
        this.setOutput(true, "constant_value");
        this.setColour(60);
    }
};

Blockly.Blocks['false_const'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("false")
        this.setOutput(true, "constant_value");
        this.setColour(60);
    }
};

Blockly.Blocks['my_procedures_mutatorarg'] = {
    init: function () {
        var nameField = new Blockly.FieldTextInput(
            Blockly.Procedures.DEFAULT_ARG, this.validator_);
        nameField.oldShowEditorFn_ = nameField.showEditor_;
        var newShowEditorFn = function () {
            this.createdVariables_ = [];
            this.oldShowEditorFn_();
        };
        nameField.showEditor_ = newShowEditorFn;

        this.appendDummyInput()
            .appendField(Blockly.Msg['PROCEDURES_MUTATORARG_TITLE'])
            .appendField(nameField, 'NAME')
            .appendField("Type")
            .appendField(new Blockly.FieldDropdown(
                solidityTypes), "TYPE");
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setStyle('procedure_blocks');
        this.setTooltip(Blockly.Msg['PROCEDURES_MUTATORARG_TOOLTIP']);
        this.contextMenu = false;

        nameField.onFinishEditing_ = this.deleteIntermediateVars_;
        nameField.createdVariables_ = [];
        nameField.onFinishEditing_('x');
    },

    validator_: Blockly.Blocks['procedures_mutatorarg'].validator_,
    deleteIntermediateVars_: Blockly.Blocks['procedures_mutatorarg'].deleteIntermediateVars_
};

Blockly.Blocks['my_procedures_mutatorcontainer'] = {
    init: function () {
        this.appendDummyInput()
            .appendField(Blockly.Msg['PROCEDURES_MUTATORCONTAINER_TITLE']);
        this.appendStatementInput('STACK');
        this.setStyle('procedure_blocks');
        this.setTooltip(Blockly.Msg['PROCEDURES_MUTATORCONTAINER_TOOLTIP']);
        this.contextMenu = false;
    },
};

Blockly.StrExamples = [];

Blockly.StrExamples['counter'] = "<xml xmlns=\"https://developers.google.com/blockly/xml\"><variables><variable id=\"=0Xj?#Tp`y**Wyp*;_(w\">counter</variable></variables><block type=\"contract\" id=\"C;_1{SjWEy;i083CQ}2$\" x=\"157\" y=\"52\"><field name=\"NAME\">Counter</field><statement name=\"Properties\"><block type=\"contract_property\" id=\"C=Dlpz/.{@,(b)]y(xO_\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"363DjP[eP_2[Y?+DE5$[\"><field name=\"NAME\" id=\"=0Xj?#Tp`y**Wyp*;_(w\">counter</field><field name=\"TYPE\">2</field></block></value></block></statement><statement name=\"Functions\"><block type=\"contract_function\" id=\"`bq1sC44HOP%Z%YNPNR|\"><field name=\"Visibility\">1</field><field name=\"NAME\">Increment</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"StateModification\">0</field><field name=\"ApplyReturns\">FALSE</field><statement name=\"Instructions\"><block type=\"assignment\" id=\"$R3zzbRa0!=;zu[zG9#`\"><value name=\"Destination\"><block type=\"variable\" id=\"0wQlJg=_V^zIxVD28/}W\"><field name=\"NAME\" id=\"=0Xj?#Tp`y**Wyp*;_(w\">counter</field></block></value><value name=\"Source\"><block type=\"operation\" id=\":GNV:avib4T6xNi_?uve\"><field name=\"Operator\">0</field><value name=\"left_side\"><block type=\"variable\" id=\"~he)pnn5YTlH0Hr,#7J+\"><field name=\"NAME\" id=\"=0Xj?#Tp`y**Wyp*;_(w\">counter</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"Gi$P*b*6UYslTwkw:+xC\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></value></block></statement></block></statement></block></xml>";
Blockly.StrExamples['changeNotifier'] = "<xml xmlns=\"https://developers.google.com/blockly/xml\"><variables><variable id=\"oY@X;5GwO3oPY3:8Cenp\">oldValue</variable><variable id=\"zP!{5Qr,-*dlypiu1z8.\">newValue</variable><variable id=\"%Zu0[bet-~OY.jgt(S7!\">value</variable></variables><block type=\"contract\" id=\"%3:#+X+}zedb6=-$mYMW\" x=\"26\" y=\"47\"><field name=\"NAME\">ChangeNotifier</field><statement name=\"Properties\"><block type=\"contract_property\" id=\"CP;+m}/,^r=kF+.uMJrT\"><field name=\"Visibility\">3</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"O93U|%_f!MIuyD.VrTIT\"><field name=\"NAME\" id=\"%Zu0[bet-~OY.jgt(S7!\">value</field><field name=\"TYPE\">1</field></block></value></block></statement><statement name=\"Events\"><block type=\"contract_event\" id=\"QCh@7rQA8!]pN1Ek.an,\"><mutation><arg name=\"oldValue\" varid=\"oY@X;5GwO3oPY3:8Cenp\" type=\"1\"/><arg name=\"newValue\" varid=\"zP!{5Qr,-*dlypiu1z8.\" type=\"1\"/></mutation><field name=\"NAME\">valueChanged</field></block></statement><statement name=\"Functions\"><block type=\"contract_function\" id=\"+b;7q{cNmAJMW%?5ctx0\"><mutation><arg name=\"newValue\" varid=\"zP!{5Qr,-*dlypiu1z8.\" type=\"1\"/></mutation><field name=\"Visibility\">1</field><field name=\"NAME\">setValue</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"StateModification\">0</field><field name=\"ApplyReturns\">FALSE</field><statement name=\"Instructions\"><block type=\"if_statement\" id=\"DFuI=uas1nRC)r)MN[?F\"><value name=\"condition\"><block type=\"condition\" id=\"*zDMsh/3TVU7I$$r]J{y\"><value name=\"condition\"><block type=\"logic_operation\" id=\"I`DAjgd=*2MFt?k0Y,?p\"><field name=\"Operator\">10</field><value name=\"left_side\"><block type=\"variable\" id=\"$i.5u_/20Tvu9x{QyR9M\"><field name=\"NAME\" id=\"%Zu0[bet-~OY.jgt(S7!\">value</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"T./n|xSmH[~b4u3H1t+/\"><field name=\"NAME\" id=\"zP!{5Qr,-*dlypiu1z8.\">newValue</field></block></value></block></value></block></value><statement name=\"true_instructions\"><block type=\"event_call\" id=\"E}+/3:nOA1|vq@E8,t*`\"><mutation name=\"valueChanged\"><arg name=\"oldValue\"/><arg name=\"newValue\"/></mutation><value name=\"ARG0\"><block type=\"variable\" id=\",qFLDw:;ydWvZgQ_]]j+\"><field name=\"NAME\" id=\"%Zu0[bet-~OY.jgt(S7!\">value</field></block></value><value name=\"ARG1\"><block type=\"variable\" id=\"j%b^Pu~jJNL/*z/Se!zv\"><field name=\"NAME\" id=\"zP!{5Qr,-*dlypiu1z8.\">newValue</field></block></value><next><block type=\"assignment\" id=\"+Y#KtW8p)/GYQyU/,iuW\"><value name=\"Destination\"><block type=\"variable\" id=\"nGsdDCz}(lr58D=rxf;R\"><field name=\"NAME\" id=\"%Zu0[bet-~OY.jgt(S7!\">value</field></block></value><value name=\"Source\"><block type=\"variable\" id=\"qevXdD/!)6yOpF~L!(@C\"><field name=\"NAME\" id=\"zP!{5Qr,-*dlypiu1z8.\">newValue</field></block></value></block></next></block></statement></block></statement></block></statement></block></xml>";
Blockly.StrExamples['piggybank'] = "<xml xmlns=\"https://developers.google.com/blockly/xml\"><variables><variable id=\"]^ca(vk5q$KC9(e[Gq?^\">owner</variable><variable id=\"UpD52=NNhL[x9kt.Stv5\">amount</variable><variable id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</variable></variables><block type=\"contract\" id=\"Q5#Z2DQBW([^D,m*cm#$\" x=\"10\" y=\"64\"><field name=\"NAME\">Piggybank</field><statement name=\"Properties\"><block type=\"contract_property\" id=\"|iU=ch-[sr6Z7n}15h/9\"><field name=\"Visibility\">3</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"jRCs?UZD,gApCN@i70F/\"><field name=\"NAME\" id=\"]^ca(vk5q$KC9(e[Gq?^\">owner</field><field name=\"TYPE\">6</field></block></value><next><block type=\"contract_property\" id=\"Deq)upCze#/?2FBya%vF\"><field name=\"Visibility\">3</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"z60z#rdD3.QmpEGq^HCf\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field><field name=\"TYPE\">2</field></block></value></block></next></block></statement><statement name=\"Modifiers\"><block type=\"modifier\" id=\"fm=][/l,b=_:,RU5r(^b\"><field name=\"NAME\">onlyOwner</field><statement name=\"Instructions\"><block type=\"requirement\" id=\"^8For#f=4.]VAst!-u#8\"><field name=\"ErrorMessage\">Only owner can perform this action</field><value name=\"condition\"><block type=\"condition\" id=\"Jxq,C3TR}n{d=aibB+]A\"><value name=\"condition\"><block type=\"logic_operation\" id=\"524`bMXwJ[[.co5L`B4v\"><field name=\"Operator\">9</field><value name=\"left_side\"><block type=\"variable\" id=\"]`[eoj~d.V8ssbJHDd/=\"><field name=\"NAME\" id=\"]^ca(vk5q$KC9(e[Gq?^\">owner</field></block></value><value name=\"right_side\"><block type=\"special_value_call\" id=\"LT$3@:t2fyVxbq$juhs*\"><field name=\"value\">7</field></block></value></block></value></block></value></block></statement></block></statement><statement name=\"Constructor\"><block type=\"contract_constructor\" id=\"$!enV)XB^EiYc%,LeY!C\"><field name=\"Visibility\">1</field><statement name=\"Instructions\"><block type=\"assignment\" id=\"1LLr?lbTi=,KE|t})bB_\"><value name=\"Destination\"><block type=\"variable\" id=\"n87#t|jn2pYHxB1U([MS\"><field name=\"NAME\" id=\"]^ca(vk5q$KC9(e[Gq?^\">owner</field></block></value><value name=\"Source\"><block type=\"special_value_call\" id=\"M[WFaByt:WzWYlklR.%B\"><field name=\"value\">7</field></block></value><next><block type=\"assignment\" id=\"wnQ2]XQ{CzHL};.ve5^V\"><value name=\"Destination\"><block type=\"variable\" id=\"suS5^[!uQwD8K}ZvR$r4\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value><value name=\"Source\"><block type=\"constant_value\" id=\"^;qdh]]Zacj!6p%SOkRB\"><field name=\"value\">0</field><field name=\"TYPE\">1</field></block></value></block></next></block></statement></block></statement><statement name=\"Functions\"><block type=\"contract_function\" id=\"VOat-EfzLM6cU9)xfiN-\"><field name=\"Visibility\">1</field><field name=\"NAME\">throwIn</field><field name=\"AcceptsEthers\">TRUE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"ApplyReturns\">FALSE</field><statement name=\"Instructions\"><block type=\"assignment\" id=\"|/b${owqGXDSr^bA2y5D\"><value name=\"Destination\"><block type=\"variable\" id=\"dD2PR-Iu-]9mnO}DHr27\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value><value name=\"Source\"><block type=\"operation\" id=\"0eu626;%7|!c+KJFt0HL\"><field name=\"Operator\">0</field><value name=\"left_side\"><block type=\"variable\" id=\")bl$$_uloN/gxikeej*#\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value><value name=\"right_side\"><block type=\"special_value_call\" id=\"j~u,?xK;,Q^Iv9UrRNy)\"><field name=\"value\">9</field></block></value></block></value></block></statement><next><block type=\"contract_function\" id=\"4Cr*#*RE/NKV+#FiESuT\"><mutation><arg name=\"amount\" varid=\"UpD52=NNhL[x9kt.Stv5\" type=\"2\"/></mutation><field name=\"Visibility\">1</field><field name=\"NAME\">withdraw</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">TRUE</field><field name=\"StateModification\">0</field><field name=\"ApplyReturns\">FALSE</field><value name=\"Modifier\"><block type=\"modifier_appliance\" id=\"l0V@iV-.ASygX2GrVx%L\"><mutation name=\"onlyOwner\"/></block></value><statement name=\"Instructions\"><block type=\"requirement\" id=\"UJj4z8r]j!a#~Pjw@lH=\"><field name=\"ErrorMessage\">You don't have enough Ethers!</field><value name=\"condition\"><block type=\"condition\" id=\"r%+3[Ur~v|UUZ0I%E+FG\"><value name=\"condition\"><block type=\"logic_operation\" id=\".`vP*};V)?e`CIr=1m}P\"><field name=\"Operator\">12</field><value name=\"left_side\"><block type=\"variable\" id=\"f}0S4[5ZrNaT,0k0hI1T\"><field name=\"NAME\" id=\"UpD52=NNhL[x9kt.Stv5\">amount</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"d?~%L8FixWcom/{G(UD|\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value></block></value></block></value><next><block type=\"requirement\" id=\"$HfTn-F5B+W#j$Q/G:Hu\"><field name=\"ErrorMessage\">Amount must be greater than 0!</field><value name=\"condition\"><block type=\"condition\" id=\"eBeNC0]Cb{f%z6EBuf/,\"><value name=\"condition\"><block type=\"logic_operation\" id=\"zi]zwb8p%y$iN%4x_*~Y\"><field name=\"Operator\">13</field><value name=\"left_side\"><block type=\"variable\" id=\"g_1MyR)ZJ*GQ~9]3$ZTM\"><field name=\"NAME\" id=\"UpD52=NNhL[x9kt.Stv5\">amount</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"/u!ARVI|s-V`V]a]uJuN\"><field name=\"value\">0</field><field name=\"TYPE\">1</field></block></value></block></value></block></value><next><block type=\"assignment\" id=\"`{cJnPA%2aWsIYefF~%P\"><value name=\"Destination\"><block type=\"variable\" id=\"LEo48](4%@jopIA|bRL@\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value><value name=\"Source\"><block type=\"operation\" id=\"{VP=idDu^4ky/F$Cvh{B\"><field name=\"Operator\">1</field><value name=\"left_side\"><block type=\"variable\" id=\"KvDez3=?k+M[zWMH6E]y\"><field name=\"NAME\" id=\"+bG{U$Bl[yyma;T}.3;*\">currentBalance</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"p_E${ZV)3HSwowoKW}.G\"><field name=\"NAME\" id=\"UpD52=NNhL[x9kt.Stv5\">amount</field></block></value></block></value><next><block type=\"transfer_call\" id=\"BLd4(;w+#^aZ-ez64q:p\"><value name=\"Address\"><block type=\"variable\" id=\"3L$gKXs2gquxi*|b0,ia\"><field name=\"NAME\" id=\"]^ca(vk5q$KC9(e[Gq?^\">owner</field></block></value><value name=\"Amount\"><block type=\"variable\" id=\"IEoa;uJ2#}HlrC3[.u[A\"><field name=\"NAME\" id=\"UpD52=NNhL[x9kt.Stv5\">amount</field></block></value></block></next></block></next></block></next></block></statement></block></next></block></statement></block></xml>";
Blockly.StrExamples['factorial'] = "<xml xmlns=\"https://developers.google.com/blockly/xml\"><variables><variable id=\"`*3PgUmRiNTxU/{U?d|P\">value</variable><variable id=\"a})20zgghUCo?`{i{|!a\">valueToReturn</variable><variable id=\"Qx3{xb*AEB+DIQ}{$oae\">i</variable></variables><block type=\"contract\" id=\"JzIEL}wy!MBd,H%Sig!K\" x=\"135\" y=\"48\"><field name=\"NAME\">Factorial</field><statement name=\"Functions\"><block type=\"contract_function\" id=\"FB8`5uz,bOd=cAHE7NX7\"><mutation><arg name=\"value\" varid=\"`*3PgUmRiNTxU/{U?d|P\" type=\"1\"/></mutation><field name=\"Visibility\">1</field><field name=\"NAME\">usingRecursion</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"StateModification\">2</field><field name=\"ApplyReturns\">TRUE</field><field name=\"TYPE\">1</field><statement name=\"Instructions\"><block type=\"if_statement\" id=\"%H;vr_y~Crf-pG)mCHn@\"><value name=\"condition\"><block type=\"condition\" id=\"vRD8[y6j,Q9,VY@2*-NZ\"><value name=\"condition\"><block type=\"logic_operation\" id=\"PJ]}B5,T]~t~B{W~CRp{\"><field name=\"Operator\">9</field><value name=\"left_side\"><block type=\"variable\" id=\"rjAaH4Y{U.NG7dD1hF~p\"><field name=\"NAME\" id=\"`*3PgUmRiNTxU/{U?d|P\">value</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"(%b5PR!t@r)1*720!IMn\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></value></block></value><statement name=\"true_instructions\"><block type=\"return\" id=\"No+Z5ZmJQ-G6bV5Mz|T8\"><field name=\"ReturnValueCheckbox\">TRUE</field><value name=\"ReturnValue\"><block type=\"constant_value\" id=\"^*_pjKAv}~c+bP_4G@79\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></statement><statement name=\"false_instructions\"><block type=\"return\" id=\"JS_DVd;rNvT/x6h^ek.h\"><field name=\"ReturnValueCheckbox\">TRUE</field><value name=\"ReturnValue\"><block type=\"operation\" id=\"K7aI[/BCV4qnQP]`8X7{\"><field name=\"Operator\">4</field><value name=\"left_side\"><block type=\"variable\" id=\"wSEqfV-0PfJao}uEPL:H\"><field name=\"NAME\" id=\"`*3PgUmRiNTxU/{U?d|P\">value</field></block></value><value name=\"right_side\"><block type=\"call_returnable_function\" id=\"4+2WAV%L9[Dr9.xI!%2_\"><mutation name=\"usingRecursion\"><arg name=\"value\"/></mutation><value name=\"ARG0\"><block type=\"operation\" id=\"eB([Cc-N_2YIDc7,.X/~\"><field name=\"Operator\">1</field><value name=\"left_side\"><block type=\"variable\" id=\"K.riiohkY(,H!Fh?Gv1F\"><field name=\"NAME\" id=\"`*3PgUmRiNTxU/{U?d|P\">value</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"T2@!4*meo1~O3gTs9/;N\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></value></block></value></block></value></block></statement></block></statement><next><block type=\"contract_function\" id=\"75;AycP^5N9eh13Bw39T\"><mutation><arg name=\"value\" varid=\"`*3PgUmRiNTxU/{U?d|P\" type=\"1\"/></mutation><field name=\"Visibility\">1</field><field name=\"NAME\">usingIteration</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"StateModification\">2</field><field name=\"ApplyReturns\">TRUE</field><field name=\"TYPE\">1</field><statement name=\"Instructions\"><block type=\"assignment\" id=\"}tE3WtM;V6}M#([g#tfZ\"><value name=\"Destination\"><block type=\"variable_declaration\" id=\"3+O[dB%C#=msC@V`h3hr\"><field name=\"NAME\" id=\"a})20zgghUCo?`{i{|!a\">valueToReturn</field><field name=\"TYPE\">1</field></block></value><value name=\"Source\"><block type=\"constant_value\" id=\"_Ms.4)628MCs_ykCm!(-\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value><next><block type=\"contract_loop\" id=\"=Oltul~@.~|9_/t?^]]Z\"><statement name=\"Initial_assignment\"><block type=\"assignment\" id=\"l.5bq5YJ]e!8bj4=;HXi\"><value name=\"Destination\"><block type=\"variable_declaration\" id=\"KvN,*NF~3yaruGsF=4Qt\"><field name=\"NAME\" id=\"Qx3{xb*AEB+DIQ}{$oae\">i</field><field name=\"TYPE\">1</field></block></value><value name=\"Source\"><block type=\"constant_value\" id=\"l2HmV3i3y,D7^i*:w`h(\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></statement><statement name=\"step_instruction\"><block type=\"assignment\" id=\",s[NX2ga)R^Ew9GV/fY=\"><value name=\"Destination\"><block type=\"variable\" id=\"ZX#J2uDy,GfSDIR5Sx?p\"><field name=\"NAME\" id=\"Qx3{xb*AEB+DIQ}{$oae\">i</field></block></value><value name=\"Source\"><block type=\"operation\" id=\"f!yKY![ZA`Q@y||5cY=}\"><field name=\"Operator\">0</field><value name=\"left_side\"><block type=\"variable\" id=\"rm,C[}7|E;+YH7+d1WwD\"><field name=\"NAME\" id=\"Qx3{xb*AEB+DIQ}{$oae\">i</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"O5jjGYUOE~U,U)@|Xf=;\"><field name=\"value\">1</field><field name=\"TYPE\">1</field></block></value></block></value></block></statement><value name=\"break_condition\"><block type=\"condition\" id=\"=*nZA{ddV8{TMm9PFDAr\"><value name=\"condition\"><block type=\"logic_operation\" id=\"thLnnD($FlN!;;HXs9Rd\"><field name=\"Operator\">12</field><value name=\"left_side\"><block type=\"variable\" id=\"3LWbYlzR8,G$@yZgOFnV\"><field name=\"NAME\" id=\"Qx3{xb*AEB+DIQ}{$oae\">i</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"qS-^,$msN(MYOOtxN}D|\"><field name=\"NAME\" id=\"`*3PgUmRiNTxU/{U?d|P\">value</field></block></value></block></value></block></value><statement name=\"Instructions\"><block type=\"assignment\" id=\"N55U^rhE|ss{m5dO/2Fe\"><value name=\"Destination\"><block type=\"variable\" id=\"w[`ay}kH9;zy=Nv++^}!\"><field name=\"NAME\" id=\"a})20zgghUCo?`{i{|!a\">valueToReturn</field></block></value><value name=\"Source\"><block type=\"operation\" id=\"nj*?W!!m+MI0w~G/G^h,\"><field name=\"Operator\">4</field><value name=\"left_side\"><block type=\"variable\" id=\"Wm+.O3{!6d6[`JMS3II)\"><field name=\"NAME\" id=\"a})20zgghUCo?`{i{|!a\">valueToReturn</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"Bf]hKeO%SC{%L%fF/3.x\"><field name=\"NAME\" id=\"Qx3{xb*AEB+DIQ}{$oae\">i</field></block></value></block></value></block></statement><next><block type=\"return\" id=\"U%Bq$TqYkfaDoGd=OM2H\"><field name=\"ReturnValueCheckbox\">TRUE</field><value name=\"ReturnValue\"><block type=\"variable\" id=\"7eYC6L8dBgzFpHYQ/+If\"><field name=\"NAME\" id=\"a})20zgghUCo?`{i{|!a\">valueToReturn</field></block></value></block></next></block></next></block></statement></block></next></block></statement></block></xml>";
Blockly.StrExamples['simlpeAuction'] = "<xml xmlns=\"https://developers.google.com/blockly/xml\"><variables><variable id=\"YnGV![r34]Y0`NQ~TnJa\">amount</variable><variable id=\"4S77[Q(mbtuTk}(eoqa#\">bidder</variable><variable id=\"qu|:hH,,m6K!FAi{uy]9\">_beneficiary</variable><variable id=\"W::E2tnhc5V|rjb^0cz-\">_biddingTime</variable><variable id=\"bU)RN.QE!t./vM.LV|/6\">isActiveAuction</variable><variable id=\"|pr-}ECv_%`C$..=?Eqr\">winner</variable><variable id=\"ky:U{CO]]le=L`Yybg^F\">beneficiary</variable><variable id=\"S7G8ud);0Dg$7s*;Z`)d\">auctionEndTime</variable><variable id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</variable><variable id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</variable></variables><block type=\"contract\" id=\"*30)BG4-j391gFO4Y,hk\" x=\"132\" y=\"38\"><field name=\"NAME\">SimpleAuction</field><statement name=\"Properties\"><block type=\"contract_property\" id=\"8LC,=c;@LLq+V4!CFv.6\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"ofYyE?TGyIAvHgs!rR0s\"><field name=\"NAME\" id=\"bU)RN.QE!t./vM.LV|/6\">isActiveAuction</field><field name=\"TYPE\">0</field></block></value><next><block type=\"contract_property\" id=\"s[yj4A8IHJOxOABTB8@[\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"jPGZb,#E2dfK=r|.L42U\"><field name=\"NAME\" id=\"ky:U{CO]]le=L`Yybg^F\">beneficiary</field><field name=\"TYPE\">6</field></block></value><next><block type=\"contract_property\" id=\"kNvX%3~6n7XW=b%f}S.F\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"#JnOX(2Yb1D/GRWc9DI?\"><field name=\"NAME\" id=\"S7G8ud);0Dg$7s*;Z`)d\">auctionEndTime</field><field name=\"TYPE\">2</field></block></value><next><block type=\"contract_property\" id=\":fGiV|5-XC0cEdSLNh*n\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"urU]h]Fin{wRL6HUUy{x\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field><field name=\"TYPE\">2</field></block></value><next><block type=\"contract_property\" id=\"Z^*sdx}QFe`#=acH8wrv\"><field name=\"Visibility\">1</field><value name=\"Variable\"><block type=\"variable_declaration\" id=\"mWSWk9uxmuvU7M148bSP\"><field name=\"NAME\" id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</field><field name=\"TYPE\">6</field></block></value></block></next></block></next></block></next></block></next></block></statement><statement name=\"Events\"><block type=\"contract_event\" id=\";0j{C[[:?siDgU3UJmXp\"><mutation><arg name=\"amount\" varid=\"YnGV![r34]Y0`NQ~TnJa\" type=\"2\"/><arg name=\"bidder\" varid=\"4S77[Q(mbtuTk}(eoqa#\" type=\"6\"/></mutation><field name=\"NAME\">highestBidIncrease</field><next><block type=\"contract_event\" id=\"kz8_uRs%LN6G~putmLBb\"><mutation><arg name=\"amount\" varid=\"YnGV![r34]Y0`NQ~TnJa\" type=\"2\"/><arg name=\"winner\" varid=\"|pr-}ECv_%`C$..=?Eqr\" type=\"6\"/></mutation><field name=\"NAME\">auctionEnded</field></block></next></block></statement><statement name=\"Constructor\"><block type=\"contract_constructor\" id=\"M^ZsusI6v5:dFBon_{jz\"><mutation><arg name=\"_beneficiary\" varid=\"qu|:hH,,m6K!FAi{uy]9\" type=\"6\"/><arg name=\"_biddingTime\" varid=\"W::E2tnhc5V|rjb^0cz-\" type=\"2\"/></mutation><field name=\"Visibility\">1</field><statement name=\"Instructions\"><block type=\"assignment\" id=\"Z2li,SoZ^Z=@[W.aM1ah\"><value name=\"Destination\"><block type=\"variable\" id=\"NE7O!y/J%E|d,0y|bF5~\"><field name=\"NAME\" id=\"ky:U{CO]]le=L`Yybg^F\">beneficiary</field></block></value><value name=\"Source\"><block type=\"variable\" id=\"gfQ2$[`.ZZc(yRQC,NJ,\"><field name=\"NAME\" id=\"qu|:hH,,m6K!FAi{uy]9\">_beneficiary</field></block></value><next><block type=\"assignment\" id=\":?mZob8_-cLHHanh!,Os\"><value name=\"Destination\"><block type=\"variable\" id=\"qJDa+xWXNtD@xi9}y;Ol\"><field name=\"NAME\" id=\"S7G8ud);0Dg$7s*;Z`)d\">auctionEndTime</field></block></value><value name=\"Source\"><block type=\"variable\" id=\"_jNUl+N:gBleEJ}$0#,5\"><field name=\"NAME\" id=\"W::E2tnhc5V|rjb^0cz-\">_biddingTime</field></block></value><next><block type=\"assignment\" id=\"RHVz49[[kK~`shM6clKd\"><value name=\"Destination\"><block type=\"variable\" id=\"Prsd,h,[;P{*POK{^J!7\"><field name=\"NAME\" id=\"bU)RN.QE!t./vM.LV|/6\">isActiveAuction</field></block></value><value name=\"Source\"><block type=\"true_const\" id=\"7l/ZiO@|ds.`FALy6_WG\"/></value></block></next></block></next></block></statement></block></statement><statement name=\"Functions\"><block type=\"contract_function\" id=\"j`1i2v?L+tqKBC(o*@%J\"><field name=\"Visibility\">1</field><field name=\"NAME\">bid</field><field name=\"AcceptsEthers\">TRUE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"ApplyReturns\">FALSE</field><statement name=\"Instructions\"><block type=\"requirement\" id=\"G3%8lwRUCy/St7@@qZ1?\"><field name=\"ErrorMessage\">Bid value too small</field><value name=\"condition\"><block type=\"condition\" id=\"uenf~*}ga/X*}VO7!af^\"><value name=\"condition\"><block type=\"logic_operation\" id=\"3iZ0nO=)bKt^RAqB/vIR\"><field name=\"Operator\">13</field><value name=\"left_side\"><block type=\"special_value_call\" id=\"C=A9^n:FN)}C]jR]lz=x\"><field name=\"value\">9</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"D{}$x(7g+2@[,Cfu=[N9\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value></block></value></block></value><next><block type=\"requirement\" id=\"7o!:581RIl[DR0}6f%N,\"><field name=\"ErrorMessage\">Too late</field><value name=\"condition\"><block type=\"condition\" id=\"CW|_7F0|/^~X%,)G%x.{\"><value name=\"condition\"><block type=\"logic_operation\" id=\"*[{Ke*kqOEKS~`DvjV-/\"><field name=\"Operator\">11</field><value name=\"left_side\"><block type=\"special_value_call\" id=\":HkJ-X}`2{iPvn:6|!LW\"><field name=\"value\">10</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"Z.=772xWmrjO,E-z6-#S\"><field name=\"NAME\" id=\"S7G8ud);0Dg$7s*;Z`)d\">auctionEndTime</field></block></value></block></value></block></value><next><block type=\"if_statement\" id=\"Qp(T/Yxb4*`4jngz2bh{\"><value name=\"condition\"><block type=\"condition\" id=\",|E()y-aS%L]?.*Q4`@E\"><value name=\"condition\"><block type=\"logic_operation\" id=\"q/##1(~e/`X.cAvz7TIq\"><field name=\"Operator\">13</field><value name=\"left_side\"><block type=\"variable\" id=\"H:v#rfqqhNcTfNbxS8{(\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"~pgt{Hf(6QTol*9D~dz4\"><field name=\"value\">0</field><field name=\"TYPE\">1</field></block></value></block></value></block></value><statement name=\"true_instructions\"><block type=\"transfer_call\" id=\"tp;n+#AhnJ+KPZ$EUgYk\"><value name=\"Address\"><block type=\"variable\" id=\"9)VQ%A86P(CPFr6y`hLb\"><field name=\"NAME\" id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</field></block></value><value name=\"Amount\"><block type=\"variable\" id=\"^:Nlb:HD:RQO8%*yd$SU\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value></block></statement><next><block type=\"assignment\" id=\")HM2s.QC-YlU)#^wo[oH\"><value name=\"Destination\"><block type=\"variable\" id=\"#$#Y$34OXS32K8KI?0n[\"><field name=\"NAME\" id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</field></block></value><value name=\"Source\"><block type=\"special_value_call\" id=\"Yx*]}P@5)r*]OT[f6J@!\"><field name=\"value\">7</field></block></value><next><block type=\"assignment\" id=\"fQ{%#4;H%)gRJ$S(t(Uc\"><value name=\"Destination\"><block type=\"variable\" id=\"Y-NZ7a{y|sm3%E,i@07!\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><value name=\"Source\"><block type=\"special_value_call\" id=\"P;.V44}Z`9ix+lYB!AE7\"><field name=\"value\">9</field></block></value><next><block type=\"event_call\" id=\"p|tw_`EH=+TcP_}nKUXU\"><mutation name=\"highestBidIncrease\"><arg name=\"amount\"/><arg name=\"bidder\"/></mutation><value name=\"ARG0\"><block type=\"variable\" id=\",`gp)GLf5Uh,,+^_L;zI\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><value name=\"ARG1\"><block type=\"variable\" id=\"l^FTWo2o+}(UdJi4UB#e\"><field name=\"NAME\" id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</field></block></value></block></next></block></next></block></next></block></next></block></next></block></statement><next><block type=\"contract_function\" id=\"N[GQzlF$~+k`B:~E:cQL\"><field name=\"Visibility\">1</field><field name=\"NAME\">endAuction</field><field name=\"AcceptsEthers\">FALSE</field><field name=\"ApplyModifier\">FALSE</field><field name=\"StateModification\">0</field><field name=\"ApplyReturns\">FALSE</field><statement name=\"Instructions\"><block type=\"requirement\" id=\"2K-4%#D~^STGAWupBasQ\"><field name=\"ErrorMessage\">Too early</field><value name=\"condition\"><block type=\"condition\" id=\"ddVdw3,^aNCKl8lk,q#.\"><value name=\"condition\"><block type=\"logic_operation\" id=\"X$8E;v[bT3vBY^ZI^Ol?\"><field name=\"Operator\">14</field><value name=\"left_side\"><block type=\"special_value_call\" id=\"tIz3mb3Zu)r%j~6atp?5\"><field name=\"value\">10</field></block></value><value name=\"right_side\"><block type=\"variable\" id=\"W`rL3.0`%|y%tS)_FpYT\"><field name=\"NAME\" id=\"S7G8ud);0Dg$7s*;Z`)d\">auctionEndTime</field></block></value></block></value></block></value><next><block type=\"requirement\" id=\"9roF*PkiASSe0NU;=oqk\"><field name=\"ErrorMessage\">The auction has been already deactivated</field><value name=\"condition\"><block type=\"condition\" id=\"VxHcnc#6/vU0vD3*ELpg\"><value name=\"condition\"><block type=\"logic_operation\" id=\"RUP8|p%pYH9S*hh_[DKA\"><field name=\"Operator\">9</field><value name=\"left_side\"><block type=\"variable\" id=\")iA6F)cU}mb):](v2Diz\"><field name=\"NAME\" id=\"bU)RN.QE!t./vM.LV|/6\">isActiveAuction</field></block></value><value name=\"right_side\"><block type=\"true_const\" id=\"iQXR0f#yC+]9*n$snC0H\"/></value></block></value></block></value><next><block type=\"assignment\" id=\"JH$r=(AhbH?+%g.%MZH~\"><value name=\"Destination\"><block type=\"variable\" id=\"ze%lDOucLbCELrDLa8/T\"><field name=\"NAME\" id=\"bU)RN.QE!t./vM.LV|/6\">isActiveAuction</field></block></value><value name=\"Source\"><block type=\"false_const\" id=\"Dde,FIWMT0%CA#ce??o}\"/></value><next><block type=\"if_statement\" id=\"3$T;h|?BdZ-zzpG8}n1!\"><value name=\"condition\"><block type=\"condition\" id=\"0utwHa[/d_0sia_3_k1O\"><value name=\"condition\"><block type=\"logic_operation\" id=\"qwCZdUnv0QT)ua5uHTm8\"><field name=\"Operator\">13</field><value name=\"left_side\"><block type=\"variable\" id=\"+b99Ej|XY#+@Dy{w;J-r\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><value name=\"right_side\"><block type=\"constant_value\" id=\"@KG_z1~w7K6vJ)=W6l]M\"><field name=\"value\">0</field><field name=\"TYPE\">1</field></block></value></block></value></block></value><statement name=\"true_instructions\"><block type=\"transfer_call\" id=\"DVV:n%-jued^p*W?*lNS\"><value name=\"Address\"><block type=\"variable\" id=\"D%tV3Fg;],+Ys.7QTTk.\"><field name=\"NAME\" id=\"ky:U{CO]]le=L`Yybg^F\">beneficiary</field></block></value><value name=\"Amount\"><block type=\"variable\" id=\"HGd~yk`E5+TH}cqXnanO\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><next><block type=\"event_call\" id=\"8@01r`e1V)}:B3q4GVc%\"><mutation name=\"auctionEnded\"><arg name=\"amount\"/><arg name=\"winner\"/></mutation><value name=\"ARG0\"><block type=\"variable\" id=\"hQ}(/XnufWUU1mUFlEY,\"><field name=\"NAME\" id=\"vVN-E,3x#|NghuF,}6NO\">highestBid</field></block></value><value name=\"ARG1\"><block type=\"variable\" id=\"026;m5bEBt#PUn?-sSsG\"><field name=\"NAME\" id=\"^__O-:W8:ydR}pXj~4R=\">highestBidder</field></block></value></block></next></block></statement></block></next></block></next></block></next></block></statement></block></next></block></statement></block></xml>";

Blockly.getExampleAsXML = function (exampleName) {
    parser = new DOMParser();
    return parser.parseFromString(Blockly.StrExamples[exampleName], "text/xml").documentElement;
}

Blockly.setExampleOnWorkspace = function (exampleName, targetWorkspace) {
    targetWorkspace.clear()
    Blockly.Xml.domToWorkspace(Blockly.getExampleAsXML(exampleName), targetWorkspace);
}

myFunctionCategoryCallback = function (workspace) {
    var tuples = Blockly.MyDynamicInputs.allProcedures(workspace);
    var allFunctions = [];
    var returnableFunctions = [];
    for (var i = 0; i < tuples.length; i++)
    {
        allFunctions.push(tuples[i][0]);
        if (tuples[i][1]) {
            returnableFunctions.push(tuples[i][0]);
        }
    }

    var voidElements = Blockly.MyDynamicInputs.populateElements(allFunctions, 'call_void_function');
    var returnableElements = Blockly.MyDynamicInputs.populateElements(returnableFunctions, 'call_returnable_function');

    return [].concat(voidElements).concat(returnableElements)
};

myEventsCategoryCallback = function (workspace) {
    var tuples = Blockly.MyDynamicInputs.allEvents(workspace);
    return Blockly.MyDynamicInputs.populateElements(tuples, 'event_call');
};

myModifiersCategoryCallback = function (workspace) {
    var tuples = Blockly.MyDynamicInputs.allModifiers(workspace);
    return Blockly.MyDynamicInputs.populateElements(tuples, 'modifier_appliance');
};
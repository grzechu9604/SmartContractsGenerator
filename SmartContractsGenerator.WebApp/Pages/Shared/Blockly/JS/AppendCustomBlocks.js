﻿var iAssignables = ["variable", "operation", "constant_value", "special_value", "call_returnable_function", "balance_call"];
var iValueContainers = ["variable", "variable_declaration"];
var solidityTypes = [["Bool", "0"], ["Int", "1"], ["UInt", "2"], ["Fixed", "3"], ["UFixed", "4"], ["Address", "5"], ["Address Payable", "6"], ["String", "7"]];

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
                var varType = childNode.getAttribute('type');
                var varId = childNode.getAttribute('varid') || childNode.getAttribute('varId');
                this.arguments_.push(varName);
                var variable = Blockly.Variables.getOrCreateVariablePackage(
                    this.workspace, varId, varName, varType);
                if (variable != null) {
                    this.argumentVarModels_.push(variable);
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
            var varType = paramBlock.getFieldValue('TYPE');
            this.arguments_.push(varName);
            var variable = this.workspace.getVariable(varName, varType);
            this.argumentVarModels_.push(variable);

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
            .appendField(new Blockly.FieldTextInput("[value]"), "Value");
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
        this.setPreviousStatement(true, 'IInstruction');
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
            .setCheck("operation")
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
            .appendField(new Blockly.FieldDropdown([["+", "0"], ["-", "1"], ["%", "2"], ["/", "3"], ["*", "4"], ["!", "5"], ["||", "6"], ["&&", "7"], ["^", "8"], ["==", "9"], ["!=", "10"]]), "Operator");
        this.appendValueInput("right_side")
            .setCheck(iAssignables);
        this.setOutput(true, "operation");
        this.setColour(60);
        this.setTooltip("");
        this.setHelpUrl("");
    }
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
            .setCheck("operation")
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
                solidityTypes,
                this.typeValidator_), "TYPE");
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setStyle('procedure_blocks');
        this.setTooltip(Blockly.Msg['PROCEDURES_MUTATORARG_TOOLTIP']);
        this.contextMenu = false;

        nameField.onFinishEditing_ = this.deleteIntermediateVars_;
        nameField.createdVariables_ = [];
        nameField.onFinishEditing_('x');
    },

    typeValidator_: function (newType) {
        var sourceBlock = this.getSourceBlock();

        if (sourceBlock.isInFlyout) {
            return newType;
        }

        var name = sourceBlock.getFieldValue("NAME");

        var outerWs = Blockly.Mutator.findParentWs(sourceBlock.workspace);
        var model = outerWs.getVariable(name, newType);

        if (model && model.type != newType) {
            outerWs.deleteVariableById(model.getId());
            model = null;
        }

        if (!model) {
            model = outerWs.createVariable(name, newType);
            if (model && this.createdVariables_) {
                this.createdVariables_.push(model);
            }
        }

        return newType;
    },

    validator_: function (varName) {
        var sourceBlock = this.getSourceBlock();
        var varType = sourceBlock.getFieldValue("TYPE");

        var outerWs = Blockly.Mutator.findParentWs(sourceBlock.workspace);
        if (!varName) {
            return null;
        }

        // Prevents duplicate parameter names in functions
        var workspace = sourceBlock.workspace.targetWorkspace ||
            sourceBlock.workspace;
        var blocks = workspace.getAllBlocks(false);
        var caselessName = varName.toLowerCase();
        for (var i = 0; i < blocks.length; i++) {
            if (blocks[i].id == this.getSourceBlock().id) {
                continue;
            }
            // Other blocks values may not be set yet when this is loaded.
            var otherVar = blocks[i].getFieldValue('NAME');
            if (otherVar && otherVar.toLowerCase() == caselessName) {
                return null;
            }
        }

        // Don't create variables for arg blocks that
        // only exist in the mutator's flyout.
        if (sourceBlock.isInFlyout) {
            return varName;
        }

        var model = outerWs.getVariable(varName, varType);
        if (model && model.name != varName) {
            // Rename the variable (case change)
            outerWs.renameVariableById(model.getId(), varName);
        }
        if (!model) {
            model = outerWs.createVariable(varName, varType);
            if (model && this.createdVariables_) {
                this.createdVariables_.push(model);
            }
        }
        return varName;
    },
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
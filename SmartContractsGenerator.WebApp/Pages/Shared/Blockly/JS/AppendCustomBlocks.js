
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
        Blockly.Procedures.mutateCallers(this);
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
        Blockly.Procedures.mutateCallers(this);
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
        Blockly.Procedures.mutateCallers(this);
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
            .setCheck(null)
            .appendField("Events");
        this.appendStatementInput("Modifiers")
            .setCheck(null)
            .appendField("Modifiers");
        this.appendStatementInput("Constructor")
            .setCheck("contract_constructor")
            .appendField("Constructor");
        this.appendStatementInput("Functions")
            .setCheck(null)
            .appendField("Functions");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['contract_constructor'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Constructor")
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Visibility")
            .appendField(new Blockly.FieldDropdown([["Public", "1"], ["Internal", "2"]]), "Visibility");
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck(null);
        this.setPreviousStatement(true, "contract_constructor");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
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

Blockly.Blocks['contract_property'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Property");
        this.appendDummyInput()
            .appendField("Visibility")
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
        var nameField = new Blockly.FieldTextInput('',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField("Contract event")
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
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
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['assignment'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Assignment");
        this.appendValueInput("Destination")
            .setCheck(["variable", "variable_declaration"]);
        this.appendDummyInput()
            .appendField("=");
        this.appendValueInput("Source")
            .setCheck(["variable", "operation", "constant_value"]);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['event_call'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Emit")
            .appendField(this.id, 'NAME');
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setStyle('procedure_blocks');
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
        this.setPreviousStatement(true);
        this.setNextStatement(true);
    }
};

Blockly.Blocks['call_returnable_function'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Execute")
            .appendField(this.id, 'NAME');
        this.setOutput(true, null);
        this.setStyle('procedure_blocks');
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
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setStyle('procedure_blocks');
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
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['operation'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Operation");
        this.appendValueInput("left_side")
            .setCheck(["variable", "operation", "constant_value"])
            .appendField("Left");
        this.appendDummyInput()
            .appendField("Operator")
            .appendField(new Blockly.FieldDropdown([["+", "0"], ["-", "1"], ["%", "2"], ["/", "3"], ["*", "4"], ["!", "5"], ["||", "6"], ["&&", "7"], ["^", "8"], ["==", "9"], ["!=", "10"]]), "Operator");
        this.appendValueInput("right_side")
            .setCheck(["variable", "operation", "constant_value"])
            .appendField("Right");
        this.setOutput(true, "operation");
        this.setColour(230);
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
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['variable'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Variable")
            .appendField(new Blockly.FieldVariable("item"), "NAME");
        this.setOutput(true, "variable");
        this.setColour(230);
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
            .setCheck(null)
            .appendField("Instructions");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
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
            .setCheck(null)
            .appendField("If true");
        this.appendStatementInput("false_instructions")
            .setCheck(null)
            .appendField("If false");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
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
        this.appendDummyInput()
            .appendField("Function");
        var nameField = new Blockly.FieldTextInput('',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField(Blockly.Msg['PROCEDURES_DEFRETURN_TITLE'])
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Accepts ethers")
            .appendField(new Blockly.FieldCheckbox("FALSE"), "AcceptsEthers");
        this.appendDummyInput()
            .appendField("Apply modifier")
            .appendField(modifierCheckbox, "ApplyModifier");
        this.appendDummyInput("Visibility")
            .appendField("Visibility")
            .appendField(new Blockly.FieldDropdown([["External", "0"], ["Public", "1"], ["Private", "3"], ["Internal", "2"]]), "Visibility");
        this.appendDummyInput("StateModification")
            .appendField("State modification type")
            .appendField(new Blockly.FieldDropdown([["ReadWrite", "0"], ["ReadOnly", "1"], ["Calculation only", "2"]]), "StateModification");
        this.appendDummyInput()
            .appendField("Return value")
            .appendField(returnsCheckbox, "ApplyReturns");
        this.appendDummyInput("InstructionsLabel")
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck(null);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
        this.statementConnection_ = null;
    },

    updateModifierInput_: function (applyModifierInput) {
        var inputExists = this.getInput("Modifier");
        if (applyModifierInput) {
            if (!inputExists) {
                this.appendValueInput("Modifier")
                    .appendField("Modifier: ")
                    .setCheck(["modifier_appliance"]);
                this.moveInputBefore('Modifier', 'Visibility');
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
                    .appendField(new Blockly.FieldDropdown([["int256", "int256"], ["uint256", "uint256"], ["bool", "bool"]]), "TYPE");
                this.moveInputBefore('ReturningType', 'InstructionsLabel');
            }
        } else if (inputExists) {
            this.removeInput('ReturningType');
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
        var nameField = new Blockly.FieldTextInput('',
            Blockly.Procedures.rename);
        nameField.setSpellcheck(false);
        this.appendDummyInput()
            .appendField("Modifier");
        this.appendDummyInput()
            .appendField(nameField, 'NAME')
            .appendField('', 'PARAMS');
        this.setMutator(new Blockly.Mutator(['my_procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck(null);
        this.setPreviousStatement(true, "contract_constructor");
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
        this.arguments_ = [];
        this.argumentVarModels_ = [];
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

Blockly.Blocks['moddifier_appliance'] = {
    init: function () {
        this.appendDummyInput('TOPROW')
            .appendField("Use modifier")
            .appendField(this.id, 'NAME');
        this.setOutput(true, null);
        this.setStyle('procedure_blocks');
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
    defType_: 'moddifier_appliance'
};

Blockly.Blocks['variable_declaration'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Variable declaration");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable("item"), "NAME");
        this.appendDummyInput()
            .appendField("type:")
            .appendField(new Blockly.FieldDropdown([["int256", "int256"], ["uint256", "uint256"], ["bool", "bool"]]), "TYPE");
        this.setInputsInline(false);
        this.setOutput(true, "variable_declaration");
        this.setColour(230);
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

        this.setPreviousStatement(true, null);
        this.setNextStatement(false, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    },

    updateReturnValueInput_: function (returnValueChecked) {
        var inputExists = this.getInput("ReturnValue");
        if (returnValueChecked) {
            if (!inputExists) {
                this.appendValueInput("ReturnValue")
                    .appendField("Value")
                    .setCheck(["variable", "operation", "constant_value"]);
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
                [["int256", "int256"], ["uint256", "uint256"], ["bool", "bool"]],
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
    return Blockly.MyDynamicInputs.populateElements(tuples, 'moddifier_appliance');
};
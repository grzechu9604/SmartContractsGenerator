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
            .appendField("Constructor");
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
    }
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
        this.appendDummyInput()
            .appendField("Contract event");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable("[event name]"), "NAME");
        this.appendStatementInput("Parameters")
            .setCheck("variable");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
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
        this.appendDummyInput()
            .appendField("Call event:")
            .appendField(new Blockly.FieldVariable("[event name]"), "NAME");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['call_returnable_function'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Call function and get value:")
            .appendField(new Blockly.FieldVariable("[function name]"), "NAME");
        this.setOutput(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['call_void_function'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Call function:")
            .appendField(new Blockly.FieldVariable("[function name]"), "NAME");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
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
        var checkbox = new Blockly.FieldCheckbox("FALSE", function (applyModifierChecked) {
            this.sourceBlock_.updateShape_(applyModifierChecked == "TRUE");
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
        this.setMutator(new Blockly.Mutator(['procedures_mutatorarg']));
        this.appendDummyInput()
            .appendField("Apply modifier")
            .appendField(checkbox, "ApplyModifier");
        this.appendDummyInput()
            .appendField("Visibility")
            .appendField(new Blockly.FieldDropdown([["External", "0"], ["Public", "1"], ["Private", "3"], ["Internal", "2"]]), "Visibility");
        this.appendDummyInput()
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
        this.setStatements_(true);
        this.statementConnection_ = null;
    },

    updateShape_: function (applyModifierInput) {
        var inputExists = this.getInput("Modifier");
        if (applyModifierInput) {
            if (!inputExists) {
                this.appendDummyInput("Modifier")
                    .appendField("Modifier")
                    .appendField(new Blockly.FieldVariable("[modifier name]"), "Modifier");
            }
        } else if (inputExists) {
            this.removeInput('Modifier');
        }
    },

    setStatements_: Blockly.Blocks['procedures_defnoreturn'].setStatements_,
    updateParams_: Blockly.Blocks['procedures_defnoreturn'].updateParams_,
    mutationToDom: Blockly.Blocks['procedures_defnoreturn'].mutationToDom,
    domToMutation: Blockly.Blocks['procedures_defnoreturn'].domToMutation,
    decompose: Blockly.Blocks['procedures_defnoreturn'].decompose,
    compose: Blockly.Blocks['procedures_defnoreturn'].compose,
    getProcedureDef: function () {
        return [this.getFieldValue('NAME'), this.arguments_, true];
    },
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
        this.appendDummyInput()
            .appendField("Modifier");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable("[modifier name]"), "NAME");
        this.appendDummyInput()
            .appendField("Instructions");
        this.appendStatementInput("Instructions")
            .setCheck(null);
        this.setPreviousStatement(true, "contract_constructor");
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['variable_declaration'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Variable declaration");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable("item"), "NAME");
        this.appendDummyInput()
            .appendField("Type")
            .appendField(new Blockly.FieldDropdown([["int256", "int256"], ["uint256", "uint256"], ["bool", "bool"]]), "Type");
        this.setInputsInline(false);
        this.setOutput(true, "variable_declaration");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

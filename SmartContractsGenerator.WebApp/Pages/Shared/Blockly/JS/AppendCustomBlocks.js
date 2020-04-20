Blockly.Blocks['contract'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Contract");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldTextInput("[insert contract name]"), "Name");
        this.appendStatementInput("Properties")
            .setCheck("contract_property")
            .appendField("Properties");
        this.appendStatementInput("Events")
            .setCheck(null)
            .appendField("Events");
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
            .setCheck("variable")
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
            .appendField(new Blockly.FieldTextInput("[contract name]"), "Name");
        this.appendStatementInput("Parameters")
            .setCheck("Variable");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['declaration'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Declaration");
        this.appendStatementInput("Variable")
            .setCheck("Variable");
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
        this.setOutput(true, null);
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
            .setCheck(null);
        this.appendDummyInput()
            .appendField("=");
        this.appendValueInput("Source")
            .setCheck(null);
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
            .appendField("Call event:");
        this.appendValueInput("event_to_call")
            .setCheck(null);
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
            .appendField("Call function and get value");
        this.appendValueInput("Name")
            .setCheck(null)
            .appendField("Function");
        this.setOutput(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['call_void_function'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Call function");
        this.appendValueInput("Name")
            .setCheck(null)
            .appendField("Function");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['condition'] = {
    init: function () {
        this.appendValueInput("Name")
            .setCheck("Operation")
            .appendField("Condition")
            .appendField("");
        this.setOutput(true, "Condition");
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
            .setCheck("Operation")
            .appendField("Left");
        this.appendDummyInput()
            .appendField("Operator")
            .appendField(new Blockly.FieldDropdown([["+", "+"], ["-", "-"], ["*", "*"]]), "Operator");
        this.appendValueInput("right_side")
            .setCheck("Operation")
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
        this.appendValueInput("Condition")
            .setCheck("Condition")
            .appendField("Condition");
        this.appendValueInput("Message")
            .setCheck("String")
            .appendField("Error message");
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
            .appendField(new Blockly.FieldVariable("item"), "Name");
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
        this.appendValueInput("Initial_assignment")
            .setCheck("Assignment")
            .appendField("Initial assignment");
        this.appendValueInput("step_instruction")
            .setCheck("Operation")
            .appendField("Step instruction");
        this.appendValueInput("break_condition")
            .setCheck("Operation")
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
            .setCheck("Condition")
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
        this.appendDummyInput()
            .appendField("Function");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldTextInput("[function name]"), "Name");
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
    }
};

Blockly.Blocks['modifier'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Modifier");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldTextInput("[modifier name]"), "Name");
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

Blockly.Blocks['variable_declaration'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Variable declaration");
        this.appendDummyInput()
            .appendField("Name")
            .appendField(new Blockly.FieldVariable("item"), "Name");
        this.appendDummyInput()
            .appendField("Type")
            .appendField(new Blockly.FieldDropdown([["int256", "int256"], ["uint256", "uint256"], ["bool", "bool"]]), "Type");
        this.setInputsInline(false);
        this.setOutput(true, "variable");
        this.setColour(230);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};
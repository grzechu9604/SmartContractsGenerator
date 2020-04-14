Blockly.XmlGenerator = new Blockly.Generator('XmlGenerator');

Blockly.XmlGenerator.init = function (workspace) {
    // Create a dictionary of definitions to be printed before the code.
    Blockly.XmlGenerator.definitions_ = Object.create(null);
    // Create a dictionary mapping desired function names in definitions_
    // to actual function names (to avoid collisions with user functions).
    Blockly.XmlGenerator.functionNames_ = Object.create(null);

    if (!Blockly.XmlGenerator.variableDB_) {
        Blockly.XmlGenerator.variableDB_ =
            new Blockly.Names(Blockly.XmlGenerator.RESERVED_WORDS_);
    } else {
        Blockly.XmlGenerator.variableDB_.reset();
    }

    Blockly.XmlGenerator.variableDB_.setVariableMap(workspace.getVariableMap());

    var defvars = [];
    // Add developer variables (not created or named by the user).
    var devVarList = Blockly.Variables.allDeveloperVariables(workspace);
    for (var i = 0; i < devVarList.length; i++) {
        defvars.push(Blockly.XmlGenerator.variableDB_.getName(devVarList[i],
            Blockly.Names.DEVELOPER_VARIABLE_TYPE));
    }

    // Add user variables, but only ones that are being used.
    var variables = Blockly.Variables.allUsedVarModels(workspace);
    for (var i = 0; i < variables.length; i++) {
        defvars.push(Blockly.XmlGenerator.variableDB_.getName(variables[i].getId(),
            Blockly.VARIABLE_CATEGORY_NAME));
    }

    // Declare all of the variables.
    if (defvars.length) {
        Blockly.XmlGenerator.definitions_['variables'] =
            'var ' + defvars.join(', ') + ';';
    }
};

/**
 * Prepend the generated code with the variable definitions.
 * @param {string} code Generated code.
 * @return {string} Completed code.
 */
Blockly.XmlGenerator.finish = function (code) {
    // Convert the definitions dictionary into a list.
    var definitions = [];
    for (var name in Blockly.XmlGenerator.definitions_) {
        definitions.push(Blockly.XmlGenerator.definitions_[name]);
    }
    // Clean up temporary data.
    delete Blockly.XmlGenerator.definitions_;
    delete Blockly.XmlGenerator.functionNames_;
    Blockly.XmlGenerator.variableDB_.reset();
    return definitions.join('\n\n') + '\n\n\n' + code;
};

Blockly.XmlGenerator['contract'] = function (block) {
    var text_name = block.getFieldValue('Name');
    var statements_properties = Blockly.XmlGenerator.statementToCode(block, 'Properties');
    var statements_events = Blockly.XmlGenerator.statementToCode(block, 'Events');
    var statements_constructor = Blockly.XmlGenerator.statementToCode(block, 'Constructor');
    var statements_functions = Blockly.XmlGenerator.statementToCode(block, 'Functions');

    return '<contract>'
        + '<name>' + text_name + '</name>'
        + statements_properties
        + statements_events
        + statements_constructor
        + statements_functions
        + '</contract>';
};

Blockly.XmlGenerator['contract_constructor'] = function (block) {
    var dropdown_visibility = block.getFieldValue('Visibility');
    var statements_instrictions = Blockly.XmlGenerator.statementToCode(block, 'Instrictions');

    return "<constructor>"
        + "<visibility>" + dropdown_visibility + "</visibility>"
        + statements_instrictions
        + "</constructor>"
};

Blockly.XmlGenerator['contract_property'] = function (block) {
    var dropdown_visibility = block.getFieldValue('Visibility');
    var value_name = Blockly.XmlGenerator.valueToCode(block, 'NAME', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['contract_event'] = function (block) {
    var text_name = block.getFieldValue('NAME');
    var statements_parameters = Blockly.XmlGenerator.statementToCode(block, 'Parameters');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['declaration'] = function (block) {
    var statements_variable = Blockly.XmlGenerator.statementToCode(block, 'Variable');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['constant_value'] = function (block) {
    var text_value = block.getFieldValue('Value');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.XmlGenerator.ORDER_NONE];
};

Blockly.XmlGenerator['assignment'] = function (block) {
    var statements_destination = Blockly.XmlGenerator.statementToCode(block, 'Destination');
    var statements_source = Blockly.XmlGenerator.statementToCode(block, 'Source');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['event_call'] = function (block) {
    var value_event_to_call = Blockly.XmlGenerator.valueToCode(block, 'event_to_call', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['call_returnable_function'] = function (block) {
    var value_name = Blockly.XmlGenerator.valueToCode(block, 'NAME', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.XmlGenerator.ORDER_NONE];
};

Blockly.XmlGenerator['call_void_function'] = function (block) {
    var value_name = Blockly.XmlGenerator.valueToCode(block, 'NAME', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['condition'] = function (block) {
    var value_name = Blockly.XmlGenerator.valueToCode(block, 'NAME', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.XmlGenerator.ORDER_NONE];
};

Blockly.XmlGenerator['operation'] = function (block) {
    var value_left_side = Blockly.XmlGenerator.valueToCode(block, 'left_side', Blockly.XmlGenerator.ORDER_ATOMIC);
    var dropdown_operator = block.getFieldValue('Operator');
    var value_right_side = Blockly.XmlGenerator.valueToCode(block, 'right_side', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.XmlGenerator.ORDER_NONE];
};

Blockly.XmlGenerator['requirement'] = function (block) {
    var value_condition = Blockly.XmlGenerator.valueToCode(block, 'Condition', Blockly.XmlGenerator.ORDER_ATOMIC);
    var value_message = Blockly.XmlGenerator.valueToCode(block, 'Message', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['variable'] = function (block) {
    var value_name = Blockly.XmlGenerator.valueToCode(block, 'Name', Blockly.XmlGenerator.ORDER_ATOMIC);
    var value_type = Blockly.XmlGenerator.valueToCode(block, 'Type', Blockly.XmlGenerator.ORDER_ATOMIC);
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.XmlGenerator.ORDER_NONE];
};

Blockly.XmlGenerator['contract_loop'] = function (block) {
    var value_initial_assignment = Blockly.XmlGenerator.valueToCode(block, 'Initial_assignment', Blockly.XmlGenerator.ORDER_ATOMIC);
    var value_step_instruction = Blockly.XmlGenerator.valueToCode(block, 'step_instruction', Blockly.XmlGenerator.ORDER_ATOMIC);
    var value_break_condition = Blockly.XmlGenerator.valueToCode(block, 'break_condition', Blockly.XmlGenerator.ORDER_ATOMIC);
    var statements_instructions = Blockly.XmlGenerator.statementToCode(block, 'instructions');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['if_statement'] = function (block) {
    var value_condition = Blockly.XmlGenerator.valueToCode(block, 'condition', Blockly.XmlGenerator.ORDER_ATOMIC);
    var statements_true_instructions = Blockly.XmlGenerator.statementToCode(block, 'true_instructions');
    var statements_false_instructions = Blockly.XmlGenerator.statementToCode(block, 'false_instructions');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['contract_function'] = function (block) {
    var text_name = block.getFieldValue('Name');
    var dropdown_visibility = block.getFieldValue('Visibility');
    var statements_instrictions = Blockly.XmlGenerator.statementToCode(block, 'Instrictions');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};

Blockly.XmlGenerator['modifier'] = function (block) {
    var text_name = block.getFieldValue('Name');
    var statements_instrictions = Blockly.XmlGenerator.statementToCode(block, 'Instrictions');
    // TODO: Assemble XmlGenerator into code variable.
    var code = '...;\n';
    return code;
};
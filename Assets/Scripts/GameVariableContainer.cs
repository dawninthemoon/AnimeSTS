using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariableContainer {
    private readonly Dictionary<string, int> _variables;

    public GameVariableContainer() {
        _variables = new Dictionary<string, int>();
    }

    public int GetVariable(string variableName) {
        int value = -1;
        if (_variables.TryGetValue(variableName, out int variable)) {
            value = variable;
        }
        return value;
    }

    public void SetVariable(string variableName, int value) {
        if (_variables.TryGetValue(variableName, out int variable)) {
            _variables[variableName] = value;
        }
        else {
            _variables.Add(variableName, value);
        }
    }

    public void AddValue(string variableName, int value) {
        if (_variables.TryGetValue(variableName, out int variable)) {
            _variables[variableName] = variable + value;
        }
        else {
            _variables.Add(variableName, value);
        }
    }

    public bool LessThen(string variableName, int value) {
        return _variables[variableName] < value;
    }

    public bool GreaterThen(string variableName, int value) {
        return _variables[variableName] > value;
    }

    public bool Equals(string variableName, int value) {
        return _variables[variableName] == value;
    }
}

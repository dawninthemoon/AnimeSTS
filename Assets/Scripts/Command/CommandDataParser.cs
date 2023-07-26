using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandDataParser {
    private static readonly char EqualCharacter = '=';
    private static readonly char CommaCharacter = ',';
    private static readonly char NewlineCharacater = '\n';
    private GameVariableContainer _gameVariables;
    private delegate bool ConditionCheckDelegate(string name, int value);
    private readonly Dictionary<string, ConditionCheckDelegate> _conditionCheckCallback;

    public CommandDataParser(GameVariableContainer variables) {
        _gameVariables = variables;

        _conditionCheckCallback = new Dictionary<string, ConditionCheckDelegate>();
        _conditionCheckCallback.Add("Less", _gameVariables.LessThen);
        _conditionCheckCallback.Add("Greater", _gameVariables.GreaterThen);
        _conditionCheckCallback.Add("Equal", _gameVariables.Equals);
    }

    public Dictionary<string, int> ParseVariable(EntityBase caster, string metadata) {
        Dictionary<string, int> variableData = new Dictionary<string, int>();

        string[] splitedWithNewLine = metadata.Split(NewlineCharacater);
        foreach (string variable in splitedWithNewLine) {
            string[] splitedWithComma = variable.Split(CommaCharacter);
            string[] splitedWithEqual = splitedWithComma[0].Split(EqualCharacter);

            int value = int.Parse(splitedWithEqual[1]);
            for (int i = 1; i < splitedWithComma.Length; ++i) {
                value = GetEffectAmount(caster, splitedWithComma[i], value);
            }
            variableData.Add(splitedWithEqual[0], value);
        }

        return variableData;
    }

    public bool CheckCondition(string metadata) {
        string[] splitedWithComma = metadata.Split(CommaCharacter);
        
        string checkType = splitedWithComma[1];
        string variableName = splitedWithComma[0];
        int value = int.Parse(splitedWithComma[2]);

        return _conditionCheckCallback[checkType].Invoke(variableName, value);
    }

    public int GetEffectAmount(EntityBase caster, string type, int value) {
        if (type.Equals("attack")) {
            value += caster.GetEffectAmount("strength");
            if (caster.GetEffectAmount("weak") > 0) {
                value = Mathf.FloorToInt(value * 0.75f);
            }
        }
        else if (type.Equals("defense")) {
            value += caster.GetEffectAmount("dexterity");
            if (caster.GetEffectAmount("frail") > 0) {
                value = Mathf.FloorToInt(value * 0.75f);
            }
        }
        return value;
    }
}

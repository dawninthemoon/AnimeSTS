using System;
using System.Collections.Generic;
using UnityEngine;

public class CardVariableParser {
    private static readonly char EqualCharacter = '=';
    private static readonly char CommaCharacter = ',';
    private static readonly char NewlineCharacater = '\n';
    private EntityBase _player;

    public CardVariableParser(EntityBase player) {
        _player = player;
    }

    public Dictionary<string, int> Parse(string metadata) {
        Dictionary<string, int> variableData = new Dictionary<string, int>();

        string[] splitedWithNewLine = metadata.Split(NewlineCharacater);
        foreach (string variable in splitedWithNewLine) {
            string[] splitedWithComma = variable.Split(CommaCharacter);
            string[] splitedWithEqual = splitedWithComma[0].Split(EqualCharacter);

            int value = int.Parse(splitedWithEqual[1]);
            for (int i = 1; i < splitedWithComma.Length; ++i) {
                value = GetEffectAmount(splitedWithComma[i], value);
            }
            variableData.Add(splitedWithEqual[0], value);
        }

        return variableData;
    }

    public int GetEffectAmount(string type, int value) {
        if (type.Equals("attack")) {
            value += _player.GetEffectAmount("strength");
            if (_player.GetEffectAmount("weak") > 0) {
                value = Mathf.FloorToInt(value * 0.75f);
            }
        }
        else if (type.Equals("defense")) {
            value += _player.GetEffectAmount("dexterity");
            if (_player.GetEffectAmount("frail") > 0) {
                value = Mathf.FloorToInt(value * 0.75f);
            }
        }
        return value;
    }
}

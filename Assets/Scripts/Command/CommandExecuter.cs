using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandExecuter : MonoBehaviour {
    private Dictionary<string, IBattleCommand> _commandDictionary;

    private void Start() {
        var nestedType = typeof(BattleCommand).GetNestedTypes(System.Reflection.BindingFlags.Public);
        string interfaceName = "IBattleCommand";
        _commandDictionary = nestedType
                                .Where(type => (type.GetInterface(interfaceName) != null))
                                .Select(type => type)
                                .ToDictionary(type => type.Name, type => System.Activator.CreateInstance(type) as IBattleCommand);
    }

    public void ExecuteCard(CommandInfo[] commands, GameData data, EntityBase caster, EntityBase target) {
        foreach (CommandInfo command in commands) {
            StartCoroutine(ExecuteCommand(command, data, caster, target));
        }
    }

    private IEnumerator ExecuteCommand(CommandInfo commandInfo, GameData data, EntityBase caster, EntityBase target) {
        if (_commandDictionary.TryGetValue(commandInfo.name, out IBattleCommand instance)) {
            IEnumerator coroutine = instance.Execute(caster, target, data, commandInfo.value);
            while (coroutine.MoveNext()) {
                var nestCoroutine = coroutine?.Current as YieldInstruction;
                yield return nestCoroutine;
            }
        }
    }
}

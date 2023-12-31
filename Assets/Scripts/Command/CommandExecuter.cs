using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandExecuter : MonoBehaviour {
    private Dictionary<string, IBattleCommand> _commandDictionary;
    private WaitForSeconds _commandDelay;

    private void Start() {
        _commandDelay = new WaitForSeconds(0.15f);

        var nestedType = typeof(BattleCommand).GetNestedTypes(System.Reflection.BindingFlags.Public);
        string interfaceName = "IBattleCommand";
        _commandDictionary = nestedType
                                .Where(type => (type.GetInterface(interfaceName) != null))
                                .Select(type => type)
                                .ToDictionary(type => type.Name, type => System.Activator.CreateInstance(type) as IBattleCommand);
    }

    public void ExecuteCard(CommandInfo[] commands, GameData data, EntityBase caster, EntityBase target) {
        StartCoroutine(ExecuteCommands(commands, data, caster, target));
    }

    private IEnumerator ExecuteCommands(CommandInfo[] commands, GameData data, EntityBase caster, EntityBase target) {
        int commandLength = commands.Length;
        for (int i = 0; i < commandLength; ++i) {
            CommandInfo commandInfo = commands[i];

            if (_commandDictionary.TryGetValue(commandInfo.name, out IBattleCommand instance)) {
                IEnumerator coroutine = instance.Execute(caster, target, data, commandInfo.value);
                while (coroutine.MoveNext()) {
                    var nestCoroutine = coroutine?.Current as YieldInstruction;
                    yield return nestCoroutine;
                }
                yield return _commandDelay;
            }
        }
    }
}

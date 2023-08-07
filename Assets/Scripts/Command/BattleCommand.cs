using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {
    public CommandDataParser Parser {
        get; private set;
    }
    public CardDeck Deck {
        get; private set;
    }
    public Dictionary<string, int> CurrentVariableData {
        get; set;
    }
    public EnemyHandler EnemyHandler {
        get; set;
    }
    public GameData(CommandDataParser parser, CardDeck cardDeck) {
        Parser = parser;
        Deck = cardDeck;
    }
}

public interface IBattleCommand {
    IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value);
}

public class BattleCommand {
    public class Attack : IBattleCommand {
        public IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value) { 
            int amount = data.CurrentVariableData[value];
            caster.StartAnimation("attack");
            target.TakeDamage(amount);
            yield break;
        }

        public IEnumerator Execute(EntityBase caster, EntityBase target, int amount) { 
            caster.StartAnimation("attack");
            target.TakeDamage(amount);
            yield break;
        }
    }

    public class AttackRandom : IBattleCommand {
        public IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value) { 
            int amount = data.CurrentVariableData[value];
            target = data.EnemyHandler.GetRandomEnemy();
            target.TakeDamage(amount);
            yield break;
        }
    }

    public class AttackAll : IBattleCommand {
        public IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value) { 
            int amount = data.EnemyHandler.EnemyList.Count;
            int numOfEnemies = data.EnemyHandler.EnemyList.Count;
            for (int i = 0; i < numOfEnemies; ++i) {
                target = data.EnemyHandler.EnemyList[i];
                target.TakeDamage(amount);
            }
            yield break;
        }
    }

    public class Block : IBattleCommand {
        public IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value) {
            int amount = data.CurrentVariableData[value];
            caster.GainBlock(amount);
            yield break;
        }

        public IEnumerator Execute(EntityBase caster, EntityBase target, int amount) {
            caster.GainBlock(amount);
            yield break;
        }
    }

    public class Vulnerable : IBattleCommand {
        public IEnumerator Execute(EntityBase caster, EntityBase target, GameData data, string value) {
            int amount = data.CurrentVariableData[value];
            target.AddEffectValue("vulnerable", amount);
            yield break;
        }

        public IEnumerator Execute(EntityBase caster, EntityBase target, int amount) {
            target.AddEffectValue("vulnerable", amount);
            yield break;
        }
    }
}

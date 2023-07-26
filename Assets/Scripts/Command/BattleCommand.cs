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
    public GameData(CommandDataParser parser, CardDeck cardDeck) {
        Parser = parser;
        Deck = cardDeck;
    }
}

public interface IBattleCommand {
    IEnumerator Execute(GameData data, string value);
}

public class BattleCommand {
    public class Attack : IBattleCommand {
        public IEnumerator Execute(GameData data, string value) { 
            yield break;
        }
    }

    public class Block : IBattleCommand {
        public IEnumerator Execute(GameData data, string value) { 
            yield break;
        }
    }
}

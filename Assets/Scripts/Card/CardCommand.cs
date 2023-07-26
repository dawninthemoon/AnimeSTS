using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData {

}

public interface ICardCommand {
    IEnumerator Execute(SharedData data, int amount);
}

public class CardCommand {
    public class Attack : ICardCommand {
        public IEnumerator Execute(SharedData data, int amount) { 
            yield break;
        }
    }

    public class Block : ICardCommand {
        public IEnumerator Execute(SharedData data, int amount) { 
            yield break;
        }
    }
}

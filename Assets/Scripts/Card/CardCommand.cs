using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData {

}

public interface ICardCommand {
    IEnumerator Execute(SharedData data, string amount);
}

public class CardCommand {
    public class Attack : ICardCommand {
        public IEnumerator Execute(SharedData data, string amount) { 
            yield break;
        }
    }

    public class Block : ICardCommand {
        public IEnumerator Execute(SharedData data, string amount) { 
            yield break;
        }
    }
}

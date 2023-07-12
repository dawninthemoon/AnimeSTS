using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager {
    private Dictionary<string, GameObject> _cachedGameObjects = new Dictionary<string, GameObject>();

    public T GetAsset<T>(string name) where T : Object {
        T asset = Resources.Load<T>(name);
        return asset;
    }
}

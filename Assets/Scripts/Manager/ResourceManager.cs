using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager> {
    private Dictionary<string, Sprite> _cachedSprites = new Dictionary<string, Sprite>();

    public T GetAsset<T>(string name) where T : Object {
        T asset = Resources.Load<T>(name);
        return asset;
    }

    public Sprite GetSpriteByCache(string name) {
        Sprite sprite;
        if (_cachedSprites.TryGetValue(name, out sprite)) {
            return sprite;
        }
        sprite = GetAsset<Sprite>(name);
        _cachedSprites.Add(name, sprite);
        return sprite;
    }
}

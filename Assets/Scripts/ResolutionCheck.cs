using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionCheck : MonoBehaviour {
    private bool _wasFullScreen;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(960, 540, false);
    }

    void Update() {
        bool fullScreen = Screen.fullScreen;
        if (fullScreen && !_wasFullScreen) {
            _wasFullScreen = true;
            Screen.SetResolution(1920, 1080, true);
        }
        else if (!fullScreen && _wasFullScreen) {
            _wasFullScreen = false;
            Screen.SetResolution(960, 540, false);
        }
    }
}
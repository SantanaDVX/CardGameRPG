﻿using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HideInEditor : MonoBehaviour {

    void OnEnable() {
        toggleRenderingsParent(!Application.isEditor || Application.isPlaying);
    }

    void OnDisable() {
        toggleRenderingsParent(true);        
    }

    private void toggleRenderingsParent(bool status) {
        toggleRenderings(status);
        foreach (HideInEditor childHider in GetComponentsInChildren<HideInEditor>()) {
            childHider.toggleRenderings(status);
        }
    }

    public void toggleRenderings(bool status) {
        foreach (Renderer tmp in GetComponents<Renderer>()) {
            tmp.enabled = status;
        }
        foreach (Image tmp in GetComponents<Image>()) {
            tmp.enabled = status;
        }
        foreach (Text tmp in GetComponents<Text>()) {
            tmp.enabled = status;
        }
        foreach (Canvas tmp in GetComponents<Canvas>()) {
            tmp.enabled = status;
        }
    }
}
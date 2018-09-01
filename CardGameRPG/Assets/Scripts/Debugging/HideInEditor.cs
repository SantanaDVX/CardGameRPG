using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HideInEditor : MonoBehaviour {
    void OnEnable() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().enabled = !Application.isEditor || Application.isPlaying;
        } else if (GetComponent<Image>() != null) {
            GetComponent<Image>().enabled = !Application.isEditor || Application.isPlaying;
        } else if (GetComponent<Text>() != null) {
            GetComponent<Text>().enabled = !Application.isEditor || Application.isPlaying;
        }
    }
    void OnDisable() {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().enabled = true;
        } else if (GetComponent<Image>() != null) {
            GetComponent<Image>().enabled = true;
        } else if (GetComponent<Text>() != null) {
            GetComponent<Text>().enabled = true;
        }
    }
}
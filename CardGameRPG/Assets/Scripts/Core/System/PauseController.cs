using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {
    public GameObject pauseText;

    public static bool paused = false;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)
            && SubSceneNode.getNode("Battle") != null
            && SubSceneNode.getNode("Battle").isActive()) {
            paused = !paused;
            pauseText.SetActive(paused);

            if (paused) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
        }
    }
}

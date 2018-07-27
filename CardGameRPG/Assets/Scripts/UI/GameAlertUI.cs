using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAlertUI : MonoBehaviour {

    GameObject panel;
    public Text gameAlertText;

    private void Awake() {
        panel = gameAlertText.gameObject.transform.parent.gameObject;
        panel.SetActive(false);
    }

    public void setText(string text) {
        panel.SetActive(true);
        gameAlertText.text = " " + text + " ";
    }

    public void deactivateText() {
        gameAlertText.text = "";
        panel.SetActive(false);
    }
    
    private static GameAlertUI gameAlertUI;
    public static GameAlertUI Instance() {
        if (!gameAlertUI) {
            gameAlertUI = FindObjectOfType<GameAlertUI>();
        }
        return gameAlertUI;
    }
}

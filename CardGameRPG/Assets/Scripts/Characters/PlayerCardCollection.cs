using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardCollection : MonoBehaviour {
    public List<GameObject> cardCollection;


    private void Start() {
        foreach (GameObject card in cardCollection) {
            PlayerCardProgress.Instance().createNewCardAtTrained(card);
        }
        CardProgressController.Instance().loadCardProgression();
    }

    private static PlayerCardCollection playerCardCollection;
    public static PlayerCardCollection Instance() {
        if (!playerCardCollection) {
            playerCardCollection = FindObjectOfType<PlayerCardCollection>();
        }
        return playerCardCollection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardCollection : MonoBehaviour {
    public List<GameObject> cardCollection;


    private void Start() {
        foreach (GameObject card in cardCollection) {
            createLearnedProgress(card);
        }
        CardProgressController.Instance().loadCardProgression();
    }

    private void createLearnedProgress(GameObject card) {
        CardProgress progress = new CardProgress(card);
        progress.progress = CardProgression.Trained;
        //progress.expPoints = 120;
        PlayerCardProgress.Instance().cards.Add(card.GetComponent<CardDetails>().cardName, progress);
    }

    private static PlayerCardCollection playerCardCollection;
    public static PlayerCardCollection Instance() {
        if (!playerCardCollection) {
            playerCardCollection = FindObjectOfType<PlayerCardCollection>();
        }
        return playerCardCollection;
    }
}

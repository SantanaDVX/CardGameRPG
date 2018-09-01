using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProgress {
    public GameObject card;
    public string cardName;
    public CardProgression progress;
    public int expPoints;

    public CardProgress(GameObject card) {
        this.card = card;
        CardDetails details = card.GetComponent<CardDetails>();
        cardName = details.cardName;
        progress = CardProgression.Unlearned;
        expPoints = 0;
    }
}

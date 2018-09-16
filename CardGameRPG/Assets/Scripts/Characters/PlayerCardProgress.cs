using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardProgress : MonoBehaviour {
    public int untrainedExpCap;
    public int trainedExpCap;
    public int adpetExpCap;
    public int proficientExpCap;
    public int masteredExpCap;

    public int selfCastExpGain;
    public int enemyCastExpGain;

    public Dictionary<string, CardProgress> cards;

    private void Awake() {
        cards = new Dictionary<string, CardProgress>();
    }

    public void createNewCardAtTrained(GameObject card) {
        CardProgress progress = new CardProgress(card);
        progress.progress = CardProgression.Trained;
        cards.Add(card.GetComponent<CardDetails>().cardName, progress);
    }

    public void cardExpGain(CardDetails card) {
        if (!cards.ContainsKey(card.cardName)) {
            CardProgress cardProgTmp = new CardProgress(card.prefabRef);
            cards.Add(card.cardName, cardProgTmp);
            CardProgressController.Instance().addCardProg(cardProgTmp, true);
        }

        CardProgress cardProg = cards[card.cardName];
        cardProg.expPoints += (card.character == CombatCharacter.Player() ? selfCastExpGain : enemyCastExpGain);

        int expCap = 0;

        switch (cardProg.progress) {
            case CardProgression.Unlearned: {
                    expCap = untrainedExpCap;
                    break;
                }
            case CardProgression.Trained: {
                    expCap = trainedExpCap;
                    break;
                }
            case CardProgression.Adept: {
                    expCap = adpetExpCap;
                    break;
                }
            case CardProgression.Proficient: {
                    expCap = proficientExpCap;
                    break;
                }
            case CardProgression.Mastered: {
                    expCap = masteredExpCap;
                    break;
                }
            case CardProgression.Perfected: {
                    expCap = 1;
                    break;
                }
        }

        cardProg.expPoints = Mathf.Clamp(cardProg.expPoints, 0, expCap);
        CardProgressController.Instance().checkIfRankUpTextShouldShow(cardProg.cardName);
    }
        
    private static PlayerCardProgress playerCardProgress;
    public static PlayerCardProgress Instance() {
        if (!playerCardProgress) {
            playerCardProgress = FindObjectOfType<PlayerCardProgress>();
        }
        return playerCardProgress;
    }
}

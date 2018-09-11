using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    public CombatCharacter character;
    
    public float bufferWaitTime = 0.0f;
    public float inBetweenCardsBuffer = 0.5f;

    public bool aiThinking;

    public bool debug = false;


    private void Awake() {
        character = GetComponent<CombatCharacter>();
        aiThinking = false;
        //EventManager.StartListening(TurnMaster.getStartPhaseTrigger(Phase.Action, character.gameObject.GetInstanceID()), aiActionPhase);
        character.ai = this;
        bufferWaitTime = 0;
        debug = false;
    }

    public void nudgeAIForDecision() {
        aiThinking = true;
    }

    private void Update() {
        if (aiThinking) {
            if (bufferWaitTime >= inBetweenCardsBuffer) {
                attemptToCastCard();
                bufferWaitTime = 0;
            } else {
                bufferWaitTime += Time.deltaTime;
            }
        }
    }

    private bool firstIsBigger(BaseCard first, BaseCard second) {
        return first.details.getEnergyPlayCost() + first.details.getFocusPlayCost() > second.details.getEnergyPlayCost() + second.details.getFocusPlayCost();
    }

    private void attemptToCastCard() {
        if (debug) Debug.Log("attemptToCastCard");
        bool cardCast = false;

        int plannedEnergy = 0;
        int plannedFocus = 0;
        int plannedActions = 0;
        List<BaseCard> plannedCards = new List<BaseCard>();
        List<BaseCard> handProxy = new List<BaseCard>();
        foreach (BaseCard card in character.hand.cards) {
            handProxy.Add(card);
        }

        for (int i = 0; i < character.hand.cards.Count; i++) {
            if (debug) Debug.Log("~");
            if (debug) Debug.Log("Begining new cycle");
            List<BaseCard> castableCards = new List<BaseCard>();
            foreach (BaseCard card in handProxy) {
                if (card.details.isCardPlayable(plannedEnergy, plannedFocus, plannedActions)) {
                    castableCards.Add(card);
                    if (debug) Debug.Log(card.details.cardName + ": is playable");
                } else {
                    if (debug) Debug.Log(card.details.cardName + ": is NOT playable");
                }
            }

            if (castableCards.Count > 0) {
                BaseCard currentPriorityCard = null;
                foreach (BaseCard card in castableCards) {
                    if (currentPriorityCard == null) {
                        currentPriorityCard = card;
                    } else if (currentPriorityCard.details.subTypes.Contains(CardSubType.Attack)) {
                        if (card.details.subTypes.Contains(CardSubType.Attack)
                            && firstIsBigger(card, currentPriorityCard)) {
                            currentPriorityCard = card;
                        }
                    } else {
                        if (firstIsBigger(card, currentPriorityCard)) {
                            currentPriorityCard = card;
                        }
                    }
                }

                if (debug) Debug.Log(currentPriorityCard.details.cardName + " was CHOSEN!");
                plannedEnergy -= currentPriorityCard.details.getEnergyPlayCost();
                plannedFocus -= currentPriorityCard.details.getFocusPlayCost();
                plannedActions -= 1;
                plannedCards.Add(currentPriorityCard);
                handProxy.Remove(currentPriorityCard);
            } else {
                break;
            }
        }

        if (plannedCards.Count > 0) {
            cardCast = plannedCards[plannedCards.Count - 1].details.attemptToPlayCard();
        }

        if (!cardCast) {
            aiThinking = false;
            character.passedPriority = true;
            bufferWaitTime = 0;
        }
    }
}

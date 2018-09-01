using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    public CombatCharacter character;
    
    public float bufferWaitTime = 0.0f;
    public float inBetweenCardsBuffer = 0.5f;

    public bool aiThinking;


    private void Awake() {
        character = GetComponent<CombatCharacter>();
        aiThinking = false;
        //EventManager.StartListening(TurnMaster.getStartPhaseTrigger(Phase.Action, character.gameObject.GetInstanceID()), aiActionPhase);
        character.ai = this;
    }

    public void nudgeAIForDecision() {
        aiThinking = true;
    }

    private void Update() {
        if (aiThinking) {
            if (bufferWaitTime >= inBetweenCardsBuffer) {
                attemptToCastNextCard();
                bufferWaitTime = 0;
            } else {
                bufferWaitTime += Time.deltaTime;
            }
        }
    }

    private void attemptToCastNextCard() {
        bool cardCast = false;

        foreach (BaseCard card in character.hand.cards) {
            cardCast = card.details.attemptToPlayCard();
            if (cardCast) {
                break;
            }
        }

        if (!cardCast) {
            aiThinking = false;
            character.passedPriority = true;
        }
    }
}

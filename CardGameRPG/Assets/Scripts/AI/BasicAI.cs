using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    public CombatCharacter character;

    public bool myActionPhase = false;
    public bool myDefense = false;
    public float bufferWaitTime = 0.0f;
    public float inBetweenCardsBuffer = 0.5f;

    private void Awake() {
        EventManager.StartListening(TurnMaster.getStartPhaseTrigger(Phase.Action, character.gameObject.GetInstanceID()), aiActionPhase);
        character.ai = this;
    }

    private void aiActionPhase() {
        myActionPhase = true;
    }

    public bool aiThinking() {
        return myActionPhase;
    }

    public static bool anyAIThinking() {
        BasicAI[] ais = FindObjectsOfType<BasicAI>();
        foreach (BasicAI ai in ais) {
            if (ai.aiThinking()) {
                return true;
            }
        }

        return false;
    }

    public void handleDefense() {
        myDefense = true;
    }

    private void Update() {
        if (myActionPhase) {
            if (TurnMaster.subphaseAction == SubphaseAction.WaitingForPlayerInput) {
                if (bufferWaitTime >= inBetweenCardsBuffer) {
                    attemptToCastNextCard();
                    bufferWaitTime = 0;
                } else {
                    bufferWaitTime += Time.deltaTime;
                }
            }
        }
        if (myDefense) {
            if (TurnMaster.subphaseAction == SubphaseAction.WaitingForDefenseResponse) {
                if (bufferWaitTime >= inBetweenCardsBuffer) {
                    attemptToCastNextCard();
                    bufferWaitTime = 0;
                } else {
                    bufferWaitTime += Time.deltaTime;
                }
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
            if (myActionPhase) {
                myActionPhase = false;
                TurnMaster.Instance().incrementPhase();
            } else if (myDefense) {
                myDefense = false;
            }
        }
    }
}

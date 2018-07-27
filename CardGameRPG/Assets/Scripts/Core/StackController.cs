using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour {

    public float phaseBufferLength;
    public float postCardMoveWait;
    public List<BaseCard> theStack;

    public bool isCharacterBeingTargeted(CombatCharacter character) {
        if (theStack.Count == 0) {
            return false;
        } else {
            return stackTop().details.isCardTargetingCharacter(character);
        }
    }

    protected BaseCard stackTop() {
        return theStack[theStack.Count - 1];
    }

    public void addToStack(BaseCard card) {
        theStack.Add(card);
        precastAbilityIndex = 0;
        resolvingPrecastTriggers = true;
        executeCurrentPrecastTrigger();
    }

    private void executeCurrentPrecastTrigger() {
        stackTop().details.getAbility(precastAbilityIndex).preCastTrigger(stackTop().details);
    }

    public void setPostCardMove() {
        waitTimer = postCardMoveWait;
    }

    public float phaseBuffer = 0.0f;
    public float waitTimer = 0.0f;
    public bool transitionPhases = false;
    public bool resolvingPrecastTriggers = false;
    public int precastAbilityIndex = 0;
    private void Update() {
        if (TurnMaster.Instance().gameStarted) {
            if (waitTimer > 0) {
                waitTimer -= Time.deltaTime;
            } else {
                bool movingCard = false;
                foreach (BaseCard card in theStack) {
                    if (card.lerping) {
                        movingCard = true;
                        break;
                    }
                }

                if (resolvingPrecastTriggers) {
                    if (stackTop().details.getAbility(precastAbilityIndex).isPrecastResolved()) {
                        precastAbilityIndex++;
                        if (precastAbilityIndex == stackTop().details.getAbilitiesLength()) {
                            resolvingPrecastTriggers = false;
                        } else {
                            executeCurrentPrecastTrigger();
                        }
                    }
                } else if (movingCard) {
                    // Do nothing?
                } else if (transitionPhases) {
                    phaseBuffer += Time.deltaTime;
                    if (phaseBuffer >= phaseBufferLength) {
                        TurnMaster.Instance().incrementPhase();
                        transitionPhases = false;
                        phaseBuffer = 0.0f;
                    }
                } else {
                    bool isCardPlayable = false;

                    for (int i = 0; i < TurnMaster.Instance().charactersInCombat.Count; i++) {
                        int index = (i + TurnMaster.Instance().currentCharacterTurnIndex) % TurnMaster.Instance().charactersInCombat.Count;
                        CombatCharacter character = TurnMaster.Instance().charactersInCombat[index];
                        foreach (BaseCard card in character.hand.cards) {
                            if (card.details.isCardPlayable()) {
                                isCardPlayable = true;
                                // TODO
                                // Get Response!!!
                                break;
                            }
                        }

                        if (isCardPlayable) {
                            break;
                        }
                    }

                    if (!isCardPlayable) {
                        resolveTopOfStack();
                    }
                }
            }
        }
    }

    private void resolveTopOfStack() {
        if (theStack.Count == 0) {
            transitionPhases = true;
        } else {
            BaseCard card = stackTop();

            card.details.executeAbilities();

            theStack.Remove(card);
            card.putInDiscard();
        }
    }

    private static StackController stackController;
    public static StackController Instance() {
        if (!stackController) {
            stackController = FindObjectOfType<StackController>();
        }
        return stackController;
    }
}

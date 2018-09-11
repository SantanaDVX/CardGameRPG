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
        if (theStack.Count > 0) {
            return theStack[theStack.Count - 1];
        } else {
            return null;
        }
    }

    public bool isTopCardType(CardSubType subType) {
        BaseCard card = stackTop();
        return card != null && card.details.subTypes.Contains(subType);
    }

    public bool isCardSubTypeInStack(CardSubType subType) {
        foreach (BaseCard card in theStack) {
            if (card.details.subTypes.Contains(subType)) {
                return true;
            }
        }

        return false;
    }

    public void addToStack(BaseCard card) {
        TurnMaster.Instance().setContinueButton(false);
        theStack.Add(card);
        resetPriority();
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

    private void resetPriority() {
        playerPriorityIndex = 0;
        foreach (CombatCharacter character in TurnMaster.Instance().charactersInCombat) {
            character.passedPriority = false;
        }
    }

    public CombatCharacter getPriorityCharacter() {
        int index = (playerPriorityIndex + TurnMaster.Instance().currentCharacterTurnIndex) % TurnMaster.Instance().charactersInCombat.Count;
        return TurnMaster.Instance().charactersInCombat[index];
    }

    public float phaseBuffer = 0.0f;
    public float waitTimer = 0.0f;
    public bool transitionPhases = false;
    public bool resolvingPrecastTriggers = false;
    public bool resolvingAbilities = false;
    public int precastAbilityIndex = 0;
    public int playerPriorityIndex = 0;
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
                } else if (resolvingAbilities) {
                    resolveTopOfStack();
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
                    bool someoneHasPlay = false;

                    for (; playerPriorityIndex < TurnMaster.Instance().charactersInCombat.Count; playerPriorityIndex++) {
                        CombatCharacter character = getPriorityCharacter();
                        someoneHasPlay = character.hasPlay();

                        if (someoneHasPlay) {
                            break;
                        }
                    }
                    if (!someoneHasPlay) {
                        resetPriority();
                        if (theStack.Count == 0) {
                            transitionPhases = true;
                        } else {
                            resolvingAbilities = true;
                        }
                    }
                }
            }
        }
    }

    private void resolveTopOfStack() {
        BaseCard card = stackTop();
        bool cardFinished = card.details.executeAbilities();

        if (cardFinished) {
            resolvingAbilities = false;
            int repeatCount = card.details.repeatCount;
            if (repeatCount > 0) {
                card.character.hand.addRepeatCard(card);
                theStack.Remove(card);
            } else {
                theStack.Remove(card);
                card.putInDiscard();
            }
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

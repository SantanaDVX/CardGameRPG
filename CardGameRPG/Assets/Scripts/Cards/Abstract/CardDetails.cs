using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetails : MonoBehaviour {

    public CombatCharacter character;
    public string cardName;
    public int energyPlayCost;
    public int focusPlayCost;
    public bool requireFreeHand;
    public Sprite art;
    public CardType type;
    public List<CardSubType> subTypes;
    public BaseAbility[] abilities;
    public int learnCost;
    public BaseCard cardBase;
    public GameObject prefabRef;

    private bool waitingForPlayArea = false;
    
    private bool resolutionDone = false;
    private float abilityPlayedTime = 0.0f;

    private float minTimeAfterBeginResolution = 2.0f;

    public string getTypeLine() {
        string line = type.ToString() + ": ";

        bool first = true;
        foreach (CardSubType subType in subTypes) {
            if (!first) {
                line += " - ";
            }
            line += subType.ToString();

            first = false;
        }

        return line;
    }

    public virtual string getAbilitiesTextBox() {
        return getTextBoxFromAbilityArray(abilities);
    }

    protected string getTextBoxFromAbilityArray(BaseAbility[] abilityArray) {
        string output = "";
        foreach (BaseAbility ability in abilityArray) {
            if (output != "") {
                output += "\n";
            }

            output += ability.getTextBoxText();
        }

        return output;
    }

    public string getRequirements() {
        if (requireFreeHand) {
            return "\nRequire free hand.";
        } else {
            return "";
        }
    }

    public bool isCardPlayable() {
        /*
        Debug.Log("ME: " + cardName);
        Debug.Log("Active char (" + (character != TurnMaster.Instance().activeCharacter()).ToString() + "): " + TurnMaster.Instance().activeCharacter());
        Debug.Log("character.activeBlock (" + (character.activeBlock == null).ToString() + "): " + character.activeBlock);
        Debug.Log("Phase (" + (TurnMaster.currentPhase == Phase.Action).ToString() + "): " + TurnMaster.currentPhase);
        Debug.Log("Subphase (" + (TurnMaster.subphaseAction == SubphaseAction.WaitingForDefenseResponse).ToString() + "): " + TurnMaster.subphaseAction);
        Debug.Log("playableInCurrentPhase: " + playableInCurrentPhase());
        */
        return playableNow()
            && character.actions > 0
            && character.energy >= energyPlayCost
            && character.focus >= focusPlayCost
            && (!requireFreeHand || (requireFreeHand && character.getIfFreeHand()));
    }

    private bool playableNow() {
        return (character == TurnMaster.Instance().activeCharacter()
             && TurnMaster.currentPhase == Phase.Action
             && StackController.Instance().theStack.Count == 0
             && (subTypes.Contains(CardSubType.Skill)
              || subTypes.Contains(CardSubType.Attack)))
            || (character != TurnMaster.Instance().activeCharacter()
             && TurnMaster.currentPhase == Phase.Action
             && StackController.Instance().isCharacterBeingTargeted(character)
             && (subTypes.Contains(CardSubType.Fast)
              || (subTypes.Contains(CardSubType.Defend)
               && character.activeBlock == null)));
    }

    public void cardClicked() {
        if (character != null) {
            TurnMaster.Instance().checkTargetListener(cardBase);
            attemptToPlayCard();
        }
    }

    public bool attemptToPlayCard() {
        if (isCardPlayable()) {
            character.actions--;
            character.energy -= energyPlayCost;
            character.focus -= focusPlayCost;
            character.hand.removeFromHand(cardBase);
            character.refreshUI();

            cardBase.transform.parent = PlayArea.Instance().transform;
            cardBase.startLerp(PlayArea.Instance().getNextStackPosition());

            StackController.Instance().addToStack(cardBase);
            cardBase.transform.localEulerAngles = new Vector3(0, 0, 0);
            cardBase.hovered = false;
            cardBase.hoverLerping = false;
            cardBase.beingPlayedLerping = true;
            character.refreshUI();

            return true;
        }

        return false;
    }

    private void Start() {
        character = GetComponentInParent<BaseCard>().character;
    }

    public virtual BaseAbility getAbility(int index) {
        return abilities[index];
    }

    public virtual int getAbilitiesLength() {
        return abilities.Length;
    }

    public void executeAbilities() {
        for (int i = 0; i < getAbilitiesLength(); i++) {
            BaseAbility ability = getAbility(i);
            ability.activateAbility(this);
        }
    }

    public bool isCardTargetingCharacter(CombatCharacter character) {
        foreach (BaseAbility ability in abilities) {
            if (ability is TargetAbility) {
                TargetAbility tarAbility = ability as TargetAbility;
                if (tarAbility.target is CombatCharacter) {
                    CombatCharacter tarCharacter = tarAbility.target as CombatCharacter;
                    if (tarCharacter == character) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
}

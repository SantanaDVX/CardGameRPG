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

    protected int abilityResolutionIndex = -1;
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
        return playableInCurrentPhase()
            && character.actions > 0
            && character.energy >= energyPlayCost
            && character.focus >= focusPlayCost
            && (!requireFreeHand || (requireFreeHand && character.getIfFreeHand()));
    }

    private bool playableInCurrentPhase() {
        return (character == TurnMaster.Instance().activeCharacter()
             && TurnMaster.currentPhase == Phase.Action
             && TurnMaster.subphaseAction == SubphaseAction.WaitingForPlayerInput
             && (subTypes.Contains(CardSubType.Skill)
              || subTypes.Contains(CardSubType.Attack)))
            || (character != TurnMaster.Instance().activeCharacter()
             && character.activeBlock == null
             && TurnMaster.currentPhase == Phase.Action
             && TurnMaster.subphaseAction == SubphaseAction.WaitingForDefenseResponse
             && (subTypes.Contains(CardSubType.Defend)
              || subTypes.Contains(CardSubType.Fast)));
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
            waitingForPlayArea = true;
            PlayArea.Instance().cards.Add(cardBase);
            cardBase.transform.localEulerAngles = new Vector3(0, 0, 0);
            TurnMaster.subphaseAction = SubphaseAction.Animating;
            cardBase.hovered = false;
            cardBase.hoverLerping = false;
            character.refreshUI();

            return true;
        }

        return false;
    }

    private void Start() {
        character = GetComponentInParent<BaseCard>().character;
    }

    private void Update() {
        if (waitingForPlayArea && !cardBase.lerping) {
            waitingForPlayArea = false;

            StartCoroutine(playCardAnimation());
        }

        if (resolutionDone
         && Time.time >= abilityPlayedTime + minTimeAfterBeginResolution) {
            resolutionDone = false;
            finishResolution();
        }

        if (abilityResolutionIndex >= 0 && abilityResolutionIndex < abilities.Length) {
            BaseAbility ability = abilities[abilityResolutionIndex];
            if (ability is TargetAbility) {
                TargetAbility tarAbility = ability as TargetAbility;
                if (tarAbility.needToCheckResolution) {
                    tarAbility.PsuedoUpdate();
                }
            }
        }
    }

    IEnumerator playCardAnimation() {
        abilityPlayedTime = Time.time;

        yield return new WaitForSeconds(1);

        continueResolution();
    }

    protected virtual BaseAbility getNextAbilityToResolve() {
        return abilities[abilityResolutionIndex];
    }

    protected virtual bool areAbilitiesAllRan() {
        return abilityResolutionIndex >= abilities.Length;
    }
    
    public void continueResolution() {
        abilityResolutionIndex++;
        if (areAbilitiesAllRan()) {
            resolutionDone = true;
            return;
        } else {
            BaseAbility ability = getNextAbilityToResolve();
            ability.activateAbility(this);
            
            if (!(ability is TargetAbility)) {
                continueResolution();
            }
        }
    }

    public void finishResolution() {
        PlayArea.Instance().cards.Remove(cardBase);
        if (PlayArea.Instance().abilityOnStack()) {
            TurnMaster.subphaseAction = SubphaseAction.WaitingForDefenseResponse;
        } else {
            TurnMaster.subphaseAction = SubphaseAction.WaitingForPlayerInput;
        }
        cardBase.putInDiscard();
    }
}

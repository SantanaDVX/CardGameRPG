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
    protected int abilityResolutionIndex;

    private void Awake() {
        abilityResolutionIndex = 0;
    }

    public int getFocusPlayCost() {
        if ((character == null || character == CombatCharacter.Player())
          && PlayerCardProgress.Instance().cards.ContainsKey(cardName)) {
            CardProgress prog = PlayerCardProgress.Instance().cards[cardName];
            if (prog.progress == CardProgression.Perfected) {
                return Mathf.FloorToInt(((float)focusPlayCost) * 0.8f);
            } else if (prog.progress == CardProgression.Proficient
                    || prog.progress == CardProgression.Mastered) {
                return Mathf.FloorToInt(((float)focusPlayCost) * 0.9f);
            }
        }

        return focusPlayCost;
    }

    public int getEnergyPlayCost() {
        if ((character == null || character == CombatCharacter.Player())
          && PlayerCardProgress.Instance().cards.ContainsKey(cardName)) {
            CardProgress prog = PlayerCardProgress.Instance().cards[cardName];
            if (prog.progress == CardProgression.Perfected) {
                return Mathf.FloorToInt(((float)energyPlayCost) * 0.8f);
            } else if (prog.progress == CardProgression.Proficient
                    || prog.progress == CardProgression.Mastered) {
                return Mathf.FloorToInt(((float)energyPlayCost) * 0.9f);
            }
        }

        return energyPlayCost;
    }

    public int getFocusLearnCost() {
        if ((character == null || character == CombatCharacter.Player())
          && PlayerCardProgress.Instance().cards.ContainsKey(cardName)) {
            CardProgress prog = PlayerCardProgress.Instance().cards[cardName];
            if (prog.progress == CardProgression.Perfected) {
                return Mathf.FloorToInt(((float)learnCost) * 0.8f);
            } else if (prog.progress == CardProgression.Proficient
                    || prog.progress == CardProgression.Mastered) {
                return Mathf.FloorToInt(((float)learnCost) * 0.9f);
            }
        }

        return learnCost;
    }

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
        bool playablity = playableNow()
                       && character.actions > 0
                       && character.energy >= getEnergyPlayCost()
                       && character.focus >= getFocusPlayCost()
                       && (!requireFreeHand || (requireFreeHand && character.getIfFreeHand()));

        cardBase.setGlow(playablity);

        return playablity;
    }

    private bool playableNow() {
        return character == StackController.Instance().getPriorityCharacter()
            && (character == TurnMaster.Instance().activeCharacter()
             && TurnMaster.currentPhase == Phase.Action
             && StackController.Instance().theStack.Count == 0
             && (subTypes.Contains(CardSubType.Skill)
              || subTypes.Contains(CardSubType.Attack)))
            || (character != TurnMaster.Instance().activeCharacter()
             && TurnMaster.currentPhase == Phase.Action
             && StackController.Instance().isCharacterBeingTargeted(character)
             && (subTypes.Contains(CardSubType.Fast)
              || (subTypes.Contains(CardSubType.Defend)
               && character.activeBlock == null
               && !StackController.Instance().isCardSubTypeInStack(CardSubType.Defend))));
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
            character.energy -= getEnergyPlayCost();
            character.focus -= getFocusPlayCost();
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
            character.hand.checkHandGlow();
            cardBase.setGlow(false);
            PlayerCardProgress.Instance().cardExpGain(this);

            return true;
        }

        return false;
    }

    public virtual BaseAbility getAbility(int index) {
        return abilities[index];
    }

    public virtual int getAbilitiesLength() {
        return abilities.Length;
    }

    public bool executeAbilities() {
        BaseAbility ability = getAbility(abilityResolutionIndex);
        ability.activateAbility(this);
        if (ability.isAbilityResolved()) {
            abilityResolutionIndex++;
        }
        return abilityResolutionIndex == getAbilitiesLength();
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
            } else if (ability is MultiTargetAbility) {
                MultiTargetAbility tarAbility = ability as MultiTargetAbility;
                foreach (Targetable target in tarAbility.targets) {
                    if (target is CombatCharacter) {
                        CombatCharacter tarCharacter = target as CombatCharacter;
                        if (tarCharacter == character) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
}

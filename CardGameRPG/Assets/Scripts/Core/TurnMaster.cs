﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnMaster : MonoBehaviour {

    public static Phase currentPhase;
    public static SubphaseAction subphaseAction = SubphaseAction.WaitingForPlayerInput;

    private static Phase startPhase = Phase.Upkeep;
    public GameObject continueButton;

    public Text phaseText;
    public int turnNumber = 1;
    private float timeWaitingInPhase = 0.0f;
    private float phaseTurnoverTime = 0.75f;

    public List<CombatCharacter> charactersInCombat;
    public int currentCharacterTurnIndex;

    public TargetListener targetListener;
    public TargetAbility targetAbilityWaitingForPlayerResponse;

    public static string STARTPHASEPREFIX = "start-phase-";
    public static string ENDPHASEPREFIX = "end-phase-";

    private bool gameStarted = false;

    public static string getGenericStartPhaseTrigger() {
        return STARTPHASEPREFIX + "-generic";
    }
    public static string getStartPhaseTrigger(Phase phase, int characterID) {
        return STARTPHASEPREFIX + phase.ToString() + "-" + characterID;
    }
    public static string getEndPhaseTrigger(Phase phase, int characterID) {
        return ENDPHASEPREFIX + phase.ToString() + "-" + characterID;
    }

    public static string getStartTurnTrigger(int characterID) {
        return getStartPhaseTrigger((Phase)Enum.GetValues(typeof(Phase)).GetValue(0), characterID);
    }
    public static string getEndTurnTrigger(int characterID) {
        return getEndPhaseTrigger((Phase)Enum.GetValues(typeof(Phase)).GetValue(Enum.GetValues(typeof(Phase)).Length - 1), characterID);
    }
    

    public void startGame() {
        currentPhase = startPhase;
        phaseText.text = currentPhase.ToString();
        phaseText.transform.parent.gameObject.SetActive(true);
        charactersInCombat = new List<CombatCharacter>(FindObjectsOfType<CombatCharacter>());
        charactersInCombat.Sort((x, y) => ((x.playerCharacter ? "a_" : "b_") + x.name).CompareTo(((y.playerCharacter ? "a_" : "b_") + y.name)));
        currentCharacterTurnIndex = 0;
        gameStarted = true;
    }

    public CombatCharacter activeCharacter() {
        return (charactersInCombat.Count <= currentCharacterTurnIndex ? null : charactersInCombat[currentCharacterTurnIndex]);
    }

    public void continueButtonActivate() {
        if (activeCharacter() == CombatCharacter.Player()
         && currentPhase == Phase.Action
         && subphaseAction == SubphaseAction.WaitingForPlayerInput) {
            incrementPhase();
        } else if (activeCharacter() != CombatCharacter.Player()
                && currentPhase == Phase.Action
                && subphaseAction == SubphaseAction.WaitingForDefenseResponse
                && targetAbilityWaitingForPlayerResponse != null) {
            targetAbilityWaitingForPlayerResponse.playerSkippedResponse = true;
            setContinueButton(false);
            targetAbilityWaitingForPlayerResponse = null;
        }
    }

    public void incrementPhase() {
        EventManager.TriggerEvent(getEndPhaseTrigger(currentPhase, activeCharacter().gameObject.GetInstanceID()));
        bool grabNextPhase = false;

        foreach (Phase phase in Enum.GetValues(typeof(Phase))) {
            if (grabNextPhase) {
                currentPhase = phase;
                grabNextPhase = false;
                break;
            } else if (phase == currentPhase) {
                grabNextPhase = true;
            }
        }

        if (grabNextPhase) {
            while (true) {
                currentCharacterTurnIndex++;
                if (currentCharacterTurnIndex >= charactersInCombat.Count) {
                    currentCharacterTurnIndex = 0;
                }
                if (activeCharacter() != null) {
                    break;
                }
            }
            currentPhase = startPhase;
            turnNumber++;
        }

        setContinueButton(false);
        if (activeCharacter().playerCharacter) {
            if (currentPhase == Phase.Action) {
                setContinueButton(true);
            }
        } else if (!activeCharacter().playerCharacter) {
            if (currentPhase == Phase.End
             && CombatCharacter.Player().hasFastCard()) {
                setContinueButton(true);
            }
        }

        timeWaitingInPhase = 0;
        phaseText.text = currentPhase.ToString();
        EventManager.TriggerEvent(getGenericStartPhaseTrigger());
        EventManager.TriggerEvent(getStartPhaseTrigger(currentPhase, activeCharacter().gameObject.GetInstanceID()));
    }

    public void setContinueButton(bool active) {
        continueButton.SetActive(active);
        CombatCharacter.Player().refreshUI();
    }

    public bool waitingOnPlayerPhaseButton() {
        return continueButton.activeSelf;
    }
    
    private void Update() {
        if (gameStarted) {
            if (!(PlayArea.Instance().abilityOnStack()
               || waitingOnPlayerPhaseButton()
               || BasicAI.anyAIThinking())) {
                timeWaitingInPhase += Time.deltaTime;
            }

            if (timeWaitingInPhase >= phaseTurnoverTime) {
                incrementPhase();
            }
        }
    }

    public void checkTargetListener(Targetable target) {
        if (targetListener != null) {
            bool targetChosen = targetListener.checkTarget(target);
            if (targetChosen) {
                targetListener = null;
            }
        }
    }

    private static TurnMaster eventManager;

    private static TurnMaster turnMaster;
    public static TurnMaster Instance() {
        if (!turnMaster) {
            turnMaster = FindObjectOfType<TurnMaster>();
        }
        return turnMaster;
    }
}

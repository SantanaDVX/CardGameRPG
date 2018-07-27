using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAbility : BaseAbility {
    public TargetCategory targetCategory;
    public string gameAlertPrompt;
    TargetListener targetListener;
    protected CardDetails details;
    bool targetDodged;

    public bool needToCheckResolution = false;
    bool resolved = false;
    float timeOnResolutionCheck;
    float resolutionCheckWait = 0.5f;

    public bool playerSkippedResponse;

    Targetable target;

    public override void activateAbility(CardDetails details) {
        targetDodged = false;
        resolved = false;
        needToCheckResolution = false;
        playerSkippedResponse = false;
        this.details = details;
        GameObject go = Instantiate(PrefabDictionary.Instance().targetListener, details.cardBase.transform);
        targetListener = go.GetComponent<TargetListener>();
        targetListener.targetCatgeory = targetCategory;
        targetListener.targetSource = details.cardBase;
        targetListener.sourceAbility = this;
        GameAlertUI.Instance().setText(getAlertPromptText());
    }

    public virtual string getAlertPromptText() {
        return gameAlertPrompt;
    }

    public void resolveTargeting(Targetable target) {
        this.target = target;
        needToCheckResolution = true;
        GameAlertUI.Instance().deactivateText();
        if (targetCategory == TargetCategory.Enemy) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            //Debug.Log("Resolve Targeting");
            if (targetCharacter.checkDodge(details.character)) {
                targetDodged = true;
                CombatResultsUI.Instance().dodgeHappened();
            } else {
                //Debug.Log("Not dodged");
                TurnMaster.subphaseAction = SubphaseAction.WaitingForDefenseResponse;
                if (targetCharacter.hasDefendPossibility()) {
                    //Debug.Log("Can Defend");
                    if (targetCharacter.playerCharacter) {
                        targetCharacter.refreshUI();
                        TurnMaster.Instance().targetAbilityWaitingForPlayerResponse = this;
                        TurnMaster.Instance().setContinueButton(true);
                    } else {
                        targetCharacter.ai.handleDefense();
                    }
                }
            }
        }
        timeOnResolutionCheck = 0;
    }

    public void PsuedoUpdate() {
        timeOnResolutionCheck += Time.deltaTime;
        if (needToCheckResolution && timeOnResolutionCheck >= resolutionCheckWait && !resolved && target != null) {
            bool readyForResolution = true;
            if (!playerSkippedResponse) {
                if (targetCategory == TargetCategory.Enemy) {
                    CombatCharacter targetCharacter = target as CombatCharacter;
                    if (targetCharacter.hasDefendPossibility()
                     || PlayArea.Instance().cardOnTopOfStack(details.cardBase)) {
                        readyForResolution = false;
                    }
                }
            }

            if (readyForResolution) {
                if (!targetDodged) {
                    utilizeTarget(target);
                }
                details.continueResolution();
                resolved = true;
            }
        }
    }

    public abstract void utilizeTarget(Targetable target);
}

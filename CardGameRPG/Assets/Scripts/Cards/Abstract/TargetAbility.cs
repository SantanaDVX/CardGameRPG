using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAbility : BaseAbility {
    public TargetCategory targetCategory;
    public string gameAlertPrompt;
    TargetListener targetListener;
    protected CardDetails details;
    public bool undodgeable;
    public float accuracyMod = 1;
    public bool targetOnResolution;
        
    public Targetable target;
    
    public override void preCastTriggerOther(CardDetails details) {
        this.details = details;
        listeningStarted = false;
        if (!targetOnResolution) {
            precastResolve = false;
            startListening();
        }
    }

    public virtual string getAlertPromptText() {
        return gameAlertPrompt;
    }

    private bool listeningStarted = false;
    public override void activateAbility(CardDetails src) {
        this.details = src;
        if (targetOnResolution) {
            if (!listeningStarted) {
                abilityResolve = false;
                startListening();
            }
        } else {
            activateTargetAbility();
        }
    }

    public abstract void activateTargetAbility();

    protected void startListening() {
        listeningStarted = true;
        GameObject go = Instantiate(PrefabDictionary.Instance().targetListener, details.cardBase.transform);
        targetListener = go.GetComponent<TargetListener>();
        targetListener.targetCatgeory = targetCategory;
        targetListener.targetSource = details.cardBase;
        targetListener.sourceAbility = this;
        GameAlertUI.Instance().setText(getAlertPromptText());
    }

    public void resolveTargeting(Targetable target) {
        this.target = target;
        GameAlertUI.Instance().deactivateText();
        if (targetCategory == TargetCategory.Enemy) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            //Debug.Log("Resolve Targeting");
            if ((!undodgeable) && targetCharacter.checkDodge(details.character, accuracyMod)) {
                FloatingDamageTextController.Instance().createDodgeText(targetCharacter.transform);
                this.target = null;
            } else {
                //Debug.Log("Not dodged");
            }
        }

        if (targetOnResolution) {
            abilityResolve = true;
            activateTargetAbility();
        } else {
            precastResolve = true;
        }
    }
    
}

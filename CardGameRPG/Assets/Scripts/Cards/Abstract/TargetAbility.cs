using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAbility : BaseAbility {
    public TargetCategory targetCategory;
    public string gameAlertPrompt;
    TargetListener targetListener;
    protected CardDetails details;
    public bool dodgeable;
        
    public Targetable target;
    
    public override void preCastTrigger(CardDetails details) {
        precastResolve = false;
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
        precastResolve = true;
        GameAlertUI.Instance().deactivateText();
        if (targetCategory == TargetCategory.Enemy) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            //Debug.Log("Resolve Targeting");
            if (dodgeable && targetCharacter.checkDodge(details.character)) {
                CombatResultsUI.Instance().dodgeHappened();
                this.target = null;
            } else {
                //Debug.Log("Not dodged");
            }
        }
    }
    
}

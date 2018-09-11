using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiTargetAbility : BaseAbility {
    public TargetCategory targetCategory;
    public string gameAlertPrompt;
    MultiTargetListener targetListener;
    protected CardDetails details;
    public bool undodgeable;
    public float accuracyMod = 1;
    public int targetCount;
    public bool uniqueTargets;
    protected bool dodged = false;
        
    public List<Targetable> targets;
    
    public override void preCastTriggerOther(CardDetails details) {
        precastResolve = false;
        this.details = details;
        GameObject go = Instantiate(PrefabDictionary.Instance().multiTargetListener, details.cardBase.transform);
        targetListener = go.GetComponent<MultiTargetListener>();
        targetListener.targetCatgeory = targetCategory;
        targetListener.targetSource = details.cardBase;
        targetListener.multiTargetSourceAbility = this;
        targetListener.targetCount = targetCount;
        targetListener.uniqueTargets = uniqueTargets;
        GameAlertUI.Instance().setText(getAlertPromptText());
    }

    public virtual string getAlertPromptText() {
        return gameAlertPrompt;
    }

    public void resolveTargeting(List<Targetable> targets) {
        this.targets = targets;
        precastResolve = true;
        GameAlertUI.Instance().deactivateText();
        if (targetCategory == TargetCategory.Enemy) {
            for (int i = targets.Count - 1; i >= 0; i--) {
                CombatCharacter targetCharacter = this.targets[i] as CombatCharacter;

                //Debug.Log("Resolve Targeting");
                if ((!undodgeable) && targetCharacter.checkDodge(details.character, accuracyMod)) {
                    FloatingDamageTextController.Instance().createDodgeText(targetCharacter.transform);
                    this.targets.RemoveAt(i);
                } else {
                    //Debug.Log("Not dodged");
                }
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour {
    public string textBoxText;
    public bool includeVarX;
    public ResourceType varXResource;
    protected int varX;

    public virtual string getTextBoxText() {
        return getVarXText() + textBoxText;
    }

    protected virtual string getVarXText() {
        if (includeVarX) {
            return "You may pay X " + varXResource.ToString() + " when you play this.\n";
        } else {
            return "";
        }
    }

    public void setVarX(int varX) {
        this.varX = varX;
        varXDetermined = true;
    }

    protected bool abilityResolve = true;
    public bool isAbilityResolved() {
        return abilityResolve;
    }

    protected bool precastResolve = true;
    protected bool varXDetermined = false;
    public bool isPrecastResolved() {
        return precastResolve
            && (!includeVarX
            || (includeVarX
             && varXDetermined));
    }
    public void preCastTrigger(CardDetails details) {
        varXDetermined = false;
        if (includeVarX) {
            XAbilityController.Instance().activateXField(this);
        }
        preCastTriggerOther(details);
    }
    public virtual void preCastTriggerOther(CardDetails details) { }
    public abstract void activateAbility(CardDetails details);
}

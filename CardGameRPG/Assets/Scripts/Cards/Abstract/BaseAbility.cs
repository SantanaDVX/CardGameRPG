using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour {
    public string textBoxText;
    public virtual string getTextBoxText() {
        return textBoxText;
    }

    protected bool precastResolve = true;
    public bool isPrecastResolved() { return precastResolve; }
    public virtual void preCastTrigger(CardDetails details) { }
    public abstract void activateAbility(CardDetails details);
}

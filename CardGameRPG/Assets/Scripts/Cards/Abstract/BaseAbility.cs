using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour {
    public string textBoxText;
    public virtual string getTextBoxText() {
        return textBoxText;
    }
    public abstract void activateAbility(CardDetails details);
}

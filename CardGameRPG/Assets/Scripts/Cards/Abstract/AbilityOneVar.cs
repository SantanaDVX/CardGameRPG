using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityOneVar : BaseAbility {

    public int X;

    public override string getTextBoxText() {
        return fixPlurals(textBoxText.Replace(" X ", " " + X.ToString() + " "));
    }

    public string fixPlurals(string text) {
        string plurality = (X == 1 ? "" : "s");
        return text.Replace("(s?)", plurality);
    }
}

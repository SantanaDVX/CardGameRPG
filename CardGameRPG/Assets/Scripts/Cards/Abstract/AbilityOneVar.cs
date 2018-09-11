using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityOneVar : BaseAbility {

    public int X;

    public override string getTextBoxText() {
        // Do note: I am not check X properly if X is start of a word
        return fixPlurals(textBoxText.Replace(" X ", " " + X.ToString() + " ").Replace(" X", " " + X.ToString()));
    }

    public string fixPlurals(string text) {
        string plurality = (X == 1 ? "" : "s");
        return text.Replace("(s?)", plurality);
    }
}

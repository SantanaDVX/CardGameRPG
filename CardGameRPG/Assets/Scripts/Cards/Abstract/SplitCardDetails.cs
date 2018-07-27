using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCardDetails : CardDetails {

    public BaseAbility[] otherTurnAbilities;

    public override BaseAbility getAbility(int index) {
        if (TurnMaster.Instance().activeCharacter() == character) {
            return abilities[index];
        } else {
            return otherTurnAbilities[index];
        }
    }

    public override int getAbilitiesLength() {
        if (TurnMaster.Instance().activeCharacter() == character) {
            return abilities.Length;
        } else {
            return otherTurnAbilities.Length;
        }
    }
    
    public override string getAbilitiesTextBox() {
        return getTextBoxFromAbilityArray(abilities) + "\nOR\n" + getTextBoxFromAbilityArray(otherTurnAbilities);
    }
}

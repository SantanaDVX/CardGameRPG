using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCardDetails : CardDetails {

    public BaseAbility[] otherTurnAbilities;

    protected override BaseAbility getNextAbilityToResolve() {
        if (TurnMaster.Instance().activeCharacter() == character) {
            return abilities[abilityResolutionIndex];
        } else {
            return otherTurnAbilities[abilityResolutionIndex];
        }
    }

    protected override bool areAbilitiesAllRan() {
        if (TurnMaster.Instance().activeCharacter() == character) {
            return abilityResolutionIndex >= abilities.Length;
        } else {
            return abilityResolutionIndex >= otherTurnAbilities.Length;
        }
    }
    
    public override string getAbilitiesTextBox() {
        return getTextBoxFromAbilityArray(abilities) + "\nOR\n" + getTextBoxFromAbilityArray(otherTurnAbilities);
    }
}

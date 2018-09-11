using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealXDamageMultiTarget : MultiTargetXAbility {
    
    public float weaponMultiply;
    public float varXMultiply;

    public override void activateAbility(CardDetails src) {
        foreach (Targetable target in targets) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            int damageToDo = X 
                           + Convert.ToInt32(weaponMultiply * ((float)details.character.getWeaponDamage()))
                           + Convert.ToInt32(varXMultiply * ((float)varX));

            bool ignoreArmor = details.subTypes.Contains(CardSubType.Magic);

            targetCharacter.getAttacked(damageToDo, ignoreArmor);
        }
    }

    public override string getAlertPromptText() {
        return "Choose " + targetCount + (uniqueTargets ? " unqiue" : "") + "\ntargets to deal damage to.";
    }

    public override string getTextBoxText() {
        string damageText = "";
        if (X > 0) {
            damageText = X.ToString();
        }
        
        if (weaponMultiply > 0) {
            string multiplyText = "";
            if (weaponMultiply != 1) {
                multiplyText = weaponMultiply.ToString();
            }
            damageText += (!damageText.Equals("") ? "+" : "") + multiplyText + "W";
        }
        
        if (varXMultiply > 0 && includeVarX) {
            string multiplyText = "";
            if (weaponMultiply != 1) {
                multiplyText = varXMultiply.ToString();
            }
            damageText += (!damageText.Equals("") ? "+" : "") + multiplyText + "X";
        }

        return getVarXText() + "Deal " + damageText + " damage to " + targetCount + (uniqueTargets ? " unqiue" : "") + " target enemies.";
    }
}

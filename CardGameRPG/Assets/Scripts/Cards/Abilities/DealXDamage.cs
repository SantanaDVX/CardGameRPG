using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealXDamage : TargetXAbility {
    
    public float weaponMultiply;
    public float varXMultiply;

    public override void activateTargetAbility() {
        if (target != null) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            int damageToDo = X 
                           + Convert.ToInt32(weaponMultiply * ((float)details.character.getWeaponDamage()))
                           + Convert.ToInt32(varXMultiply * ((float)varX));

            targetCharacter.getAttacked(damageToDo);
        }
    }

    public override string getAlertPromptText() {
        return "Deal damage to who?";
    }

    public override string getTextBoxText() {
        string weaponText = "";
        if (weaponMultiply > 0) {
            string multiplyText = "";
            if (weaponMultiply != 1) {
                multiplyText = weaponMultiply.ToString();
            }
            weaponText = "+" + multiplyText + "W";
        }

        string varXText = "";
        if (varXMultiply > 0 && includeVarX) {
            string multiplyText = "";
            if (weaponMultiply != 1) {
                multiplyText = varXMultiply.ToString();
            }
            varXText = "+" + multiplyText + "X";
        }

        return getVarXText() + "Deal " + X.ToString() + varXText + weaponText + " damage to target enemy.";
    }
}

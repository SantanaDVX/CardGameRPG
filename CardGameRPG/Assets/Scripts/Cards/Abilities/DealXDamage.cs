using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealXDamage : TargetXAbility {
    
    public float weaponMultiply;

    public override void activateAbility(CardDetails src) {
        CombatCharacter targetCharacter = target as CombatCharacter;

        Debug.Log("src: " + src.cardName);
        Debug.Log("details: " + details.cardName);
        int damageToDo = X + Convert.ToInt32(weaponMultiply * ((float)details.character.getWeaponDamage()));
        
        targetCharacter.getAttacked(damageToDo);
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

        return "Deal " + X.ToString() + weaponText + " damage to target enemy.";
    }
}

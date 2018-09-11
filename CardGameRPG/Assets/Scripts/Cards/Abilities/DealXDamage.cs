using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealXDamage : TargetXAbility {
    
    public float weaponMultiply;
    public float varXMultiply;
    public List<BuffBonusLookup> buffMultipliers;

    public override void activateTargetAbility() {
        if (target != null) {
            CombatCharacter targetCharacter = target as CombatCharacter;

            int damageToDo = X 
                           + Convert.ToInt32(weaponMultiply * ((float)details.character.getWeaponDamage()))
                           + Convert.ToInt32(varXMultiply * ((float)varX));

            foreach (BuffBonusLookup bbl in buffMultipliers) {
                damageToDo += bbl.multiplier * details.character.getBuffCategoryCount(bbl.buff);
            }

            bool ignoreArmor = details.subTypes.Contains(CardSubType.Magic);

            targetCharacter.getAttacked(damageToDo, ignoreArmor);
        }
    }

    public override string getAlertPromptText() {
        return "Deal damage to who?";
    }

    protected string getBuffBonusString() {
        string buffString = "";

        if (buffMultipliers.Count > 0) {
            buffString = "+(";

            string multi = "";
            foreach (BuffBonusLookup bbl in buffMultipliers) {
                buffString += multi + bbl.multiplier.ToString() + " * # of your " + bbl.buff.ToString() + " buffs";
                multi = "+";
            }
            buffString += ")";
        }

        return buffString;
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

        string accuracyText = "";
        if (accuracyMod != 1) {
            accuracyText = " with " + accuracyMod.ToString() + "x accuracy";
        }

        return getVarXText() + "Deal " + X.ToString() + varXText + weaponText + getBuffBonusString() + " damage to target enemy" + accuracyText + ".";
    }
}

[Serializable]
public struct BuffBonusLookup {
    public BuffCategory buff;
    public int multiplier;
}

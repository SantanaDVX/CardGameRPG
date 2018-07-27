using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainBlock : BaseAbility {
    public int blockAmount;
    public float weaponMultiply;

    public override string getTextBoxText() {
        string weaponText = "";
        if (weaponMultiply > 0) {
            string multiplyText = "";
            if (weaponMultiply != 1) {
                multiplyText = weaponMultiply.ToString();
            }
            weaponText = "+" + multiplyText + "W";
        }

        return "Block " + blockAmount.ToString() + weaponText + " damage from next attack.";
    }

    public override void activateAbility(CardDetails details) {
        int totalBlockAmount = blockAmount + Convert.ToInt32(weaponMultiply * ((float)details.character.getWeaponDamage()));

        Block block = new Block(totalBlockAmount);
        details.character.activeBlock = block;
    }
}

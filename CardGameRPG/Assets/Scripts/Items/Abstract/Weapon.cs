using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : BaseEquippable {
    public int weaponDamage;
    public bool freeHand;

    public override string getTooltip() {
        string tooltip = "";

        tooltip += weaponDamage.ToString() + " damage\n";
        tooltip += base.getTooltip();
        if (freeHand) {
            tooltip += "\n"
                     + "Free hand available.";
        }

        return trimEnd(tooltip);
    }
}

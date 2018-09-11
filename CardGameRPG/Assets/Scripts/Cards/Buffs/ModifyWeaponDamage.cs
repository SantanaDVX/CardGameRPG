using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyWeaponDamage : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.weaponDamageMod += mult * X;
    }
}

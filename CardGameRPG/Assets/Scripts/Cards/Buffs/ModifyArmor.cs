using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmor : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.armor += mult * X;
    }
}

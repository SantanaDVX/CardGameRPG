using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyAvoidance : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.avoidance += mult * X;
    }
}

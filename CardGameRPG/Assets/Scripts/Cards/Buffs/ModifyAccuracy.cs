using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyAccuracy : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.accuracy += mult * X;
    }
}

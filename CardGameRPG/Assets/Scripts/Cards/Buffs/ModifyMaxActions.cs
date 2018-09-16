using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMaxActions : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.otherSourcesActionTurnAmountValue += mult * X;
    }
}

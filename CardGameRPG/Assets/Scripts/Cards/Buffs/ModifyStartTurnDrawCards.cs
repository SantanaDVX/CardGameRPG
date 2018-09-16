using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyStartTurnDrawCards : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.otherSourcesCardDrawTurnAmountValue += mult * X;
    }
}

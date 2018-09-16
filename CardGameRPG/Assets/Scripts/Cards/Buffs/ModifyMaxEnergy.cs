using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMaxEnergy : BaseBuff {
    public int X;

    public override void applyBuff(int mult) {
        target.otherSourcesEnergyTurnAmountValue += mult * X;
    }
}

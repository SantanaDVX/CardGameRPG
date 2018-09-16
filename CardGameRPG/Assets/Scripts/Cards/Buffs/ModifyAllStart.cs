using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyAllStart : BaseBuff {
    public int action;
    public int energy;
    public int focus;
    public int cards;

    public override void applyBuff(int mult) {
        target.otherSourcesActionTurnAmountValue += mult * action;
        target.otherSourcesEnergyTurnAmountValue += mult * energy;
        target.otherSourcesFocusTurnAmountValue += mult * focus;
        target.otherSourcesCardDrawTurnAmountValue += mult * cards;
    }
}

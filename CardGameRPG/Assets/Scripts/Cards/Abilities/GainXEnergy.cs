using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainXEnergy : AbilityOneVar {
    public override void activateAbility(CardDetails details) {
        details.character.energy += X;
    }
}

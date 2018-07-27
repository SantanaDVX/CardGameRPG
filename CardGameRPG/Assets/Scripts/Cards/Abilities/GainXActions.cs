using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainXActions : AbilityOneVar {
    public override void activateAbility(CardDetails details) {
        details.character.actions += X;
    }
}
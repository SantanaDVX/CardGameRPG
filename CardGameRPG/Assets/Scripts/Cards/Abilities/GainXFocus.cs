using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainXFocus : AbilityOneVar {
    public override void activateAbility(CardDetails details) {
        details.character.focus += X;
    }
}

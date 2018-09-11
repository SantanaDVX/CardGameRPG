using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainXHealth : AbilityOneVar {
    public override void activateAbility(CardDetails details) {
        details.character.health = Mathf.Clamp(details.character.health + X, details.character.health, details.character.maxHealth);
    }
}

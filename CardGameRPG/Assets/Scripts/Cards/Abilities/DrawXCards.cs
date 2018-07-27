using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawXCards : AbilityOneVar {    
    public override void activateAbility(CardDetails details) {
        details.character.drawXFromDeck(X);
    }
}

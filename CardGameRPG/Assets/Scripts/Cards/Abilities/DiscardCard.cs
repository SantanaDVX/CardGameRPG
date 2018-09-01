using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : TargetAbility {

    public override void activateTargetAbility() {
        BaseCard card = target as BaseCard;
        card.character.hand.discardCard(card);
    }
}

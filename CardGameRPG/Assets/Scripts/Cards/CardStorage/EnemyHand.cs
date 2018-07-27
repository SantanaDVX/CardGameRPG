using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : Hand {
    public Vector3 handDisplacement;


    public override bool getDefaultHiddenStatus() {
        return true;
    }

    public override void organizeCards() {
        for (int i = 0; i < cards.Count; i++) {
            BaseCard card = cards[i];
            
            card.transform.localPosition = new Vector3(handDisplacement.x * i
                                                     , handDisplacement.y * i
                                                     , handDisplacement.z * i);
            card.transform.localEulerAngles = new Vector3(0, (card.hiddenCard ? 0 : 180), 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand {
    public Vector3 handDisplacement;
    public float rotationVariation;

    public override bool getDefaultHiddenStatus() {
        return false;
    }

    public override void organizeCards() {
        float halfPoint = (((float)cards.Count) + 1.0f) / 2.0f;

        for (int i = 0; i < cards.Count; i++) {
            BaseCard card = cards[i];

            float diff = (i + 1.0f) - halfPoint;

            card.handPosition = transform.position + new Vector3(handDisplacement.x * diff
                                                               , handDisplacement.y * Mathf.Abs(diff)
                                                               , handDisplacement.z * -Mathf.Abs(diff));

            card.startLerp(card.handPosition);

            card.transform.localEulerAngles = new Vector3(0, 0, rotationVariation * diff);
        }
    }

    public override void checkHandGlow() {
        foreach (BaseCard card in cards) {
            card.setGlow();
        }
    }
}

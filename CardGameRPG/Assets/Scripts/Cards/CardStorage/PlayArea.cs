using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {

    public List<BaseCard> cards;
    public Vector3 stackDisplacement;
    
    public bool abilityOnStack() {
        return cards.Count > 0;
    }

    public bool cardOnTopOfStack(BaseCard card) {
        int indexOfThisCard = PlayArea.Instance().cards.IndexOf(card);
        return cards.Count > (indexOfThisCard + 1);
    }

    public Vector3 getNextStackPosition() {
        return transform.position + transform.TransformVector((stackDisplacement * cards.Count));
    }


    private static PlayArea playArea;
    public static PlayArea Instance() {
        if (!playArea) {
            playArea = FindObjectOfType<PlayArea>();
        }
        return playArea;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimagusTrackController : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Tooltip[] children = (CombatCharacter.Player() as AnimagusCharacter).animagusTrack.GetComponentsInChildren<Tooltip>();
        for (int i = 0; i < children.Length; i++) {
            setTooltip(children[i], i);
        }
    }

    private void setTooltip(Tooltip tip, int i) {
        int col = i % 5;
        int row = i / 5;
        
        tip.tooltipTitle = "At the start of your turn:";

        string tooltip = "";
        tooltip += "  " + AnimagusCharacter.energyAlignmentValues[col] + " Energy\n";
        tooltip += "  " + AnimagusCharacter.actionAlignmentValues[col] + " Actions\n";
        tooltip += "  " + AnimagusCharacter.cardDrawAlignmentValues[row] + " Cards\n";
        tooltip += "  " + AnimagusCharacter.focusAlignmentValues[row] + " Focus";
        tip.tooltipContent = tooltip;
    }        
}

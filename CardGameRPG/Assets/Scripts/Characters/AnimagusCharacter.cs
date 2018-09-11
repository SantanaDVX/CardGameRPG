using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimagusCharacter : CombatCharacter {

    public int actionEnergySlot;
    public int cardDrawFocusSlot;

    public GameObject animagusTrack;

    public override void refreshUI() {
        base.refreshUI();
        Image[] children = animagusTrack.GetComponentsInChildren<Image>();
        for (int i = 1; i < children.Length; i++) {
            if (i == actionEnergySlot + (5 * cardDrawFocusSlot)) {
                children[i].color = Color.red;
            } else {
                children[i].color = Color.white;
            }
        }
    }

    public override int actionTurnAmount {
        get { return actionAlignmentValues[actionEnergySlot] + otherSourcesActionTurnAmountValue; }
        protected set {}
    }

    public override int energyTurnAmount {
        get { return energyAlignmentValues[actionEnergySlot] + otherSourcesEnergyTurnAmountValue; }
        protected set { }
    }

    public override int cardDrawTurnAmount {
        get { return cardDrawAlignmentValues[cardDrawFocusSlot] + otherSourcesCardDrawTurnAmountValue; }
        protected set { }
    }

    public override int focusTurnAmount {
        get { return focusAlignmentValues[cardDrawFocusSlot] + otherSourcesFocusTurnAmountValue; }
        protected set { }
    }

    public static int[] actionAlignmentValues = new int[] {
        1,
        2,
        3,
        4,
        5
    };

    public static int[] energyAlignmentValues = new int[] {
        110,
        95,
        80,
        65,
        50
    };

    public static int[] cardDrawAlignmentValues = new int[] {
        7,
        6,
        5,
        4,
        3
    };

    public static int[] focusAlignmentValues = new int[] {
        10,
        25,
        40,
        55,
        80
    };
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CombatCharacter {

    public int nonAnimagusActionTurnAmountValue;
    public int nonAnimagusEnergyTurnAmountValue;
    public int nonAnimagusCardDrawTurnAmountValue;
    public int nonAnimagusFocusTurnAmountValue;

    public override void refreshUI() {
        base.refreshUI();
        EnemyUIController.Instance().animagusTrackTab.SetActive(false);
    }

    public override int actionTurnAmount {
        get { return nonAnimagusActionTurnAmountValue + otherSourcesActionTurnAmountValue; }
        protected set { }
    }

    public override int energyTurnAmount {
        get { return nonAnimagusEnergyTurnAmountValue + otherSourcesEnergyTurnAmountValue; }
        protected set { }
    }

    public override int cardDrawTurnAmount {
        get { return nonAnimagusCardDrawTurnAmountValue + otherSourcesCardDrawTurnAmountValue; }
        protected set { }
    }

    public override int focusTurnAmount {
        get { return nonAnimagusFocusTurnAmountValue + otherSourcesFocusTurnAmountValue; }
        protected set { }
    }
}

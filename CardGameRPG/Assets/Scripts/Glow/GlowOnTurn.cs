using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOnTurn : GlowObject {
    public CombatCharacter character;

    private new void Start() {
        base.Start();
        if (character == null) {
            character = GetComponentInParent<BaseCard>().character;
        }
        EventManager.StartListening(TurnMaster.getStartTurnTrigger(character.gameObject.GetInstanceID()), startGlow);
        EventManager.StartListening(TurnMaster.getEndTurnTrigger(character.gameObject.GetInstanceID()), endGlow);
        if (TurnMaster.Instance().activeCharacter() == character) {
            startGlow();
        }
    }
}


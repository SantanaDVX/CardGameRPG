using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOnPhase : GlowObject {
    public Phase glowPhase;
    public bool glowOnlyOnActivePlayer;

    public CombatCharacter character;

    private new void Start() {
        base.Start();
        if (character == null) {
            character = GetComponentInParent<BaseCard>().character;
        }

        if (!glowOnlyOnActivePlayer || (glowOnlyOnActivePlayer && character.playerCharacter)) {
            EventManager.StartListening(TurnMaster.getStartPhaseTrigger(glowPhase, character.gameObject.GetInstanceID()), startGlow);
            EventManager.StartListening(TurnMaster.getEndPhaseTrigger(glowPhase, character.gameObject.GetInstanceID()), endGlow);
            if (TurnMaster.currentPhase == glowPhase) {
                startGlow();
            }
        }
    }
}

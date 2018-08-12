using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetListener : MonoBehaviour {
    public TargetCategory targetCatgeory;
    public BaseCard targetSource;
    public TargetAbility sourceAbility;

    public Targetable target;

    private void Awake() {
        TurnMaster.Instance().targetListener = this;
    }

    private void Start() {
        if (!targetSource.character.playerCharacter) {
            if (targetCatgeory == TargetCategory.Enemy) {
                TurnMaster.Instance().checkTargetListener(CombatCharacter.Player());
            }
        }
    }

    public virtual bool checkTarget(Targetable target) {
        if (targetCatgeory == TargetCategory.Enemy
         && target is CombatCharacter
         && target != targetSource.character) {
            sourceAbility.resolveTargeting(target);
            return true;
        } else if (targetCatgeory == TargetCategory.CardInOwnHand
         && target is BaseCard) {
            BaseCard card = target as BaseCard;
            if (card.character.hand.cards.Contains(card)) {
                sourceAbility.resolveTargeting(target);
                return true;
            }
        }
        return false;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetListener : TargetListener {

    public List<Targetable> targets;
    public int targetCount;
    public MultiTargetAbility multiTargetSourceAbility;
    public bool uniqueTargets;

    private void Awake() {
        TurnMaster.Instance().targetListener = this;
    }

    private void Update() {
        if (!targetSource.character.playerCharacter) {
            if (targetCatgeory == TargetCategory.Enemy) {
                TurnMaster.Instance().checkTargetListener(CombatCharacter.Player());
            }
        }
    }

    public override bool checkTarget(Targetable target) {
        if (targetCatgeory == TargetCategory.Enemy
         && target is CombatCharacter
         && target != targetSource.character
         && (!uniqueTargets || (uniqueTargets && !targets.Contains(target)))) {
            targets.Add(target);
        } else if (targetCatgeory == TargetCategory.CardInOwnHand
         && target is BaseCard) {
            BaseCard card = target as BaseCard;
            if (card.character.hand.cards.Contains(card)) {
                targets.Add(target);
            }
        }

        bool resolve = false;
        if (targets.Count == targetCount) {
            resolve = true;
        } else if (uniqueTargets) {
            if (targetCatgeory == TargetCategory.Enemy) {
                CombatCharacter tarCharacter = target as CombatCharacter;
                if (tarCharacter.playerCharacter) {
                    resolve = true;
                } else if (targets.Count == (TurnMaster.Instance().charactersInCombat.Count - 1)) {
                    resolve = true;
                }
            } else if (targetCatgeory == TargetCategory.CardInOwnHand) {
                BaseCard card = target as BaseCard;
                if (targets.Count == card.character.hand.cards.Count) {
                    resolve = true;
                }
            }
        }

        if (resolve) {
            multiTargetSourceAbility.resolveTargeting(targets);
            return true;
        } else {
            return false;
        }
    }    
}

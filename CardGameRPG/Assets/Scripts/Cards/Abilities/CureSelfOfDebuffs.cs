using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureSelfOfDebuffs : BaseAbility {

    public BuffCategory buffCategoryToRemove;

    public override void activateAbility(CardDetails details) {
        for (int i = details.character.buffs.Count; i >= 0; i--) {
            if (details.character.buffs[i].categories.Contains(buffCategoryToRemove)) {
                details.character.buffs[i].removeDebuff();
            }
        }
    }
}

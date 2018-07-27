using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySelfBuffAbility : BaseAbility {

    public GameObject abilityPrefab;

    public override void activateAbility(CardDetails details) {
        GameObject buffGO = Instantiate(abilityPrefab, details.character.transform);
        BaseBuff buff = buffGO.GetComponent<BaseBuff>();
        buff.createBuff(details.character, details.character);
    }
}

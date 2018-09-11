using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRandomSelfBuffAbility : ApplySelfBuffAbility {
    
    public List<GameObject> randomBuffList;

    public override GameObject getBuff() {
        return randomBuffList[Random.Range(0, randomBuffList.Count)];
    }
    
}

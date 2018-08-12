using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanelController : MonoBehaviour {

    public GearUI gearUI;
    public BuffUI buffUI;

    public void updateTabs(CombatCharacter character) {
        gearUI.updateIcons(character);
        buffUI.updateBuffs(character);
    }
}

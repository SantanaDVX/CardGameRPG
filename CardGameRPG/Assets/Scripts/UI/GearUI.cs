using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearUI : MonoBehaviour {
    public Image armorIconSlot;
    public Image weaponIconSlot;

    public void updateIcons(CombatCharacter character) {

        if (character.equippedWeapon != null) {
            weaponIconSlot.sprite = character.equippedWeapon.itemIcon;
        }
        if (character.equippedArmor != null) {
            armorIconSlot.sprite = character.equippedArmor.itemIcon;
        }
    }
}

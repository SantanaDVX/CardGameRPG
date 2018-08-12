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
            weaponIconSlot.GetComponent<Tooltip>().tooltipTitle = character.equippedWeapon.itemName;
            weaponIconSlot.GetComponent<Tooltip>().tooltipContent = character.equippedWeapon.getTooltip();
        }
        if (character.equippedArmor != null) {
            armorIconSlot.sprite = character.equippedArmor.itemIcon;
            armorIconSlot.GetComponent<Tooltip>().tooltipTitle = character.equippedArmor.itemName;
            armorIconSlot.GetComponent<Tooltip>().tooltipContent = character.equippedArmor.getTooltip();
        }
    }
}

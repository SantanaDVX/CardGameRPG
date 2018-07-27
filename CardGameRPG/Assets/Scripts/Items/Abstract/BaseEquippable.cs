using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEquippable : BaseItem {
    public int avoidanceMod;
    public int armorMod;
    public int accuracyMod;
    public int actionMod;
    public int energyMod;
    public int cardsMod;
    public int focusMod;

    public void equip(CombatCharacter character) {
        changeEquipStatus(character, 1);
    }
    public void unequip(CombatCharacter character) {
        changeEquipStatus(character, -1);
    }
    public void changeEquipStatus(CombatCharacter character, int multi) {
        character.avoidance += avoidanceMod * multi;
        character.armor += armorMod * multi;
        character.accuracy += accuracyMod * multi;
        character.otherSourcesActionTurnAmountValue += actionMod * multi;
        character.otherSourcesEnergyTurnAmountValue += energyMod * multi;
        character.otherSourcesCardDrawTurnAmountValue += cardsMod * multi;
        character.otherSourcesFocusTurnAmountValue += focusMod * multi;
        changeUniqueStatus(character, multi);
    }

    public virtual string getTooltip() {
        string tooltip = "";

        if (avoidanceMod != 0) {
            tooltip += startStr(avoidanceMod) + " avoidance\n";
        }

        if (armorMod != 0) {
            tooltip += startStr(armorMod) + " armor\n";
        }

        if (accuracyMod != 0) {
            tooltip += startStr(accuracyMod) + " accuracy\n";
        }

        if (actionMod != 0) {
            tooltip += startStr(actionMod) + " action" + getS(actionMod) + " per turn\n";
        }

        if (energyMod != 0) {
            tooltip += startStr(energyMod) + " energy per turn\n";
        }

        if (cardsMod != 0) {
            tooltip += startStr(cardsMod) + " card" + getS(cardsMod) + " per turn\n";
        }

        if (focusMod != 0) {
            tooltip += startStr(focusMod) + " focus per turn\n";
        }

        return trimEnd(tooltip);
    }

    private string startStr(int val) {
        return (val > 0 ? "+" : "-") + Mathf.Abs(val).ToString();
    }

    private string getS(int val) {
        return (Mathf.Abs(val) == 1 ? "" : "s");
    }

    protected string trimEnd(string str) {
        return str.TrimEnd('\r', '\n');
    }

    public virtual void changeUniqueStatus(CombatCharacter character, int multi) { }
}

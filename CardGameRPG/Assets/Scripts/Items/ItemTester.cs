using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour {
    public Weapon weapon;
    public Armor armor;

    public CombatCharacter character;

	void Start () {
        if (weapon != null) {
            character.equipItem(weapon);
        }
        if (armor != null) {
            character.equipItem(armor);
        }
    }
}

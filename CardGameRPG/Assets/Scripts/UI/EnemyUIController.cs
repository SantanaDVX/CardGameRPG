using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIController : MonoBehaviour {

    public CharacterUI ui;
    public GearUI gearUI;

    public GameObject animagusTrackTab;

    public void setUI(CombatCharacter character) {
        character.ui = this.ui;
        character.gearUI = this.gearUI;
    }

    private static EnemyUIController enemyUIController;
    public static EnemyUIController Instance() {
        if (!enemyUIController) {
            enemyUIController = FindObjectOfType<EnemyUIController>();
        }
        return enemyUIController;
    }
}

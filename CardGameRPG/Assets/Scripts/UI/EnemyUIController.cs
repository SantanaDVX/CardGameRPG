using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIController : MonoBehaviour {

    public CharacterUI ui;
    public TabPanelController tabPanel;

    public GameObject animagusTrackTab;

    public void setUI(CombatCharacter character) {
        character.ui = this.ui;
        character.tabPanel = this.tabPanel;
    }

    private static EnemyUIController enemyUIController;
    public static EnemyUIController Instance() {
        if (!enemyUIController) {
            enemyUIController = FindObjectOfType<EnemyUIController>();
        }
        return enemyUIController;
    }
}

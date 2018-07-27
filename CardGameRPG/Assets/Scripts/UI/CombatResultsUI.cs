using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatResultsUI : MonoBehaviour {

    public GameObject panel;
    public Text dmgText;
    public Text armorText;
    public Text blockText;
    public Text healthLostText;
    public float resultsTimeShown;

    public GameObject dodgedPanel;
    public float dodgedTimeShown;


    public void setCombatResultsUI(int dmg, int armor, int block, int healthLost) {
        panel.SetActive(true);

        dmgText.text = dmg.ToString();
        armorText.text = armor.ToString();
        blockText.text = block.ToString();
        healthLostText.text = "-" + healthLost.ToString();

        StartCoroutine(removeCombatResults());
    }

    public void dodgeHappened() {
        dodgedPanel.SetActive(true);
        
        StartCoroutine(removeDodgedPanel());
    }

    IEnumerator removeDodgedPanel() {
        yield return new WaitForSeconds(dodgedTimeShown);

        dodgedPanel.SetActive(false);
    }

    IEnumerator removeCombatResults() {
        yield return new WaitForSeconds(resultsTimeShown);

        panel.SetActive(false);
    }


    private static CombatResultsUI combatResultsUI;
    public static CombatResultsUI Instance() {
        if (!combatResultsUI) {
            combatResultsUI = FindObjectOfType<CombatResultsUI>();
        }
        return combatResultsUI;
    }
}

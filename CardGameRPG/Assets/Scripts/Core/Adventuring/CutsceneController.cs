using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour {
    private string battleScene = "BattleScene";

    public GameObject cutscenePageParagraphPrefab;
    private Adventure curAdventure;
    private Cutscene curCutscene;
    private int curCutscenePageIndex;

    public void preBattleCutscene(Adventure adventure) {
        curAdventure = adventure;
        startCutscene(curAdventure.preAdventureCutscene);
    }

    public void resolvePreBattleCutscene() {
        switchToBattle();
        AdventureController.Instance().startAdventure(curAdventure);
    }

    private void switchToBattle() {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(battleScene));
        GameController.Instance().switchToNode("Battle");
    }


    public void startCutscene(Cutscene cutscene) {
        curCutscene = cutscene;
        curCutscenePageIndex = 0;
        setCutscenePage(curCutscene.pages[curCutscenePageIndex]);
    }

    private void endCutscene() {
        if (curCutscene == curAdventure.preAdventureCutscene) {
            resolvePreBattleCutscene();
        } else {
            switchToBattle();
            AdventureController.Instance().continueWithAdventure();
        }
    }

    public void turnCutscenePage() {
        curCutscenePageIndex++;
        if (curCutscenePageIndex == curCutscene.pages.Count) {
            endCutscene();
        } else {
            setCutscenePage(curCutscene.pages[curCutscenePageIndex]);
        }
    }

    public void setCutscenePage(CutscenePage page) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string paragraph in page.paragraphs) {
            GameObject paraGO = Instantiate(cutscenePageParagraphPrefab, transform);
            paraGO.GetComponent<Text>().text = paragraph;
        }
    }

    private static CutsceneController cutsceneController;
    public static CutsceneController Instance() {
        if (!cutsceneController) {
            cutsceneController = FindObjectOfType<CutsceneController>();
        }
        return cutsceneController;
    }
}

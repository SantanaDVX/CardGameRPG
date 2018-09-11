using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour {
    private string battleScene = "BattleScene";

    public GameObject cutscenePageParagraphPrefab;
    private Adventure curAdventure;
    private Story curStory;
    private Cutscene curCutscene;
    private int curCutscenePageIndex;

    private void resetCurSetting() {
        curAdventure = null;
        curStory = null;
    }

    public void preBattleCutscene(Adventure adventure) {
        resetCurSetting();
        curAdventure = adventure;
        startCutscene(curAdventure.preAdventureCutscene);
    }

    public void storyCutscene(Story story) {
        resetCurSetting();
        curStory = story;
        startCutscene(curStory.storyCutscene);
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
        if (curAdventure != null) {
            if (curCutscene == curAdventure.preAdventureCutscene) {
                resolvePreBattleCutscene();
            } else {
                switchToBattle();
                AdventureController.Instance().continueWithAdventure();
            }
        } else if (curStory != null) {
            GameController.Instance().finishResolvable(curStory);
        } else {
            throw new System.Exception("Unexpected cutscene transition");
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

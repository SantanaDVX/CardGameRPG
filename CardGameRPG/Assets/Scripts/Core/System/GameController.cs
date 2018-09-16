using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private string worldScene = "WorldScene";
    private string battleScene = "BattleScene";
    private string previousnode = "Adventure";

    private Adventure nextAdventure;

    public void startStory(Story story) {
        PlayerStoryProgress.Instance().incrementStoryFlag(AdventureFlag.StoryContentCnt);
        switchToNode("Cutscene");
        CutsceneController.Instance().storyCutscene(story);
    }

    public void startPreadventure(Adventure adventure) {
        nextAdventure = adventure;
        switchToNode("Preadventure");
        PreadventureCardController.Instance().setupCollection();
    }

    public void startAdventure() {
        StartCoroutine(loadBattleSceneAndStart(nextAdventure));
    }

    IEnumerator loadBattleSceneAndStart(Adventure adventure) {
        switchToNode("Cutscene");
        AsyncOperation loading = SceneManager.LoadSceneAsync("Scenes/" + battleScene, LoadSceneMode.Additive);
        yield return loading;
        CutsceneController.Instance().preBattleCutscene(adventure);
    }

    public void finishAdventure(Adventure adventure) {
        PlayerStoryProgress.Instance().decrementStoryFlag(AdventureFlag.StoryContentCnt);
        finishResolvable(adventure);
        StartCoroutine(removeBattleScene());
    }

    public void finishResolvable(LocationContentWithResolution lcwr) {
        switchToNode("Resolution");
        ResolutionController.Instance().setResolutionPage(lcwr);
    }

    public void finishResolution() {
        DateController.Instance().moveTimeForward(1);
        switchToNode("World");
    }

    IEnumerator removeBattleScene() {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(worldScene));
        AsyncOperation loading = SceneManager.UnloadSceneAsync("Scenes/" + battleScene);
        yield return loading;
    }

    public void switchToNode(string node) {
        foreach (KeyValuePair<string, SubSceneNode> nodePair in SubSceneNode.getDict()) {
            if (nodePair.Value.isActive()) {
                nodePair.Value.setActive(false);
                previousnode = nodePair.Key;
                break;
            }
        }

        SubSceneNode.getNode(node).setActive(true);
    }

    public void returnToPreviousNode() {
        switchToNode(previousnode);
    }

    private static GameController gameController;
    public static GameController Instance() {
        if (!gameController) {
            gameController = FindObjectOfType<GameController>();
        }
        return gameController;
    }
}

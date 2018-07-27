using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private string battleScene = "BattleScene";    
    private string previousnode = "Adventure";

    public void startBattle() {
        SubSceneNode.getNode("Adventure").setActive(false);
        StartCoroutine(loadBattleSceneAndStart());
    }

    IEnumerator loadBattleSceneAndStart() {
        AsyncOperation loading = SceneManager.LoadSceneAsync("Scenes/" + battleScene, LoadSceneMode.Additive);
        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(battleScene));
        PlayerInfo.Instance().loadPlayerInfo();
        TurnMaster.Instance().startGame();
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
    
    public void switchToCollectionView() {
        switchToNode("Collection");
        CollectionController.Instance().setupCollection();
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

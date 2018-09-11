using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureController : MonoBehaviour {
    public Adventure adventure;
    public Encounter currentEncounter;
    private string battleScene = "BattleScene";
    public GameObject victoryAnimationPrefab;
    public Transform victoryAnimationTransform;

    public void startAdventure(Adventure adv) {
        preBattle = false;
        GameObject advGO = Instantiate(adv.gameObject, transform);
        adventure = advGO.GetComponent<Adventure>();
        currentEncounter = adventure.getNextEncounter();
        PlayerInfo.Instance().loadPlayerInfo();
        SideboardController.Instance().loadSideboard();
        startEncounter();
    }

    protected void finishAdventure() {
        foreach (AdventureFlag flag in adventure.completionFlags) {
            PlayerStoryProgress.Instance().incrementStoryFlag(flag);
        }
        CombatCharacter.Player().discard.discardContents = new List<GameObject>();
        CombatCharacter.Player().hand.cards = new List<BaseCard>();
        CombatCharacter.Player().deck.deckContents = new List<GameObject>();
        PlayerInfo.Instance().sideboard = new List<GameObject>();
        GameController.Instance().finishAdventure(adventure);
    }

    public void victoryAnimation() {
        Instantiate(victoryAnimationPrefab, victoryAnimationTransform);
    }

    public void finishEncounter() {
        preBattle = false;
        if (currentEncounter.postEncounterCutscene != null) {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(battleScene));
            GameController.Instance().switchToNode("Cutscene");
            CutsceneController.Instance().startCutscene(currentEncounter.postEncounterCutscene);
        } else {
            continueWithAdventure();
        }
    }

    public void startEncounter() {
        preBattle = true;

        if (currentEncounter.preEncounterCutscene != null) {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(battleScene));
            GameController.Instance().switchToNode("Cutscene");
            CutsceneController.Instance().startCutscene(currentEncounter.preEncounterCutscene);
        } else {
            continueWithAdventure();
        }
    }

    private bool preBattle;
    public void continueWithAdventure() {
        if (preBattle) {
            List<CombatCharacter> enemies = EnemySpawner.Instance().spawnEnemies(currentEncounter.enemies);
            TurnMaster.Instance().startBattle(enemies);
        } else {
            currentEncounter = adventure.getNextEncounter();
            if (currentEncounter == null) {
                finishAdventure();
            } else {
                startEncounter();
            }
        }
    }

    private static AdventureController adventureController;
    public static AdventureController Instance() {
        if (!adventureController) {
            adventureController = FindObjectOfType<AdventureController>();
        }
        return adventureController;
    }
}

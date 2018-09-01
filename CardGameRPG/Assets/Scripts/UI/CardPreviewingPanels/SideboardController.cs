using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideboardController : MonoBehaviour {

    public Transform sideboardListTransform;
    public GameObject sideboardListItemPrefab;
    public BaseCard sideboardPreviewCard;
    public Button addToDeckButton;

    public GameObject currentPreviewedCard;

    public void toggleSideboardPanel() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void loadSideboardAtStart() {
        loadSideboard();
        gameObject.SetActive(false);
    }

    public void loadSideboard() {
        foreach (Transform child in sideboardListTransform) {
            GameObject.Destroy(child.gameObject);
        }

        if (PlayerInfo.Instance().sideboard.Count > 0) {
            currentPreviewedCard = PlayerInfo.Instance().sideboard[0];
        }
        previewCard(currentPreviewedCard);

        foreach (GameObject card in PlayerInfo.Instance().sideboard) {
            GameObject sideboardItemGO = Instantiate(sideboardListItemPrefab, sideboardListTransform);
            sideboardItemGO.GetComponent<SideboardListItem>().setCard(card);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(sideboardListTransform as RectTransform);

        if (!CombatCharacter.Player().atleastOneSideboardLearnable()) {
            gameObject.SetActive(false);
        }
    }

    public void addActiveCardToDeck() {
        if (CombatCharacter.Player().focus >= currentPreviewedCard.GetComponent<CardDetails>().getFocusLearnCost()) {
            CombatCharacter.Player().focus -= currentPreviewedCard.GetComponent<CardDetails>().getFocusLearnCost();
            CombatCharacter.Player().discard.discardContents.Add(currentPreviewedCard);
            PlayerInfo.Instance().sideboard.Remove(currentPreviewedCard);
            loadSideboard();
            CombatCharacter.Player().refreshUI();
        }
    }

    public void previewCard(GameObject card) {
        currentPreviewedCard = card;
        sideboardPreviewCard.loadCardDetails(currentPreviewedCard);
    }

    private static SideboardController sideboardController;
    public static SideboardController Instance() {
        if (!sideboardController) {
            sideboardController = FindObjectOfType<SideboardController>();
        }
        return sideboardController;
    }
}

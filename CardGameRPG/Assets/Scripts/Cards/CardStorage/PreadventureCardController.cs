using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreadventureCardController : MonoBehaviour {
    private int MIN_PREADVENTURE_DECK_SIZE = 10;

    public Vector3 collectionOffset;
    public GameObject collectionHolder;
    private int columnCnt = 5;

    public List<GameObject> collectionCards = new List<GameObject>();

    public int totalFocusCost;
    public int cardCnt;
    public Text focusText;
    public Text cardCntText;
    public Button startAdventureButton;

    public void beginAdventure() {
        foreach (GameObject card in collectionCards) {
            GameObject cardRef = card.GetComponent<BaseCard>().details.prefabRef;
            if (card.GetComponent<PreadventureCardSelector>().inDeck) {
                PlayerInfo.Instance().deck.deckContents.Add(cardRef);
            } else {
                PlayerInfo.Instance().sideboard.Add(cardRef);
            }
        }
        GameController.Instance().startAdventure();
    }
 
    public void toggleCard(CardDetails card, bool added) {
        totalFocusCost += (added ? card.getFocusLearnCost() : -1 * card.getFocusLearnCost());
        cardCnt += (added ? 1 : -1);

        focusText.text = totalFocusCost + " / " + PlayerInfo.Instance().preadventureFocus + " Focus";
        cardCntText.text = cardCnt + " / " + MIN_PREADVENTURE_DECK_SIZE + " Cards";

        startAdventureButton.interactable = cardCnt >= MIN_PREADVENTURE_DECK_SIZE
                                         && totalFocusCost <= PlayerInfo.Instance().preadventureFocus;

    }
    
    public void setupCollection() {
        foreach (GameObject collectionCardGO in collectionCards) {
            Destroy(collectionCardGO);
        }
        totalFocusCost = 0;
        cardCnt = 0;
        collectionCards = new List<GameObject>();

        displayPlayerCardCollection();
        positionCards();
    }

    private void displayPlayerCardCollection() {
        foreach (GameObject cardGO in PlayerCardCollection.Instance().cardCollection) {
            GameObject go = Instantiate(PrefabDictionary.Instance().cardBase, Vector3.zero, Quaternion.Euler(collectionHolder.transform.rotation.eulerAngles + new Vector3(90, 0, 0)), collectionHolder.transform);
            go.AddComponent<PreadventureCardSelector>();
            BaseCard card = go.GetComponent<BaseCard>();
            //Instantiate(PrefabDictionary.Instance().inDeckOverlay, go.transform);
            card.loadCardDetails(cardGO);

            collectionCards.Add(go);
        }
    }

    private void positionCards() {
        for (int i = 0; i < collectionCards.Count; i++) {
            GameObject go = collectionCards[i];
            go.transform.localPosition = new Vector3(collectionOffset.x * (i % columnCnt), 0, collectionOffset.z * Mathf.Floor(i / columnCnt));
        }
    }    

    private static PreadventureCardController preadventureCardController;
    public static PreadventureCardController Instance() {
        if (!preadventureCardController) {
            preadventureCardController = FindObjectOfType<PreadventureCardController>();
        }
        return preadventureCardController;
    }
}

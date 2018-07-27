using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionController : MonoBehaviour {

    public Vector3 collectionOffset;
    public GameObject collectionHolder;
    private int columnCnt = 5;

    public List<GameObject> collectionCards = new List<GameObject>();
    

    public void setupCollection() {
        foreach (GameObject collectionCardGO in collectionCards) {
            Destroy(collectionCardGO);
        }
        collectionCards = new List<GameObject>();

        displayCollectionSection(PlayerInfo.Instance().deck.deckContents, true);
        displayCollectionSection(PlayerInfo.Instance().sideboard, false);
        positionCards();
    }

    private void displayCollectionSection(List<GameObject> cardPrefabs, bool includedInDeck) {
        int cardIndex = collectionCards.Count;
        foreach (GameObject cardGO in cardPrefabs) {
            GameObject go = Instantiate(PrefabDictionary.Instance().cardBase, Vector3.zero, Quaternion.Euler(90, 0, 0), collectionHolder.transform.transform);
            BaseCard card = go.GetComponent<BaseCard>();
            if (includedInDeck) {
                GameObject overlay = Instantiate(PrefabDictionary.Instance().inDeckOverlay, go.transform);
            }
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

    private static CollectionController collectionController;
    public static CollectionController Instance() {
        if (!collectionController) {
            collectionController = FindObjectOfType<CollectionController>();
        }
        return collectionController;
    }
}

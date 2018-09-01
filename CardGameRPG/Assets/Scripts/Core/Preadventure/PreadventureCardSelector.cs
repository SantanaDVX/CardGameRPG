using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreadventureCardSelector : MonoBehaviour {
    public bool inDeck;
    private GameObject overlay;    

    private void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (inDeck) {
                Destroy(overlay);
                overlay = null;
            } else {
                overlay = Instantiate(PrefabDictionary.Instance().preadventureCardInDeckOverlay, gameObject.transform);
            }
            inDeck = !inDeck;
            PreadventureCardController.Instance().toggleCard(GetComponent<BaseCard>().details, inDeck);
        }
    }
}

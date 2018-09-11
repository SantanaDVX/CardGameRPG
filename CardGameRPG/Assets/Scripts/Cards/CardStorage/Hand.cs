using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hand : MonoBehaviour {

    public List<BaseCard> cards;
    public Transform spawnPoint;

    public bool debugReorganizeCards = false;

    public void addRepeatCard(BaseCard card) {
        card.details.repeatCard();
        addCard(card);
    }

    public void addNewCard(GameObject details) {
        GameObject go = Instantiate(PrefabDictionary.Instance().cardBase, spawnPoint.position, Quaternion.identity, transform.transform);
        BaseCard card = go.GetComponent<BaseCard>();
        card.character = character;
        card.hiddenCard = getDefaultHiddenStatus();
        card.loadCardDetails(details);

        addCard(card);
    }

    protected void addCard(BaseCard card) {
        cards.Add(card);

        organizeCards();
    }

    public CombatCharacter character;

    public void discardCard(int i) {
        discardCard(cards[i]);
    }

    public void discardCard(BaseCard card) {
        removeFromHand(card);
        card.putInDiscard();
    }

    public void removeFromHand(BaseCard card) {
        cards.Remove(card);
        organizeCards();
    }

    private void Update() {
        if (debugReorganizeCards) {
            debugReorganizeCards = false;
            organizeCards();
        }
    }

    public virtual void checkHandGlow() { }
    public abstract void organizeCards();
    public abstract bool getDefaultHiddenStatus();

}
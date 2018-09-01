using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hand : MonoBehaviour {

    public List<BaseCard> cards;
    public Transform spawnPoint;

    public void addCard(GameObject details) {
        GameObject go = Instantiate(PrefabDictionary.Instance().cardBase, spawnPoint.position, Quaternion.identity, transform.transform);
        BaseCard card = go.GetComponent<BaseCard>();
        card.character = character;
        card.hiddenCard = getDefaultHiddenStatus();
        card.loadCardDetails(details);

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

    public virtual void checkHandGlow() { }
    public abstract void organizeCards();
    public abstract bool getDefaultHiddenStatus();

}
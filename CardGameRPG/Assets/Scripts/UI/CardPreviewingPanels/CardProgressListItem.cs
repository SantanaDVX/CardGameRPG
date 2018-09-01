using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardProgressListItem : MonoBehaviour, IPointerClickHandler {
    public Text cardNameText;
    public Text focusCostText;
    public GameObject rankUpText;
    public GameObject card;

    public void setCard(CardProgress cardProg) {
        card = cardProg.card;
        cardNameText.text = card.GetComponent<CardDetails>().cardName;
        focusCostText.text = card.GetComponent<CardDetails>().getFocusLearnCost().ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left
         || eventData.button == PointerEventData.InputButton.Middle
         || eventData.button == PointerEventData.InputButton.Right) {
            CardProgressController.Instance().previewCard(card);
        }
    }
}

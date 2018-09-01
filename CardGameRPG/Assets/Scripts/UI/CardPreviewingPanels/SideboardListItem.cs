using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SideboardListItem : MonoBehaviour, IPointerClickHandler {
    public Text cardNameText;
    public Text focusCostText;
    public GameObject card;

    public void setCard(GameObject card) {
        this.card = card;
        cardNameText.text = card.GetComponent<CardDetails>().cardName;
        focusCostText.text = card.GetComponent<CardDetails>().getFocusLearnCost().ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left
         || eventData.button == PointerEventData.InputButton.Middle
         || eventData.button == PointerEventData.InputButton.Right) {
            SideboardController.Instance().previewCard(card);
        }
    }
}


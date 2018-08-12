using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private bool mouseOver;

    public string tooltipTitle;

    public string tooltipContent;

    public void OnPointerEnter(PointerEventData eventData) {
        if (!mouseOver) {
            TooltipController.Instance().setTooltip(tooltipTitle, tooltipContent);
            mouseOver = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (mouseOver) {
            TooltipController.Instance().disableTooltip();
            mouseOver = false;
        }
    }
}

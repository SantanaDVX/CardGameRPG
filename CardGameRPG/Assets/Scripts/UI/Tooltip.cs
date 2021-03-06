﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private bool mouseOver;

    public string tooltipTitle;

    public string tooltipContent;

    public List<BuffCategory> buffCategories;

    public void OnPointerEnter(PointerEventData eventData) {
        if (!mouseOver) {
            TooltipController.Instance().setTooltip(tooltipTitle, tooltipContent, buffCategories);
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

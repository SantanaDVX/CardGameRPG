using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour {

    public GameObject tooltipRoot;

    public float tooltipPreStartTime;

    public Text tooltipTitleText;
    public Text tooltipContentText;

    private void Start() {
        tooltipRoot.SetActive(false);
    }

    private void Update() {
        if (tooltipRoot.activeSelf) {
            tooltipRoot.transform.position = Input.mousePosition;
        }
    }

    public void setTooltip(string title, string tooltipContent) {
        tooltipTitleText.text = title;
        tooltipContentText.text = tooltipContent;
        
        tooltipRoot.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTitleText.transform.parent.GetComponent<RectTransform>());
    }

    public void disableTooltip() {
        tooltipRoot.SetActive(false);
    }
    

    private static TooltipController tooltipController;
    public static TooltipController Instance() {
        if (!tooltipController) {
            tooltipController = FindObjectOfType<TooltipController>();
        }
        return tooltipController;
    }
}

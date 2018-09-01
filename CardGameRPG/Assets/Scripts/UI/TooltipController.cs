using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour {

    public GameObject tooltipRoot;
    public Transform buffCategoriesTransform;
    public GameObject buffCategoryPrefab;

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
        setTooltip(title, tooltipContent, null);
    }

    public void setTooltip(string title, string tooltipContent, List<BuffCategory> buffCategories) {
        tooltipTitleText.text = title;
        tooltipContentText.text = tooltipContent;
        
        foreach (Transform child in buffCategoriesTransform) {
            GameObject.Destroy(child.gameObject);
        }

        if (buffCategories != null) {
            foreach (BuffCategory buffCat in buffCategories) {
                GameObject buffCatGO = Instantiate(buffCategoryPrefab, buffCategoriesTransform);
                buffCatGO.GetComponent<Text>().text = buffCat.ToString();
            }
        }

        tooltipRoot.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(buffCategoriesTransform as RectTransform);
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

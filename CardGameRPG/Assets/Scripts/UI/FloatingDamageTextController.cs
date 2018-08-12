using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageTextController : MonoBehaviour {

    public Transform battleCanvas;
    public Camera battleCamera;

    public void createDodgeText(Transform target) {
        Vector2 screenPosition = battleCamera.WorldToScreenPoint(target.position);
        GameObject go = Instantiate(PrefabDictionary.Instance().DodgeText);
        go.transform.SetParent(battleCanvas, false);

        go.transform.position = screenPosition;
    }

    public void createFloatingDamageText(Transform target, int rawDamage, int armor, int block, int healthLost) {
        Vector2 screenPosition = battleCamera.WorldToScreenPoint(target.position);
        GameObject go = Instantiate(PrefabDictionary.Instance().FloatingDamageText);
        go.transform.SetParent(battleCanvas, false);

        go.transform.position = screenPosition;

        go.GetComponent<FloatingDamageText>().SetText(rawDamage, armor, block, healthLost);
    }

    private static FloatingDamageTextController floatingDamageTextController;
    public static FloatingDamageTextController Instance() {
        if (!floatingDamageTextController) {
            floatingDamageTextController = FindObjectOfType<FloatingDamageTextController>();
        }
        return floatingDamageTextController;
    }
}

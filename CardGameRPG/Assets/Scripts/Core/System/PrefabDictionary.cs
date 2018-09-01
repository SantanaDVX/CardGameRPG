using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDictionary : MonoBehaviour {
    private static PrefabDictionary prefabDictionary;
    public static PrefabDictionary Instance() {
        if (!prefabDictionary) {
            prefabDictionary = FindObjectOfType<PrefabDictionary>();
        }
        return prefabDictionary;
    }

    public GameObject cardBase;
    public GameObject targetListener;
    public GameObject multiTargetListener;
    public GameObject FloatingDamageText;
    public GameObject DodgeText;
    public GameObject preadventureCardInDeckOverlay;
}

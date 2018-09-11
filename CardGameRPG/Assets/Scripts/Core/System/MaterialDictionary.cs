using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MaterialDictionary : MonoBehaviour {

    public List<MaterialLookup> materials;
    public Dictionary<string, Material> materialDictionary;

    public List<RarityLookup> rarityGems;
    public Dictionary<CardRarity, Sprite> rarityGemsDictionary;

    public List<LocationLookup> locationButtons;
    public Dictionary<LocationType, Sprite> locationButtonsDictionary;

    private void OnEnable() {
        materialDictionary = new Dictionary<string, Material>();
        foreach (MaterialLookup ml in materials) {
            materialDictionary[ml.key] = ml.material;
        }
        rarityGemsDictionary = new Dictionary<CardRarity, Sprite>();
        foreach (RarityLookup rl in rarityGems) {
            rarityGemsDictionary[rl.rarity] = rl.sprite;
        }
        locationButtonsDictionary = new Dictionary<LocationType, Sprite>();
        foreach (LocationLookup ll in locationButtons) {
            locationButtonsDictionary[ll.location] = ll.locationButton;
        }
    }
    
    private static MaterialDictionary materialDictionaryInstance;
    public static MaterialDictionary Instance() {
        if (!materialDictionaryInstance) {
            materialDictionaryInstance = FindObjectOfType<MaterialDictionary>();
        }
        return materialDictionaryInstance;
    }
}

[Serializable]
public struct MaterialLookup {
    public string key;
    public Material material;
}

[Serializable]
public struct RarityLookup {
    public CardRarity rarity;
    public Sprite sprite;
}

[Serializable]
public struct LocationLookup {
    public LocationType location;
    public Sprite locationButton;
}



